namespace Plugin.ApkImageView.Directory
{
	partial class DocumentManifest
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
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.tvXml = new System.Windows.Forms.TreeView();
			this.pgInfo = new System.Windows.Forms.PropertyGrid();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.tvXml);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.pgInfo);
			this.splitMain.Size = new System.Drawing.Size(150, 150);
			this.splitMain.SplitterDistance = 80;
			this.splitMain.TabIndex = 0;
			// 
			// tvXml
			// 
			this.tvXml.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvXml.Location = new System.Drawing.Point(0, 0);
			this.tvXml.Name = "tvXml";
			this.tvXml.Size = new System.Drawing.Size(150, 80);
			this.tvXml.TabIndex = 0;
			this.tvXml.HideSelection = false;
			this.tvXml.AfterSelect += tvXml_AfterSelect;
			// 
			// pgInfo
			// 
			this.pgInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgInfo.Location = new System.Drawing.Point(0, 0);
			this.pgInfo.Name = "pgInfo";
			this.pgInfo.Size = new System.Drawing.Size(150, 46);
			this.pgInfo.TabIndex = 2;
			// 
			// DirectoryManifest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Name = "DirectoryManifest";
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.TreeView tvXml;
		private System.Windows.Forms.PropertyGrid pgInfo;
	}
}
