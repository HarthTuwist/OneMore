﻿//************************************************************************************************
// Copyright © 2020 Steven M Cohn.  All rights reserved.
//************************************************************************************************

namespace River.OneMoreAddIn.Commands
{
	using River.OneMoreAddIn.Dialogs;
	using River.OneMoreAddIn.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using System.Xml.Linq;


	internal partial class TaggedDialog : LocalizableForm
	{
		private readonly string separator;
		private readonly OneNote one;
		private bool editing = false;


		public TaggedDialog()
		{
			InitializeComponent();

			filterBox.PressedEnter += Search;
			scopeBox.SelectedIndex = 0;

			separator = AddIn.Culture.TextInfo.ListSeparator;

			// disposed in Dispose()
			one = new OneNote();
		}


		private void ChangedFilter(object sender, EventArgs e)
		{
			var enabled = filterBox.Text.Trim().Length > 0;
			searchButton.Enabled = enabled;

			if (enabled)
			{
				var tags = FormatFilter(filterBox.Text.ToLower())
					.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
					.Select(v => Regex.Replace(v.Trim(), @"^-", string.Empty))
					.ToList();

				editing = true;
				foreach (TagCheckBox tag in tagsFlow.Controls)
				{
					tag.Checked = tags.Contains(tag.Text);
				}
				editing = false;
			}
		}


		private void ChangeScope(object sender, EventArgs e)
		{
			tagsFlow.Controls.Clear();

			OneNote.Scope scope = OneNote.Scope.Notebooks;
			switch (scopeBox.SelectedIndex)
			{
				case 1: scope = OneNote.Scope.Sections; break;
				case 2: scope = OneNote.Scope.Pages; break;
			}

			var tags = TagHelpers.FetchRecentTags(scope, 30);

			if (tags.Count > 0)
			{
				var sorted = tags.OrderBy(k => k.Key.StartsWith("#") ? k.Key.Substring(1) : k.Key);

				foreach (var s in sorted)
				{
					var tag = new TagCheckBox(s.Value);
					tag.CheckedChanged += ChangeTagSelection;
					tagsFlow.Controls.Add(tag);
				}
			}
		}


		private void ChangeTagSelection(object sender, EventArgs e)
		{
			if (editing)
			{
				return;
			}

			var box = sender as TagCheckBox;
			if (box.Checked)
			{
				if (filterBox.Text.Trim().Length == 0)
				{
					filterBox.Text = box.Text;
				}
				else
				{
					// check if user already type in this tag
					var tags = filterBox.Text.Split(
						new string[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToList();

					if (!tags.Any(t => t.Equals(box.Text, StringComparison.CurrentCultureIgnoreCase)))
					{
						filterBox.Text = $"{FormatFilter(filterBox.Text)}{separator} {box.Text}";
					}
				}

				clearLabel.Enabled = true;
			}
			else
			{
				var text = Regex.Replace(
					FormatFilter(filterBox.Text), $@"(?:\s|^)\-?{box.Text}(?:{separator}|$)", string.Empty);

				// removing entry at end of string will leave a comma at end of string
				filterBox.Text = text.EndsWith(separator)
					? text.Substring(0, text.Length - 1).Trim()
					: text.Trim();

				var count = 0;
				foreach (TagCheckBox tag in tagsFlow.Controls)
				{
					if (tag.Checked)
					{
						count++;
						break;
					}
				}

				if (count == 0)
				{
					clearLabel.Enabled = false;
				}
			}
		}


		private string FormatFilter(string filter)
		{
			// collapse multiple spaces to single space
			var text = Regex.Replace(filter.Trim(), @"[ ]{2,}", " ");

			// clean up spaces preceding commas
			text = Regex.Replace(text, $@"\s+{separator}", ",");

			// clean up spaces after the negation operator
			text = Regex.Replace(text, @"\-\s+", "-");

			// clean up extra commas at start or end of string
			text = Regex.Replace(text, $@"(^\s?{separator}\s?)|(\s?{separator}\s?$)", string.Empty);

			return text;
		}


		private void ClearFilters(object sender, EventArgs e)
		{
			filterBox.Text = string.Empty;
			foreach (TagCheckBox tag in tagsFlow.Controls)
			{
				tag.Checked = false;
			}
		}


		private void ClickNode(object sender, TreeNodeMouseClickEventArgs e)
		{
			// thanksfully, Bounds specifies bounds of label
			var node = e.Node as HierarchyNode;
			if (node.Hyperlinked && e.Node.Bounds.Contains(e.Location))
			{
				var pageId = node.Root.Attribute("ID").Value;
				if (!pageId.Equals(one.CurrentPageId))
				{
					one.NavigateTo(pageId);
				}
			}
		}


		private void Search(object sender, EventArgs e)
		{
			toolStrip.Enabled = false;
			resultTree.Nodes.Clear();

			var text = filterBox.Text.ToLower();
			if (string.IsNullOrEmpty(text))
			{
				return;
			}

			var includedTags = FormatFilter(filterBox.Text.ToLower())
				.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
				.Select(v => v.Trim())
				.ToList();

			var excludedTags = includedTags.Where(t => t[0] == '-')
				.Select(t => t.Substring(1).Trim())
				.ToList();

			includedTags = includedTags.Except(excludedTags).ToList();

			var scopeId = string.Empty;
			switch (scopeBox.SelectedIndex)
			{
				case 1: scopeId = one.CurrentNotebookId; break;
				case 2: scopeId = one.CurrentSectionId; break;
			}

			var results = one.SearchMeta(scopeId, Page.TaggingMetaName);
			var ns = one.GetNamespace(results);

			// remove recyclebin nodes
			results.Descendants()
				.Where(n => n.Name.LocalName == "UnfiledNotes" ||
							n.Attribute("isRecycleBin") != null ||
							n.Attribute("isInRecycleBin") != null)
				.Remove();

			// filter
			var metas = results.Descendants(ns + "Meta")
				.Where(m => m.Attribute("name").Value == Page.TaggingMetaName);

			if (metas == null)
			{
				return;
			}

			// filter out unmatched pages, keep track in separate list because metas can't be
			// modified while enumerating
			var dead = new List<XElement>();
			foreach (var meta in metas)
			{
				var tags = meta.Attribute("content").Value.ToLower()
					.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
					.Select(v => v.Trim())
					.ToList();

				if (tags.Count > 0)
				{
					if ((excludedTags.Count > 0 && tags.Any(t => excludedTags.Contains(t))) ||
						(includedTags.Count > 0 && !tags.Any(t => includedTags.Contains(t))))
					{
						Logger.Current.WriteLine(
							$"dead '{string.Join(",", tags)}'");

						dead.Add(meta.Parent);
					}
				}
			}

			// remove unmatched pages
			dead.ForEach(d => d.Remove());

			// remove empty leaf nodes
			var pruning = true;
			while (pruning)
			{
				var elements = results.Descendants()
					.Where(d => d.Name.LocalName != "Meta" && !d.HasElements);

				pruning = elements.Any();
				if (pruning)
				{
					elements.Remove();
				}
			}

			if (results.HasElements)
			{
				resultTree.Populate(results, one.GetNamespace(results));
				toolStrip.Enabled = true;
			}
		}


		private void ToggleChecks(object sender, EventArgs e)
		{
			ToggleChecks(resultTree.Nodes, sender == checkAllButton);
		}


		private void ToggleChecks(TreeNodeCollection nodes, bool check)
		{
			foreach (TreeNode node in nodes)
			{
				node.Checked = check;
				if (node.Nodes?.Count > 0)
				{
					ToggleChecks(node.Nodes, check);
				}
			}
		}


		private void Cancel(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}