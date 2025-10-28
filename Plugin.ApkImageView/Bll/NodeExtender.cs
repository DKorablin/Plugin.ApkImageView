using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plugin.ApkImageView.Bll
{
	internal static class NodeExtender
	{
		internal static readonly Color NullColor = Color.Gray;
		internal static readonly Color ExceptionColor = Color.Red;
		private static Font _nullFont;
		internal static Font NullFont
			=> NodeExtender._nullFont ?? (NodeExtender._nullFont = new Font(Control.DefaultFont, FontStyle.Italic));

		/// <summary>Set the default style for the node</summary>
		/// <param name="node"></param>
		public static void SetDefaultStyle(this TreeNode node)
		{
			node.NodeFont = Control.DefaultFont;
			node.ForeColor = Control.DefaultForeColor;
		}

		/// <summary>The list item contains a null value</summary>
		/// <param name="item">List item</param>
		/// <returns>The list item contains an exception</returns>
		public static Boolean IsException(this ListViewItem item)
			=> item.ForeColor == NodeExtender.ExceptionColor;

		/// <summary>The node is in the exception state</summary>
		/// <param name="node">Node</param>
		/// <returns>An exception has been written to the node</returns>
		public static Boolean IsException(this TreeNode node)
			=> node.ForeColor == NodeExtender.ExceptionColor;

		public static Boolean IsException(this ToolStripItem item)
			=> item.ForeColor == NodeExtender.ExceptionColor;

		/// <summary>The root node is closed and its contents need to be loaded</summary>
		/// <param name="node">Node</param>
		/// <returns>The node is closed and its children need to be loaded</returns>
		public static Boolean IsClosedRootNode(this TreeNode node)
			=> node.Parent == null && node.IsClosedEmptyNode();

		/// <summary>The node is closed and its contents need to be loaded</summary>
		/// <param name="node">Node</param>
		/// <returns>The node is closed and its children need to be loaded</returns>
		public static Boolean IsClosedEmptyNode(this TreeNode node)
			=> node.Nodes.Count == 1 && (node.Nodes[0].Text.Length == 0 || node.Nodes[0].IsException());

		public static void SetNull(this ListViewItem item)
		{
			item.Font = NodeExtender.NullFont;
			item.ForeColor = NodeExtender.NullColor;
		}

		/// <summary>Set the node to null</summary>
		/// <param name="node">Node</param>
		public static void SetNull(this TreeNode node)
		{
			node.NodeFont = NodeExtender.NullFont;
			node.ForeColor = NodeExtender.NullColor;
		}

		public static void SetNull(this ToolStripItem item)
		{
			item.Font = NodeExtender.NullFont;
			item.ForeColor = NodeExtender.NullColor;
		}

		public static Boolean IsNull(this ListViewItem item)
			=> item.ForeColor == NodeExtender.NullColor;

		/// <summary>Node contains null</summary>
		/// <param name="node">The tree node</param>
		/// <returns>User contains null</returns>
		public static Boolean IsNull(this TreeNode node)
			=> node.ForeColor == NodeExtender.NullColor;

		public static Boolean IsNull(this ToolStripItem item)
			=> item.ForeColor == NodeExtender.NullColor;

		/// <summary>Write an exception to the node</summary>
		/// <param name="node">Node</param>
		/// <param name="exc">Exception</param>
		public static void SetException(this TreeNode node, Exception exc)
			=> node.SetException(exc.Message);

		/// <summary>Write an exception to a list item</summary>
		/// <param name="item">List item</param>
		public static void SetException(this ListViewItem item)
			=> item.ForeColor = NodeExtender.ExceptionColor;

		/// <summary>Write an exception message to the node</summary>
		/// <param name="node">Node</param>
		/// <param name="exceptionMessage">Message describing the exception</param>
		public static void SetException(this TreeNode node, String exceptionMessage)
		{
			node.ForeColor = NodeExtender.ExceptionColor;
			node.Text = exceptionMessage;
		}

		public static void SetException(this ToolStripItem item, Exception exc)
		{
			item.ForeColor = NodeExtender.ExceptionColor;
			item.Text = exc.Message;
		}

		internal static void DisposeFonts()
		{
			NodeExtender._nullFont?.Dispose();
			NodeExtender._nullFont = null;
		}
	}
}