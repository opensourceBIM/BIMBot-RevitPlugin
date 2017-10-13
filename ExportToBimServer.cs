using System;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BimServerExchange.Forms;
using BimServerExchange.Runtime;
// ReSharper disable UnusedMember.Global

namespace BimServerExchange
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	[Journaling(JournalingMode.NoCommandData)]
	//public class BimServerExchange : IExternalCommand
	public class ExportToBimServer : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIDocument uidoc = commandData.Application.ActiveUIDocument;

			if (string.IsNullOrEmpty(uidoc.Document.PathName))
			{
				throw new IcnException("The project must be saved before it can be exported to a BIMserver", 10, "QuickExport");
			}
			if (uidoc.Document.PathName.EndsWith(".rfa", StringComparison.InvariantCultureIgnoreCase))
			{
				throw new IcnException("Revit Families can not be exported to a BIMserver", 10, "QuickExport");
			}

			try
			{
				BimServerExchangeForm form = new BimServerExchangeForm(uidoc);
				form.Cmd.BimServerExchange.Init();
				while (!form.Cmd.BimServerExchange.CheckLogin())
				{
					BimServerLoginForm login = new BimServerLoginForm(uidoc, form.Data, false);
					if (DialogResult.Cancel == login.ShowDialog()) return Result.Cancelled;

					form.Data.CopyFrom(login.Data);
					break;
				}

				// show the export/import dialog (now it is connected to a BimServer that we can query)
				form.InitialiseProjects();
				form.PreselectProject(uidoc.Document?.PathName);
				form.ShowDialog();
			}
			catch (IcnException iex)
			{
				iex.Display(@"Exception in BIMserver Export");
			}
			catch (Exception ex)
			{
				string mssg = ex.Message;
				MessageBox.Show(mssg, @"Exception in BIMserver Export");
			}

			// autocancel for now
			return Result.Cancelled;
		}
	}
}
