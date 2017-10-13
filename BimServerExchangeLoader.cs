using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace BimServerExchange
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	[Journaling(JournalingMode.NoCommandData)]
	class BimServerExchangeLoader : IExternalApplication
	{
		#region fields
		private string _tabName;
		#endregion fields

		#region properties
		private string TabName
		{
			get
			{
				if (!string.IsNullOrEmpty(_tabName)) return _tabName;
				_tabName = "BIMserver";
				return _tabName;
			}
			set
			{
				if (string.IsNullOrEmpty(value)) _tabName = "BIMserver";
				else _tabName = value;
			}
		}
		#endregion properties

		#region IExternalApplication Members
		Result IExternalApplication.OnStartup(UIControlledApplication application)
		{
			string addinName = application.ActiveAddInId.GetAddInName();
			TabName = addinName;

			// create the menu tab and panel
			application.CreateRibbonTab("BIMserver");
			RibbonPanel panel = application.CreateRibbonPanel("BIMserver", TabName);

			// add the buttons to the panel
			Result res = CreatePushButton(panel, "Projects", "BimServerExchange.ExportToBimServer", "Manage projects on the BIMserver", Properties.Resources.BiMserver_manager);
			if (Result.Succeeded != res) return res;

			res = CreatePushButton(panel, "Export", "BimServerExchange.QuickExport", "Export the active project to a BIMserver", Properties.Resources.BiMServer_Upload);
			if (Result.Succeeded != res) return res;

			res = CreatePushButton(panel, "View", "BimServerExchange.OpenBimView", "Open the BIMViews page from the BIMServer", Properties.Resources.BiMserver_View);
			if (Result.Succeeded != res) return res;

			res = CreatePushButton(panel, "About", "BimServerExchange.AboutAddin", "About this Addin", Properties.Resources.BIMserver_Info);
			if (Result.Succeeded != res) return res;

			//#if DEBUG
			//res = CreatePushButton(panel, "Reset", "BimServerExchange.ResetConnection", "Erase the current connection settings (forces a new login form)", Properties.Resources.BimServer);
			//if (Result.Succeeded != res) return res;
			//#endif
			return Result.Succeeded;
		}

		Result IExternalApplication.OnShutdown(UIControlledApplication application)
		{
			// Remove the command binding on shutdown
			//return base.OnShutdown(application);
			return Result.Succeeded;
		}
		#endregion IExternalApplication Members

		private Result CreatePushButton(RibbonPanel panel, string titel, string addinCommand, string tooltip, Bitmap bitmap)
		{
			PushButtonData data = new PushButtonData(titel, titel, Assembly.GetExecutingAssembly().Location, addinCommand);
			PushButton btn = panel.AddItem(data) as PushButton;
			if (null == btn)
				return Result.Failed;

			btn.ToolTip = tooltip;
			ImageSourceConverter conv = new ImageSourceConverter();

			Bitmap bmp = new Bitmap(bitmap);
			using (MemoryStream memory = new MemoryStream())
			{
				bmp.Save(memory, ImageFormat.Png);
				memory.Position = 0;
				BitmapImage img = new BitmapImage();
				img.BeginInit();
				img.StreamSource = memory;
				img.CacheOption = BitmapCacheOption.OnLoad;
				img.EndInit();
				btn.LargeImage = img;
			}
			using (MemoryStream memory = new MemoryStream())
			{
				bmp.Save(memory, ImageFormat.Png);
				memory.Position = 0;
				BitmapImage img = new BitmapImage();
				img.BeginInit();
				img.StreamSource = memory;
				img.CacheOption = BitmapCacheOption.OnLoad;
				img.EndInit();
				btn.Image = img;
			}

			return Result.Succeeded;
		}
	}
}
