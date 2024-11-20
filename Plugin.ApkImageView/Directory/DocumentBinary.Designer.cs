namespace Plugin.ApkImageView.Directory
{
	partial class DocumentBinary
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentBinary));
			System.Windows.Forms.ToolStripSeparator tsSep;
			this.bvBytes = new System.ComponentModel.Design.ByteViewer();
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsddlView = new System.Windows.Forms.ToolStripComboBox();
			this.tsbnSave = new System.Windows.Forms.ToolStripButton();
			tsSep = new System.Windows.Forms.ToolStripSeparator();
			this.tsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// bvBytes
			// 
			this.bvBytes.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
			this.bvBytes.ColumnCount = 1;
			this.bvBytes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bvBytes.Location = new System.Drawing.Point(0, 25);
			this.bvBytes.Name = "bvBytes";
			this.bvBytes.RowCount = 1;
			this.bvBytes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.bvBytes.Size = new System.Drawing.Size(495, 125);
			this.bvBytes.TabIndex = 0;
			// 
			// tsMain
			// 
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddlView,
            tsSep,
            this.tsbnSave});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(495, 25);
			this.tsMain.TabIndex = 1;
			// 
			// tsddlView
			// 
			this.tsddlView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tsddlView.Name = "tsddlView";
			this.tsddlView.Size = new System.Drawing.Size(121, 25);
			this.tsddlView.SelectedIndexChanged += new System.EventHandler(this.tsddlView_SelectedIndexChanged);
			// 
			// tsbnSave
			// 
			this.tsbnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbnSave.Image")));
			this.tsbnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnSave.Name = "tsbnSave";
			this.tsbnSave.Size = new System.Drawing.Size(23, 22);
			this.tsbnSave.Text = "Save...";
			this.tsbnSave.Click += new System.EventHandler(this.tsbnSave_Click);
			// 
			// tsSep
			// 
			tsSep.Name = "tsSep";
			tsSep.Size = new System.Drawing.Size(6, 25);
			// 
			// DocumentBinary
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.bvBytes);
			this.Controls.Add(this.tsMain);
			this.Name = "DocumentBinary";
			this.Size = new System.Drawing.Size(495, 150);
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.ComponentModel.Design.ByteViewer bvBytes;
		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripComboBox tsddlView;
		private System.Windows.Forms.ToolStripButton tsbnSave;
	}
}