using System.Windows.Forms;
using Autodesk.Revit.UI;
using BimServerExchange.Commands;
using BimServerExchange.Runtime;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable NotAccessedVariable

namespace BimServerExchange.Forms
{
	public partial class ProcessingStatusForm : Form
	{
		private UIDocument _uidoc;
		private Commander _cmd;

		private UIDocument UIDoc
		{
			get
			{
				if (null != _uidoc) return _uidoc;

				throw new IcnException("Property UIDoc not set to a reference", 10, "ProcessingStatusForm");
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

				throw new IcnException("Property Cmd no set to a reference", 10, "ProcessingStatusForm");
			}
		}

		public ProcessingStatusForm(UIDocument uidoc)
		{
			InitializeComponent();
			InitialiseForm(uidoc);
		}

		private void InitialiseForm(UIDocument uidoc)
		{
			UIDoc = uidoc;
			_cmd = new Commander(this, UIDoc);
		}

		internal void CloseForm()
		{
			Close();
		}

		internal void ReportStatus(string status, bool reset)
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
		}

		internal void RunStatusUpdate(BimServerExchangeData connectionData, int topicId)
		{
			Cmd.ProcessingStatus.InitialiseProgressbar();

			Cmd.ProcessingStatus.ShowResultInForm("test");

			string result;
			Cmd.ServerInterface.UploadProcessingStatusAsync(connectionData, topicId, out result);
		}
	}
}