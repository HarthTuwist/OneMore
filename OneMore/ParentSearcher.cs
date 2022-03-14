using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using River.OneMoreAddIn.UI;
using System.Windows.Forms;
using System.Xml.Linq;
using Resx = River.OneMoreAddIn.Properties.Resources;





namespace River.OneMoreAddIn
{
    public class ParentSearcher
    {
		private readonly OneNote one;
		private Models.Page page;
		private readonly XNamespace ns;

		public XElement results;



		public Task SearchParentsTask;
		public string ParentIDCurrentlyChecked;
		private System.Threading.CancellationTokenSource SearchParentTokenSource;
		private System.Threading.CancellationToken SearchParentToken;

		public ParentSearcher()
		{
			one = new OneNote(out page, out ns);
			SearchParentTokenSource = new System.Threading.CancellationTokenSource();
			SearchParentToken = SearchParentTokenSource.Token;
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


		public async Task SearchParents()
		{
			//string startId = one.CurrentNotebookId; //default just look in this notebook
			string startId = string.Empty; //default just look in this notebook



			//search for Zettel Data as well, to filter for things that aren't Zettels so we don't have to load and checkthem
			results = one.Search(startId, "Zettel Data " + page.Title);
			if (results.HasElements)
			{
				//		.Where(e => e.Attribute("isRecycleBin") == null &&
				//	e.Attribute("isInRecycleBin") == null)


				//	public Page GetPage(string pageId,

				var PageDescendants = results.Descendants(ns + "Page").ToList(); //convert to List, to avoid the array changing due to garbage collection on elem.Remove()
				foreach (XElement elem in PageDescendants)
				{
					if(SearchParentToken.IsCancellationRequested)
                    {
						SearchParentTokenSource = new System.Threading.CancellationTokenSource();
						SearchParentToken = SearchParentTokenSource.Token;
						SearchParentsTask = SearchParents();
						return;
                    }

					Models.Page LoadedPage = one.GetPage(elem.Attribute("ID").Value);
					if (LoadedPage != null)
					{
						if (!CheckIfParent(LoadedPage.Root))
						{
							elem.Remove();
						}
					}
				}

				//if (results.HasElements)//Might have changed to to removal
				//{
				//	resultTree.Populate(results, one.GetNamespace(results));
				//}
			}

			//CheckIfParent(page.Root);
		}


		//TODO this code doesn't really work. Needs to be checked and fixed. Also, it's currently not implemented
		public async void CheckIfParentRefreshNeeded()
		{
			page = one.GetPage();
			if(page.PageId != ParentIDCurrentlyChecked)
            {
				ParentIDCurrentlyChecked = page.PageId;


				if (SearchParentsTask.IsCompleted)
				{
					SearchParentTokenSource = new System.Threading.CancellationTokenSource();
					SearchParentToken = SearchParentTokenSource.Token;
					SearchParentsTask = SearchParents();
				}
				else
				{
					SearchParentTokenSource.Cancel();
					//restart of the Task will be done in SearchParents() when it realizes it must be canceled
					//TODO: can this lead to rare problems with race conditions?
				}
            }
		}

	}
}

