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
			int minor = 0;														// minor/patch build version
			string av = app.VersionNumber.Substring(2, 2);			// revit version indicator
			int build = 3;														// build number
			// show the about dialog for this addin
			AboutForm form = new AboutForm(revitVersion, $"{major}.{minor}.{av}.{build:0000}");
			form.ShowDialog();

			// autocancel for now as there is no effect in Revit that needs to be committed
			return Result.Cancelled;
		}
	}
}


