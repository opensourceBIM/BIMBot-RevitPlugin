using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BimServerExchange.Runtime;

namespace BimServerExchange
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	[Journaling(JournalingMode.NoCommandData)]
	class ResetConnection : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			BimServerExchangeData data = new BimServerExchangeData();
			data.ResetProperties();

			return Result.Succeeded;
		}
	}
}
