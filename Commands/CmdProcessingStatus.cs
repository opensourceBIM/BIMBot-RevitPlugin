using System;
using System.Windows.Forms;
using BimServerExchange.Forms;
using BimServerExchange.Runtime;
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Commands
{
	internal class CmdProcessingStatus
	{
		#region fields
		private ProcessingStatusForm _form;
		#endregion fields

		#region properies
		private ProcessingStatusForm Form
		{
			get
			{
				if (null != _form)
					return _form;

				throw new IcnException("Property Form not set to a reference", 10, "CmdProgressStatusForm");
			}
			set { _form = value; }
		}
		#endregion properties

		#region ctors, dtor
		internal CmdProcessingStatus(ProcessingStatusForm form)
		{
			Form = form;
		}

		internal void Init()
		{
			Form.Cmd.ServerInterface.Progress += OnProgress;
		}


		private void OnProgress(object sender, ProgressEventArgs a)
		{
			//Debug.Print($"Progress at {a.Value}");
			Form.UploadProgressbar.Invoke(new Action(() =>
			{
				Form.UploadProgressbar.PerformStep();
				Form.UploadProgressbar.Update();
			}));
		}
		#endregion ctors, dtor

		internal void InitialiseProgressbar()
		{
			//Form.UploadProgressbar.Properties.Step = 1;
			//Form.UploadProgressbar.Properties.PercentView = true;
			//Form.UploadProgressbar.Properties.Minimum = 0;
			//Form.UploadProgressbar.Properties.Maximum = 100;

			Form.UploadProgressbar.Step = 1;
			Form.UploadProgressbar.Minimum = 0;
			Form.UploadProgressbar.Maximum = 100;
			Form.UploadProgressbar.Style = ProgressBarStyle.Continuous;
		}

		internal void InitialiseProgressbar(int len)
		{
			//Form.UploadProgressbar.Properties.Step = 4096;
			//Form.UploadProgressbar.Properties.PercentView = true;
			//Form.UploadProgressbar.Properties.Minimum = 0;
			//Form.UploadProgressbar.Properties.Maximum = len;

			Form.UploadProgressbar.Step = 4096;
			Form.UploadProgressbar.Style = ProgressBarStyle.Continuous;
			Form.UploadProgressbar.Minimum = 0;
			Form.UploadProgressbar.Maximum = len;
		}

		internal void ShowProgressInForm(int val)
		{
			if (val < 0) val = 0;
			//if (val > Form.UploadProgressbar.Properties.Maximum) val = Form.UploadProgressbar.Properties.Maximum;
			if (val > Form.UploadProgressbar.Maximum) val = Form.UploadProgressbar.Maximum;

			//Form.UploadProgressbar.Position = val;
			Form.UploadProgressbar.Value = val;
			Form.UploadProgressbar.Update();
		}

		public void ShowResultInForm(string token)
		{
			if (string.IsNullOrEmpty(token)) token = "No results";
			Form.OutputEdt.Text = token.Replace("||", " \n");
		}
	}
}
