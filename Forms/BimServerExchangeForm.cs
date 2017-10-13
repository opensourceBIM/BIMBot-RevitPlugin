using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using BimServerExchange.Commands;
using BimServerExchange.Objects;
using BimServerExchange.Runtime;
using TextBox = System.Windows.Forms.TextBox;
using ComboBox = System.Windows.Forms.ComboBox;
using System.Diagnostics;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace BimServerExchange.Forms
{
	public partial class BimServerExchangeForm : Form
	{
		#region Declarations
		#region Fields, Constants
		private int _bitfields;
		private Commander _cmd;
		private BimServerExchangeData _data;
		private BimServerExchangeData _newProjectData;
		private IFCExporterAPI _exporter;
		private List<SProject> _projects;
		private List<SRevision> _revisions;
		private UIDocument _uidoc;
		private List<string> _bimServerApps;
		#endregion

		#region Properties, Indexers
		internal Commander Cmd
		{
			get
			{
				if (null != _cmd) return _cmd;

				throw new IcnException("Property Cmd no set", 10, "BimServerExchangeForm");
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

		internal BimServerExchangeData Data
		{
			get
			{
				if (null != _data) return _data;

				_data = new BimServerExchangeData();
				_data.LoadFromProperties(false);
				return _data;
			}
		}

		internal BimServerExchangeData NewProjectData
		{
			get
			{
				if (null != _newProjectData) return _newProjectData;

				_newProjectData = new BimServerExchangeData();
				_newProjectData.LoadFromProperties(false);
				return _newProjectData;
			}
		}

		internal IFCExporterAPI Exporter
		{
			get
			{
				if (null != _exporter) return _exporter;

				_exporter = new IFCExporterAPI(Cmd, UIDoc);
				return _exporter;
			}
			set
			{
				_exporter = value;
			}
		}

		internal List<SProject> Projects
		{
			get
			{
				if (null != _projects) return _projects;

				_projects = new List<SProject>();
				return _projects;
			}
		}

		internal List<SRevision> Revisions
		{
			get
			{
				if (null != _revisions) return _revisions;

				_revisions = new List<SRevision>();
				return _revisions;
			}
		}

		internal SProject SelectedProject
		{
			get
			{
				List<TreeNode> selection = GetSelectedNodes();
				if (null == selection || selection.Count <= 0) return null;

				return selection[0].Tag as SProject;
			}
		}

		internal string SelectedProjectLeaf
		{
			get
			{
				List<TreeNode> selection = GetSelectedNodes();
				if (null == selection || selection.Count <= 0) return string.Empty;

				TreeNode sel = selection[0];
				SProject data = sel.Tag as SProject;
				if (null == data) return string.Empty;

				string res = data.name;
				//TreeListNode par = sel.ParentNode;
				TreeNode par = sel.Parent;
				data = par.Tag as SProject;
				while (null != data && data.oid > 0)
				{
					res = data.name + "." + res;

					par = par.Parent;
					data = par.Tag as SProject;
					if (null == data) break;
				}

				return res;
			}
		}

		internal string SelectedProjectName
		{
			get
			{
				List<TreeNode> selection = GetSelectedNodes();
				if (null == selection || selection.Count <= 0) return string.Empty;

				return GetSelectionHierarchy(selection[0]);
			}
		}

		internal SRevision SelectedRevision
		{
			get
			{
				ListView.SelectedIndexCollection selection = RevisionsTree.SelectedIndices;
				if (selection.Count <= 0) return null;

				ListViewItem sel = RevisionsTree.Items[selection[0]];
				SRevision data = sel.Tag as SRevision;
				return data;
			}
		}

		internal UIDocument UIDoc
		{
			get
			{
				if (null != _uidoc) return _uidoc;

				throw new IcnException("Property UIDoc not set to a reference", 10, "BimServerExchangeForm");
			}
			set
			{
				if (null == value) throw new IcnException("Attempt to clear the value of Property UIDoc", 10, "BimServerExchangeForm");

				_uidoc = value;
			}
		}

		internal bool UseStandardRevitExporter
		{
			get { return Data.UseStandardRevitExporter; }
			set { Data.UseStandardRevitExporter = value; }
		}

		internal List<string> BimServerApps
		{
			get
			{
				if (null != _bimServerApps)
					return _bimServerApps;

				_bimServerApps = new List<string>();
				return _bimServerApps;
			}
		}
		#endregion
		#endregion

		#region Constructor & Initialize
		public BimServerExchangeForm(UIDocument uidoc)
		{
			InitializeComponent();
			InitialiseForm(uidoc);
		}

		private void InitialiseForm(UIDocument uidoc)
		{
			// initialisation order is critical. do not change
			UIDoc = uidoc;
			_cmd = new Commander(this, UIDoc);
			Exporter = new IFCExporterAPI(Cmd, UIDoc);
			// Data must already be initialised

			Cmd.BimServerExchange.InitialiseRevisionTree();

			List<string> formats = new List<string>(2) { "Ifc2x3tc1", "Ifc4" };
			// retry to select the ifc format now the required list has been set
			Cmd.BimServerExchange.FillFormatsCombobox(formats, "Ifc2x3tc1");
			//ProtocolRadio.SelectedIndex = Data.BimServerStreaming ? 0 : 1;
			StreamingRadio.Checked = Data.BimServerStreaming;
			SequentialRadio.Checked = !Data.BimServerStreaming;
		}

		internal void InitialiseProjects()
		{
			List<SProject> projects = Cmd.ServerInterface.GetAllProjects(Data);
			Cmd.BimServerExchange.FillTree(projects);
			//TreeListNode node = ProjectsTree.GetNodeByVisibleIndex(0);
			//if (null != node) ProjectsTree.FocusedNode = node;
		}
		#endregion

		#region application methods
		internal void ReportStatus(string status, bool reset)
		{
			if (reset)
			{
				Cmd.BimServerExchange.InitialiseProgressbar();
			}

			//if (int.TryParse(status, out int val))
			//{
			//	Cmd.BimServerExchange.SetProgress(val);
			//	return;
			//}

			Cmd.BimServerExchange.ShowResultInForm(status ?? string.Empty);
		}

		internal void PreselectProject(string projectname)
		{
			if (string.IsNullOrEmpty(projectname)) return;

			string fname = Path.GetFileNameWithoutExtension(projectname);
			if (string.IsNullOrEmpty(fname)) return;

			Cmd.BimServerExchange.SelectProjectInTree(fname);
		}
		#endregion application methods

		#region Private Methods
		private string GetSelectionHierarchy(TreeNode sel)
		{
			if (null == sel) return string.Empty;

			List<string> selection = new List<string>();
			SProject data = sel.Tag as SProject;
			if (null != data)
			{
				selection.Add(data.name);
			}

			sel = sel.Parent;
			while (null != sel)
			{
				data = sel.Tag as SProject;
				if (null != data && data.oid >= 0)
				{
					selection.Insert(0, data.name);
				}
				sel = sel.Parent;
			}

			if (selection.Count <= 0)
			{
				return string.Empty;
			}

			return string.Join(".", selection);
		}

		private List<TreeNode> GetSelectedNodes(TreeNodeCollection nodes = null)
		{
			if (null == nodes) nodes = ProjectsTree.Nodes;
			List<TreeNode> res = new List<TreeNode>();

			foreach (TreeNode node in nodes)
			{
				if (/*node.Checked || */node.IsSelected) res.Add(node);

				if (node.Nodes.Count <= 0) continue;

				List<TreeNode> subsel = GetSelectedNodes(node.Nodes);
				if (null != subsel && subsel.Count > 0) res.AddRange(subsel);
			}

			return res;
		}
		#endregion

		#region event handlers
		private void SimpleButton_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;
			if (null == btn)
			{
				return;
			}

			if (CancelBtn == btn)
			{
				// if we are still connected disconnect now
				if (Connected)
				{
					Cmd.ServerInterface.Logout(Data);
					Connected = false;
				}
				// then close the dialog normally
				DialogResult = DialogResult.OK;
				Close();
				return;
			}

			if (LoginBtn == btn)
			{
				if (Connected)
				{
					Cmd.ServerInterface.Logout(Data);
					Connected = false;
					UploadBtn.Enabled = false;
				}

				BimServerLoginForm form = new BimServerLoginForm(UIDoc, Data, false);
				if (DialogResult.OK != form.ShowDialog())
				{
					return;
				}

				Data.CopyFrom(form.Data);
				Cmd.ShowResult(Data.Token);
				Data.SaveToProperties(false); // save the settings in the properties so the next call will default with them
				UploadBtn.Enabled = true; // if we have a connection we can enable the action buttons (upload)
				Connected = true; // mark as connected
				return;
			}

			if (SaveBtn == btn)
			{
				string name = SelectedProjectName;
				if (string.IsNullOrEmpty(name)) return;

				Data.ProjectName = name;
				Cmd.BimServerExchange.SaveBimServerLink(name);

				// report to the user
				Cmd.BimServerExchange.ShowResultInForm($"This revit document is now linked to BIMserver project '{name}'");
				return;
			}

			if (SaveIfcBtn == btn)
			{
				string path = Cmd.BimServerExchange.GetCurrentProjectPath();
				if (string.IsNullOrEmpty(path) || !File.Exists(path))
				{
					return;
				}

				path = Cmd.BimServerExchange.AskForIfcSavePath(path);
				if (string.IsNullOrEmpty(path))
				{
					return;
				}

				Cmd.ShowResult(path);
				try
				{
					Cmd.BimServerExchange.ActivateButtons(false);
					Cmd.BimServerExchange.ExportProjectToIFC(path);
					IfcEdt.Text = path;
					Cmd.BimServerExchange.DoUpload();
				}
				catch (IcnException iex)
				{
					iex.Display("Exception in Export to IFC");
				}
				catch (Exception ex)
				{
					Cmd.ShowResult($"Export to IFC failed ({ex.Message})");
					Cmd.ShowResult(ex.Message);
				}
				finally
				{
					Cmd.BimServerExchange.ActivateButtons(true);
				}
				return;
			}

			if (LoadIfcBtn == btn)
			{
				string path = Cmd.BimServerExchange.AskForIfcUploadPath();
				if (string.IsNullOrEmpty(path) || !File.Exists(path))
				{
					return;
				}

				IfcEdt.Text = path;
				Cmd.BimServerExchange.DoUpload();
				return;
			}

			if (CopyProjectNameBtn == btn)
			{
				string name = UIDoc.Document.PathName;
				if (string.IsNullOrEmpty(name))
				{
					return;
				}

				string fname = Path.GetFileNameWithoutExtension(name);
				if (string.IsNullOrEmpty(fname))
				{
					return;
				}

				ProjectNameEdt.Text = fname;
				Cmd.BimServerExchange.SelectProjectInTree(fname);
				return;
			}

			if (ProjectAddBtn == btn)
			{
				if (string.IsNullOrEmpty(ProjectNameEdt.Text))
				{
					Cmd.ShowResult("The revit project does not have a name (it must be saved before running this tool)");
					return;
				}

				string ifcFormat = NewProjectData.IfcFormat;
				if (ifcFormat.StartsWith("ifc2x3", StringComparison.InvariantCultureIgnoreCase)) ifcFormat = "ifc2x3tc1";
				else ifcFormat = "ifc4";
				string fullname = Cmd.BimServerExchange.AddProject(SelectedProject, ProjectNameEdt.Text, ProjectDescEdt.Text, ifcFormat);
				if (string.IsNullOrEmpty(fullname))
				{
					Cmd.ShowResult("Could not add a new project");
					return;
				}

				UploadBtn.Enabled = true;
				// reset the projects list
				Projects.Clear();
				Projects.AddRange(Cmd.ServerInterface.GetAllProjects(Data));
				return;
			}

			if (UploadBtn == btn)
			{
				Cmd.BimServerExchange.DoUpload();
				return;
			}

			if (CancelUploadBtn == btn)
			{
				Cmd.BimServerExchange.ActivateButtons(true);
				Cmd.ServerInterface.CancelDownload();
				return;
			}

			// the exact structure of the IFC download request/response is currently undetermined,
			// we can't get the filename
			if (DownloadBtn == btn)
			{
				// see if there is a project up and running
				try
				{
					SProject proj = SelectedProject;
					if (null == proj) throw new IcnException("No project selected", 10, "Download");

					SRevision file = SelectedRevision;
					if (null == file) throw new IcnException("No IFC file selected", 10, "Download");

					string scheme = proj.schema;
					if (string.IsNullOrEmpty(scheme) || !scheme.StartsWith("Ifc", StringComparison.InvariantCultureIgnoreCase))
					{
						Cmd.ShowResult($"Unknown IFC scheme '{scheme ?? string.Empty}'. Attemping to download with 'Ifc2x3tc1 (Streaming)'");
						scheme = "Ifc2x3tc1 (Streaming)";
					}
					SSerializer serialiser = Cmd.ServerInterface.GetSerialiser(Data, proj, scheme);
					//SSerializer serialiser = Cmd.ServerInterface.GetSerialiser(Data, proj, "json");
					if (null == serialiser) throw new IcnException($"Serializer '{scheme}' not found in project '{SelectedProjectName}'", 10, "Download");

					// construct a default pathname out of the current revit project location and the BIMserver project name
					string path = UIDoc.Document.PathName;
					if (!string.IsNullOrEmpty(path)) path = Path.GetDirectoryName(path);

					path = Cmd.BimServerExchange.AskForIfcDownloadFilename(path, proj.name + ".ifc");
					if (string.IsNullOrEmpty(path)) throw new IcnException($"No path supplied to download to", 10, "Download");

					if (File.Exists(path))
					{
						string backup = Path.ChangeExtension(path, "bak");
						if (File.Exists(backup))
						{
							File.Delete(backup);
						}
						File.Move(path, backup);
					}

					// we may have linked files. And we likely will need additional controls (e.g. for the Project to upload to) and additional data to go with this
					Cmd.ServerInterface.DownloadFile(Data, path, file.oid, serialiser.oid);
					Cmd.ShowResult($"IFC file '{path}' is copied form the BIMsrver");
				}
				catch (IcnException iex)
				{
					iex.Display(@"Exception in Download IFC file");
					Cmd.ShowResult(iex.ToString());
				}
				catch (Exception ex)
				{
					Cmd.ShowResult($"Download IFC file failed ({ex.Message})");
					Cmd.ShowResult(ex.Message);
				}

				return;
			}

			throw new IcnException($"SimpleButton '{btn.Name}' activated that has no code", 20, "BimServerExchangeForm");
		}

		private void TextEdit_Changed(object sender, EventArgs e)
		{
			TextBox edt = sender as TextBox;
			if (null == edt) return;

			if (ProjectNameEdt == edt)
			{
				// we need to verify that we are not trying to create an existing project
				// deleted projects are special cases as they can be undeleted instead but for now the GUI does not allow to delete and does not need to undelete
				string name = edt.Text;
				ProjectAddBtn.Enabled = Cmd.BimServerExchange.CheckIfProjectnameIsUnique(name);
				return;
			}

			throw new IcnException($"TextEdit '{edt.Name}' activated that has no code", 20, "BimServerExchangeForm");
		}

		private void TreeView_Select(object sender, TreeViewEventArgs e)
		{
			TreeView tree = sender as TreeView;
			if (null == tree) return;

			if (ProjectsTree == tree)
			{
				// if the treelist selection changed it may be that the name becomes (in)eligible for adding
				string name = ProjectNameEdt.Text;
				ProjectAddBtn.Enabled = !string.IsNullOrEmpty(name) && Cmd.BimServerExchange.CheckIfProjectnameIsUnique(name);

				name = SelectedProjectName;
				bool enable = !string.IsNullOrEmpty(name) && 0 != string.Compare(name, "BIMserver Projects", StringComparison.InvariantCultureIgnoreCase);
				UploadBtn.Enabled = enable;
				if (enable)
				{
					SProject project = Cmd.ServerInterface.GetProject(Data, name);
					List<SRevision> list = null;

					try
					{
						list = Cmd.ServerInterface.GetAllFilesOfProject(Data, project);
					}
					catch (IcnException iex)
					{
						Cmd.BimServerExchange.ShowResultInForm(iex.ToString());
					}
					catch (Exception ex)
					{
						Cmd.BimServerExchange.ShowResultInForm($"Generic Error in selecting a project ({ex.Message})");
					}
					finally
					{
						if (null == list) list = new List<SRevision>();
					}

					Cmd.BimServerExchange.FillRevisionTree(list);
					if (RevisionsTree.Items.Count > 0)
					{
						RevisionsTree.Items[0].Selected = true;
						RevisionsTree.Items[0].Checked = true;
					}
					DownloadBtn.Enabled = true;
				}
				else
				{
					Cmd.BimServerExchange.FillRevisionTree(null);
					DownloadBtn.Enabled = false;
				}
				RevisionsGrp.Text = $@"Revisions in project {SelectedProjectLeaf}";

				return;
			}

			throw new IcnException($"TreeView '{tree.Name}' activated that has no code", 20, "BimServerExchangeForm");
		}

		private void TreeView_MouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			TreeView tree = sender as TreeView;
			if (null == tree) return;
			if (e.Button != MouseButtons.Right) return;

			if (ProjectsTree == tree)
			{
				TreeNode node = tree.GetNodeAt(e.X, e.Y);
				if (null == node) return;

				SProject project = node.Tag as SProject;
				if (null == project) return;

				int poid = project.oid;
				contextMenuStrip1.Items.Clear();

				ToolStripMenuItem item = new ToolStripMenuItem("Go to Bimviews");
				item.Click += MenuItem_Click;
				item.Tag = string.Format($"apps/bimviews/?page=Project&poid={poid}");
				contextMenuStrip1.Items.Add(item);

				foreach (string name in BimServerApps)
				{
					ToolStripMenuItem mi = new ToolStripMenuItem(name);
					mi.Click += MenuItem_Click;
					mi.Tag = string.Format($"apps/{name}/?page=Project&poid={poid}");
					contextMenuStrip1.Items.Add(item);
				}
				contextMenuStrip1.Show(tree, e.Location);

				return;
			}

			throw new IcnException($"TreeView '{tree.Name}' activated that has no code", 20, "BimServerExchangeForm");
		}

		private void MenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem mi = sender as ToolStripMenuItem;
			if (null == mi) return;

			string tag = mi.Tag as string;
			if (string.IsNullOrEmpty(tag)) return;

			string url = $"{Data.Url}:{Data.Port}/{tag}";
			Process.Start(url);
		}

		/*
		private void TreeList_SelectionChanged(object sender, FocusedNodeChangedEventArgs e)
		{
			ListView tree = sender as ListView;
			if (null == tree)
			{
				return;
			}

			if (RevisionsTree == tree)
			{
				//TreeListNode sel = RevisionsTree.Selection[0];
				//DownloadBtn.Enabled = null != sel;
				return;
			}

			throw new IcnException($"TreeList '{tree.Name}' activated that has no code", 20, "BimServerSelectProjectForm");
		}
		*/

		private void ComboBox_SelectionChanged(object sender, EventArgs e)
		{
			ComboBox cbx = sender as ComboBox;
			if (null == cbx) return;

			if (IFCFormatCbx == cbx)
			{
				int idx = cbx.SelectedIndex;
				if (idx < 0 || idx >= cbx.Items.Count) return;

				string val = cbx.Items[idx] as string;
				if (string.IsNullOrEmpty(val)) return;

				//if (ProtocolRadio.SelectedIndex == 1) val += " (streaming)";
				if (StreamingRadio.Checked) val += " (streaming)";
				NewProjectData.IfcFormat = val;

				return;
			}

			throw new IcnException($"ComboBox '{cbx.Name}' activated that has no code", 20, "BimServerExchangeForm");
		}

		private void RadioGroup_SelectionChanged(object sender, EventArgs e)
		{
			RadioButton radio = sender as RadioButton;

			if (null == radio) return;

			if (StreamingRadio == radio)
			{
				int idx = IFCFormatCbx.SelectedIndex;
				if (idx < 0 || idx >= IFCFormatCbx.Items.Count) return;

				string val = IFCFormatCbx.Items[idx] as string;
				if (string.IsNullOrEmpty(val)) return;

				bool useStreaming = radio.Checked;
				NewProjectData.BimServerStreaming = useStreaming;

				if (SequentialRadio.Checked == useStreaming) SequentialRadio.Checked = !useStreaming;
				if (useStreaming) val += " (streaming)";
				NewProjectData.IfcFormat = val;

				return;
			}

			if (StreamingRadio == radio)
			{
				int idx = IFCFormatCbx.SelectedIndex;
				if (idx < 0 || idx >= IFCFormatCbx.Items.Count) return;

				string val = IFCFormatCbx.Items[idx] as string;
				if (string.IsNullOrEmpty(val)) return;

				bool useStreaming = !radio.Checked;
				NewProjectData.BimServerStreaming = useStreaming;

				if (StreamingRadio.Checked == useStreaming) StreamingRadio.Checked = !useStreaming;
				if (useStreaming) val += " (streaming)";
				NewProjectData.IfcFormat = val;

				return;
			}

			throw new IcnException($"RadioGroup '{radio.Name}' activated that has no code", 20, "BimServerExchangeForm");
		}
		#endregion
	}
}