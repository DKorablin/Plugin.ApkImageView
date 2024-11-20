using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using AlphaOmega.Debug;
using AlphaOmega.Debug.Manifest;
using AlphaOmega.Windows.Forms;
using Plugin.ApkImageView.Bll;
using Plugin.ApkImageView.Directory;
using SAL.Windows;

namespace Plugin.ApkImageView
{
	public partial class PanelTOC : UserControl
	{
		private const String Caption = "Android Package View";
		#region Properties

		private PluginWindows Plugin => (PluginWindows)this.Window.Plugin;

		private IWindow Window => (IWindow)base.Parent;

		private SystemImageList SmallImageList { get; set; }

		private SectionNodeType? SelectedHeader => tvToc.SelectedNode.Tag as SectionNodeType?;

		private String SelectedPE
		{
			get
			{
				TreeNode node = tvToc.SelectedNode;
				while(node.Parent != null)
					node = node.Parent;
				return (String)node.Tag;
			}
		}
		#endregion Properties

		public PanelTOC()
		{
			this.InitializeComponent();
			gridSearch.TreeView = tvToc;
			this.SmallImageList = new SystemImageList(SystemImageListSize.SmallIcons);
			SystemImageListHelper.SetImageList(tvToc, this.SmallImageList, false);
		}

		protected override void OnCreateControl()
		{
			this.Window.Closing += new EventHandler<CancelEventArgs>(Window_Closing);
			lvInfo.Plugin = this.Plugin;

			String[] loadedFiles = this.Plugin.Settings.LoadedFiles;
			foreach(String file in loadedFiles)
				this.FillToc(file);
			this.ChangeTitle();

			this.Plugin.Binaries.PeListChanged += new EventHandler<PeListChangedEventArgs>(Plugin_PeListChanged);
			this.Plugin.Packages.PeListChanged += new EventHandler<PeListChangedEventArgs>(Plugin_PeListChanged);
			this.Plugin.Settings.PropertyChanged += Settings_PropertyChanged;
			base.OnCreateControl();
		}

		private void Window_Closing(Object sender, CancelEventArgs e)
		{
			this.Plugin.Binaries.PeListChanged -= new EventHandler<PeListChangedEventArgs>(Plugin_PeListChanged);
			this.Plugin.Packages.PeListChanged -= new EventHandler<PeListChangedEventArgs>(Plugin_PeListChanged);
			this.Plugin.Settings.PropertyChanged -= Settings_PropertyChanged;
		}

		/// <summary>Изменить заголовок окна</summary>
		private void ChangeTitle()
			=> this.Window.Caption = tvToc.Nodes.Count > 0
				? $"{PanelTOC.Caption} ({tvToc.Nodes.Count})"
				: this.Window.Caption = PanelTOC.Caption;

		/// <summary>Поиск узла в дереве по пути к файлу</summary>
		/// <param name="filePath">Путь к файлу</param>
		/// <returns>Найденный узел в дереве или null</returns>
		private TreeNodePackage FindNode(String fileKey)
		{
			foreach(TreeNodePackage node in tvToc.Nodes)
				if(node.PathKey == fileKey)
					return node;
			return null;
		}

		protected override Boolean ProcessDialogKey(Keys keyData)
		{
			switch(keyData)
			{
			case Keys.Control | Keys.O:
				this.tsbnOpen_Click(this, EventArgs.Empty);
				return true;
			default:
				return base.ProcessDialogKey(keyData);
			}
		}

		private void Plugin_PeListChanged(Object sender, PeListChangedEventArgs e)
		{
			if(base.InvokeRequired)
				base.Invoke((MethodInvoker)delegate { this.Plugin_PeListChanged(sender, e); });
			else
				switch(e.Type)
				{
				case PeListChangeType.Added:
					TreeNodePackage root = this.FillToc(e.FilePath);
					if(root != null)
						tvToc.SelectedNode = root;
					break;
				case PeListChangeType.Changed:
					TreeNodePackage node = this.FindNode(e.FilePath);
					if(node.IsSelected)
						this.tvToc_AfterSelect(sender, new TreeViewEventArgs(node));
					break;
				case PeListChangeType.Removed:
					this.FindNode(e.FilePath).Remove();
					break;
				default:
					throw new NotImplementedException(e.Type.ToString());
				}
			this.ChangeTitle();
		}

		private void Settings_PropertyChanged(Object sender, PropertyChangedEventArgs e)
		{
			switch(e.PropertyName)
			{
			case nameof(PluginSettings.MaxArrayDisplay):
			case nameof(PluginSettings.ShowAsHexValue):
				if(tvToc.SelectedNode != null)
					this.tvToc_AfterSelect(sender, new TreeViewEventArgs(tvToc.SelectedNode));
				break;
			}
		}

		private void OpenBinaryDocument(TreeNodePackage node)
		{
			if(node == null || node.IsNull())
				return;

			if(this.Plugin.BinaryViewers.TryGetValue(node.NodeType, out Type wnd))
				this.Plugin.HostWindows.Windows.CreateWindow(this.Plugin,
					wnd.ToString(),
					true,
					DockState.Document,
					new DocumentBaseSettings() { FilePath = node.Path, Node = node.NodeType, });
		}

		private void OpenDirectoryDocument(TreeNodePackage node)
		{
			if(node == null || node.IsNull())
				return;

			if(this.Plugin.DirectoryViewers.TryGetValue(node.NodeType, out Type wnd))
				this.Plugin.HostWindows.Windows.CreateWindow(this.Plugin,
					wnd.ToString(),
					true,
					DockState.Document,
					new DocumentBaseSettings() { FilePath = node.Path, Node = node.NodeType, });
		}

		private TreeNodePackage FillToc(String filePath)
		{
			//Проверка на уже добавленные файлы в дерево
			TreeNodePackage n = this.FindNode(filePath);
			if(n != null)
			{
				tvToc.SelectedNode = n;
				return null;
			}

			TreeNodePackage result = new TreeNodePackage(filePath);
			result.ImageIndex = result.SelectedImageIndex = this.SmallImageList.IconIndex(filePath);
			tvToc.Nodes.Add(result);
			return result;
		}

		private void tsbnOpen_Click(Object sender, EventArgs e)
			=> this.tsbnOpen_DropDownItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiOpenFile));

		private void tsbnOpen_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			tsbnOpen.DropDown.Close(ToolStripDropDownCloseReason.ItemClicked);

			if(e.ClickedItem == tsmiOpenFile)
			{
				using(OpenFileDialog dlg = new OpenFileDialog() { CheckFileExists = true, Multiselect = true, Filter = "Supported files|*.apk;*.dex;*.jar;AndroidManifest.xml|Android package (*.apk)|*.apk|Dalvik files (*.dex)|*.dex|Java Package (*.jar)|*.jar|Android Manifest (AndroidManifest.xml)|AndroidManifest.xml|All files (*.*)|*.*", Title = "Choose Android Package", })
					if(dlg.ShowDialog() == DialogResult.OK)
						foreach(String filePath in dlg.FileNames)
							this.Plugin.OpenFile(filePath, false);

			} else if(e.ClickedItem == tsmiOpenBase64)
			{
				using(HexLoadDlg dlg = new HexLoadDlg())
					if(dlg.ShowDialog() == DialogResult.OK)
						this.Plugin.Binaries.OpenFile(null, dlg.Result);
			} else
				throw new NotSupportedException(e.ClickedItem.ToString());
		}

		private void cmsToc_Opening(Object sender, CancelEventArgs e)
		{
			TreeNodePackage node = (TreeNodePackage)tvToc.SelectedNode;
			tsmiTocUnload.Visible = tsmiTocExplorerView.Visible = tsmiTocBinView.Visible = false;
			Boolean showUnload = false;
			Boolean showBinView = false;

			if(node != null)
			{
				tsmiTocUnload.Visible = tsmiTocExplorerView.Visible = showUnload = node.Parent == null;//PE File

				if(!node.IsNull() && this.Plugin.BinaryViewers.ContainsKey(node.NodeType))
					tsmiTocBinView.Visible = showBinView = true;
			}

			e.Cancel = !showUnload && !showBinView;
		}

		private void cmsToc_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			TreeNodePackage node = (TreeNodePackage)tvToc.SelectedNode;
			if(e.ClickedItem == tsmiTocUnload)
			{
				if(node != null && node.IsFile)
					this.Plugin.CloseFile(node.PathKey);
			} else if(e.ClickedItem == tsmiTocBinView)
			{//TODO: Тут не поддерживаются файлы ресурсов и манифестов
				String fileKey = node.GetFileNode().PathKey;
				this.OpenBinaryDocument(node);
			} else if(e.ClickedItem == tsmiTocExplorerView)
			{
				String filePath = this.SelectedPE;
				Shell32.OpenFolderAndSelectItem(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));
			} else
				throw new NotImplementedException(e.ClickedItem.ToString());
		}

		private void tvToc_AfterSelect(Object sender, TreeViewEventArgs e)
		{
			lvInfo.Items.Clear();
			lvInfo.Visible = true;
			txtInfo.Text = String.Empty;
			txtInfo.Visible = false;

			splitToc.Panel2Collapsed = false;

			TreeNodePackage node = e.Node as TreeNodePackage;
			if(node == null)//Exception node
				lvInfo.DataBind(e.Node.Tag);
			else if(node.IsFile && node.Parent == null)//Описание файла
				lvInfo.DataBind(new FileInfo(node.Path[0]));
			else
			{
				try
				{
					switch(node.NodeType)
					{
					case SectionNodeType.Header:
					case SectionNodeType.MapList:
					case SectionNodeType.Sections://Директория DEX файла
						base.Cursor = Cursors.WaitCursor;

						Object section = this.Plugin.GetNodeDataRecursive(node.Path);
						lvInfo.DataBind(section);
						break;
					case SectionNodeType.Package://Android Package
						base.Cursor = Cursors.WaitCursor;

						Object package = this.Plugin.GetNodeDataRecursive(node.Path);
						lvInfo.DataBind(package);
						break;
					case SectionNodeType.ApkManifest://Apk Manifest
						base.Cursor = Cursors.WaitCursor;

						AndroidManifest xml = (AndroidManifest)this.Plugin.GetNodeDataRecursive(node.Path);
						txtInfo.Text = xml.Node.ConvertToString();
						lvInfo.Visible = false;
						txtInfo.Visible = true;
						break;
					case SectionNodeType.Resource://Apk Resources
						base.Cursor = Cursors.WaitCursor;

						ArscFile resource = (ArscFile)this.Plugin.GetNodeDataRecursive(node.Path);
						lvInfo.DataBind(resource.ResourceMap);
						break;
					default:
						lvInfo.DataBind(node.Tag);//Generic объект
						break;
					}
				} finally
				{
					base.Cursor = Cursors.Default;
				}
			}
		}

		private void tvToc_MouseClick(Object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				TreeViewHitTestInfo info = tvToc.HitTest(e.Location);
				if(info.Node != null)
				{
					tvToc.SelectedNode = info.Node;
					cmsToc.Show(tvToc, e.Location);
				}
			}
		}

		private void tvToc_MouseDoubleClick(Object sender, MouseEventArgs e)
		{
			TreeViewHitTestInfo info = tvToc.HitTest(e.Location);

			TreeNodePackage node = info.Node as TreeNodePackage;
			this.OpenDirectoryDocument(node);
		}

		private void tvToc_KeyDown(Object sender, KeyEventArgs e)
		{
			switch(e.KeyData)
			{
			case Keys.Delete:
			case Keys.Back:
				this.cmsToc_ItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiTocUnload));
				e.Handled = true;
				break;
			case Keys.Return:
				TreeNodePackage node = tvToc.SelectedNode as TreeNodePackage;
				this.OpenDirectoryDocument(node);
				e.Handled = true;
				break;
			case Keys.C | Keys.Control:
				if(tvToc.SelectedNode != null)
					Clipboard.SetText(tvToc.SelectedNode.Text);
				e.Handled = true;
				break;
			case Keys.V | Keys.Control:
				if(Clipboard.ContainsText())
				{
					String filePath = Clipboard.GetText();
					if(File.Exists(filePath))
						this.Plugin.OpenFile(filePath, false);
				}
				e.Handled = true;
				break;
			}
		}

		private void tvToc_BeforeExpand(Object sender, TreeViewCancelEventArgs e)
		{
			TreeNodePackage node = (TreeNodePackage)e.Node;
			if(e.Action == TreeViewAction.Expand && node.IsClosedEmptyNode())
			{
				try
				{
					TreeNodePackage fileNode = node.GetFileNode();
					switch(node.NodeType)
					{
					case SectionNodeType.Package:
						ApkFile zip = this.Plugin.Packages.LoadFile(fileNode.PathKey);
						if(node.PathKey != fileNode.PathKey)
						{
							Byte[] entry = zip.GetFile(node.Text);
							zip = this.Plugin.Packages.LoadFile(node.PathKey, entry);
						}
						node.ExpandPackage(zip);
						break;
					case SectionNodeType.Dex:
						DexFile dex;
						if(node.PathKey == fileNode.PathKey)
							dex = this.Plugin.Binaries.LoadFile(fileNode.PathKey);
						else
						{
							ApkFile zip1 = this.Plugin.Packages.LoadFile(fileNode.PathKey);
							Byte[] entry1 = zip1.GetFile(node.Text);
							dex = this.Plugin.Binaries.LoadFile(node.PathKey, entry1);
						}
						node.ExpandDex(dex);
						break;
					}
				} catch(Exception exc)
				{
					e.Node.Nodes[0].SetException(exc);
					e.Node.Nodes[0].Tag = exc;
					return;
				}
			}
		}

		private void tvToc_DragEnter(Object sender, DragEventArgs e)
			=> e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;

		private void tvToc_DragDrop(Object sender, DragEventArgs e)
		{
			foreach(String filePath in (String[])e.Data.GetData(DataFormats.FileDrop))
				this.Plugin.OpenFile(filePath, false);
		}

		private void txtInfo_KeyDown(Object sender, KeyEventArgs e)
		{
			switch(e.KeyData)
			{
			case Keys.A | Keys.Control:
				txtInfo.SelectAll();
				e.Handled = true;
				break;
			}
		}
	}
}