using System;
using System.IO;
using BimServerExchange.Forms;
using BimServerExchange.Runtime;
using Autodesk.Revit.UI;
using BimServerExchange.Objects;
using System.Diagnostics;
using System.Windows.Forms;

namespace BimServerExchange.Commands
{
	class CmdBimServerExport
	{
		#region fields
		private BimServerExportForm _form;
		private UIDocument _uidoc;
		#endregion fields

		#region properies
		internal BimServerExportForm Form
		{
			get
			{
				if (null != _form)
					return _form;

				throw new IcnException("Property Form not set to a reference", 10, "CmdBimServerExport");
			}
			private set { _form = value; }
		}

		private UIDocument UIDoc
		{
			get
			{
				if (null != _uidoc)
					return _uidoc;

				throw new IcnException("Property UIDoc not set to a reference", 10, "CmdBimServerExport");
			}
			set { _uidoc = value; }
		}
		#endregion properties

		#region ctors, dtor
		internal CmdBimServerExport(BimServerExportForm form, UIDocument uidoc)
		{
			Form = form;
			UIDoc = uidoc;
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
				//while(Form.UploadProgressbar.Position < a.Value) Form.UploadProgressbar.PerformStep();
				// input validation
				//if (a.Value < Form.UploadProgressbar.Properties.Minimum) a.Value = Form.UploadProgressbar.Properties.Minimum;
				//if (a.Value > Form.UploadProgressbar.Properties.Maximum) a.Value = Form.UploadProgressbar.Properties.Maximum;
				if (a.Value < Form.UploadProgressbar.Minimum) a.Value = Form.UploadProgressbar.Minimum;
				if (a.Value > Form.UploadProgressbar.Maximum) a.Value = Form.UploadProgressbar.Maximum;
				// actually set the progress state and force an update of the control to display it
				//Form.UploadProgressbar.Position = a.Value;
				Form.UploadProgressbar.Value = a.Value;
				Form.UploadProgressbar.Update();
			}));
		}


		internal bool CheckLogin()
		{
			Form.Data.LoadFromProperties(false);
			// Project is required but it is checked separately as it does not impact the login per se (only the upload and view processes)
			//if (string.IsNullOrEmpty(Form.Data.ProjectName)) return false;

			string token;
			try
			{
				token = Form.Cmd.ServerInterface.Login(Form.Data);
			}
			catch (IcnException iex)
			{
				//MessageBox.Show($@"{iex.CallerData} - {iex.Message}", @"BIMserver Login");
				return false;
			}

			if (string.IsNullOrEmpty(token)) return false;

			Form.Data.Token = token;

			/*
			 * this option has been removed from the config form by request from TNO.
			 * we can no longer require it for the connect (as we can't fix this problem from that form)
			// a login to the quickexport also requires that a project is selected
			if (string.IsNullOrEmpty(Form.Data.ProjectName)) return false;

			if (null == kp) kp = new KnownProjects();
			if (!kp.ContainsKey(UIDoc.Document.PathName ?? string.Empty)) return false;
			*/

			return true;
		}

		internal bool CheckProject(KnownProjects kp)
		{
			string path = UIDoc?.Document?.PathName;
			if (string.IsNullOrEmpty(path) || !File.Exists(path)) return false;

			if (kp.ContainsKey(path))
			{
				string item = kp[path];
				if (string.IsNullOrEmpty(item)) return false;

				Form.Data.ProjectName = item;
				return true;
			}

			return false;
		}
		#endregion ctors, dtor

		#region private methods
		private bool IfcFormatsMatch(string fileIfcFormat, string projectIfcFormat)
		{
			if (string.IsNullOrEmpty(fileIfcFormat) || string.IsNullOrEmpty(projectIfcFormat)) return false;

			if (0 == String.Compare(fileIfcFormat, "IFC2X3", StringComparison.InvariantCultureIgnoreCase))
			{
				if (projectIfcFormat.IndexOf(fileIfcFormat, StringComparison.InvariantCultureIgnoreCase) == 0) return true;
			}

			if (0 == String.Compare(fileIfcFormat, "IFC 2X3", StringComparison.InvariantCultureIgnoreCase))
			{
				if (projectIfcFormat.IndexOf("IFC2X3", StringComparison.InvariantCultureIgnoreCase) == 0) return true;
			}

			if (0 == String.Compare(fileIfcFormat, "IFC4", StringComparison.InvariantCultureIgnoreCase))
			{
				if (projectIfcFormat.IndexOf(fileIfcFormat, StringComparison.InvariantCultureIgnoreCase) == 0) return true;
			}

			if (0 == String.Compare(fileIfcFormat, "IFC 4", StringComparison.InvariantCultureIgnoreCase))
			{
				if (projectIfcFormat.IndexOf("IFC4", StringComparison.InvariantCultureIgnoreCase) == 0) return true;
			}

			return false;
		}
		#endregion private methods

		internal void InitialiseProgressbar()
		{
			//Form.UploadProgressbar.Properties.Step = 1;
			//Form.UploadProgressbar.Properties.PercentView = true;
			//Form.UploadProgressbar.Properties.Minimum = 0;
			//Form.UploadProgressbar.Properties.Maximum = 100;

			Form.UploadProgressbar.Step = 1;
			Form.UploadProgressbar.Style = ProgressBarStyle.Continuous;
			Form.UploadProgressbar.Minimum = 0;
			Form.UploadProgressbar.Maximum = 100;
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

		internal string ExportProjectToIFC(string path)
		{
			if (string.IsNullOrEmpty(path)) return string.Empty;

			if (Form.UseStandardRevitExporter)
			{
				string dir = Path.GetDirectoryName(path);
				if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

				Form.Cmd.IfcExporter.Exporter.SavePath = path;
				if (File.Exists(path))
				{
					string bak = Path.ChangeExtension(path, "bak");
					if (File.Exists(bak)) File.Delete(bak);
					File.Copy(path, bak);
					File.Delete(path);
				}
				Form.Cmd.IfcExporter.Exporter.Run();
				return path;
			}

			// for the external IFC exporter path is a directory that contains the ifc files
			if (path.EndsWith(".ifc", StringComparison.InvariantCultureIgnoreCase))
			{
				path = Path.GetDirectoryName(path);
			}
			Form.Cmd.IfcExporter.Exporter.SavePath = path;
			Form.Cmd.IfcExporter.Exporter.Run();

			// we return the actual filename of the IFC file (which gets nested in a directory with the samae name)
			string fname = Path.GetFileNameWithoutExtension(UIDoc.Document.PathName);
			if (string.IsNullOrEmpty(fname)) return path;

			return Path.Combine(path ?? string.Empty, fname + ".ifc");
		}

		internal void QuickCheckinFile(string path, string comment)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path))
			{
				throw new IcnException("No IFC file selected to export", 10, "QuickExport");
			}

			if (string.IsNullOrEmpty(comment))
			{
				comment = Path.GetFileName(path);
			}

			// get the projectId (from the project stored in Properties.Settings.Default.BimServerProject!)
			SProject project = Form.Cmd.ServerInterface.GetProject(Form.Data, Form.Data.ProjectName);
			if (null == project)
			{
				throw new IcnException($"Project '{Form.Data.ProjectName}' is not found on the BIMserver", 10, "QuickExport");
			}

			// get the deserializerId (from the setting of the project)
			string fileIfcFormat = GetIFCFormatOfFile(path); // this may contain the " (streaming)" suffix as it is meant to look up the deserializer
			string projectIfcFormat = project.schema; // this is the actual IFC schema used in this project
			string fname = Path.GetFileNameWithoutExtension(path);
			if (string.IsNullOrEmpty(fname)) fname = path;
			if (!IfcFormatsMatch(fileIfcFormat, projectIfcFormat))
			{
				throw new IcnException($"Could not copy '{fname}' to the BIMserver. The IFC format '{fileIfcFormat}' differs from the project format '{projectIfcFormat}'", 10, "QuickExport");
			}

			SDeserializer deserialiser = Form.Cmd.ServerInterface.GetDeserialiser(Form.Data, project.oid, projectIfcFormat);
			if (null == deserialiser)
			{
				throw new IcnException($"Serializer '{projectIfcFormat}' is unknown in project '{Form.Data.ProjectName}'", 10, "QuickExport");
			}

			// do the actual export
			string result;
			int topicId = Form.Cmd.ServerInterface.CheckinFile(Form.Data, path, comment, project.oid, deserialiser.oid, true, out result);
			if (topicId < 0)
			{
				// result is either cancelled or failed
				if (result.IndexOf("failed", StringComparison.InvariantCultureIgnoreCase) > 0)
				{
					throw new IcnException($"Project '{fname}' is not copied to the BIMserver", 10, "QuickExport");
				}

				ShowResultInForm(result);
				return;
			}

			// What we really want to do here is start a new modeless form and have that report progress while we close this form
			// when the modeless form completes it can close and show this form again. This will probably mess up the commanders that
			// are running. And we also need to create keyboard shortcuts for the exporter as we will have to trigger the command (not just the form)
			// The better option is to run the exporter in a modeless dialog, then we can keep it up and running (but will need to guard many of its
			// commander methods against it being destroyed) and use it for output just as we do now

			// we can't cancel the processing by the BIMserver so disable that option
			ActivateCancelButton(false);

			// then run the actual monitoring process. This will make the form (and revit) block for potentially a very long time
			topicId = Form.Cmd.ServerInterface.MonitorProcessingState(Form.Data, topicId, out result);

			if (Form.Cancelling)
			{
				return;
			}

			if (0 != string.Compare(result, "OK", StringComparison.InvariantCultureIgnoreCase) &&
				 0 != string.Compare(result, "Project is uploaded to the BIMserver", StringComparison.InvariantCultureIgnoreCase))
			{
				throw new IcnException($"Project '{fname}' is not copied to the BIMserver ({result})", 10, "QuickExport");
			}

			// normally we have the (final) result here stating that the upload was successful
			ShowResultInForm(result);
		}

		private string GetIFCFormatOfFile(string path)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path))
				return "Undefined";

			using (StreamReader reader = new StreamReader(path))
			{
				string line;
				while (null != (line = reader.ReadLine()))
				{
					if (string.IsNullOrEmpty(line))
						continue;
					if (!line.StartsWith("FILE_SCHEMA", StringComparison.InvariantCultureIgnoreCase))
						continue;

					int st = line.IndexOf("'", StringComparison.InvariantCultureIgnoreCase);
					if (st < 0)
						continue;

					int en = line.IndexOf("'", st + 1, StringComparison.InvariantCultureIgnoreCase);
					if (en < st)
						continue;

					string res = line.Substring(st + 1, en - st - 1);
					if (!string.IsNullOrEmpty(res))
						return res;
				}

				reader.Close();
			}

			return "Undefined";
		}

		internal void DeleteTemporaryFiles(string path)
		{
			if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;

			if (Form.UseStandardRevitExporter)
			{
				File.Delete(path);
				return;
			}

			string fdir = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(fdir) || !Directory.Exists(fdir)) return;
			Directory.Delete(fdir, true);

			// export goes to a folder, there is not automatically a subfolder with the same name
			//string fname = Path.GetFileNameWithoutExtension(path);
			//string leaf = Path.GetFileName(fdir);

			//if (0 == string.Compare(fname, leaf, StringComparison.InvariantCultureIgnoreCase))
			//{
			//	Directory.Delete(fdir);
			//	return;
			//}

			//File.Delete(path);
		}

		internal void ActivateButtons(bool set)
		{
			Form.ButtonPnl.Enabled = set;
			Form.CancelBtn.Enabled = !set;
		}

		internal void ActivateCancelButton(bool set)
		{
			Form.CancelBtn.Enabled = set;
		}

		internal void ShowProgressInForm(int val)
		{
			Form.UploadProgressbar.Invoke(new Action(() =>
			{
				if (val < 0) val = 0;
				//if (val > Form.UploadProgressbar.Properties.Maximum) val = Form.UploadProgressbar.Properties.Maximum;
				if (val > Form.UploadProgressbar.Maximum) val = Form.UploadProgressbar.Maximum;

				//Form.UploadProgressbar.Position = val;
				Form.UploadProgressbar.Value = val;
				Form.UploadProgressbar.Update();
			}));
		}

		internal void ShowResultInForm(string token)
		{
			if (string.IsNullOrEmpty(token)) token = "No results";

			if (Form.OutputEdt.InvokeRequired)
			{
				Form.Invoke(new Action(() =>
				{
					Form.OutputEdt.Text = token.Replace("||", " \n");
				}));
				return;
			}

			Form.OutputEdt.Text = token.Replace("||", " \n");
		}

		internal void OpenView()
		{
			Form.Handler.Command = BimServerExportEventHandler.Commands.kOpenProject;
			Form.Event.Raise();
		}

		internal void ExportEvent()
		{
			string path = null;

			try
			{
				// we do not show the form, rather we use its commander directly
				string fname = Path.GetFileNameWithoutExtension(UIDoc.Document.PathName) ?? "output";
				Guid guid = Guid.NewGuid();
				path = Path.Combine(Path.GetTempPath(), guid.ToString("D"), fname + ".ifc");

				ActivateButtons(false);
				ActivateCancelButton(false);
				path = ExportProjectToIFC(path);
				ActivateCancelButton(true);

				if (!File.Exists(path))
				{
					Form.ReportStatus("Project could not be exported to IFC", false);
					ActivateButtons(true);
					DeleteTemporaryFiles(path);
					return;
				}

				QuickCheckinFile(path, Form.Comment);

			}
			catch (IcnException iex)
			{
				iex.Display("Checkin Project");
			}
			catch (Exception ex)
			{
				ShowResultInForm($"Checkin Project failed ({ex.Message})");
			}
			finally
			{
				ActivateButtons(true);
				if (!string.IsNullOrEmpty(path)) DeleteTemporaryFiles(path);
			}

		}

		internal void OpenProjectEvent()
		{
			SProject project = Form.Cmd.ServerInterface.GetProject(Form.Data, Form.Data.ProjectName);
			if (null == project) return;
			int poid = project.oid;
			string tag = $"apps/bimviews/?page=Project&poid={poid}";
			string url = $"{Form.Data.Url}:{Form.Data.Port}/{tag}";
			Process.Start(url);
		}

		internal void LoginEvent()
		{
			if (Form.Connected)
			{
				Form.Cmd.ServerInterface.Logout(Form.Data);
				Form.Connected = false;
				Form.ExportBtn.Enabled = false;
			}

			BimServerLoginForm form = new BimServerLoginForm(UIDoc, Form.Data);
			if (DialogResult.OK != form.ShowDialog())
			{
				return;
			}

			Form.Data.CopyFrom(form.Data);
			Form.Cmd.ShowResult("Successfully connected to the BIMserver" /*Data.Token*/);
			Form.ExportBtn.Enabled = true; // if we have a connection we can enable the action buttons (upload)
			Form.Connected = true; // mark as connected
		}

		internal void StopReportingProgress()
		{
			Form.Cmd.ServerInterface.ShowProgress = false;
			Form.Cmd.ServerInterface.StopMonitoringProgress();
		}
	}
}
