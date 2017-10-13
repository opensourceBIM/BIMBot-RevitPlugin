using System;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BimServerExchange.Forms;
using BimServerExchange.Runtime;

namespace BimServerExchange
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	[Journaling(JournalingMode.NoCommandData)]
	//public class BimServerExchange : IExternalCommand
	public class QuickExport : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIDocument uidoc = commandData.Application.ActiveUIDocument;

			try
			{
				if (string.IsNullOrEmpty(uidoc.Document.PathName))
				{
					throw new IcnException("The project must be saved before it can be exported to a BIMserver", 10, "QuickExport");
				}
				if (uidoc.Document.PathName.EndsWith(".rfa", StringComparison.InvariantCultureIgnoreCase))
				{
					throw new IcnException("Revit Families can not be exported to a BIMserver", 10, "QuickExport");
				}

				BimServerExportForm form = new BimServerExportForm(uidoc);
				// check if there is a connection to a BIMserver configured, if not keep asking the user till he creates one or gives up
				while (!form.Cmd.BimServerExport.CheckLogin())
				{
					BimServerLoginForm login = new BimServerLoginForm(uidoc, form.Data);
					if (DialogResult.Cancel == login.ShowDialog()) return Result.Cancelled;

					form.Data.CopyFrom(login.Data);
					break;
				}
				// at this point we have a valid connection to the BIMserver.

				KnownProjects kp = new KnownProjects();
				//if (string.IsNullOrEmpty(form.Data.ProjectName) || !kp.ContainsKey(uidoc.Document.PathName ?? string.Empty))
				if (!form.Cmd.BimServerExport.CheckProject(kp))
				{
					MessageBox.Show(@"This Revit project is not yet linked to a project on the BIMserver. Use the Projects tool do to so before uploading", @"BIMserver Export");
					return Result.Cancelled;
				}

				// initialise the form controls with the settings from form.Data
				form.Cmd.BimServerExport.Init();
				form.Show();			// <-- making this form modeless means that errors are no longer caught in the command
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
