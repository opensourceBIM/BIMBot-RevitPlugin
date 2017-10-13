using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using BimServerExchange.Forms;
using BimServerExchange.Objects;
using BimServerExchange.Runtime;
// ReSharper disable NotAccessedField.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

namespace BimServerExchange.Commands
{
	class CmdBimServerExchange
	{
		#region fields
		private BimServerExchangeForm _form;
		private UIDocument _uidoc;
		private CancellationTokenSource _ctSource;
		#endregion fields

		#region properies
		internal BimServerExchangeForm Form
		{
			get
			{
				if (null != _form) return _form;

				throw new IcnException("Property Form not set to a reference", 10, "CmdBimServerExchange");
			}
			private set { _form = value; }
		}

		private UIDocument UIDoc
		{
			get
			{
				if (null != _uidoc) return _uidoc;

				throw new IcnException("Property UIDoc not set to a reference", 10, "CmdBimServerExchange");
			}
			set { _uidoc = value; }
		}
		#endregion properties

		#region ctors, dtor
		internal CmdBimServerExchange(BimServerExchangeForm form, UIDocument uidoc)
		{
			Form = form;
			UIDoc = uidoc;
		}

		internal void Init()
		{
			Form.Cmd.ServerInterface.Progress += OnProgress;
		}

		internal bool CheckLogin()
		{
			Form.Data.LoadFromProperties(false);
			string token;

			try
			{
				token = Form.Cmd.ServerInterface.Login(Form.Data);
			}
			catch (IcnException iex)
			{
#if DEBUG
				MessageBox.Show($@"{iex.CallerData} - {iex.Message}", @"BIMServer Login");
#endif
				return false;
			}

			if (string.IsNullOrEmpty(token)) return false;

			Form.Data.Token = token;
			return true;
		}
		#endregion ctors, dtor

		#region private methods
		private List<SProject> GetChildProjectsOf(SProject par)
		{
			if (null == par) return null;

			List<SProject> res = new List<SProject>();
			// get the full list, including the deleted ones (Form.Projects only contains the active projects so it can't be used here)
			List<SProject> projects = Form.Cmd.ServerInterface.GetAllProjects(Form.Data, true);
			foreach (SProject proj in projects)
			{
				if (proj.parentId != par.oid) continue;

				res.Add(proj);
			}

			return res;
		}

		private void HideInternalColumns()
		{
			/*
			foreach (TreeListColumn col in Form.ProjectsTree.Columns)
			{
				if (0 == string.Compare(col.FieldName, "name", StringComparison.InvariantCultureIgnoreCase))
				{
					col.Caption = @"Name";
					col.VisibleIndex = 0;
					col.Width = 200;
					continue;
				}

				if (0 == string.Compare(col.FieldName, "state", StringComparison.InvariantCultureIgnoreCase))
				{
					col.Caption = @"State";
					col.VisibleIndex = 1;
					col.Width = 50;
					col.Fixed = FixedStyle.Right;
					// setting FixedWidth to true will prevent the column from being resized. If for some reason 50 pixels isn't wide
					// enough for the text then it can't be adjusted. By switching it off the column can be resized, but it will resize
					// when the entire control gets wider or more narrow too.
					//col.OptionsColumn.FixedWidth = true;
					continue;
				}

				col.VisibleIndex = -1;
			}
			*/
		}

		private void HideInternalRevisionColumns()
		{
			/*
			foreach (TreeListColumn col in Form.RevisionsTree.Columns)
			{
				if (0 == string.Compare(col.FieldName, "id", StringComparison.InvariantCultureIgnoreCase))
				{
					col.Caption = @"Revision";
					col.VisibleIndex = 0;
					col.Width = 80;
					col.Fixed = FixedStyle.Left;
					col.OptionsColumn.FixedWidth = true;
					continue;
				}

				if (0 == string.Compare(col.FieldName, "Datum", StringComparison.InvariantCultureIgnoreCase))
				{
					col.Caption = @"Date";
					col.VisibleIndex = 1;
					col.Width = 120;
					col.OptionsColumn.FixedWidth = true;
					continue;
				}

				if (0 == string.Compare(col.FieldName, "Desc", StringComparison.InvariantCultureIgnoreCase))
				{
					col.Caption = @"Description";
					col.VisibleIndex = 3;
					col.Width = 200;
					continue;
				}

				col.VisibleIndex = -1;
			}
			*/
		}

		private void ClearSelection(TreeNodeCollection nodes = null)
		{
			if (null == nodes)
			{
				nodes = Form.ProjectsTree.Nodes;
				Form.ProjectsTree.SelectedNode = null;
			}

			foreach (TreeNode node in nodes)
			{
				node.Checked = false;

				if (node.Nodes.Count > 0) ClearSelection(node.Nodes);
			}
		}

		private TreeNode FindNodeByValue(string fname, TreeNodeCollection nodes = null)
		{
			if (string.IsNullOrEmpty(fname)) return null;

			if (null == nodes) nodes = Form.ProjectsTree.Nodes;
			foreach (TreeNode node in nodes)
			{
				SProject project = node.Tag as SProject;
				if (null != project && 0 == string.Compare(fname, project.name, StringComparison.InvariantCultureIgnoreCase)) return node;

				if (node.Nodes.Count > 0)
				{
					TreeNode res = FindNodeByValue(fname, node.Nodes);
					if (null != res) return res;
				}
			}

			return null;
		}

		private void FillSubTree(TreeNode root, List<SProject> projects)
		{
			if (null == projects || projects.Count <= 0) return;

			List<SProject> subprojects = GetProjectsFromListWithParent(projects, root);
			if (null == subprojects || subprojects.Count <= 0) return;

			foreach (SProject project in subprojects)
			{
				TreeNode node = root.Nodes.Add(project.oid.ToString(), project.name);
				node.Tag = project;
				FillSubTree(node, projects);
			}
		}

		private List<SProject> GetProjectsFromListWithParent(List<SProject> projects, TreeNode root)
		{
			if (null == root) return null;
			if (!int.TryParse(root.Name, out int key)) return null;

			List<SProject> res = new List<SProject>();

			foreach (SProject project in projects)
			{
				if (project.oid < 0) continue; // skip the root project
				if (key != project.parentId) continue;

				res.Add(project);
			}

			return res;
		}
		#endregion private methods

		public void ShowResultInForm(string token)
		{
			if (string.IsNullOrEmpty(token)) token = "No results";
			Form.OutputEdt.Text = token.Replace("||", " \n");
		}

		internal string GetCurrentProjectPath()
		{
			// if the document already has a name
			string res = UIDoc.Document.PathName;
			if (!string.IsNullOrEmpty(res) && File.Exists(res))
			{
				// if the document is not changed from the last save we don't need to save it again
				// if the document is readonly it can't be modified (or saved) so use it as is
				if (!UIDoc.Document.IsModified || UIDoc.Document.IsReadOnly) return res;
				// otherwise try to save the document first so the last state will be exported to IFC
				UIDoc.Document.Save();
				return res;
			}

			// project is not yet saved
			DialogResult opt = MessageBox.Show(@"The project must first be saved", @"BIMserver Exchange", MessageBoxButtons.YesNo);
			if (DialogResult.No == opt) return string.Empty;

			UIDoc.Document.Save();
			res = UIDoc.Document.PathName;
			return res;
		}

		internal string AskForIfcSavePath(string path)
		{
			// if no path is given try to use the current location of the active revit project
			if (string.IsNullOrEmpty(path)) path = Form.UIDoc?.Document?.PathName;
			if (string.IsNullOrEmpty(path))
			{
				// if there is no current revit document we shouldn't be able to arrive here, but go with a default path that at least exists
				path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "output.ifc");
			}

			// standard IFC Exporter shipped with Revit creates a file, so we need to use the SaveFileDialog
			if (Form.UseStandardRevitExporter)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.InitialDirectory = Path.GetDirectoryName(path) ?? string.Empty;
				dlg.FileName = Path.GetFileName(path);
				if (string.IsNullOrEmpty(dlg.FileName)) dlg.FileName = @"outpu.ifc";
				dlg.CheckFileExists = false;
				dlg.CheckPathExists = false;
				dlg.OverwritePrompt = true;
				dlg.AddExtension = true;
				dlg.RestoreDirectory = true;
				dlg.DefaultExt = "ifc";
				dlg.Filter = @"IFC (*.ifc)|*.ifc|All files (*.*)| *.*";
				dlg.FilterIndex = 1;

				if (DialogResult.OK != dlg.ShowDialog()) return string.Empty;

				return dlg.FileName;
			}

			// the addin produces much better IFC export, but creates a directory
			FolderBrowserDialog dialog = new FolderBrowserDialog();

			dialog.Description = @"Select a folder to write the IFC file to";
			dialog.SelectedPath = path;
			dialog.ShowNewFolderButton = true;
			if (DialogResult.OK != dialog.ShowDialog()) return null;

			return Path.Combine(dialog.SelectedPath, Path.GetFileNameWithoutExtension(path));
		}

		internal string AskForIfcDownloadFilename(string fpath, string fname)
		{
			if (string.IsNullOrEmpty(fpath))
			{
				fpath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			}
			if (string.IsNullOrEmpty(fname))
			{
				fname = "output.ifc";
			}

			SaveFileDialog dlg = new SaveFileDialog();

			dlg.InitialDirectory = fpath;
			dlg.FileName = fname;
			dlg.CheckFileExists = false;
			dlg.CheckPathExists = true;
			dlg.OverwritePrompt = true;
			dlg.AddExtension = true;
			dlg.RestoreDirectory = true;
			dlg.DefaultExt = "ifc";
			dlg.Filter = @"IFC (*.ifc)|*.ifc|All files (*.*)| *.*";
			dlg.FilterIndex = 1;

			if (DialogResult.OK != dlg.ShowDialog()) return string.Empty;

			return dlg.FileName;
		}

		internal string AskForIfcUploadPath()
		{
			string path = Form.UIDoc.Document.PathName;
			if (!string.IsNullOrEmpty(path) && File.Exists(path)) path = Path.ChangeExtension(path, "ifc");
			if (string.IsNullOrEmpty(path)) path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "output.ifc");

			OpenFileDialog dlg = new OpenFileDialog();

			dlg.Title = @"Select an IFC file to copy to the BimServer";
			dlg.InitialDirectory = Path.GetDirectoryName(path) ?? string.Empty;
			dlg.FileName = Path.GetFileName(path);
			if (string.IsNullOrEmpty(dlg.FileName)) dlg.FileName = "output.ifc";
			dlg.CheckFileExists = true;
			dlg.CheckPathExists = true;
			dlg.AddExtension = true;
			dlg.RestoreDirectory = true;
			dlg.DefaultExt = "ifc";
			dlg.Filter = @"IFC (*.ifc)|*.ifc|All files (*.*)| *.*";
			dlg.FilterIndex = 1;

			if (DialogResult.OK != dlg.ShowDialog()) return string.Empty;

			return dlg.FileName;
		}

		internal void ExportProjectToIFC(string path)
		{
			if (string.IsNullOrEmpty(path)) return;

			_ctSource = new CancellationTokenSource();
			if (Form.UseStandardRevitExporter)
			{
				string dir = Path.GetDirectoryName(path);
				if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

				Form.Exporter.SavePath = path;
				if (File.Exists(path))
				{
					string bak = Path.ChangeExtension(path, "bak");
					if (File.Exists(bak)) File.Delete(bak);
					File.Copy(path, bak);
					File.Delete(path);
				}
				Form.Exporter.Run();
				_ctSource = null;
				return;
			}

			// for the external IFC exporter path is a directory that contains the ifc files
			Form.Exporter.SavePath = path;
			Form.Exporter.Run();
			_ctSource = null;
		}

		internal void InitialiseProgressbar()
		{
			//Form.UploadProgressbar.Properties.Step = 1;
			//Form.UploadProgressbar.Properties.PercentView = true;
			//Form.UploadProgressbar.Properties.Minimum = 0;
			//Form.UploadProgressbar.Properties.Maximum = 100;
			//Form.UploadProgressbar.Position = 0;

			Form.UploadProgressbar.Step = 1;
			Form.UploadProgressbar.Style = ProgressBarStyle.Continuous;
			Form.UploadProgressbar.Minimum = 0;
			Form.UploadProgressbar.Maximum = 100;
			Form.UploadProgressbar.Value = 0;
		}

		internal void InitialiseProgressbar(int len)
		{
			//Form.UploadProgressbar.Properties.Step = 4096;
			//Form.UploadProgressbar.Properties.PercentView = true;
			//Form.UploadProgressbar.Properties.Minimum = 0;
			//Form.UploadProgressbar.Properties.Maximum = len;
			//Form.UploadProgressbar.Position = 0;

			Form.UploadProgressbar.Step = 4096;
			Form.UploadProgressbar.Style = ProgressBarStyle.Continuous;
			Form.UploadProgressbar.Minimum = 0;
			Form.UploadProgressbar.Maximum = len;
			Form.UploadProgressbar.Value = 0;
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

		internal bool CheckIfProjectnameIsUnique(string name)
		{
			if (string.IsNullOrEmpty(name)) return false;

			SProject par = Form.SelectedProject;
			if (null == par) return false;

			List<SProject> projects = GetChildProjectsOf(par);
			if (null == projects || projects.Count <= 0) return true;

			foreach (SProject proj in projects)
			{
				if (0 == string.Compare(proj.name, name, StringComparison.InvariantCultureIgnoreCase)) return false;
			}

			return true;
		}

		internal string AddProject(SProject parent, string projectName, string projectDesc, string ifcFormat)
		{
			if (null == parent || string.IsNullOrEmpty(projectName)) return null;

			if (null == projectDesc) projectDesc = string.Empty;
			if (string.IsNullOrEmpty(ifcFormat)) ifcFormat = "ifc2x3tc1";

			SProject res;
			if (parent.oid < 0)
			{
				res = Form.Cmd.ServerInterface.AddRootProject(Form.Data, projectName, projectDesc, ifcFormat);
			}
			else
			{
				res = Form.Cmd.ServerInterface.AddProjectTo(Form.Data, projectName, projectDesc, parent, ifcFormat);
			}
			List<SProject> projects = Form.Cmd.ServerInterface.GetAllProjects(Form.Data);
			FillTree(projects);

			//Form.ProjectsTree.FocusedNode = Form.ProjectsTree.FindNodeByID(res.oid);
			return Form.SelectedProjectName;
		}

		internal void FillTree(List<SProject> projects)
		{
			Form.Projects.Clear();
			//Form.ProjectsTree.DataSource = null;

			if (null == projects || projects.Count <= 0) return;

			Form.Projects.AddRange(projects);

			TreeNode root = Form.ProjectsTree.Nodes.Add("-1", "BIMserver");
			FillSubTree(root, projects);

			// after creating the datasource we have a lot of columns that we need to hide
			HideInternalColumns();
			//Form.ProjectsTree.ExpandAll();
			root.ExpandAll();
		}

		internal void InitialiseRevisionTree()
		{
			Form.RevisionsTree.Columns.Add("id", "Revision", 80, HorizontalAlignment.Left, -1);
			Form.RevisionsTree.Columns.Add("Datum", "Data", 120);
			Form.RevisionsTree.Columns.Add("Desc", "Description", 200);
		}

		internal void FillRevisionTree(List<SRevision> list)
		{
			Form.Revisions.Clear();
			Form.RevisionsTree.Items.Clear();

			if (null == list || list.Count <= 0) return;

			Form.Revisions.AddRange(list);

			foreach (SRevision item in list)
			{
				string[]  values = new string[3];

				values[0] = item.id.ToString();
				values[1] = item.Datum;
				values[2] = item.Desc;

				ListViewItem li = new ListViewItem(values);
				li.Tag = item;

				Form.RevisionsTree.Items.Add(li);
			}

			HideInternalRevisionColumns();
		}

		internal void SelectProjectInTree(string fname)
		{
			ClearSelection();
			if (string.IsNullOrEmpty(fname)) return;
			if (Form.ProjectsTree.Nodes.Count <= 0) return;

			TreeNode sel = FindNodeByValue(fname);
			if (null != sel)
			{
				//sel.Checked = true;
				Form.ProjectsTree.SelectedNode = sel;
				Form.ProjectsTree.Select();					// this is required to make .net highlight the selected line
			}
		}

		internal bool ValidateAndAcceptFilename(string projectName, string path)
		{
			// invalid input can not be validated
			if (string.IsNullOrEmpty(projectName)) return false;
			if (string.IsNullOrEmpty(path) || !File.Exists(path)) return false;

			string[] projects = projectName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
			string pname = projects[projects.Length - 1];
			string fname = Path.GetFileNameWithoutExtension(path);
			if (string.IsNullOrEmpty(pname) || string.IsNullOrEmpty(fname)) return false;

			if (0 == string.Compare(fname, pname, StringComparison.InvariantCultureIgnoreCase)) return true;

			// ask the user if the file should be uploaded to a project with a different name
			ConfirmationForm form = new ConfirmationForm($"The filename '{fname}'|does not match|the project name '{pname}'||Do you want to upload anyway?");
			if (DialogResult.No == form.ShowDialog()) return false;

			return true;
		}

		internal string GetFileTag(string path)
		{
			// handle edge cases
			if (string.IsNullOrEmpty(path))
			{
				return Form.TagEdt.Text;
			}

			if (string.IsNullOrEmpty(Form.TagEdt.Text))
			{
				return Path.GetFileName(path);
			}

			string tag = $"{Form.TagEdt.Text} - {Path.GetFileName(path)}";
			return tag;
		}

		public void ActivateButtons(bool set)
		{
			Form.ButtonPnl.Enabled = set;
			Form.ProjectsPnl.Enabled = set;
			Form.RevisionsGrp.Enabled = set;
			Form.LoadIfcBtn.Enabled = set;
			Form.SaveIfcBtn.Enabled = set;
			Form.UploadBtn.Enabled = set;

			Form.CancelUploadBtn.Enabled = !set;
		}

		public void SetProgress(int val)
		{
			if (val < 0) val = 0;
			if (val > Form.UploadProgressbar.Maximum) val = Form.UploadProgressbar.Maximum;

			Form.UploadProgressbar.Value = val;
			Form.UploadProgressbar.Update();
		}

		public void DoUpload()
		{
			// see if there is a project up and running
			try
			{
				string projectName = Form.SelectedProjectName;
				SProject proj = Form.Cmd.ServerInterface.GetProject(Form.Data, projectName);
				if (null == proj)
				{
					throw new IcnException("No project selected", 10, "Upload");
				}

				string schema = proj.schema;
				if (string.IsNullOrEmpty(schema)) schema = "Ifc2x3tc1";
				SDeserializer deserialiser = Form.Cmd.ServerInterface.GetDeserialiser(Form.Data, proj.oid, schema);
				if (null == deserialiser)
				{
					throw new IcnException($"Serializer '{schema}' not found in project '{projectName}'", 10, "Upload");
				}

				string path = Form.IfcEdt.Text;
				if (string.IsNullOrEmpty(path) || !File.Exists(path))
				{
					path = Form.Cmd.BimServerExchange.AskForIfcUploadPath();
				}
				if (string.IsNullOrEmpty(path) || !File.Exists(path))
				{
					throw new IcnException($"IFC file '{path ?? string.Empty}' not found", 10, "Upload");
				}
				// this checks if the file and project name are the same and asks the user to confirm if they are not
				if (!Form.Cmd.BimServerExchange.ValidateAndAcceptFilename(projectName, path))
				{
					return;
				}

				// we may have linked files. And we likely will need additional controls (e.g. for the Project to upload to) and additional data to go with this
				string tag = Form.Cmd.BimServerExchange.GetFileTag(path);
				string result;
				Form.Cmd.BimServerExchange.ActivateButtons(false);
				int fileId = Form.Cmd.ServerInterface.CheckinFile(Form.Data, path, tag ?? string.Empty, proj.oid, deserialiser.oid, true, out result);
				if (fileId > 0)
				{
					Form.CancelUploadBtn.Enabled = false;
					fileId = Form.Cmd.ServerInterface.MonitorProcessingState(Form.Data, fileId, out result);
				}

				Form.Cmd.ShowResult($"Copy IFC file '{path}' to the BIMserver\nResult: '{result}'");

				List<SRevision> revisions = Form.Cmd.ServerInterface.GetAllFilesOfProject(Form.Data, proj);
				Form.Cmd.BimServerExchange.FillRevisionTree(revisions);
			}
			catch (IcnException iex)
			{
				iex.Display(@"Exception in Upload IFC file");
				Form.Cmd.ShowResult(iex.ToString());
			}
			catch (Exception ex)
			{
				Form.Cmd.ShowResult($"Select/add project failed ({ex.Message})");
				Form.Cmd.ShowResult(ex.Message);
			}
			finally
			{
				Form.Cmd.BimServerExchange.ActivateButtons(true);
			}
		}

		public void SaveBimServerLink(string name)
		{
			if (string.IsNullOrEmpty(name)) return;

			Form.Data.SaveToProperties(true);
			// link the revit project to the bimserver project in the config file
			string path = UIDoc?.Document?.PathName ?? String.Empty;
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				KnownProjects projects = new KnownProjects();
				if (projects.ContainsKey(path)) projects[path] = Form.Data.ProjectName;
				else projects.Add(path, Form.Data.ProjectName);

				projects.Save(true);
			}

		}

		internal void FillFormatsCombobox(List<string> formats, string def)
		{
			Form.IFCFormatCbx.Items.Clear();
			if (null == formats || formats.Count < 0) return;

			foreach (string format in formats)
			{
				Form.IFCFormatCbx.Items.Add(format);
			}

			if (string.IsNullOrEmpty(def)) return;

			for (int i = 0; i < Form.IFCFormatCbx.Items.Count; i++)
			{
				string name = Form.IFCFormatCbx.Items[i] as string;
				if (string.IsNullOrEmpty(name)) continue;

				if (0 != string.Compare(name, def, StringComparison.InvariantCultureIgnoreCase)) continue;

				Form.IFCFormatCbx.SelectedIndex = i;
				break;
			}
		}
	}
}
