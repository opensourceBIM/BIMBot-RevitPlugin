namespace BimServerExchange.Forms
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.ButtonPnl = new System.Windows.Forms.Panel();
			this.OktBtn = new System.Windows.Forms.Button();
			this.MainPnl = new System.Windows.Forms.Panel();
			this.LinkEdt = new System.Windows.Forms.LinkLabel();
			this.CopyrightEdt = new System.Windows.Forms.TextBox();
			this.LicenseEdt = new System.Windows.Forms.RichTextBox();
			this.DescriptionEdt = new System.Windows.Forms.TextBox();
			this.VersionEdt = new System.Windows.Forms.TextBox();
			this.ButtonPnl.SuspendLayout();
			this.MainPnl.SuspendLayout();
			this.SuspendLayout();
			// 
			// ButtonPnl
			// 
			this.ButtonPnl.Controls.Add(this.OktBtn);
			this.ButtonPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ButtonPnl.Location = new System.Drawing.Point(0, 150);
			this.ButtonPnl.Name = "ButtonPnl";
			this.ButtonPnl.Size = new System.Drawing.Size(364, 31);
			this.ButtonPnl.TabIndex = 2;
			// 
			// OktBtn
			// 
			this.OktBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.OktBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OktBtn.Location = new System.Drawing.Point(277, 5);
			this.OktBtn.Name = "OktBtn";
			this.OktBtn.Size = new System.Drawing.Size(75, 23);
			this.OktBtn.TabIndex = 0;
			this.OktBtn.Text = "OK";
			// 
			// MainPnl
			// 
			this.MainPnl.Controls.Add(this.LinkEdt);
			this.MainPnl.Controls.Add(this.CopyrightEdt);
			this.MainPnl.Controls.Add(this.LicenseEdt);
			this.MainPnl.Controls.Add(this.DescriptionEdt);
			this.MainPnl.Controls.Add(this.VersionEdt);
			this.MainPnl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPnl.Location = new System.Drawing.Point(0, 0);
			this.MainPnl.Name = "MainPnl";
			this.MainPnl.Size = new System.Drawing.Size(364, 150);
			this.MainPnl.TabIndex = 3;
			// 
			// LinkEdt
			// 
			this.LinkEdt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.LinkEdt.Location = new System.Drawing.Point(228, 102);
			this.LinkEdt.Name = "LinkEdt";
			this.LinkEdt.Size = new System.Drawing.Size(124, 15);
			this.LinkEdt.TabIndex = 6;
			this.LinkEdt.TabStop = true;
			this.LinkEdt.Text = "License agreement";
			this.LinkEdt.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.LinkEdt.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LicenseEdt_LinkClicked);
			// 
			// CopyrightEdt
			// 
			this.CopyrightEdt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CopyrightEdt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.CopyrightEdt.Location = new System.Drawing.Point(231, 86);
			this.CopyrightEdt.Name = "CopyrightEdt";
			this.CopyrightEdt.ReadOnly = true;
			this.CopyrightEdt.Size = new System.Drawing.Size(121, 13);
			this.CopyrightEdt.TabIndex = 5;
			this.CopyrightEdt.Text = "ICN Solution b.v. 2018";
			this.CopyrightEdt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.CopyrightEdt.Visible = false;
			// 
			// LicenseEdt
			// 
			this.LicenseEdt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LicenseEdt.Enabled = false;
			this.LicenseEdt.Location = new System.Drawing.Point(6, 126);
			this.LicenseEdt.Name = "LicenseEdt";
			this.LicenseEdt.ReadOnly = true;
			this.LicenseEdt.Size = new System.Drawing.Size(346, 18);
			this.LicenseEdt.TabIndex = 4;
			this.LicenseEdt.Text = "";
			this.LicenseEdt.Visible = false;
			// 
			// DescriptionEdt
			// 
			this.DescriptionEdt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DescriptionEdt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.DescriptionEdt.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DescriptionEdt.Location = new System.Drawing.Point(3, 3);
			this.DescriptionEdt.Multiline = true;
			this.DescriptionEdt.Name = "DescriptionEdt";
			this.DescriptionEdt.ReadOnly = true;
			this.DescriptionEdt.Size = new System.Drawing.Size(222, 96);
			this.DescriptionEdt.TabIndex = 3;
			this.DescriptionEdt.Text = "BIMServer Addin\r\nfor Revit {0}";
			this.DescriptionEdt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// VersionEdt
			// 
			this.VersionEdt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.VersionEdt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.VersionEdt.Location = new System.Drawing.Point(231, 3);
			this.VersionEdt.Name = "VersionEdt";
			this.VersionEdt.ReadOnly = true;
			this.VersionEdt.Size = new System.Drawing.Size(121, 13);
			this.VersionEdt.TabIndex = 2;
			this.VersionEdt.Text = "1.0.0.0001";
			this.VersionEdt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(364, 181);
			this.Controls.Add(this.MainPnl);
			this.Controls.Add(this.ButtonPnl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(380, 285);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(380, 220);
			this.Name = "AboutForm";
			this.ShowInTaskbar = false;
			this.Text = "About Revit BIMServer Addin";
			this.ButtonPnl.ResumeLayout(false);
			this.MainPnl.ResumeLayout(false);
			this.MainPnl.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.Panel ButtonPnl;
		internal System.Windows.Forms.Button OktBtn;
		internal System.Windows.Forms.Panel MainPnl;
		internal System.Windows.Forms.TextBox VersionEdt;
		internal System.Windows.Forms.TextBox DescriptionEdt;
		private System.Windows.Forms.RichTextBox LicenseEdt;
		internal System.Windows.Forms.TextBox CopyrightEdt;
		internal System.Windows.Forms.LinkLabel LinkEdt;
	}
}