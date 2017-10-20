using System.Drawing;
using System.Windows.Forms;

namespace BimServerExchange.Forms
{
	partial class BimServerExchangeForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BimServerExchangeForm));
			this.ButtonPnl = new System.Windows.Forms.Panel();
			this.SaveBtn = new System.Windows.Forms.Button();
			this.DownloadBtn = new System.Windows.Forms.Button();
			this.LoginBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.OutputPnl = new System.Windows.Forms.Panel();
			this.OutputEdt = new System.Windows.Forms.TextBox();
			this.UploadProgressbar = new System.Windows.Forms.ProgressBar();
			this.UploadBtn = new System.Windows.Forms.Button();
			this.IfcEdt = new System.Windows.Forms.TextBox();
			this.SaveIfcBtn = new System.Windows.Forms.Button();
			this.ExportPnl = new System.Windows.Forms.Panel();
			this.UploadGrp = new System.Windows.Forms.GroupBox();
			this.CancelUploadBtn = new System.Windows.Forms.Button();
			this.IfcDescLbl = new System.Windows.Forms.Label();
			this.IfcNameLbl = new System.Windows.Forms.Label();
			this.LoadIfcBtn = new System.Windows.Forms.Button();
			this.TagEdt = new System.Windows.Forms.TextBox();
			this.ProjectsPnl = new System.Windows.Forms.Panel();
			this.ProjectsGrp = new System.Windows.Forms.GroupBox();
			this.ProjectsTree = new System.Windows.Forms.TreeView();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.CopyProjectNameBtn = new System.Windows.Forms.Button();
			this.ProjectDescLbl = new System.Windows.Forms.Label();
			this.ProjectNameLbl = new System.Windows.Forms.Label();
			this.ProjectDescEdt = new System.Windows.Forms.TextBox();
			this.ProjectNameEdt = new System.Windows.Forms.TextBox();
			this.ProjectAddBtn = new System.Windows.Forms.Button();
			this.MainSplit = new System.Windows.Forms.SplitContainer();
			this.AddProjectGrp = new System.Windows.Forms.GroupBox();
			this.IFCSettingsPnl = new System.Windows.Forms.Panel();
			this.StreamingRadio = new System.Windows.Forms.RadioButton();
			this.SequentialRadio = new System.Windows.Forms.RadioButton();
			this.IFCFormatCbx = new System.Windows.Forms.ComboBox();
			this.IFCFormatLbl = new System.Windows.Forms.Label();
			this.RevisionsGrp = new System.Windows.Forms.GroupBox();
			this.RevisionsTree = new System.Windows.Forms.ListView();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.ButtonPnl.SuspendLayout();
			this.OutputPnl.SuspendLayout();
			this.ExportPnl.SuspendLayout();
			this.UploadGrp.SuspendLayout();
			this.ProjectsPnl.SuspendLayout();
			this.ProjectsGrp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MainSplit)).BeginInit();
			this.MainSplit.Panel1.SuspendLayout();
			this.MainSplit.Panel2.SuspendLayout();
			this.MainSplit.SuspendLayout();
			this.AddProjectGrp.SuspendLayout();
			this.IFCSettingsPnl.SuspendLayout();
			this.RevisionsGrp.SuspendLayout();
			this.SuspendLayout();
			// 
			// ButtonPnl
			// 
			this.ButtonPnl.Controls.Add(this.SaveBtn);
			this.ButtonPnl.Controls.Add(this.DownloadBtn);
			this.ButtonPnl.Controls.Add(this.LoginBtn);
			this.ButtonPnl.Controls.Add(this.CancelBtn);
			this.ButtonPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ButtonPnl.Location = new System.Drawing.Point(0, 501);
			this.ButtonPnl.Name = "ButtonPnl";
			this.ButtonPnl.Size = new System.Drawing.Size(884, 31);
			this.ButtonPnl.TabIndex = 0;
			// 
			// SaveBtn
			// 
			this.SaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.SaveBtn.Location = new System.Drawing.Point(715, 3);
			this.SaveBtn.Name = "SaveBtn";
			this.SaveBtn.Size = new System.Drawing.Size(75, 23);
			this.SaveBtn.TabIndex = 5;
			this.SaveBtn.Text = "Save";
			this.SaveBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// DownloadBtn
			// 
			this.DownloadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.DownloadBtn.Enabled = false;
			this.DownloadBtn.Location = new System.Drawing.Point(486, 3);
			this.DownloadBtn.Name = "DownloadBtn";
			this.DownloadBtn.Size = new System.Drawing.Size(75, 23);
			this.DownloadBtn.TabIndex = 4;
			this.DownloadBtn.Text = "Download";
			this.DownloadBtn.Visible = false;
			this.DownloadBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// LoginBtn
			// 
			this.LoginBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.LoginBtn.Enabled = false;
			this.LoginBtn.FlatAppearance.BorderSize = 0;
			this.LoginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.LoginBtn.Image = global::BimServerExchange.Properties.Resources.ButtonImage_Config;
			this.LoginBtn.Location = new System.Drawing.Point(3, 3);
			this.LoginBtn.Name = "LoginBtn";
			this.LoginBtn.Size = new System.Drawing.Size(24, 24);
			this.LoginBtn.TabIndex = 3;
			this.LoginBtn.Text = "+";
			this.toolTip1.SetToolTip(this.LoginBtn, "Configure the connection to the BIMserver");
			this.LoginBtn.Visible = false;
			this.LoginBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelBtn.Location = new System.Drawing.Point(796, 3);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 23);
			this.CancelBtn.TabIndex = 0;
			this.CancelBtn.Text = "Close";
			this.CancelBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// OutputPnl
			// 
			this.OutputPnl.Controls.Add(this.OutputEdt);
			this.OutputPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.OutputPnl.Location = new System.Drawing.Point(0, 431);
			this.OutputPnl.Name = "OutputPnl";
			this.OutputPnl.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.OutputPnl.Size = new System.Drawing.Size(510, 70);
			this.OutputPnl.TabIndex = 2;
			// 
			// OutputEdt
			// 
			this.OutputEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OutputEdt.Location = new System.Drawing.Point(6, 8);
			this.OutputEdt.Multiline = true;
			this.OutputEdt.Name = "OutputEdt";
			this.OutputEdt.ReadOnly = true;
			this.OutputEdt.Size = new System.Drawing.Size(492, 59);
			this.OutputEdt.TabIndex = 0;
			// 
			// UploadProgressbar
			// 
			this.UploadProgressbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.UploadProgressbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(140)))), ((int)(((byte)(180)))));
			this.UploadProgressbar.Location = new System.Drawing.Point(73, 83);
			this.UploadProgressbar.Name = "UploadProgressbar";
			this.UploadProgressbar.Size = new System.Drawing.Size(342, 18);
			this.UploadProgressbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.UploadProgressbar.TabIndex = 14;
			// 
			// UploadBtn
			// 
			this.UploadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.UploadBtn.Enabled = false;
			this.UploadBtn.Location = new System.Drawing.Point(442, 81);
			this.UploadBtn.Name = "UploadBtn";
			this.UploadBtn.Size = new System.Drawing.Size(54, 23);
			this.UploadBtn.TabIndex = 10;
			this.UploadBtn.Text = "Upload";
			this.UploadBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// IfcEdt
			// 
			this.IfcEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IfcEdt.Location = new System.Drawing.Point(73, 30);
			this.IfcEdt.Name = "IfcEdt";
			this.IfcEdt.Size = new System.Drawing.Size(363, 20);
			this.IfcEdt.TabIndex = 11;
			// 
			// SaveIfcBtn
			// 
			this.SaveIfcBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SaveIfcBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.SaveIfcBtn.Image = global::BimServerExchange.Properties.Resources.ButtonImage_IFC;
			this.SaveIfcBtn.Location = new System.Drawing.Point(472, 28);
			this.SaveIfcBtn.Name = "SaveIfcBtn";
			this.SaveIfcBtn.Size = new System.Drawing.Size(24, 24);
			this.SaveIfcBtn.TabIndex = 9;
			this.SaveIfcBtn.Text = "E";
			this.SaveIfcBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// ExportPnl
			// 
			this.ExportPnl.Controls.Add(this.UploadGrp);
			this.ExportPnl.Enabled = false;
			this.ExportPnl.Location = new System.Drawing.Point(0, 0);
			this.ExportPnl.Name = "ExportPnl";
			this.ExportPnl.Size = new System.Drawing.Size(509, 110);
			this.ExportPnl.TabIndex = 12;
			this.ExportPnl.Visible = false;
			// 
			// UploadGrp
			// 
			this.UploadGrp.Controls.Add(this.CancelUploadBtn);
			this.UploadGrp.Controls.Add(this.IfcDescLbl);
			this.UploadGrp.Controls.Add(this.IfcNameLbl);
			this.UploadGrp.Controls.Add(this.SaveIfcBtn);
			this.UploadGrp.Controls.Add(this.IfcEdt);
			this.UploadGrp.Controls.Add(this.UploadBtn);
			this.UploadGrp.Controls.Add(this.UploadProgressbar);
			this.UploadGrp.Controls.Add(this.LoadIfcBtn);
			this.UploadGrp.Controls.Add(this.TagEdt);
			this.UploadGrp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.UploadGrp.Location = new System.Drawing.Point(0, 0);
			this.UploadGrp.Name = "UploadGrp";
			this.UploadGrp.Size = new System.Drawing.Size(509, 110);
			this.UploadGrp.TabIndex = 17;
			this.UploadGrp.TabStop = false;
			this.UploadGrp.Text = "Export to IFC and Upload to project";
			// 
			// CancelUploadBtn
			// 
			this.CancelUploadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelUploadBtn.Enabled = false;
			this.CancelUploadBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.CancelUploadBtn.Image = global::BimServerExchange.Properties.Resources.ButtonImage_Cancel;
			this.CancelUploadBtn.Location = new System.Drawing.Point(420, 84);
			this.CancelUploadBtn.Name = "CancelUploadBtn";
			this.CancelUploadBtn.Size = new System.Drawing.Size(16, 16);
			this.CancelUploadBtn.TabIndex = 19;
			this.CancelUploadBtn.Text = "o";
			this.CancelUploadBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// IfcDescLbl
			// 
			this.IfcDescLbl.Location = new System.Drawing.Point(6, 57);
			this.IfcDescLbl.Name = "IfcDescLbl";
			this.IfcDescLbl.Size = new System.Drawing.Size(53, 13);
			this.IfcDescLbl.TabIndex = 18;
			this.IfcDescLbl.Text = "Description";
			// 
			// IfcNameLbl
			// 
			this.IfcNameLbl.Location = new System.Drawing.Point(6, 33);
			this.IfcNameLbl.Name = "IfcNameLbl";
			this.IfcNameLbl.Size = new System.Drawing.Size(27, 13);
			this.IfcNameLbl.TabIndex = 17;
			this.IfcNameLbl.Text = "Name";
			// 
			// LoadIfcBtn
			// 
			this.LoadIfcBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LoadIfcBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.LoadIfcBtn.Image = global::BimServerExchange.Properties.Resources.ButtonImage_Open;
			this.LoadIfcBtn.Location = new System.Drawing.Point(442, 28);
			this.LoadIfcBtn.Name = "LoadIfcBtn";
			this.LoadIfcBtn.Size = new System.Drawing.Size(24, 24);
			this.LoadIfcBtn.TabIndex = 15;
			this.LoadIfcBtn.Text = "o";
			this.LoadIfcBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// TagEdt
			// 
			this.TagEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TagEdt.Location = new System.Drawing.Point(73, 54);
			this.TagEdt.Name = "TagEdt";
			this.TagEdt.Size = new System.Drawing.Size(423, 20);
			this.TagEdt.TabIndex = 16;
			// 
			// ProjectsPnl
			// 
			this.ProjectsPnl.Controls.Add(this.ProjectsGrp);
			this.ProjectsPnl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProjectsPnl.Location = new System.Drawing.Point(0, 0);
			this.ProjectsPnl.MinimumSize = new System.Drawing.Size(350, 250);
			this.ProjectsPnl.Name = "ProjectsPnl";
			this.ProjectsPnl.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.ProjectsPnl.Size = new System.Drawing.Size(370, 393);
			this.ProjectsPnl.TabIndex = 5;
			// 
			// ProjectsGrp
			// 
			this.ProjectsGrp.Controls.Add(this.ProjectsTree);
			this.ProjectsGrp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProjectsGrp.Location = new System.Drawing.Point(0, 0);
			this.ProjectsGrp.Name = "ProjectsGrp";
			this.ProjectsGrp.Padding = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.ProjectsGrp.Size = new System.Drawing.Size(367, 393);
			this.ProjectsGrp.TabIndex = 0;
			this.ProjectsGrp.TabStop = false;
			this.ProjectsGrp.Text = "Projects";
			// 
			// ProjectsTree
			// 
			this.ProjectsTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ProjectsTree.ContextMenuStrip = this.contextMenuStrip1;
			this.ProjectsTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProjectsTree.FullRowSelect = true;
			this.ProjectsTree.HideSelection = false;
			this.ProjectsTree.Location = new System.Drawing.Point(3, 19);
			this.ProjectsTree.Name = "ProjectsTree";
			this.ProjectsTree.Size = new System.Drawing.Size(361, 371);
			this.ProjectsTree.TabIndex = 1;
			this.ProjectsTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Select);
			this.ProjectsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Select);
			this.ProjectsTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_MouseClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// CopyProjectNameBtn
			// 
			this.CopyProjectNameBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CopyProjectNameBtn.FlatAppearance.BorderSize = 0;
			this.CopyProjectNameBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.CopyProjectNameBtn.Image = global::BimServerExchange.Properties.Resources.ButtonImage_Revit;
			this.CopyProjectNameBtn.Location = new System.Drawing.Point(315, 21);
			this.CopyProjectNameBtn.Name = "CopyProjectNameBtn";
			this.CopyProjectNameBtn.Size = new System.Drawing.Size(24, 24);
			this.CopyProjectNameBtn.TabIndex = 8;
			this.CopyProjectNameBtn.Text = "+";
			this.toolTip1.SetToolTip(this.CopyProjectNameBtn, "Read the name of the current Revit Project and set it as the BIMserver project");
			this.CopyProjectNameBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// ProjectDescLbl
			// 
			this.ProjectDescLbl.Location = new System.Drawing.Point(5, 50);
			this.ProjectDescLbl.Name = "ProjectDescLbl";
			this.ProjectDescLbl.Size = new System.Drawing.Size(63, 13);
			this.ProjectDescLbl.TabIndex = 7;
			this.ProjectDescLbl.Text = "Description";
			// 
			// ProjectNameLbl
			// 
			this.ProjectNameLbl.Location = new System.Drawing.Point(5, 26);
			this.ProjectNameLbl.Name = "ProjectNameLbl";
			this.ProjectNameLbl.Size = new System.Drawing.Size(60, 13);
			this.ProjectNameLbl.TabIndex = 6;
			this.ProjectNameLbl.Text = "Name";
			// 
			// ProjectDescEdt
			// 
			this.ProjectDescEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ProjectDescEdt.Location = new System.Drawing.Point(81, 47);
			this.ProjectDescEdt.Name = "ProjectDescEdt";
			this.ProjectDescEdt.Size = new System.Drawing.Size(282, 20);
			this.ProjectDescEdt.TabIndex = 5;
			// 
			// ProjectNameEdt
			// 
			this.ProjectNameEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ProjectNameEdt.Location = new System.Drawing.Point(81, 23);
			this.ProjectNameEdt.Name = "ProjectNameEdt";
			this.ProjectNameEdt.Size = new System.Drawing.Size(229, 20);
			this.ProjectNameEdt.TabIndex = 4;
			this.ProjectNameEdt.TextChanged += new System.EventHandler(this.TextEdit_Changed);
			// 
			// ProjectAddBtn
			// 
			this.ProjectAddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ProjectAddBtn.Enabled = false;
			this.ProjectAddBtn.FlatAppearance.BorderSize = 0;
			this.ProjectAddBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ProjectAddBtn.Image = global::BimServerExchange.Properties.Resources.ButtonImage_Add;
			this.ProjectAddBtn.Location = new System.Drawing.Point(339, 21);
			this.ProjectAddBtn.Name = "ProjectAddBtn";
			this.ProjectAddBtn.Size = new System.Drawing.Size(24, 24);
			this.ProjectAddBtn.TabIndex = 3;
			this.ProjectAddBtn.Text = "+";
			this.toolTip1.SetToolTip(this.ProjectAddBtn, "Create a new BIMserver project with the given name");
			this.ProjectAddBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// MainSplit
			// 
			this.MainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainSplit.Location = new System.Drawing.Point(0, 0);
			this.MainSplit.Name = "MainSplit";
			// 
			// MainSplit.Panel1
			// 
			this.MainSplit.Panel1.Controls.Add(this.ProjectsPnl);
			this.MainSplit.Panel1.Controls.Add(this.AddProjectGrp);
			this.MainSplit.Panel1.Text = "Panel1";
			// 
			// MainSplit.Panel2
			// 
			this.MainSplit.Panel2.Controls.Add(this.RevisionsGrp);
			this.MainSplit.Panel2.Controls.Add(this.OutputPnl);
			this.MainSplit.Panel2.Controls.Add(this.ExportPnl);
			this.MainSplit.Panel2.Text = "Panel2";
			this.MainSplit.Size = new System.Drawing.Size(884, 501);
			this.MainSplit.SplitterDistance = 370;
			this.MainSplit.TabIndex = 5;
			this.MainSplit.Text = "splitContainerControl1";
			// 
			// AddProjectGrp
			// 
			this.AddProjectGrp.Controls.Add(this.IFCSettingsPnl);
			this.AddProjectGrp.Controls.Add(this.CopyProjectNameBtn);
			this.AddProjectGrp.Controls.Add(this.ProjectNameEdt);
			this.AddProjectGrp.Controls.Add(this.ProjectDescLbl);
			this.AddProjectGrp.Controls.Add(this.ProjectAddBtn);
			this.AddProjectGrp.Controls.Add(this.ProjectNameLbl);
			this.AddProjectGrp.Controls.Add(this.ProjectDescEdt);
			this.AddProjectGrp.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.AddProjectGrp.Location = new System.Drawing.Point(0, 393);
			this.AddProjectGrp.Name = "AddProjectGrp";
			this.AddProjectGrp.Size = new System.Drawing.Size(370, 108);
			this.AddProjectGrp.TabIndex = 2;
			this.AddProjectGrp.TabStop = false;
			this.AddProjectGrp.Text = "Add Project";
			// 
			// IFCSettingsPnl
			// 
			this.IFCSettingsPnl.Controls.Add(this.StreamingRadio);
			this.IFCSettingsPnl.Controls.Add(this.SequentialRadio);
			this.IFCSettingsPnl.Controls.Add(this.IFCFormatCbx);
			this.IFCSettingsPnl.Controls.Add(this.IFCFormatLbl);
			this.IFCSettingsPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.IFCSettingsPnl.Location = new System.Drawing.Point(3, 75);
			this.IFCSettingsPnl.Name = "IFCSettingsPnl";
			this.IFCSettingsPnl.Size = new System.Drawing.Size(364, 30);
			this.IFCSettingsPnl.TabIndex = 17;
			// 
			// StreamingRadio
			// 
			this.StreamingRadio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.StreamingRadio.BackColor = System.Drawing.Color.Transparent;
			this.StreamingRadio.Location = new System.Drawing.Point(207, 7);
			this.StreamingRadio.Name = "StreamingRadio";
			this.StreamingRadio.Size = new System.Drawing.Size(76, 20);
			this.StreamingRadio.TabIndex = 7;
			this.StreamingRadio.Text = "Streaming";
			this.toolTip1.SetToolTip(this.StreamingRadio, "Internal processing format of IFC files on the BIMserver");
			this.StreamingRadio.UseVisualStyleBackColor = false;
			this.StreamingRadio.CheckedChanged += new System.EventHandler(this.RadioGroup_SelectionChanged);
			// 
			// SequentialRadio
			// 
			this.SequentialRadio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SequentialRadio.BackColor = System.Drawing.Color.Transparent;
			this.SequentialRadio.Location = new System.Drawing.Point(289, 7);
			this.SequentialRadio.Name = "SequentialRadio";
			this.SequentialRadio.Size = new System.Drawing.Size(68, 20);
			this.SequentialRadio.TabIndex = 8;
			this.SequentialRadio.Text = "Classic";
			this.toolTip1.SetToolTip(this.SequentialRadio, "Internal processing format of IFC files on the BIMserver");
			this.SequentialRadio.UseVisualStyleBackColor = false;
			this.SequentialRadio.CheckedChanged += new System.EventHandler(this.RadioGroup_SelectionChanged);
			// 
			// IFCFormatCbx
			// 
			this.IFCFormatCbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IFCFormatCbx.Location = new System.Drawing.Point(78, 7);
			this.IFCFormatCbx.Name = "IFCFormatCbx";
			this.IFCFormatCbx.Size = new System.Drawing.Size(123, 21);
			this.IFCFormatCbx.TabIndex = 6;
			this.toolTip1.SetToolTip(this.IFCFormatCbx, "IFC format used do serialize and deserialize IFC files on th BIMserver");
			this.IFCFormatCbx.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectionChanged);
			// 
			// IFCFormatLbl
			// 
			this.IFCFormatLbl.Location = new System.Drawing.Point(5, 10);
			this.IFCFormatLbl.Name = "IFCFormatLbl";
			this.IFCFormatLbl.Size = new System.Drawing.Size(67, 13);
			this.IFCFormatLbl.TabIndex = 17;
			this.IFCFormatLbl.Text = "IFC Format";
			// 
			// RevisionsGrp
			// 
			this.RevisionsGrp.Controls.Add(this.RevisionsTree);
			this.RevisionsGrp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RevisionsGrp.Location = new System.Drawing.Point(0, 0);
			this.RevisionsGrp.Name = "RevisionsGrp";
			this.RevisionsGrp.Padding = new System.Windows.Forms.Padding(3, 0, 0, 10);
			this.RevisionsGrp.Size = new System.Drawing.Size(510, 431);
			this.RevisionsGrp.TabIndex = 13;
			this.RevisionsGrp.TabStop = false;
			this.RevisionsGrp.Text = "Revisions in project {0}";
			// 
			// RevisionsTree
			// 
			this.RevisionsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RevisionsTree.FullRowSelect = true;
			this.RevisionsTree.HideSelection = false;
			this.RevisionsTree.Location = new System.Drawing.Point(6, 20);
			this.RevisionsTree.MultiSelect = false;
			this.RevisionsTree.Name = "RevisionsTree";
			this.RevisionsTree.Size = new System.Drawing.Size(491, 411);
			this.RevisionsTree.TabIndex = 2;
			this.RevisionsTree.UseCompatibleStateImageBehavior = false;
			this.RevisionsTree.View = System.Windows.Forms.View.Details;
			// 
			// BimServerExchangeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 532);
			this.Controls.Add(this.MainSplit);
			this.Controls.Add(this.ButtonPnl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 300);
			this.Name = "BimServerExchangeForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "BIMserver ";
			this.ButtonPnl.ResumeLayout(false);
			this.OutputPnl.ResumeLayout(false);
			this.OutputPnl.PerformLayout();
			this.ExportPnl.ResumeLayout(false);
			this.UploadGrp.ResumeLayout(false);
			this.UploadGrp.PerformLayout();
			this.ProjectsPnl.ResumeLayout(false);
			this.ProjectsGrp.ResumeLayout(false);
			this.MainSplit.Panel1.ResumeLayout(false);
			this.MainSplit.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MainSplit)).EndInit();
			this.MainSplit.ResumeLayout(false);
			this.AddProjectGrp.ResumeLayout(false);
			this.AddProjectGrp.PerformLayout();
			this.IFCSettingsPnl.ResumeLayout(false);
			this.RevisionsGrp.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.Panel ButtonPnl;
		private System.Windows.Forms.Button CancelBtn;
		internal System.Windows.Forms.Panel OutputPnl;
		internal System.Windows.Forms.TextBox OutputEdt;
		internal System.Windows.Forms.TextBox IfcEdt;
		internal System.Windows.Forms.Button SaveIfcBtn;
		internal System.Windows.Forms.Button UploadBtn;
		internal System.Windows.Forms.Panel ExportPnl;
		internal System.Windows.Forms.ProgressBar UploadProgressbar;
		  internal System.Windows.Forms.Button LoginBtn;
		  internal System.Windows.Forms.Panel ProjectsPnl;
		  internal System.Windows.Forms.GroupBox ProjectsGrp;
		  internal System.Windows.Forms.SplitContainer MainSplit;
		  internal System.Windows.Forms.TreeView ProjectsTree;
		  internal System.Windows.Forms.Button ProjectAddBtn;
		  internal System.Windows.Forms.TextBox ProjectDescEdt;
		  internal System.Windows.Forms.TextBox ProjectNameEdt;
		  internal System.Windows.Forms.Label ProjectDescLbl;
		  internal System.Windows.Forms.Label ProjectNameLbl;
		internal System.Windows.Forms.GroupBox RevisionsGrp;
		internal System.Windows.Forms.ListView RevisionsTree;
		internal System.Windows.Forms.Button CopyProjectNameBtn;
		internal System.Windows.Forms.TextBox TagEdt;
		internal System.Windows.Forms.Button LoadIfcBtn;
		internal System.Windows.Forms.GroupBox UploadGrp;
		internal System.Windows.Forms.Label IfcDescLbl;
		internal System.Windows.Forms.Label IfcNameLbl;
		internal System.Windows.Forms.Button DownloadBtn;
		internal System.Windows.Forms.Button CancelUploadBtn;
		internal System.Windows.Forms.Button SaveBtn;
		internal System.Windows.Forms.GroupBox AddProjectGrp;
		internal System.Windows.Forms.Panel IFCSettingsPnl;
		internal System.Windows.Forms.RadioButton StreamingRadio;
		internal System.Windows.Forms.RadioButton SequentialRadio;
		internal System.Windows.Forms.ComboBox IFCFormatCbx;
		internal System.Windows.Forms.Label IFCFormatLbl;
		private ToolTip toolTip1;
		private ContextMenuStrip contextMenuStrip1;
	}
}