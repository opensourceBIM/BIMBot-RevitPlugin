using System.IO;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimServerExchange.Runtime
{
	static class ExtensionMethods
	{
		/// <summary>
		///    Geef een gelinked document terug op basis van naam
		/// </summary>
		/// <param name="rli">RevitLinkInstance</param>
		/// <param name="app">UIApplication</param>
		/// <returns></returns>
		public static Document GetLinkDocument(this RevitLinkInstance rli, UIApplication app)
		{
			string[] totaalnaam = rli.Name.Split(':');
			return
				app.Application.Documents.Cast<Document>()
					.FirstOrDefault(d =>
					{
						string fileName = Path.GetFileName(d.PathName);
						return fileName != null && fileName.Equals(totaalnaam[totaalnaam.Length - 3].Trim());
					});
		}
	}
}
