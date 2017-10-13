using System.Drawing;
using System.Windows.Forms;

namespace BimServerExchange.Forms
{
	partial class ProcessingStatusForm
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
			this.MainPnl = new System.Windows.Forms.Panel();
			this.OutputEdt = new System.Windows.Forms.TextBox();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.UploadProgressbar = new System.Windows.Forms.ProgressBar();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.MainPnl.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainPnl
			// 
			this.MainPnl.Controls.Add(this.OutputEdt);
			this.MainPnl.Controls.Add(this.CancelBtn);
			this.MainPnl.Controls.Add(this.UploadProgressbar);
			this.MainPnl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPnl.Location = new System.Drawing.Point(0, 0);
			this.MainPnl.Name = "MainPnl";
			this.MainPnl.Size = new System.Drawing.Size(284, 86);
			this.MainPnl.TabIndex = 3;
			// 
			// OutputEdt
			// 
			this.OutputEdt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OutputEdt.Location = new System.Drawing.Point(12, 36);
			this.OutputEdt.Multiline = true;
			this.OutputEdt.Name = "OutputEdt";
			this.OutputEdt.ReadOnly = true;
			this.OutputEdt.Size = new System.Drawing.Size(259, 38);
			this.OutputEdt.TabIndex = 22;
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelBtn.Enabled = false;
			this.CancelBtn.FlatAppearance.BorderSize = 0;
			this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.CancelBtn.Image = global::BimServerExchange.Properties.Resources.ButtonImage_Cancel;
			this.CancelBtn.Location = new System.Drawing.Point(255, 13);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(16, 16);
			this.CancelBtn.TabIndex = 21;
			this.CancelBtn.Text = "+";
			this.toolTip1.SetToolTip(this.CancelBtn, "Cancel the upload to the BIMserver");
			// 
			// UploadProgressbar
			// 
			this.UploadProgressbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.UploadProgressbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(140)))), ((int)(((byte)(180)))));
			this.UploadProgressbar.Location = new System.Drawing.Point(12, 12);
			this.UploadProgressbar.Name = "UploadProgressbar";
			this.UploadProgressbar.Size = new System.Drawing.Size(240, 18);
			this.UploadProgressbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.UploadProgressbar.TabIndex = 20;
			// 
			// ProcessingStatusForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 86);
			this.Controls.Add(this.MainPnl);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(3000, 500);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(200, 125);
			this.Name = "ProcessingStatusForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "ProcessingStatusForm";
			this.MainPnl.ResumeLayout(false);
			this.MainPnl.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.Panel MainPnl;
		//internal DevExpress.XtraEditors.MemoEdit OutputEdt;
		internal System.Windows.Forms.TextBox OutputEdt;
		internal System.Windows.Forms.Button CancelBtn;
		//internal DevExpress.XtraEditors.ProgressBarControl UploadProgressbar;
		internal System.Windows.Forms.ProgressBar UploadProgressbar;
		private ToolTip toolTip1;
	}
}