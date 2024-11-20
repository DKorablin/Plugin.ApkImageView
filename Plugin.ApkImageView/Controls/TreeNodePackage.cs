using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AlphaOmega.Debug;
using Plugin.ApkImageView.Bll;

namespace Plugin.ApkImageView
{
	internal class TreeNodePackage : TreeNode
	{
		public String[] Path { get; }
		public String PathKey => Constant.CreatePathKey(this.Path);
		public SectionNodeType NodeType { get; }
		public Boolean IsFile { get; private set; }

		public TreeNodePackage(String path)
			: this(new String[] { path, }, Constant.GetSectionTypeByExtension(path), true)
		{ }

		public TreeNodePackage(String[] path, SectionNodeType nodeType, Boolean isFile)
			: base(path[path.Length-1])
		{
			this.Path = path;
			this.NodeType = nodeType;
			this.IsFile = isFile;

			switch(nodeType)
			{
			case SectionNodeType.Dex:
			case SectionNodeType.Package:
				this.SetClosedEmptyNode();
				break;
			}
		}

		public TreeNodePackage GetFileNode()
		{
			TreeNodePackage result = this;
			do
			{
				if(result.IsFile)
					break;
				else
					result = (TreeNodePackage)result.Parent;

			} while(result != null);

			return result;
		}

		public TreeNodePackage GetRootNode()
		{
			TreeNodePackage result = this;
			do
			{
				if(result.Parent == null)
					break;
				else
					result = (TreeNodePackage)result.Parent;
			} while(result != null);

			return result;
		}

		public void ExpandDex(DexFile dex)
		{
			if(dex == null)
				base.Nodes[0].SetException("File not found");
			else
			{
				this.IsFile = true;
				TreeNodePackage[] nodes = new TreeNodePackage[] {
					CreateDirectoryNode(this.Path, Properties.Resources.Section_Header, SectionNodeType.Header, false),
					CreateDirectoryNode(this.Path, Properties.Resources.Section_MapList, SectionNodeType.MapList, dex.map_list.Length == 0),
					CreateDirectoryNode(this.Path, Properties.Resources.Section_Sections, SectionNodeType.Sections, false),
				};
				base.Nodes.Clear();
				base.Nodes.AddRange(nodes);
			}
		}

		public void ExpandPackage(ApkFile zip)
		{
			if(zip == null)
				base.Nodes[0].SetException("File not found");
			else
			{
				this.IsFile = true;
				List<TreeNodePackage> nodes = new List<TreeNodePackage>();
				foreach(String filePath in zip.EnumerateFiles())
					switch(Constant.GetSectionTypeByExtension(filePath))
					{
					case SectionNodeType.ApkManifest:
						nodes.Add(CreateDirectoryNode(this.Path, filePath, SectionNodeType.ApkManifest, false));
						break;
					case SectionNodeType.Package:
						nodes.Add(CreateDirectoryNode(this.Path, filePath, SectionNodeType.Package, false));
						break;
					case SectionNodeType.Dex:
						nodes.Add(CreateDirectoryNode(this.Path, filePath, SectionNodeType.Dex, false));
						break;
					case SectionNodeType.Resource:
						nodes.Add(CreateDirectoryNode(this.Path, filePath, SectionNodeType.Resource, false));
						break;
					}

				base.Nodes.Clear();
				base.Nodes.AddRange(nodes.ToArray());
			}
		}

		/// <summary>Добавить закрытый узел, в который потом можно загрузить содержимое</summary>
		/// <param name="node">Узел</param>
		private void SetClosedEmptyNode()
			=> base.Nodes.Add(new TreeNode(String.Empty) { ImageIndex = -1, SelectedImageIndex = -1, });

		private static TreeNodePackage CreateDirectoryNode(String[] path, String text, SectionNodeType type, Boolean isEmpty)
		{
			String[] childPath;
			if(text == null)
			{
				childPath = path;
				text = Constant.GetHeaderName(type);
			} else
			{
				childPath = new String[path.Length + 1];
				Array.Copy(path, childPath, path.Length);
				childPath[path.Length] = text;
			}
			
			TreeNodePackage result = new TreeNodePackage(childPath, type, false) { ImageIndex = -1, SelectedImageIndex = -1, };
			if(isEmpty)
				result.SetNull();
			else
				result.ForeColor = Color.Empty;

			return result;
		}
	}
}