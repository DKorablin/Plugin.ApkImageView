using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AlphaOmega.Debug;
using AlphaOmega.Debug.Arsc;

namespace Plugin.ApkImageView.Directory
{
	public partial class DocumentResource : DocumentBase
	{
		public DocumentResource()
			: base(SectionNodeType.Resource)
			=> InitializeComponent();

		protected override void ShowFile(Object node)
		{
			ArscFile file = (ArscFile)node;

			lvResource.Items.Clear();
			List<ListViewItem> itemsToAdd = new List<ListViewItem>();
			foreach(var item in file.ResourceMap)
				itemsToAdd.Add(new ListViewItem(new String[] { base.Plugin.FormatValue(item.Key), String.Join(", ", item.Value.Select(p=>p.Value).ToArray()), }) { Tag = item.Value.ToArray(), });

			lvResource.Items.AddRange(itemsToAdd.ToArray());
			lvResource.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		private void lvResource_SelectedIndexChanged(Object sender, EventArgs e)
		{
			if(!splitMain.Panel2Collapsed)
				lvDetails.Items.Clear();

			ListViewItem listItem = lvResource.SelectedItems.Count > 0 ? lvResource.SelectedItems[0] : null;
			if(listItem == null)
				return;

			ResourceRow[] resourceRows = (ResourceRow[])listItem.Tag;

			if(resourceRows != null)
			{
				splitMain.Panel2Collapsed = false;
				List<ListViewItem> itemsToAdd = new List<ListViewItem>();
				foreach(ResourceRow value in resourceRows)
					itemsToAdd.Add(new ListViewItem(new String[] { value.Value, value.DataType.ToString(), base.Plugin.FormatValue(value.Raw), }));

				lvDetails.Items.AddRange(itemsToAdd.ToArray());
				lvDetails.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}
	}
}