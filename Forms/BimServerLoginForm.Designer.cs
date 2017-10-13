using System.Windows.Forms;

namespace BimServerExchange.Forms
{
	 partial class BimServerLoginForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BimServerLoginForm));
			this.ButtonPnl = new System.Windows.Forms.Panel();
			this.ConnectBtn = new System.Windows.Forms.Button();
			this.Mainpnl = new System.Windows.Forms.Panel();
			this.PortEdt = new System.Windows.Forms.TextBox();
			this.PasswordEdt = new System.Windows.Forms.TextBox();
			this.LoginNameEdt = new System.Windows.Forms.TextBox();
			this.UrlEdt = new System.Windows.Forms.TextBox();
			this.PasswordLbl = new System.Windows.Forms.Label();
			this.LoginNameLbl = new System.Windows.Forms.Label();
			this.PortLbl = new System.Windows.Forms.Label();
			this.UrlLbl = new System.Windows.Forms.Label();
			this.IFCSettingsPnl = new System.Windows.Forms.Panel();
			this.StreamingRadio = new System.Windows.Forms.RadioButton();
			this.SequentialRadio = new System.Windows.Forms.RadioButton();
			this.IFCFormatCbx = new System.Windows.Forms.ComboBox();
			this.IFCFormatLbl = new System.Windows.Forms.Label();
			this.ProjectPnl = new System.Windows.Forms.Panel();
			this.ProjectsCbx = new System.Windows.Forms.ComboBox();
			this.ProjectLbl = new System.Windows.Forms.Label();
			this.IfcExportOptionsPnl = new System.Windows.Forms.Panel();
			this.ExportConfigurationLbl = new System.Windows.Forms.Label();
			this.ExportConfigurationsCbx = new System.Windows.Forms.ComboBox();
			this.IfcExportAddinChk = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.ButtonPnl.SuspendLayout();
			this.Mainpnl.SuspendLayout();
			this.IFCSettingsPnl.SuspendLayout();
			this.ProjectPnl.SuspendLayout();
			this.IfcExportOptionsPnl.SuspendLayout();
			this.SuspendLayout();
			// 
			// ButtonPnl
			// 
			this.ButtonPnl.Controls.Add(this.ConnectBtn);
			this.ButtonPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ButtonPnl.Location = new System.Drawing.Point(0, 245);
			this.ButtonPnl.Name = "ButtonPnl";
			this.ButtonPnl.Size = new System.Drawing.Size(384, 31);
			this.ButtonPnl.TabIndex = 18;
			// 
			// ConnectBtn
			// 
			this.ConnectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ConnectBtn.Location = new System.Drawing.Point(298, 4);
			this.ConnectBtn.Name = "ConnectBtn";
			this.ConnectBtn.Size = new System.Drawing.Size(75, 23);
			this.ConnectBtn.TabIndex = 8;
			this.ConnectBtn.Text = "Connect";
			this.ConnectBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// Mainpnl
			// 
			this.Mainpnl.Controls.Add(this.PortEdt);
			this.Mainpnl.Controls.Add(this.PasswordEdt);
			this.Mainpnl.Controls.Add(this.LoginNameEdt);
			this.Mainpnl.Controls.Add(this.UrlEdt);
			this.Mainpnl.Controls.Add(this.PasswordLbl);
			this.Mainpnl.Controls.Add(this.LoginNameLbl);
			this.Mainpnl.Controls.Add(this.PortLbl);
			this.Mainpnl.Controls.Add(this.UrlLbl);
			this.Mainpnl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Mainpnl.Location = new System.Drawing.Point(0, 0);
			this.Mainpnl.Name = "Mainpnl";
			this.Mainpnl.Size = new System.Drawing.Size(384, 91);
			this.Mainpnl.TabIndex = 9;
			// 
			// PortEdt
			// 
			this.PortEdt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PortEdt.Location = new System.Drawing.Point(323, 8);
			this.PortEdt.Name = "PortEdt";
			this.PortEdt.Size = new System.Drawing.Size(50, 20);
			this.PortEdt.TabIndex = 2;
			this.PortEdt.Leave += new System.EventHandler(this.TextEdit_Changed);
			// 
			// PasswordEdt
			// 
			this.PasswordEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PasswordEdt.Location = new System.Drawing.Point(100, 62);
			this.PasswordEdt.Name = "PasswordEdt";
			this.PasswordEdt.Size = new System.Drawing.Size(273, 20);
			this.PasswordEdt.TabIndex = 4;
			this.PasswordEdt.Leave += new System.EventHandler(this.TextEdit_Changed);
			// 
			// LoginNameEdt
			// 
			this.LoginNameEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LoginNameEdt.Location = new System.Drawing.Point(100, 34);
			this.LoginNameEdt.Name = "LoginNameEdt";
			this.LoginNameEdt.Size = new System.Drawing.Size(273, 20);
			this.LoginNameEdt.TabIndex = 3;
			this.LoginNameEdt.Leave += new System.EventHandler(this.TextEdit_Changed);
			// 
			// UrlEdt
			// 
			this.UrlEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.UrlEdt.Location = new System.Drawing.Point(100, 8);
			this.UrlEdt.Name = "UrlEdt";
			this.UrlEdt.Size = new System.Drawing.Size(191, 20);
			this.UrlEdt.TabIndex = 1;
			this.UrlEdt.Leave += new System.EventHandler(this.TextEdit_Changed);
			// 
			// PasswordLbl
			// 
			this.PasswordLbl.Location = new System.Drawing.Point(8, 63);
			this.PasswordLbl.Name = "PasswordLbl";
			this.PasswordLbl.Size = new System.Drawing.Size(86, 13);
			this.PasswordLbl.TabIndex = 13;
			this.PasswordLbl.Text = "Password";
			// 
			// LoginNameLbl
			// 
			this.LoginNameLbl.Location = new System.Drawing.Point(8, 37);
			this.LoginNameLbl.Name = "LoginNameLbl";
			this.LoginNameLbl.Size = new System.Drawing.Size(86, 13);
			this.LoginNameLbl.TabIndex = 12;
			this.LoginNameLbl.Text = "Login name";
			// 
			// PortLbl
			// 
			this.PortLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PortLbl.Location = new System.Drawing.Point(295, 11);
			this.PortLbl.Name = "PortLbl";
			this.PortLbl.Size = new System.Drawing.Size(26, 13);
			this.PortLbl.TabIndex = 11;
			this.PortLbl.Text = "Port";
			// 
			// UrlLbl
			// 
			this.UrlLbl.Location = new System.Drawing.Point(8, 11);
			this.UrlLbl.Name = "UrlLbl";
			this.UrlLbl.Size = new System.Drawing.Size(86, 13);
			this.UrlLbl.TabIndex = 10;
			this.UrlLbl.Text = "BIMserver URL";
			// 
			// IFCSettingsPnl
			// 
			this.IFCSettingsPnl.Controls.Add(this.StreamingRadio);
			this.IFCSettingsPnl.Controls.Add(this.SequentialRadio);
			this.IFCSettingsPnl.Controls.Add(this.IFCFormatCbx);
			this.IFCSettingsPnl.Controls.Add(this.IFCFormatLbl);
			this.IFCSettingsPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.IFCSettingsPnl.Location = new System.Drawing.Point(0, 179);
			this.IFCSettingsPnl.Name = "IFCSettingsPnl";
			this.IFCSettingsPnl.Size = new System.Drawing.Size(384, 66);
			this.IFCSettingsPnl.TabIndex = 16;
			// 
			// StreamingRadio
			// 
			this.StreamingRadio.Location = new System.Drawing.Point(102, 34);
			this.StreamingRadio.Name = "StreamingRadio";
			this.StreamingRadio.Size = new System.Drawing.Size(130, 24);
			this.StreamingRadio.TabIndex = 7;
			this.StreamingRadio.Text = "Streaming";
			// 
			// SequentialRadio
			// 
			this.SequentialRadio.Location = new System.Drawing.Point(242, 34);
			this.SequentialRadio.Name = "SequentialRadio";
			this.SequentialRadio.Size = new System.Drawing.Size(130, 24);
			this.SequentialRadio.TabIndex = 8;
			this.SequentialRadio.Text = "Classic";
			// 
			// IFCFormatCbx
			// 
			this.IFCFormatCbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IFCFormatCbx.Location = new System.Drawing.Point(100, 7);
			this.IFCFormatCbx.Name = "IFCFormatCbx";
			this.IFCFormatCbx.Size = new System.Drawing.Size(273, 21);
			this.IFCFormatCbx.TabIndex = 6;
			this.toolTip1.SetToolTip(this.IFCFormatCbx, "IFC format used do serialize and deserialize IFC files on th BIMserver");
			this.IFCFormatCbx.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectionChanged);
			// 
			// IFCFormatLbl
			// 
			this.IFCFormatLbl.Location = new System.Drawing.Point(8, 10);
			this.IFCFormatLbl.Name = "IFCFormatLbl";
			this.IFCFormatLbl.Size = new System.Drawing.Size(86, 13);
			this.IFCFormatLbl.TabIndex = 17;
			this.IFCFormatLbl.Text = "IFC Format";
			// 
			// ProjectPnl
			// 
			this.ProjectPnl.Controls.Add(this.ProjectsCbx);
			this.ProjectPnl.Controls.Add(this.ProjectLbl);
			this.ProjectPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ProjectPnl.Location = new System.Drawing.Point(0, 145);
			this.ProjectPnl.Name = "ProjectPnl";
			this.ProjectPnl.Size = new System.Drawing.Size(384, 34);
			this.ProjectPnl.TabIndex = 14;
			// 
			// ProjectsCbx
			// 
			this.ProjectsCbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ProjectsCbx.Location = new System.Drawing.Point(100, 6);
			this.ProjectsCbx.Name = "ProjectsCbx";
			this.ProjectsCbx.Size = new System.Drawing.Size(273, 21);
			this.ProjectsCbx.Sorted = true;
			this.ProjectsCbx.TabIndex = 5;
			this.toolTip1.SetToolTip(this.ProjectsCbx, "Project to upload new revisions to");
			this.ProjectsCbx.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectionChanged);
			// 
			// ProjectLbl
			// 
			this.ProjectLbl.Location = new System.Drawing.Point(8, 9);
			this.ProjectLbl.Name = "ProjectLbl";
			this.ProjectLbl.Size = new System.Drawing.Size(86, 13);
			this.ProjectLbl.TabIndex = 15;
			this.ProjectLbl.Text = "Project";
			// 
			// IfcExportOptionsPnl
			// 
			this.IfcExportOptionsPnl.Controls.Add(this.ExportConfigurationLbl);
			this.IfcExportOptionsPnl.Controls.Add(this.ExportConfigurationsCbx);
			this.IfcExportOptionsPnl.Controls.Add(this.IfcExportAddinChk);
			this.IfcExportOptionsPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.IfcExportOptionsPnl.Location = new System.Drawing.Point(0, 91);
			this.IfcExportOptionsPnl.Name = "IfcExportOptionsPnl";
			this.IfcExportOptionsPnl.Size = new System.Drawing.Size(384, 54);
			this.IfcExportOptionsPnl.TabIndex = 14;
			// 
			// ExportConfigurationLbl
			// 
			this.ExportConfigurationLbl.Location = new System.Drawing.Point(8, 31);
			this.ExportConfigurationLbl.Name = "ExportConfigurationLbl";
			this.ExportConfigurationLbl.Size = new System.Drawing.Size(86, 13);
			this.ExportConfigurationLbl.TabIndex = 14;
			this.ExportConfigurationLbl.Text = "Configuration";
			// 
			// ExportConfigurationsCbx
			// 
			this.ExportConfigurationsCbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ExportConfigurationsCbx.Location = new System.Drawing.Point(100, 28);
			this.ExportConfigurationsCbx.Name = "ExportConfigurationsCbx";
			this.ExportConfigurationsCbx.Size = new System.Drawing.Size(273, 21);
			this.ExportConfigurationsCbx.TabIndex = 1;
			this.ExportConfigurationsCbx.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectionChanged);
			// 
			// IfcExportAddinChk
			// 
			this.IfcExportAddinChk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IfcExportAddinChk.Location = new System.Drawing.Point(8, 6);
			this.IfcExportAddinChk.Name = "IfcExportAddinChk";
			this.IfcExportAddinChk.Size = new System.Drawing.Size(368, 19);
			this.IfcExportAddinChk.TabIndex = 0;
			this.IfcExportAddinChk.Text = "IFC Export addin is installed";
			this.IfcExportAddinChk.CheckedChanged += new System.EventHandler(this.CheckEdit_Changed);
			// 
			// BimServerLoginForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 276);
			this.Controls.Add(this.Mainpnl);
			this.Controls.Add(this.IfcExportOptionsPnl);
			this.Controls.Add(this.ProjectPnl);
			this.Controls.Add(this.IFCSettingsPnl);
			this.Controls.Add(this.ButtonPnl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(4000, 315);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(400, 215);
			this.Name = "BimServerLoginForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Login settings for a BIMserver";
			this.ButtonPnl.ResumeLayout(false);
			this.Mainpnl.ResumeLayout(false);
			this.Mainpnl.PerformLayout();
			this.IFCSettingsPnl.ResumeLayout(false);
			this.ProjectPnl.ResumeLayout(false);
			this.IfcExportOptionsPnl.ResumeLayout(false);
			this.ResumeLayout(false);

		  }

		  #endregion

		  internal System.Windows.Forms.Panel ButtonPnl;
		  internal System.Windows.Forms.Panel Mainpnl;
		  internal System.Windows.Forms.Button ConnectBtn;
		  internal System.Windows.Forms.TextBox PortEdt;
		  internal System.Windows.Forms.TextBox PasswordEdt;
		  internal System.Windows.Forms.TextBox LoginNameEdt;
		  internal System.Windows.Forms.TextBox UrlEdt;
		  internal System.Windows.Forms.Label PasswordLbl;
		  internal System.Windows.Forms.Label LoginNameLbl;
		  internal System.Windows.Forms.Label PortLbl;
		  internal System.Windows.Forms.Label UrlLbl;
		internal System.Windows.Forms.Panel IFCSettingsPnl;
		//internal System.Windows.Forms.RadioGroup ProtocolRadio;
		 internal System.Windows.Forms.RadioButton StreamingRadio;
		 internal System.Windows.Forms.RadioButton SequentialRadio;
		internal System.Windows.Forms.ComboBox IFCFormatCbx;
		internal System.Windows.Forms.Label IFCFormatLbl;
		internal System.Windows.Forms.Panel ProjectPnl;
		internal System.Windows.Forms.ComboBox ProjectsCbx;
		internal System.Windows.Forms.Label ProjectLbl;
		internal System.Windows.Forms.Panel IfcExportOptionsPnl;
		internal System.Windows.Forms.Label ExportConfigurationLbl;
		internal System.Windows.Forms.ComboBox ExportConfigurationsCbx;
		internal System.Windows.Forms.CheckBox IfcExportAddinChk;
		private ToolTip toolTip1;
	}
}