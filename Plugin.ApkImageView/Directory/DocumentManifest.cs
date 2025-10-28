using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using AlphaOmega.Debug.Manifest;

namespace Plugin.ApkImageView.Directory
{
	public partial class DocumentManifest : DocumentBase
	{
		public DocumentManifest()
			: base(SectionNodeType.ApkManifest)
			=> this.InitializeComponent();

		protected override void ShowFile(Object node)
		{
			AndroidManifest manifest = (AndroidManifest)node;

			TreeNode root = new TreeNode(manifest.Node.NodeName)
			{
				Tag = manifest
			};
			this.FillNodeRecursive(root, manifest);
			tvXml.Nodes.Add(root);
		}

		private void tvXml_AfterSelect(Object sender, TreeViewEventArgs e)
		{
			splitMain.Panel2Collapsed = false;
			pgInfo.SelectedObject = tvXml.SelectedNode.Tag;
		}

		private void FillNodeRecursive(TreeNode parentNode, ApkNode parent)
		{
			MethodInfo[] methods = parent.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			foreach(MethodInfo method in methods)
				if(method.GetParameters().Length == 0)
				{
					if(DocumentManifest.IsGenericType(method.ReturnType, typeof(IEnumerable<>)))
					{
						Object result = method.Invoke(parent, null);
						foreach(Object item in (System.Collections.IEnumerable)result)
						{
							ApkNode node = (ApkNode)item;
							TreeNode childNode = new TreeNode(node.Node.NodeName) { Tag = node };
							this.FillNodeRecursive(childNode, node);
							parentNode.Nodes.Add(childNode);
						}
					} else if(method.ReturnType.BaseType == typeof(ApkNode)
						|| DocumentManifest.IsGenericType(method.ReturnType.BaseType, typeof(ApkNodeT<>)))
					{
						ApkNode node = (ApkNode)method.Invoke(parent, null);
						TreeNode childNode = new TreeNode(node.Node.NodeName) { Tag = node };
						this.FillNodeRecursive(childNode, node);
						parentNode.Nodes.Add(childNode);
					}
				}
		}

		private static Boolean IsGenericType(Type type, Type genericType)
		{
			if(type.IsGenericType)
			{
				Type genericDef = type.GetGenericTypeDefinition();
				return genericDef == genericType;
			} else return false;
		}
	}
}