using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using AlphaOmega.Debug;
using AlphaOmega.Debug.Dex;
using Plugin.ApkImageView.Bll;
using Plugin.ApkImageView.Controls;

namespace Plugin.ApkImageView.Directory
{
	public partial class DocumentTables : DocumentBase
	{
		private readonly ToolStripMenuItem _menuItemGoTo;
		private readonly TreeNode _rootNode;

		public DocumentTables()
			: base(SectionNodeType.Sections)
		{
			this.InitializeComponent();
			gridSearch.ListView = lvHeaps;

			this._menuItemGoTo = new ToolStripMenuItem("GoTo");
			this._menuItemGoTo.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.menuItemGoTo_DropDownItemClicked);
			lvHeaps.ContextMenuStrip.Items.Insert(0, this._menuItemGoTo);
			lvHeaps.ContextMenuStrip.Opening += new CancelEventHandler(this.ContextMenuStrip_Opening);

			this._rootNode = new TreeNode(Constant.GetHeaderName(SectionNodeType.Sections));
			tvHierarchy.Nodes.AddRange(new TreeNode[] { this._rootNode, });
		}

		protected override void ShowFile(Object node)
		{
			lvHeaps.Plugin = base.Plugin;
			Boolean isDirty = this._rootNode.Nodes.Count > 0;
			DexFile dex = (DexFile)node;

			foreach(ITable table in dex.GetSectionTables())
				this.AddTreeNode(this._rootNode, table);

			if(!isDirty)
				this._rootNode.Expand();
			else if(isDirty && tvHierarchy.SelectedNode != null)
				this.tvHierarchy_AfterSelect(null, new TreeViewEventArgs(tvHierarchy.SelectedNode));
		}

		private void AddTreeNode(TreeNode root, ITable table)
		{
			TreeNode node = this.FindTreeNode(root, table.Type);
			if(node == null)
			{
				node = new TreeNode() { Tag = table.Type, };
				root.Nodes.Add(node);
			}

			node.Text = $"{table.Type} ({table.RowsCount:n0})";
			if(table.RowsCount == 0)
				node.SetNull();
		}

		private void ContextMenuStrip_Opening(Object sender, CancelEventArgs e)
		{
			Boolean showGoTo = false;
			if(!base.Plugin.Settings.ShowBaseMetaTables)
			{
				this._menuItemGoTo.DropDownItems.Clear();
				ListViewItem item = lvHeaps.SelectedItems.Count == 1 ? lvHeaps.SelectedItems[0] : null;
				if(item != null)
				{
					Type rowType = item.Tag.GetType();
					foreach(ColumnHeader column in lvHeaps.Columns)
					{
						Object cell = rowType.InvokeMember(column.Text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, item.Tag, null);
						IEnumerable cellEnum = cell as IEnumerable;//TODO: Not implemented
						if(cellEnum?.GetType().IsBclType() == true)
							continue;//Отсекаю System.String

						if(cell is IRowPointer)
						{
							this._menuItemGoTo.DropDownItems.Add(new ToolStripMenuItem(item.SubItems[column.Index].Text) { Tag = column.Index, });
							showGoTo = true;
						} else if(cellEnum != null)
							throw new NotImplementedException();
					}
				}
			}
			this._menuItemGoTo.Visible = showGoTo;
		}

		private void menuItemGoTo_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			lvHeaps.ContextMenuStrip.Hide();
			ListViewItem item = lvHeaps.SelectedItems.Count == 1 ? lvHeaps.SelectedItems[0] : null;
			if(item != null)
			{
				base.SuspendLayout();
				try
				{
					String columnName = lvHeaps.Columns[(Int32)e.ClickedItem.Tag].Text;
					Type rowType = item.Tag.GetType();

					Object cell = item.Tag is IRow row
						? row[columnName].Value
						: rowType.InvokeMember(columnName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, item.Tag, null);

					IRowPointer rowPointer = (IRowPointer)cell;
					tvHierarchy.SelectedNode = this.FindTreeNode(rowPointer.TableType);
					foreach(ListViewItem subNode in lvHeaps.Items)
					{
						IRow subRow = subNode.Tag as IRow;
						if(subNode.Tag is IBaseRow baseRow)
							subRow = baseRow.Row;
						if(subRow != null && subRow.Index == rowPointer.Index)
						{
							lvHeaps.Focus();
							subNode.Selected = true;
							subNode.Focused = true;
							subNode.EnsureVisible();
							break;
						}
					}
				} finally
				{
					base.ResumeLayout();
				}
			}
		}

		private void tvHierarchy_DragEnter(Object sender, DragEventArgs e)
			=> e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;

		private void tvHierarchy_DragDrop(Object sender, DragEventArgs e)
		{
			String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
			if(files.Length > 0)
				base.OpenFile(files[0]);
		}

		private void tvHierarchy_AfterSelect(Object sender, TreeViewEventArgs e)
		{
			lvHeaps.Items.Clear();

			if(e.Node.Parent != null)
			{
				try
				{
					base.Cursor = Cursors.WaitCursor;
					TableType tableType = (TableType)e.Node.Tag;

					Object table = null;
					if(!base.Plugin.Settings.ShowBaseMetaTables)
						table = this.GetTableBase(tableType.ToString());

					if(table == null)
						lvHeaps.DataBind(((DexFile)base.GetFile()).GetSectionTable(tableType));
					else
						lvHeaps.DataBind(((IEnumerable)table));

					//Сокрытие ссылок на другие таблицы
					splitDetails.Panel2Collapsed = true;
					foreach(TabPage tab in tabPointers.TabPages)
					{
						foreach(Control tabCtrl in tab.Controls)
							tabCtrl.Dispose();
						tab.Dispose();
					}
				} finally
				{
					base.Cursor = Cursors.Default;
				}
			}
			tabPointers.TabPages.Clear();//Удаление всех ранее созданных табов
		}

		private void lvHeaps_SelectedIndexChanged(Object sender, EventArgs e)
		{
			if(!base.Plugin.Settings.ShowBaseMetaTables)
			{
				//Application.DoEvents();
				ListViewItem item = lvHeaps.SelectedItems.Count == 1 ? lvHeaps.SelectedItems[0] : null;
				if(item == null)
					foreach(TabPage tab in tabPointers.TabPages)
						((ListView)tab.Controls[0]).Items.Clear();
				else
				{
					Type rowType = item.Tag.GetType();
					foreach(ColumnHeader column in lvHeaps.Columns)
					{
						Object cell = item.Tag is IRow row
							? row[column.Text].Value
							: rowType.InvokeMember(column.Text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, item.Tag, null);

						IRowPointer rowPointer = cell as IRowPointer;
						IBaseRow cellRow = cell as IBaseRow;
						IEnumerable cellEnum = cell as IEnumerable;
						Byte[] cellByteArray = cell as Byte[];
						if(cellEnum?.GetType().IsBclType() == true)
							continue;//Отсекаю System.String TODO: Не реботает с Byte[] (cellByteArray)

						if(rowPointer != null || cellEnum != null || cellRow != null || cellByteArray != null)
						{
							splitDetails.Panel2Collapsed = false;
							Control ctlCellPointer = this.GetOrCreateTabControl(column.Text, rowPointer != null || cellRow != null, cellEnum != null, cellByteArray != null);

							if(rowPointer != null)
							{//TODO: CellPointer не реализован
								ITables tables = rowPointer.Root;
								Object baseMetaTable = this.GetTableBase(rowPointer.TableType.ToString());
								if(baseMetaTable != null)
								{
									Object baseMetaRow = baseMetaTable.GetType().InvokeMember(String.Empty, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, baseMetaTable, new Object[] { rowPointer.Index, });
									((ReflectionListView)ctlCellPointer).DataBind(baseMetaRow);
								}
							} else if(cellRow != null)
								((ReflectionListView)ctlCellPointer).DataBind(cellRow);
							else if(cellEnum != null)
								((ReflectionArrayListView)ctlCellPointer).DataBind(cellEnum);
							else if(cellByteArray != null)
								((ByteViewer)ctlCellPointer).SetBytes(cellByteArray);
							else
								throw new NotSupportedException();
						}
					}
				}
			}
		}

		private Object GetTableBase(String tableType)
		{
			DexFile dex = (DexFile)base.GetFile();
			PropertyInfo property = dex.GetType().GetProperty(tableType);
			return property?.GetValue(dex, null);
		}

		private TreeNode FindTreeNode(Object tableType)
		{
			foreach(TreeNode node in tvHierarchy.Nodes)
				if(node.Tag.Equals(tableType))
					return node;
				else
				{
					TreeNode result = this.FindTreeNode(node, tableType);
					if(result != null)
						return result;
				}
			return null;
		}

		private TreeNode FindTreeNode(TreeNode root, Object tableType)
		{
			foreach(TreeNode node in root.Nodes)
				if(node.Tag.Equals(tableType))
					return node;
				else
				{
					TreeNode result = this.FindTreeNode(node, tableType);
					if(result != null)
						return result;
				}
			return null;
		}

		private Control GetOrCreateTabControl(String columnText, Boolean isListView, Boolean isArrayListView, Boolean isByteView)
		{
			TabPage tabColumn = tabPointers.TabPages.Cast<TabPage>().FirstOrDefault(p => p.Text == columnText);
			Control result;

			if(tabColumn == null)
			{
				tabColumn = new TabPage(columnText);

				result = this.CreateTabControl(isListView, isArrayListView, isByteView);
				tabColumn.Controls.Add(result);
				tabPointers.TabPages.Add(tabColumn);
			} else
			{
				result = tabColumn.Controls[0];
				if(isListView && !(result is ReflectionListView)
					|| isArrayListView && !(result is ReflectionArrayListView)
					|| isByteView && !(result is ByteViewer))
				{
					tabColumn.Controls.Clear();
					result = this.CreateTabControl(isListView, isArrayListView, isByteView);
					tabColumn.Controls.Add(result);
				}
			}

			return result;
		}

		private Control CreateTabControl(Boolean isListView, Boolean isArrayListView, Boolean isByteView)
		{
			Control result;
			if(isListView)
				result = new ReflectionListView()
				{
					Plugin = this.Plugin,
					Dock = DockStyle.Fill,
					HeaderStyle = ColumnHeaderStyle.None,
					View = View.Details,
				};
			else if(isArrayListView)
				result = new ReflectionArrayListView()
				{
					Plugin = this.Plugin,
					Dock = DockStyle.Fill,
					HeaderStyle = ColumnHeaderStyle.Clickable,
					Sorting = SortOrder.Ascending,
					View = View.Details,
				};
			else if(isByteView)
				result = new ByteViewer();
			else
				throw new NotSupportedException();

			return result;
		}
	}
}