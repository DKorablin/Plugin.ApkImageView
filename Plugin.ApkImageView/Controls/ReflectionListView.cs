using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using AlphaOmega.Debug;
using AlphaOmega.Windows.Forms;
using Plugin.ApkImageView.Bll;

namespace Plugin.ApkImageView.Controls
{
	internal class ReflectionListView : ListView
	{
		private const Int32 ColumnNameIndex = 0;
		private const Int32 ColumnValueIndex = 1;
		public PluginWindows Plugin { get; set; }
		public ReflectionListView()
		{
			base.View = View.Details;
			base.FullRowSelect = true;
			base.HeaderStyle = ColumnHeaderStyle.None;
			base.MultiSelect = true;
			base.Sorting = SortOrder.Ascending;
			base.UseCompatibleStateImageBehavior = false;
			base.HideSelection = false;

			base.ContextMenuStrip = new ContextMenuStripCopy();
		}

		public void DataBind(IRow row)
		{
			if(this.Plugin == null)
				throw new InvalidOperationException("Plugin is null");

			base.SuspendLayout();
			base.Items.Clear();

			if(row != null)
			{
				List<ListViewItem> items = new List<ListViewItem>();
				foreach(IColumn column in row.Table.Columns)
					try
					{
						items.Add(this.CreateListItem(column,
							column.Name,
							this.Plugin.FormatValue(row[column].Value),
							null,
							false));
					} catch(Exception exc)
					{
						items.Add(this.CreateListItem(column,
							column.Name,
							exc.Message,
							null, true));
					}

				base.Items.AddRange(items.ToArray());
				base.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}

			base.ResumeLayout(false);
		}

		public void DataBind(Object row)
		{
			base.SuspendLayout();
			base.Items.Clear();

			if(row != null)
			{
				if(this.Plugin == null)
					throw new InvalidOperationException("Plugin is null");

				Type rowType = row.GetType();
				if(rowType.BaseType == typeof(Array))
				{
					Int32 index = 0;
					foreach(Object item in (Array)row)
						this.DataBindI(item, this.Plugin.FormatValue(index++));
				} else if(rowType.IsGenericType && rowType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
				{
					Int32 index = 0;
					foreach(Object item in (System.Collections.IEnumerable)row)
						this.DataBindI(item, this.Plugin.FormatValue(index++));
				} else
					this.DataBindI(row, null);

				//Такой код использовать нельзя. Т.к. изредка класс инкапсулирует дочерний массив
				/*IEnumerable ienum = row as IEnumerable;
				if(ienum != null)
				{
					Int32 index = 0;
					foreach(Object item in ienum)
						this.DataBindI(item, this.Plugin.FormatValue(index++));
				} else
					this.DataBindI(row, null);*/
				base.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
			base.ResumeLayout(false);
		}

		private void DataBindI(Object row, String groupName)
		{
			List<ListViewItem> items = new List<ListViewItem>();

			foreach(MemberInfo member in row.GetType().GetMembers().Where(p => p.MemberType == MemberTypes.Field || p.MemberType == MemberTypes.Property))
				items.Add(this.CreateReflectedListItem(row,
					member,
					groupName,
					delegate { return member.GetMemberValue(row); }));

			{
				/*if(member.GetMemberType() == typeof(AlphaOmega.Debug.WinNT.IMAGE_DATA_DIRECTORY))
				{//TODO: Extended parsing
					if(member.MemberType == MemberTypes.Field || ((PropertyInfo)member).GetGetMethod().GetParameters().Length == 0)
					{
						Object value = member.GetMemberValue(row);
						this.DataBindI(value, member.Name);
					}
				}*/
			}

			base.Items.AddRange(items.ToArray());
		}

		internal ListViewItem CreateReflectedListItem(Object row, MemberInfo info, String groupName, Func<Object> deleg)
		{
			if(groupName == null)
				groupName = info.MemberType.ToString();

			String value;
			Boolean isException = false;
			try
			{
				value = this.Plugin.FormatValue(info, deleg());
			} catch(TargetInvocationException exc)
			{
				isException = true;
				value = exc.InnerException.Message;
			} catch(Exception exc)
			{
				isException = true;
				value = exc.Message;
			}
			return this.CreateListItem(row, info.Name, value, groupName, isException);
		}

		private ListViewGroup GetGroup(String groupName)
			=> base.Groups[groupName]
				?? base.Groups.Add(groupName, groupName);

		public ListViewItem CreateListItem(Object row, MemberInfo member)
			=> this.CreateListItem(row,
				member.Name,
				member.GetMemberValue(row).ToString(),
				member.MemberType.ToString(),
				false);

		public ListViewItem CreateListItem(Object row, String name, String value, String groupName, Boolean exception)
		{
			ListViewItem result = new ListViewItem() { Tag = row, };
			if(!String.IsNullOrEmpty(groupName))
				result.Group = this.GetGroup(groupName);

			if(base.Columns.Count == 0)
				base.Columns.AddRange(new ColumnHeader[]
			{
				new ColumnHeader(){ Text = "Name", },
				new ColumnHeader(){ Text = "Value", },
			});

			String[] subItems = Array.ConvertAll<String, String>(new String[base.Columns.Count], delegate(String a) { return String.Empty; });
			result.SubItems.AddRange(subItems);

			result.SubItems[ReflectionListView.ColumnNameIndex].Text = name;
			if(value == null)
			{
				result.SetNull();
				value = Properties.Resources.NullString;
			} else if(exception)
				result.SetException();

			result.SubItems[ReflectionListView.ColumnValueIndex].Text = value;
			return result;
		}
	}
}