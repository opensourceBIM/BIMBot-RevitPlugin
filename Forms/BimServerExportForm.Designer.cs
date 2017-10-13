using System.Drawing;
using System.Windows.Forms;

namespace BimServerExchange.Forms
{
	partial class BimServerExportForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BimServerExportForm));
			this.ButtonPnl = new System.Windows.Forms.Panel();
			this.OpenProjectBtn = new System.Windows.Forms.Button();
			this.LoginBtn = new System.Windows.Forms.Button();
			this.ExportBtn = new System.Windows.Forms.Button();
			this.MainPnl = new System.Windows.Forms.Panel();
			this.OutputEdt = new System.Windows.Forms.TextBox();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.UploadProgressbar = new System.Windows.Forms.ProgressBar();
			this.CommentLbl = new System.Windows.Forms.Label();
			this.CommentEdt = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.ButtonPnl.SuspendLayout();
			this.MainPnl.SuspendLayout();
			this.SuspendLayout();
			// 
			// ButtonPnl
			// 
			this.ButtonPnl.Controls.Add(this.OpenProjectBtn);
			this.ButtonPnl.Controls.Add(this.LoginBtn);
			this.ButtonPnl.Controls.Add(this.ExportBtn);
			this.ButtonPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ButtonPnl.Location = new System.Drawing.Point(0, 116);
			this.ButtonPnl.Name = "ButtonPnl";
			this.ButtonPnl.Size = new System.Drawing.Size(284, 31);
			this.ButtonPnl.TabIndex = 1;
			// 
			// OpenProjectBtn
			// 
			this.OpenProjectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.OpenProjectBtn.Location = new System.Drawing.Point(115, 3);
			this.OpenProjectBtn.Name = "OpenProjectBtn";
			this.OpenProjectBtn.Size = new System.Drawing.Size(75, 23);
			this.OpenProjectBtn.TabIndex = 4;
			this.OpenProjectBtn.Text = "> Project";
			this.OpenProjectBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// LoginBtn
			// 
			this.LoginBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.LoginBtn.FlatAppearance.BorderSize = 0;
			this.LoginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.LoginBtn.Image = global::BimServerExchange.Properties.Resources.ButtonImage_Config;
			this.LoginBtn.Location = new System.Drawing.Point(12, 3);
			this.LoginBtn.Name = "LoginBtn";
			this.LoginBtn.Size = new System.Drawing.Size(24, 24);
			this.LoginBtn.TabIndex = 3;
			this.LoginBtn.Text = "+";
			this.toolTip1.SetToolTip(this.LoginBtn, "Configure the connection to the BIMserver");
			this.LoginBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// ExportBtn
			// 
			this.ExportBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ExportBtn.Location = new System.Drawing.Point(196, 3);
			this.ExportBtn.Name = "ExportBtn";
			this.ExportBtn.Size = new System.Drawing.Size(75, 23);
			this.ExportBtn.TabIndex = 0;
			this.ExportBtn.Text = "Exporteer";
			this.ExportBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// MainPnl
			// 
			this.MainPnl.Controls.Add(this.OutputEdt);
			this.MainPnl.Controls.Add(this.CancelBtn);
			this.MainPnl.Controls.Add(this.UploadProgressbar);
			this.MainPnl.Controls.Add(this.CommentLbl);
			this.MainPnl.Controls.Add(this.CommentEdt);
			this.MainPnl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPnl.Location = new System.Drawing.Point(0, 0);
			this.MainPnl.Name = "MainPnl";
			this.MainPnl.Size = new System.Drawing.Size(284, 116);
			this.MainPnl.TabIndex = 2;
			// 
			// OutputEdt
			// 
			this.OutputEdt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OutputEdt.Location = new System.Drawing.Point(12, 74);
			this.OutputEdt.Multiline = true;
			this.OutputEdt.Name = "OutputEdt";
			this.OutputEdt.ReadOnly = true;
			this.OutputEdt.Size = new System.Drawing.Size(259, 35);
			this.OutputEdt.TabIndex = 22;
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelBtn.Enabled = false;
			this.CancelBtn.FlatAppearance.BorderSize = 0;
			this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.CancelBtn.Image = global::BimServerExchange.Properties.Resources.ButtonImage_Cancel;
			this.CancelBtn.Location = new System.Drawing.Point(255, 53);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(16, 16);
			this.CancelBtn.TabIndex = 21;
			this.CancelBtn.Text = "+";
			this.toolTip1.SetToolTip(this.CancelBtn, "Cancel the upload to the BIMserver");
			this.CancelBtn.Click += new System.EventHandler(this.SimpleButton_Click);
			// 
			// UploadProgressbar
			// 
			this.UploadProgressbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.UploadProgressbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(140)))), ((int)(((byte)(180)))));
			this.UploadProgressbar.Location = new System.Drawing.Point(12, 52);
			this.UploadProgressbar.Name = "UploadProgressbar";
			this.UploadProgressbar.Size = new System.Drawing.Size(240, 18);
			this.UploadProgressbar.Step = 1;
			this.UploadProgressbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.UploadProgressbar.TabIndex = 20;
			// 
			// CommentLbl
			// 
			this.CommentLbl.Location = new System.Drawing.Point(12, 9);
			this.CommentLbl.Name = "CommentLbl";
			this.CommentLbl.Size = new System.Drawing.Size(259, 13);
			this.CommentLbl.TabIndex = 19;
			this.CommentLbl.Text = "Comment";
			// 
			// CommentEdt
			// 
			this.CommentEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.CommentEdt.Location = new System.Drawing.Point(12, 28);
			this.CommentEdt.Name = "CommentEdt";
			this.CommentEdt.Size = new System.Drawing.Size(259, 20);
			this.CommentEdt.TabIndex = 18;
			this.toolTip1.SetToolTip(this.CommentEdt, "Short description of this revision");
			// 
			// BimServerExportForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 147);
			this.Controls.Add(this.MainPnl);
			this.Controls.Add(this.ButtonPnl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(3000, 386);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(300, 186);
			this.Name = "BimServerExportForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export project to BIMserver";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
			this.Shown += new System.EventHandler(this.Form_Shown);
			this.ButtonPnl.ResumeLayout(false);
			this.MainPnl.ResumeLayout(false);
			this.MainPnl.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.Panel ButtonPnl;
		internal System.Windows.Forms.Button LoginBtn;
		internal System.Windows.Forms.Panel MainPnl;
		internal System.Windows.Forms.Label CommentLbl;
		internal System.Windows.Forms.TextBox CommentEdt;
		internal System.Windows.Forms.ProgressBar UploadProgressbar;
		internal System.Windows.Forms.Button OpenProjectBtn;
		internal System.Windows.Forms.Button ExportBtn;
		internal System.Windows.Forms.Button CancelBtn;
		internal System.Windows.Forms.TextBox OutputEdt;
		private ToolTip toolTip1;
	}
}