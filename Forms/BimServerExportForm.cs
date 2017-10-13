using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using BimServerExchange.Commands;
using BimServerExchange.Objects;
using BimServerExchange.Runtime;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace BimServerExchange.Forms
{
	public partial class BimServerExportForm : Form
	{
		private UIDocument _uidoc;
		private Commander _cmd;
		// modeless dialog support (requires commands to be passed through an IExternalEventHandler to unlock the database and provide the active UIApplication)
		private BimServerExportEventHandler _eventHandler;
		private ExternalEvent _event;
		private BimServerExchangeData _data;
		private int _bitfields;

		internal UIDocument UIDoc
		{
			get
			{
				if (null != _uidoc) return _uidoc;

				throw new IcnException("Property UIDoc not set to a reference", 10, "BimServerExportForm");
			}
			set
			{
				if (null == value) return;

				_uidoc = value;
			}
		}
		internal Commander Cmd
		{
			get
			{
				if (null != _cmd) return _cmd;

				throw new IcnException("Property Cmd no set", 10, "BimServerExportForm");
			}
		}

		// event handler properties
		internal BimServerExportEventHandler Handler
		{
			get
			{
				if (null == _eventHandler) throw new IcnException("No eventhandler assigned", 10, "BimServerExportForm");
				return _eventHandler;
			}
		}
		internal ExternalEvent Event
		{
			get
			{
				if (null == _event) throw new IcnException("No external event assigned", 10, "BimServerExportForm");
				return _event;
			}
		}

		internal bool UseStandardRevitExporter
		{
			get
			{
				return Data.UseStandardRevitExporter;
			}
			set
			{
				Data.UseStandardRevitExporter = value;
			}
		}

		internal bool Connected
		{
			get
			{
				return (_bitfields & 2) == 2;
			}
			set
			{
				_bitfields |= 2;
				if (!value) _bitfields ^= 2;
			}
		}
		public bool Cancelling
		{
			get
			{
				return (_bitfields & 4) == 4;
			}
			private set
			{
				_bitfields |= 4;
				if (!value) _bitfields ^= 4;
			}
		}


		internal BimServerExchangeData Data
		{
			get
			{
				if (null != _data)
					return _data;

				_data = new BimServerExchangeData();
				_data.LoadFromProperties(false);
				return _data;
			}
		}

		internal string Comment
		{
			get
			{
				return CommentEdt.Text;
			}
		}

		public BimServerExportForm(UIDocument uidoc)
		{
			InitializeComponent();
			InitialiseForm(uidoc);
		}

		private void InitialiseForm(UIDocument uidoc)
		{
			UIDoc = uidoc;
			_cmd = new Commander(this, uidoc);

			// create an eventhandler 
			_eventHandler = new BimServerExportEventHandler(Cmd);
			_event = ExternalEvent.Create(_eventHandler);
		}

		#region private methods (wrapped)
		// methods here are only present to allow the orginal code to be restored more easily. Remove them
		// if the rerouting to CmdBimServerExport succeeds
		private void ExportEvent()
		{
			string path = null;

			try
			{
				// we do not show the form, rather we use its commander directly
				string fname = Path.GetFileNameWithoutExtension(UIDoc.Document.PathName) ?? "output";
				Guid guid = Guid.NewGuid();
				path = Path.Combine(Path.GetTempPath(), guid.ToString("D"), fname + ".ifc");

				Cmd.BimServerExport.ActivateButtons(false);
				path = Cmd.BimServerExport.ExportProjectToIFC(path);
				if (!File.Exists(path))
				{
					ReportStatus("Project could not be exported to IFC", false);
					Cmd.BimServerExport.ActivateButtons(true);
					Cmd.BimServerExport.DeleteTemporaryFiles(path);
					return;
				}

				Cmd.BimServerExport.QuickCheckinFile(path, Comment);

			}
			catch (IcnException iex)
			{
				iex.Display("Checkin Project");
			}
			catch (Exception ex)
			{
				Cmd.BimServerExport.ShowResultInForm($"Checkin Project failed ({ex.Message})");
			}
			finally
			{
				Cmd.BimServerExport.ActivateButtons(true);
				if (!string.IsNullOrEmpty(path)) Cmd.BimServerExport.DeleteTemporaryFiles(path);
			}
		}

		private void OpenProjectEvent()
		{
			SProject project = Cmd.ServerInterface.GetProject(Data, Data.ProjectName);
			if (null == project) return;
			int poid = project.oid;
			string tag = $"apps/bimviews/?page=Project&poid={poid}";
			string url = $"{Data.Url}:{Data.Port}/{tag}";
			Process.Start(url);
		}

		private void LoginEvent()
		{
			if (Connected)
			{
				Cmd.ServerInterface.Logout(Data);
				Connected = false;
				ExportBtn.Enabled = false;
			}

			BimServerLoginForm form = new BimServerLoginForm(UIDoc, Data);
			if (DialogResult.OK != form.ShowDialog())
			{
				return;
			}

			Data.CopyFrom(form.Data);
			Cmd.ShowResult("Successfully connected to the BIMserver"/*Data.Token*/);
			ExportBtn.Enabled = true; // if we have a connection we can enable the action buttons (upload)
			Connected = true; // mark as connected
		}
		#endregion private methods (wrapped)

		private void SimpleButton_Click(object sender, EventArgs e)
		{
			if (!(sender is Button btn)) return;

			if (ExportBtn == btn)
			{
				// usage of external event handlers is required for modeless dialogs
				Handler.Command = BimServerExportEventHandler.Commands.kExport;
				Event.Raise();

				//Cmd.BimServerExport.ExportEvent();

				//ExportEvent();
				return;
			}

			if (OpenProjectBtn == btn)
			{
				// usage of external event handlers is required for modeless dialogs
				Handler.Command = BimServerExportEventHandler.Commands.kOpenProject;
				Event.Raise();

				//Cmd.BimServerExport.OpenProjectEvent();

				//OpenProjectEvent();
				return;
			}

			if (LoginBtn == btn)
			{
				// usage of external event handlers is required for modeless dialogs
				Handler.Command = BimServerExportEventHandler.Commands.kLogin;
				Event.Raise();

				//Cmd.BimServerExport.LoginEvent();

				//LoginEvent();
				return;
			}

			if (CancelBtn == btn)
			{
				// this needs to inform the attached DownloadHandler that it needs to cancel
				Cmd.ServerInterface.CancelDownload();
				Cmd.BimServerExport.ActivateButtons(true);
				return;
			}

			throw new IcnException($"SimpleButton '{btn.Name}' activated that has no code", 10, "BimServerExportForm");
		}

		public void ReportStatus(string status, bool reset)
		{
			if (reset)
			{
				Cmd.BimServerExport.InitialiseProgressbar();
			}

			if (int.TryParse(status, out int val))
			{
				Cmd.BimServerExport.ShowProgressInForm(val);
				return;
			}

			Cmd.BimServerExport.ShowResultInForm(status ?? string.Empty);
			//MessageBox.Show(status ?? string.Empty, @"BIMserver");
		}

		private void Form_Closing(object sender, FormClosingEventArgs e)
		{
			// the form is about to go away, but the process on the server may still be running and attempting to
			// report status changes to the form. (of course the commander and serverinterface may be about to die on us too)
			Cancelling = true;
			if (Cmd.ServerInterface.MonitorProgress) Cmd.BimServerExport.StopReportingProgress();
		}

		private void Form_Shown(object sender, EventArgs e)
		{
			CommentEdt.Focus();
			CommentEdt.Select();
		}
	}
}