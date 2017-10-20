using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BimServerExchange.Forms;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace BimServerExchange
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	[Journaling(JournalingMode.NoCommandData)]
	class AboutAddin : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			//UIDocument uidoc = commandData.Application.ActiveUIDocument;
			Application app = commandData.Application.Application;
#if R2016
			// this version does not support the SubVersionNumber property
			string revitVersion = app.VersionNumber;
#elif R2017
			// this version does not support the SubVersionNumber property
			string revitVersion = app.VersionNumber;
#else
			string revitVersion = app.SubVersionNumber;
#endif
			int major = 1;														// major build version
			int minor = 0;                                        // minor/patch build version
			int build = 6;                                        // build number
			string av = app.VersionNumber.Substring(0, 4);			// revit version indicator
			// show the about dialog for this addin
			//AboutForm form = new AboutForm(revitVersion, $"{major}.{minor}.{av}.{build:0000}");
			AboutForm form = new AboutForm(revitVersion, $"{major}.{minor}.{build:00}.{av}");
			form.ShowDialog();

			// autocancel for now as there is no effect in Revit that needs to be committed
			return Result.Cancelled;
		}
	}
}


