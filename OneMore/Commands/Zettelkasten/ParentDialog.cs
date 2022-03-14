//************************************************************************************************
// Copyright © 2020 Steven M Cohn.  All rights reserved.
//************************************************************************************************

namespace River.OneMoreAddIn.Commands.Search
{
	using River.OneMoreAddIn.UI;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Forms;
	using System.Xml.Linq;
	using Resx = River.OneMoreAddIn.Properties.Resources;


	internal partial class ParentsDialog : LocalizableForm
	{
		private readonly OneNote one;
		private readonly Models.Page page;
		private readonly XNamespace ns;

		public ParentsDialog()
		{
			InitializeComponent();

			if (NeedsLocalizing())
			{
				Text = Resx.SearchDialog_Title;

				Localize(new string[]
				{
					"introLabel",
					"findLabel",
					"moveButton=word_Move",
					"copyButton=word_Copy",
					"cancelButton=word_Cancel"
				});

				scopeBox.Items.Clear();
				scopeBox.Items.AddRange(Resx.SearchDialog_scopeBox_Items.Split(new char[] { '\n' }));
			}

			scopeBox.SelectedIndex = 0;
			SelectedPages = new List<string>();

			one = new OneNote(out page, out ns);

		//	SearchParents(); this does not work here for some reason, go by events instead
		}


		public bool CheckIfZettel(XElement RootOfPage, out XElement FoundTable)
        {
			var FirstTable = RootOfPage.Descendants(ns + "Table").FirstOrDefault();

			if (FirstTable != null)
			{
				var FirstRow = FirstTable.Descendants(ns + "Row").FirstOrDefault();

				if (FirstRow != null)
				{
					var FirstCell = FirstRow.Descendants(ns + "Cell").FirstOrDefault();

					if (FirstCell != null)
					{
						var FirstT = FirstCell.Descendants(ns + "T").FirstOrDefault();

						if (FirstT.Value == "Zettel Data")
						{
							FoundTable = FirstTable;
							return true;
						}
					}
				}
			}
			FoundTable = null;
			return false;
		}

		public bool CellContainsReference(XElement InCell, string ReferenceTitle)
        {
				if (InCell.Value == ReferenceTitle)
				{
					return true;
				}

				else if (!InCell.Value.IsNullOrEmpty())
				{
					XElement ValueOfSister = null;

					try
					{
						ValueOfSister = XElement.Parse(InCell.Value); //we need to do it this way for a link. The hyperlink is in CDATA and won't be parsed otherwise
					}
					catch (System.Xml.XmlException e)
					{
						return false;
					}

					if ((ValueOfSister != null) && ValueOfSister.Value == ReferenceTitle)
					{
						return true;
					}
				}

			return false;
		}

		public bool IsZettelTableParent(XElement InTable, string ChildTitle, out bool IsMainChild)
		{
			var FirstMainChild = InTable.Descendants(ns + "Cell").FirstOrDefault(e => e.Value == "Main Child");
            if (FirstMainChild != null)
            {
                XElement SisterOfMainChild = FirstMainChild.NextNode as XElement;
				if (SisterOfMainChild != null)
				{
					//if (SisterOfMainChild.Value == ChildTitle)
					//{
					//	IsMainChild = true;
					//	return true;
					//}

					//else if (!SisterOfMainChild.Value.IsNullOrEmpty())
					//{ 
					//	var ValueOfSister = XElement.Parse(SisterOfMainChild.Value); //we need to do it this way for a link. The hyperlink is in CDATA and won't be parsed otherwise

					//	if ((ValueOfSister != null) && ValueOfSister.Value == ChildTitle)
					//	{
					//		IsMainChild = true;
					//		return true;
					//	} 
					//}
					if (CellContainsReference(SisterOfMainChild, ChildTitle))
                    {
						IsMainChild = true;
						return true;
                    }
                }
            }

			var FirstOtherChilds = InTable.Descendants(ns + "Cell").FirstOrDefault(e => e.Value == "Other Childs");
			if (FirstOtherChilds != null)
            {
				XElement SisterOfOtherChilds = FirstOtherChilds.NextNode as XElement;
                if (SisterOfOtherChilds != null)
                {
                    XElement TableInSister = SisterOfOtherChilds.Descendants(ns + "Table").FirstOrDefault(); //other children are in a table in SisterOfOtherChilds, get the first table
                    if (TableInSister != null)
                    {
                        foreach (XElement elem in TableInSister.Descendants(ns + "Cell"))//just check for all cells in the table, if one of them contains a valid reference, give a pass
                        {
                            if (CellContainsReference(elem, ChildTitle))
                            {
                                IsMainChild = false;
                                return true;
                            }
                        } 
                    }
                }

			}

			IsMainChild = false;
			return false;
		}


		public bool CheckIfParent(XElement RootOfPage)
        {
			//var FirstTable = RootOfPage.Descendants(ns + "Table").FirstOrDefault();//.FirstOrDefault();//.FirstOrDefault(e => e.Name == ns + "T");


			if (CheckIfZettel(RootOfPage, out XElement FoundTable) == true)
            {
                if (IsZettelTableParent(FoundTable, page.Title, out bool MainChild))
                {
                    return true; 
                }
            }

							//finde descendants where value = Main Child (oder: contains)
							//dann: next node value sehen, schauen, ob die unser Ziel ist


				//	var section = container.Descendants(ns + "Section")
				//.FirstOrDefault(e => e.Attribute("isCurrentlyViewed")?.Value == "true");

			//Erste Tabelle, erste row, erste cell, erste T98

			//foreach (var element in RootOfPage.Elements())
			//{
			//	DisplayResults(element, ns, node.Nodes);
			//}

			return false;
        }


		public bool CopySelections { get; private set; }


		public List<string> SelectedPages { get; private set; }


		//private void ChangeQuery(object sender, EventArgs e)
		//{
		//	searchButton.Enabled = findBox.Text.Trim().Length > 0;
		//}


		//private void SearchOnKeydown(object sender, KeyEventArgs e)
		//{
  //          if (e.KeyCode == Keys.Enter &&
  //              findBox.Text.Trim().Length > 0)
  //          {
  //              Search(sender, e);
  //          }
  //      }


		private async void SearchParents()
		{
			resultTree.Nodes.Clear();

			string startId = one.CurrentNotebookId; //default just look in this notebook
			switch (scopeBox.SelectedIndex)
			{
				case 1: startId = string.Empty; break;
				case 2: startId = one.CurrentSectionId; break;
			}
			
			//search for Zettel Data as well, to filter for things that aren't Zettels so we don't have to load and checkthem
			var results = one.Search(startId, "Zettel Data " + page.Title);
			if (results.HasElements)
			{
				//		.Where(e => e.Attribute("isRecycleBin") == null &&
				//	e.Attribute("isInRecycleBin") == null)


				//	public Page GetPage(string pageId,

				var PageDescendants = results.Descendants(ns + "Page").ToList(); //convert to List, to avoid the array changing due to garbage collection on elem.Remove()
				foreach (XElement elem in PageDescendants)
				{
					Models.Page LoadedPage = one.GetPage(elem.Attribute("ID").Value);
					if (LoadedPage != null)
                    {
						if(!CheckIfParent(LoadedPage.Root))
                        {
							elem.Remove();
                        }
                    }
				}

                if (results.HasElements)//Might have changed to to removal
                {
                    resultTree.Populate(results, one.GetNamespace(results)); 
                }
			}
			
			//CheckIfParent(page.Root);
		}


		// async event handlers should be be declared 'async void'
		private async void ClickNode(object sender, TreeNodeMouseClickEventArgs e)
		{
			// thanksfully, Bounds specifies bounds of label
			var node = e.Node as HierarchyNode;
			if (node.Hyperlinked && e.Node.Bounds.Contains(e.Location))
			{
				var pageId = node.Root.Attribute("ID").Value;
				if (!pageId.Equals(one.CurrentPageId))
				{
					await one.NavigateTo(pageId);
				}
			}
		}


		private void TreeAfterCheck(object sender, TreeViewEventArgs e)
		{
			var node = e.Node as HierarchyNode;
			var id = node.Root.Attribute("ID").Value;

			if (node.Checked)
			{
				if (!SelectedPages.Contains(id))
				{
					SelectedPages.Add(id);
				}
			}
			else if (SelectedPages.Contains(id))
			{
				SelectedPages.Remove(id);
			}
		}

		private void Nevermind(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

        private async void ParentsDialog_Activated(object sender, EventArgs e)
        {
			SearchParents();
        }
    }
}
