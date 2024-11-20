namespace Plugin.ApkImageView.Directory
{
	partial class DocumentResource
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
			this.cmsCopy = new AlphaOmega.Windows.Forms.ContextMenuStripCopy();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.lvResource = new System.Windows.Forms.ListView();
			this.colResourceId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colResourceValues = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lvDetails = new System.Windows.Forms.ListView();
			this.colDetailsResource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDetailsDataType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDetailsRawValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmsCopy
			// 
			this.cmsCopy.Name = "cmsCopy";
			this.cmsCopy.Size = new System.Drawing.Size(103, 26);
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.lvResource);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.lvDetails);
			this.splitMain.Panel2Collapsed = true;
			this.splitMain.Size = new System.Drawing.Size(150, 150);
			this.splitMain.SplitterDistance = 77;
			this.splitMain.TabIndex = 0;
			// 
			// lvResource
			// 
			this.lvResource.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colResourceId,
            this.colResourceValues});
			this.lvResource.ContextMenuStrip = this.cmsCopy;
			this.lvResource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvResource.FullRowSelect = true;
			this.lvResource.GridLines = true;
			this.lvResource.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvResource.Location = new System.Drawing.Point(0, 0);
			this.lvResource.MultiSelect = false;
			this.lvResource.HideSelection = false;
			this.lvResource.Name = "lvResource";
			this.lvResource.ShowGroups = false;
			this.lvResource.Size = new System.Drawing.Size(150, 150);
			this.lvResource.TabIndex = 0;
			this.lvResource.UseCompatibleStateImageBehavior = false;
			this.lvResource.View = System.Windows.Forms.View.Details;
			this.lvResource.SelectedIndexChanged += new System.EventHandler(this.lvResource_SelectedIndexChanged);
			// 
			// colResourceId
			// 
			this.colResourceId.Text = "Id";
			// 
			// colResourceValues
			// 
			this.colResourceValues.Text = "Values";
			// 
			// lvDetails
			// 
			this.lvDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDetailsResource,
            this.colDetailsDataType,
            this.colDetailsRawValue});
			this.lvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvDetails.FullRowSelect = true;
			this.lvDetails.GridLines = true;
			this.lvDetails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvDetails.Location = new System.Drawing.Point(0, 0);
			this.lvDetails.Name = "lvDetails";
			this.lvDetails.ShowGroups = false;
			this.lvDetails.Size = new System.Drawing.Size(150, 46);
			this.lvDetails.TabIndex = 0;
			this.lvDetails.UseCompatibleStateImageBehavior = false;
			this.lvDetails.View = System.Windows.Forms.View.Details;
			// 
			// colDetailsResource
			// 
			this.colDetailsResource.Text = "Value";
			// 
			// colDetailsDataType
			// 
			this.colDetailsDataType.Text = "Data Type";
			// 
			// colDetailsRawValue
			// 
			this.colDetailsRawValue.Text = "Raw Value";
			// 
			// DocumentResource
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Name = "DocumentResource";
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.ListView lvResource;
		private System.Windows.Forms.ColumnHeader colResourceId;
		private System.Windows.Forms.ColumnHeader colResourceValues;
		private System.Windows.Forms.ListView lvDetails;
		private System.Windows.Forms.ColumnHeader colDetailsResource;
		private AlphaOmega.Windows.Forms.ContextMenuStripCopy cmsCopy;
		private System.Windows.Forms.ColumnHeader colDetailsDataType;
		private System.Windows.Forms.ColumnHeader colDetailsRawValue;
	}
}
