//************************************************************************************************
// Copyright © 2021 Steven M Cohn.  All rights reserved.
//************************************************************************************************

namespace River.OneMoreAddIn.Commands
{
	using System.Linq;
	using System.Threading.Tasks;
	using River.OneMoreAddIn.Commands.Search;


	internal class ShowParentsCommand : Command
	{
		public ShowParentsCommand()
		{
		}


		public override async Task Execute(params object[] args)
		{
			//using (var one = new OneNote(out var page, out var ns))
			//{
			//    var ink = page.Root.Descendants(ns + "InkDrawing");

			//    if (ink.Any())
			//    {
			//        ink.ForEach(e =>
			//            one.DeleteContent(page.PageId, e.Attribute("objectID").Value));
			//    }
			//}

			//await Task.Yield();

			var copying = false;

			var dialog = new ParentsDialog();
			await dialog.RunModeless((sender, e) =>
			{
				//var d = sender as ParentsDialog;
				//if (d.DialogResult == DialogResult.OK)
				//{
				//	copying = dialog.CopySelections;
				//	pageIds = dialog.SelectedPages;

				//	var desc = copying
				//		? Resx.SearchQF_DescriptionCopy
				//		: Resx.SearchQF_DescriptionMove;

				//	using (var one = new OneNote())
				//	{
				//		one.SelectLocation(Resx.SearchQF_Title, desc, OneNote.Scope.Sections, Callback);
				//	}
				//}
			},
			20);

			await Task.Yield();

		}
	}
}
