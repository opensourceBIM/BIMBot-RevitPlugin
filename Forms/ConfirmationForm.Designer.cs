using System.Windows.Forms;

namespace BimServerExchange.Forms
{
	partial class ConfirmationForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmationForm));
			this.ButtonPnl = new System.Windows.Forms.Panel();
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.MessagePnl = new System.Windows.Forms.Panel();
			this.MessageMemo = new System.Windows.Forms.TextBox();
			this.ButtonPnl.SuspendLayout();
			this.MessagePnl.SuspendLayout();
			this.SuspendLayout();
			// 
			// ButtonPnl
			// 
			this.ButtonPnl.Controls.Add(this.OkBtn);
			this.ButtonPnl.Controls.Add(this.CancelBtn);
			this.ButtonPnl.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ButtonPnl.Location = new System.Drawing.Point(0, 169);
			this.ButtonPnl.Name = "ButtonPnl";
			this.ButtonPnl.Size = new System.Drawing.Size(284, 31);
			this.ButtonPnl.TabIndex = 1;
			// 
			// OkBtn
			// 
			this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.OkBtn.Location = new System.Drawing.Point(125, 3);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 23);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "Yes";
			// 
			// CancelBtn
			// 
			this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.No;
			this.CancelBtn.Location = new System.Drawing.Point(206, 3);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 23);
			this.CancelBtn.TabIndex = 0;
			this.CancelBtn.Text = "No";
			// 
			// MessagePnl
			// 
			this.MessagePnl.Controls.Add(this.MessageMemo);
			this.MessagePnl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MessagePnl.Location = new System.Drawing.Point(0, 0);
			this.MessagePnl.Name = "MessagePnl";
			this.MessagePnl.Size = new System.Drawing.Size(284, 169);
			this.MessagePnl.TabIndex = 2;
			// 
			// MessageMemo
			// 
			this.MessageMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MessageMemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
			this.MessageMemo.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.MessageMemo.Location = new System.Drawing.Point(3, 47);
			this.MessageMemo.Multiline = true;
			this.MessageMemo.Name = "MessageMemo";
			this.MessageMemo.ReadOnly = true;
			this.MessageMemo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.MessageMemo.Size = new System.Drawing.Size(278, 116);
			this.MessageMemo.TabIndex = 0;
			this.MessageMemo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// ConfirmationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 200);
			this.Controls.Add(this.MessagePnl);
			this.Controls.Add(this.ButtonPnl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConfirmationForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Confirm";
			this.ButtonPnl.ResumeLayout(false);
			this.MessagePnl.ResumeLayout(false);
			this.MessagePnl.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		internal System.Windows.Forms.Panel ButtonPnl;
		internal System.Windows.Forms.Button CancelBtn;
		internal System.Windows.Forms.Button OkBtn;
		internal System.Windows.Forms.Panel MessagePnl;
		//private DevExpress.XtraEditors.MemoEdit MessageMemo;
		internal System.Windows.Forms.TextBox MessageMemo;
	}
}