using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BimServerExchange.Forms;
using BimServerExchange.Runtime;
using Autodesk.Revit.UI;
using BimServerExchange.Objects;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Commands
{
	class CmdBimServerLogin
	{
		#region fields
		private BimServerLoginForm _form;
		private UIDocument _uidoc;
		//Dictionary<string, string> _projects = new Dictionary<string, string>();
		private KnownProjects _projects;
		#endregion fields

		#region properies
		internal BimServerLoginForm Form
		{
			get
			{
				if (null != _form) return _form;

				throw new IcnException("Property Form not set to a reference", 10, "CmdBimServerExchange");
			}
			private set { _form = value; }
		}

		internal UIDocument UIDoc
		{
			get
			{
				if (null != _uidoc) return _uidoc;

				throw new IcnException("Property UIDoc not set to a reference", 10, "CmdBimServerLogin");
			}
			private set { _uidoc = value; }
		}

		internal KnownProjects Projects
		{
			get
			{
				if (null != _projects) return _projects;

				_projects = new KnownProjects();
				return _projects;
			}
		}
		#endregion properties

		#region ctors, dtor
		internal CmdBimServerLogin(BimServerLoginForm form, UIDocument uidoc)
		{
			Form = form;
			UIDoc = uidoc;

			//if (form.SimpleForm) ReadProjectsXml();	// creates and fills the projects dict
		}
		#endregion ctors, dtor

		#region private methods
		private string GetProjectHierarchyName(SProject project, List<SProject> projects)
		{
			if (null == project)
				return string.Empty;

			string res = project.name;
			int id = project.parentId;

			while (id > 0)
			{
				int id1 = id;
				List<SProject> items = projects.Where(p => p.oid == id1).ToList();
				if (items.Count <= 0)
					break;

				SProject par = items[0];
				if (null == par)
					break;

				res = $"{par.name}.{res}";
				id = par.parentId;
			}

			return res;
		}
		#endregion private methods

		internal bool ValidateInput()
		{
			if (string.IsNullOrEmpty(Form.UrlEdt.Text)) return false;
			if (string.IsNullOrEmpty(Form.PortEdt.Text)) return false;
			if (string.IsNullOrEmpty(Form.LoginNameEdt.Text)) return false;
			if (string.IsNullOrEmpty(Form.PasswordEdt.Text)) return false;

			return true;
		}

		internal void WriteLoginSettings()
		{
			string token = Form.Data.Token;
			if (string.IsNullOrEmpty(token)) return;

			Form.Data.SaveToProperties(true);
			string path = UIDoc?.Document?.PathName ?? String.Empty;
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				if (Projects.ContainsKey(path)) Projects[path] = Form.Data.ProjectName;
				else Projects.Add(path, Form.Data.ProjectName);

				Projects.Save(true);
			}
		}

		internal void FillProjectsComboBox(List<SProject> projects)
		{
			//Form.ProjectsCbx.Properties.Items.Clear();
			Form.ProjectsCbx.Items.Clear();
			if (null == projects || projects.Count < -0) return;

			//Form.ProjectsCbx.BeginUpdate();
			foreach (SProject project in projects)
			{
				if (0 != string.Compare(project.state, "active", StringComparison.InvariantCultureIgnoreCase)) continue;

				string name = GetProjectHierarchyName(project, projects);
				if (string.IsNullOrEmpty(name)) continue;

				//Form.ProjectsCbx.Properties.Items.Add(name);
				Form.ProjectsCbx.Items.Add(name);
			}

			//Form.ProjectsCbx.EndUpdate();
		}

		internal void FillFormatsCombobox(List<string> formats, string def)
		{
			//Form.IFCFormatCbx.Properties.Items.Clear();
			Form.IFCFormatCbx.Items.Clear();
			if (null == formats || formats.Count < 0) return;

			foreach (string format in formats)
			{
				//Form.IFCFormatCbx.Properties.Items.Add(format);
				Form.IFCFormatCbx.Items.Add(format);
			}

			if (string.IsNullOrEmpty(def)) return;

			//for (int i = 0; i < Form.IFCFormatCbx.Properties.Items.Count; i++)
			for (int i=0; i< Form.IFCFormatCbx.Items.Count; i++)
			{
				//string name = Form.IFCFormatCbx.Properties.Items[i] as string;
				string name = Form.IFCFormatCbx.Items[i] as string;
				if (string.IsNullOrEmpty(name)) continue;

				if (0 != string.Compare(name, def, StringComparison.InvariantCultureIgnoreCase)) continue;

				Form.IFCFormatCbx.SelectedIndex = i;
				break;
			}
		}

		internal void Disconnect()
		{
			if (!Form.Connected) return;

			Form.Connected = false;
			if (string.IsNullOrEmpty(Form.Data.Token)) return;

			try
			{
				Form.Cmd.ServerInterface.Logout(Form.Data);
			}
			catch (IcnException iex)
			{
				iex.Display($"Could not disconnect from the BIMserver {Form.Data.Url}");
				Form.Cmd.ShowResult(iex.ToString());
			}
			catch (Exception ex)
			{
				Form.Cmd.ShowResult($"Could not disconnect from the BIMserver {Form.Data.Url}");
				Form.Cmd.ShowResult(ex.Message);
			}
			finally
			{
				Form.Data.Token = string.Empty;
				Form.Connected = false;
			}
		}

		internal void Connect()
		{
			if (Form.Connected) return;

			string token;
			try
			{
				token = Form.Cmd.ServerInterface.Login(Form.Data);
			}
			catch
			{
				token = string.Empty;
			}
			if (string.IsNullOrEmpty(token)) return;

			Form.Connected = true;
			Form.Data.Token = token;
		}

		internal void SelectDefaultProject(string projectName)
		{
			if (string.IsNullOrEmpty(projectName)) return;

			int idx = -1;
			//for (int i = 0; i < Form.ProjectsCbx.Properties.Items.Count; i++)
			for (int i = 0; i < Form.ProjectsCbx.Items.Count; i++)
			{
				//if (0 != string.Compare(projectName, Form.ProjectsCbx.Properties.Items[i] as string, StringComparison.InvariantCultureIgnoreCase)) continue;
				if (0 != string.Compare(projectName, Form.ProjectsCbx.Items[i] as string, StringComparison.InvariantCultureIgnoreCase)) continue;

				idx = i;
				break;
			}

			if (idx < 0) return;

			Form.ProjectsCbx.SelectedIndex = idx;
		}

		public void ActivateIfcConfiguration(bool set)
		{
			Form.ExportConfigurationLbl.Enabled = set;
			Form.ExportConfigurationsCbx.Enabled = set;
		}

		public void FillConfigurationCombobox(List<string> configurations)
		{
			//Form.ExportConfigurationsCbx.Properties.Items.Clear();
			Form.ExportConfigurationsCbx.Items.Clear();
			if (null == configurations || configurations.Count < 0) return;

			foreach (string name in configurations)
			{
				//Form.ExportConfigurationsCbx.Properties.Items.Add(name);
				Form.ExportConfigurationsCbx.Items.Add(name);
			}
		}

		internal void SetDefaultConfiguration(string configuration)
		{
			Form.ExportConfigurationsCbx.SelectedIndex = -1;
			if (string.IsNullOrEmpty(configuration)) return;

			//for (int idx = 0; idx < Form.ExportConfigurationsCbx.Properties.Items.Count; idx++)
			for (int idx = 0; idx < Form.ExportConfigurationsCbx.Items.Count; idx++)
			{
				//string key = Form.ExportConfigurationsCbx.Properties.Items[idx] as string;
				string key = Form.ExportConfigurationsCbx.Items[idx] as string;
				if (string.IsNullOrEmpty(key)) continue;

				if (string.Compare(key, configuration, StringComparison.InvariantCultureIgnoreCase) != 0) continue;

				Form.ExportConfigurationsCbx.SelectedIndex = idx;
				break;
			}
		}

		internal bool ConnectDataIncomplete()
		{
			if (string.IsNullOrEmpty(Form.UrlEdt.Text)) return true;
			if (string.IsNullOrEmpty(Form.PortEdt.Text)) return true;
			if (string.IsNullOrEmpty(Form.LoginNameEdt.Text)) return true;
			if (string.IsNullOrEmpty(Form.PasswordEdt.Text)) return true;

			return false;
		}
	}
}
