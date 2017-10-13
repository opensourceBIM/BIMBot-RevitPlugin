using System.Windows.Forms;
using Autodesk.Revit.UI;
using BimServerExchange.Forms;
using BimServerExchange.Runtime;
using Autodesk.Internal.InfoCenter;
using Autodesk.Windows;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Commands
{
	internal class Commander
	{
		#region fields
		private CmdServerInterface _serverInterface;
		private CmdIfcExporterAPI _ifcExporter;
		private CmdBimServerLogin _bimServerLogin;
		private CmdBimServerExchange _bimServerExchange;
		private CmdBimServerExport _bimServerExport;
		private CmdProcessingStatus _processingStatus;
		#endregion fields

		#region properties
		internal UIDocument UIDoc
		{
			get
			{
				// these are the subcommandoers that actually know of a UIDocument. 
				// currently only the _bimServerExport actually runs the progress
				// (_bimServerExchange does too, but the control to activate the upload never gets enabled nor visible)
				if (null != _bimServerExport) return _bimServerExport.Form.UIDoc;
				if (null != _bimServerExchange) return _bimServerExchange.Form.UIDoc;
				if (null != _bimServerLogin) return _bimServerLogin.Form.UIDoc;
				if (null != _ifcExporter) return _ifcExporter.Exporter.UIDoc;

				return null;
			}
			set
			{
				if (null == value) return;

				if (null != _bimServerExport) _bimServerExport.Form.UIDoc = value;
				if (null != _bimServerExchange) _bimServerExchange.Form.UIDoc = value;
				if (null != _bimServerLogin) _bimServerLogin.Form.UIDoc = value;
				if (null != _ifcExporter) _ifcExporter.Exporter.UIDoc = value;
			}
		}

		internal CmdServerInterface ServerInterface
		{
			get
			{
				if (null != _serverInterface) return _serverInterface;
				_serverInterface = new CmdServerInterface(this);
				// demand created can't reliably set the delegate for this subcommander
				return _serverInterface;
			}
			private set
			{
				_serverInterface = value;
			}
		}
		internal CmdIfcExporterAPI IfcExporter
		{
			get
			{
				if (null != _ifcExporter) return _ifcExporter;
				throw new IcnException("Property IfcExporter not set to a reference", 10, "Commander");
			}
			private set
			{
				_ifcExporter = value;
			}
		}
		internal CmdBimServerLogin BimServerLogin
		{
			get
			{
				if (null != _bimServerLogin) return _bimServerLogin;
				throw new IcnException("Property BimServerLogin not set to a reference", 10, "Commander");
			}
			private set
			{
				_bimServerLogin = value;
			}
		}
		internal CmdBimServerExchange BimServerExchange
		{
			get
			{
				if (null != _bimServerExchange) return _bimServerExchange;
				throw new IcnException("Property BimServerExchange not set to a reference", 10, "Commander");
			}
			private set
			{
				_bimServerExchange = value;
			}
		}
		internal CmdBimServerExport BimServerExport
		{
			get
			{
				if (null != _bimServerExport) return _bimServerExport;
				throw new IcnException("Property BimServerExport not set to a reference", 10, "Commander");
			}
			private set
			{
				_bimServerExport = value;
			}
		}
		internal CmdProcessingStatus ProcessingStatus
		{
			get
			{
				throw new IcnException("Property ProcessingStatus not set to a reference", 10, "Commander");
			}
			private set
			{
				_processingStatus = value;
			}
		}
		internal BimServerExchangeData Data
		{
			get
			{
				if (null != _bimServerExchange) return _bimServerExchange.Form.Data;
				if (null != _bimServerExport) return _bimServerExport.Form.Data;

				throw new IcnException("Property Data accessed from a Commander without any of the required Forms", 10, "Commander");
			}
		}
		#endregion properties

		#region ctors, dtor
		internal Commander(Form form, UIDocument uidoc)
		{
			if (null == form || null == uidoc)
			{
				throw new IcnException("One or more parameters not set to a reference", 10, "BimServerExchange Commander");
			}

			// shared commander
			ServerInterface = new CmdServerInterface(this);
			IfcExporter = new CmdIfcExporterAPI(this, uidoc);

			// foreach form and context that has a separate commander create it here
			if (form is BimServerLoginForm)
			{
				BimServerLogin = new CmdBimServerLogin((BimServerLoginForm)form, uidoc);
				return;
			}

			if (form is BimServerExportForm)
			{
				BimServerExportForm qx = (BimServerExportForm)form;
				BimServerExport = new CmdBimServerExport(qx, uidoc);
				ServerInterface.ReportStatus = qx.ReportStatus;
				return;
			}

			if (form is BimServerExchangeForm)
			{
				BimServerExchangeForm ex = (BimServerExchangeForm)form;
				BimServerExchange = new CmdBimServerExchange(ex, uidoc);
				ServerInterface.ReportStatus = ex.ReportStatus;
				return;
			}

			if (form is ProcessingStatusForm)
			{
				ProcessingStatusForm sx = (ProcessingStatusForm)form;
				ProcessingStatus = new CmdProcessingStatus(sx);
				ServerInterface.ReportStatus = sx.ReportStatus;
				return;
			}

			throw new IcnException($"Unhandled Form of type '{form.GetType().Name}'", 20, "Commander");
		}
		#endregion ctors, dtor

		public void ShowResult(string mssg, bool forceShowTooltip = false)
		{
			if (null != _bimServerExchange)
			{
				BimServerExchange.ShowResultInForm(mssg);
				if (!forceShowTooltip) return;
			}

			if (null != _bimServerExport)
			{
				BimServerExport.ShowResultInForm(mssg);
				if (!forceShowTooltip) return;
			}

			// quick dialog. Show result in Revit
			ResultItem ri = new ResultItem { Category = "BIMserver Upload Status", Title = mssg, IsNew = true };
			ComponentManager.InfoCenterPaletteManager.ShowBalloon(ri);
		}

		public void InitialiseProgressbar(int len)
		{
			if (null != _bimServerExchange)
			{
				BimServerExchange.InitialiseProgressbar(len);
				return;
			}

			if (null != _bimServerExport)
			{
				BimServerExport.InitialiseProgressbar(len);
				return;
			}
		}
	}
}
