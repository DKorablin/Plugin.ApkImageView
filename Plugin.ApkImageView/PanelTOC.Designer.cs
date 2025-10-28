namespace Plugin.ApkImageView
{
	partial class PanelTOC
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code
		/// <summary> Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.ToolStrip tsHead;
			this.tsbnOpen = new System.Windows.Forms.ToolStripSplitButton();
			this.tsmiOpenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiOpenBase64 = new System.Windows.Forms.ToolStripMenuItem();
			this.gridSearch = new AlphaOmega.Windows.Forms.SearchGrid();
			this.splitToc = new System.Windows.Forms.SplitContainer();
			this.tvToc = new System.Windows.Forms.TreeView();
			this.cmsToc = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiTocUnload = new System.Windows.Forms.ToolStripMenuItem();
			this.txtInfo = new System.Windows.Forms.TextBox();
			this.lvInfo = new Plugin.ApkImageView.Controls.ReflectionListView();
			this.tsmiTocBinView = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiTocExplorerView = new System.Windows.Forms.ToolStripMenuItem();
			tsHead = new System.Windows.Forms.ToolStrip();
			tsHead.SuspendLayout();
			this.splitToc.Panel1.SuspendLayout();
			this.splitToc.Panel2.SuspendLayout();
			this.splitToc.SuspendLayout();
			this.cmsToc.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsHead
			// 
			tsHead.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			tsHead.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnOpen});
			tsHead.Location = new System.Drawing.Point(0, 0);
			tsHead.Name = "tsHead";
			tsHead.Size = new System.Drawing.Size(216, 25);
			tsHead.TabIndex = 2;
			// 
			// tsbnOpen
			// 
			this.tsbnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpenFile,
            this.tsmiOpenBase64});
			this.tsbnOpen.Image = global::Plugin.ApkImageView.Properties.Resources.iconOpen;
			this.tsbnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnOpen.Name = "tsbnOpen";
			this.tsbnOpen.Size = new System.Drawing.Size(32, 22);
			this.tsbnOpen.Text = "Open";
			this.tsbnOpen.ButtonClick += new System.EventHandler(this.tsbnOpen_Click);
			this.tsbnOpen.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsbnOpen_DropDownItemClicked);
			// 
			// tsmiOpenFile
			// 
			this.tsmiOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
			this.tsmiOpenFile.Image = global::Plugin.ApkImageView.Properties.Resources.iconOpenFile;
			this.tsmiOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsmiOpenFile.Name = "tsmiOpenFile";
			this.tsmiOpenFile.Size = new System.Drawing.Size(123, 22);
			this.tsmiOpenFile.Text = "&File...";
			// 
			// tsmiOpenBase64
			// 
			this.tsmiOpenBase64.Name = "tsmiOpenBase64";
			this.tsmiOpenBase64.Size = new System.Drawing.Size(123, 22);
			this.tsmiOpenBase64.Text = "&Base64...";
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
			this.gridSearch.Location = new System.Drawing.Point(3, 155);
			this.gridSearch.Name = "gridSearch";
			this.gridSearch.Size = new System.Drawing.Size(440, 29);
			this.gridSearch.TabIndex = 1;
			this.gridSearch.TreeView = null;
			this.gridSearch.Visible = false;
			// 
			// splitToc
			// 
			this.splitToc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitToc.Location = new System.Drawing.Point(0, 25);
			this.splitToc.Name = "splitToc";
			this.splitToc.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitToc.Panel1
			// 
			this.splitToc.Panel1.Controls.Add(this.gridSearch);
			this.splitToc.Panel1.Controls.Add(this.tvToc);
			// 
			// splitToc.Panel2
			// 
			this.splitToc.Panel2.Controls.Add(this.lvInfo);
			this.splitToc.Panel2.Controls.Add(this.txtInfo);
			this.splitToc.Panel2Collapsed = true;
			this.splitToc.Size = new System.Drawing.Size(216, 258);
			this.splitToc.SplitterDistance = 137;
			this.splitToc.TabIndex = 1;
			// 
			// tvToc
			// 
			this.tvToc.AllowDrop = true;
			this.tvToc.ContextMenuStrip = this.cmsToc;
			this.tvToc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvToc.HideSelection = false;
			this.tvToc.Location = new System.Drawing.Point(0, 0);
			this.tvToc.Name = "tvToc";
			this.tvToc.Size = new System.Drawing.Size(216, 258);
			this.tvToc.TabIndex = 3;
			this.tvToc.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvToc_BeforeExpand);
			this.tvToc.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvToc_AfterSelect);
			this.tvToc.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvToc_DragDrop);
			this.tvToc.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvToc_DragEnter);
			this.tvToc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvToc_KeyDown);
			this.tvToc.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvToc_MouseClick);
			this.tvToc.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvToc_MouseDoubleClick);
			// 
			// cmsToc
			// 
			this.cmsToc.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTocUnload,
            this.tsmiTocBinView,
			this.tsmiTocExplorerView});
			this.cmsToc.Name = "cmsToc";
			this.cmsToc.Size = new System.Drawing.Size(153, 70);
			this.cmsToc.Opening += new System.ComponentModel.CancelEventHandler(this.cmsToc_Opening);
			this.cmsToc.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsToc_ItemClicked);
			// 
			// tsmiTocUnload
			// 
			this.tsmiTocUnload.Name = "tsmiTocUnload";
			this.tsmiTocUnload.Size = new System.Drawing.Size(152, 22);
			this.tsmiTocUnload.Text = "&Unload";
			// 
			// lvInfo
			// 
			this.lvInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvInfo.FullRowSelect = true;
			this.lvInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvInfo.HideSelection = false;
			this.lvInfo.Location = new System.Drawing.Point(0, 0);
			this.lvInfo.Name = "lvInfo";
			this.lvInfo.Plugin = null;
			this.lvInfo.Size = new System.Drawing.Size(150, 46);
			this.lvInfo.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvInfo.TabIndex = 2;
			this.lvInfo.UseCompatibleStateImageBehavior = false;
			this.lvInfo.View = System.Windows.Forms.View.Details;
			// 
			// txtInfo
			// 
			this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtInfo.HideSelection = false;
			this.txtInfo.Location = new System.Drawing.Point(0, 0);
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.Size = new System.Drawing.Size(150, 46);
			this.txtInfo.TabIndex = 2;
			this.txtInfo.Multiline = true;
			this.txtInfo.ReadOnly = true;
			this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtInfo.KeyDown += txtInfo_KeyDown;
			// 
			// tsmiTocBinView
			// 
			this.tsmiTocBinView.Name = "tsmiTocBinView";
			this.tsmiTocBinView.Size = new System.Drawing.Size(152, 22);
			this.tsmiTocBinView.Text = "&Bin View";
			// 
			// tsmiTocExplorerView
			// 
			this.tsmiTocExplorerView.Name = "tsmiTocExplorerView";
			this.tsmiTocExplorerView.Size = new System.Drawing.Size(210, 24);
			this.tsmiTocExplorerView.Text = "Show in &Folder";
			// 
			// PanelTOC
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitToc);
			this.Controls.Add(tsHead);
			this.Name = "PanelTOC";
			this.Size = new System.Drawing.Size(216, 283);
			tsHead.ResumeLayout(false);
			tsHead.PerformLayout();
			this.splitToc.Panel1.ResumeLayout(false);
			this.splitToc.Panel2.ResumeLayout(false);
			this.splitToc.ResumeLayout(false);
			this.cmsToc.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitToc;
		private System.Windows.Forms.TreeView tvToc;
		private Plugin.ApkImageView.Controls.ReflectionListView lvInfo;
		private System.Windows.Forms.TextBox txtInfo;
		private System.Windows.Forms.ContextMenuStrip cmsToc;
		private System.Windows.Forms.ToolStripMenuItem tsmiTocUnload;
		private System.Windows.Forms.ToolStripMenuItem tsmiOpenFile;
		private System.Windows.Forms.ToolStripMenuItem tsmiOpenBase64;
		private System.Windows.Forms.ToolStripSplitButton tsbnOpen;
		private AlphaOmega.Windows.Forms.SearchGrid gridSearch;
		private System.Windows.Forms.ToolStripMenuItem tsmiTocBinView;
		private System.Windows.Forms.ToolStripMenuItem tsmiTocExplorerView;

	}
}
