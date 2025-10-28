namespace Plugin.ApkImageView.Directory
{
	partial class DocumentTables
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
			this.tvHierarchy = new System.Windows.Forms.TreeView();
			this.splitDetails = new System.Windows.Forms.SplitContainer();
			this.gridSearch = new AlphaOmega.Windows.Forms.SearchGrid();
			this.lvHeaps = new Plugin.ApkImageView.Controls.ReflectionArrayListView();
			this.tabPointers = new System.Windows.Forms.TabControl();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.splitDetails.Panel1.SuspendLayout();
			this.splitDetails.Panel2.SuspendLayout();
			this.splitDetails.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Name = "splitMain";
			// 
			// splitContainer1.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.tvHierarchy);
			// 
			// splitContainer1.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.splitDetails);
			this.splitMain.Size = new System.Drawing.Size(426, 341);
			this.splitMain.SplitterDistance = 200;
			this.splitMain.TabIndex = 0;
			// 
			// tvHierarchy
			// 
			this.tvHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvHierarchy.AllowDrop = true;
			this.tvHierarchy.HideSelection = false;
			this.tvHierarchy.Location = new System.Drawing.Point(0, 0);
			this.tvHierarchy.Name = "tvHierarchy";
			this.tvHierarchy.Size = new System.Drawing.Size(200, 341);
			this.tvHierarchy.TabIndex = 0;
			this.tvHierarchy.DragEnter += new System.Windows.Forms.DragEventHandler(tvHierarchy_DragEnter);
			this.tvHierarchy.DragDrop += new System.Windows.Forms.DragEventHandler(tvHierarchy_DragDrop);
			this.tvHierarchy.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvHierarchy_AfterSelect);
			// 
			// splitDetails
			// 
			this.splitDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitDetails.Location = new System.Drawing.Point(0, 0);
			this.splitDetails.Name = "splitDetails";
			this.splitDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitDetails.Panel1
			// 
			this.splitDetails.Panel1.Controls.Add(this.gridSearch);
			this.splitDetails.Panel1.Controls.Add(this.lvHeaps);
			// 
			// splitDetails.Panel2
			// 
			this.splitDetails.Panel2.Controls.Add(this.tabPointers);
			this.splitDetails.Size = new System.Drawing.Size(222, 341);
			this.splitDetails.SplitterDistance = 188;
			this.splitDetails.TabIndex = 0;
			this.splitDetails.Panel2Collapsed = true;
			// 
			// gridSearch
			// 
			this.gridSearch.DataGrid = null;
			this.gridSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.gridSearch.EnableFindCase = true;
			this.gridSearch.EnableFindHighlight = true;
			this.gridSearch.EnableFindPrevNext = true;
			this.gridSearch.EnableSearchHighlight = false;
			this.gridSearch.ListView = null;
			this.gridSearch.TreeView = null;
			this.gridSearch.Location = new System.Drawing.Point(3, 155);
			this.gridSearch.Name = "gridSearch";
			this.gridSearch.Size = new System.Drawing.Size(440, 29);
			this.gridSearch.TabIndex = 1;
			this.gridSearch.Visible = false;
			// 
			// lvHeaps
			// 
			this.lvHeaps.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvHeaps.FullRowSelect = true;
			this.lvHeaps.HideSelection = false;
			this.lvHeaps.Location = new System.Drawing.Point(0, 0);
			this.lvHeaps.Name = "lvHeaps";
			this.lvHeaps.Size = new System.Drawing.Size(222, 188);
			this.lvHeaps.TabIndex = 0;
			this.lvHeaps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Clickable;
			this.lvHeaps.UseCompatibleStateImageBehavior = false;
			this.lvHeaps.View = System.Windows.Forms.View.Details;
			this.lvHeaps.SelectedIndexChanged += new System.EventHandler(lvHeaps_SelectedIndexChanged);
			// 
			// tabPointers
			// 
			this.tabPointers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPointers.Location = new System.Drawing.Point(0, 0);
			this.tabPointers.Name = "tabPointers";
			this.tabPointers.SelectedIndex = 0;
			this.tabPointers.Size = new System.Drawing.Size(222, 149);
			this.tabPointers.TabIndex = 0;
			// 
			// DocumentCor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Name = "DocumentTables";
			this.Size = new System.Drawing.Size(426, 341);
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.ResumeLayout(false);
			this.splitDetails.Panel1.ResumeLayout(false);
			this.splitDetails.Panel2.ResumeLayout(false);
			this.splitDetails.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.TreeView tvHierarchy;
		private System.Windows.Forms.SplitContainer splitDetails;
		private Plugin.ApkImageView.Controls.ReflectionArrayListView lvHeaps;
		private AlphaOmega.Windows.Forms.SearchGrid gridSearch;
		private System.Windows.Forms.TabControl tabPointers;
	}
}
