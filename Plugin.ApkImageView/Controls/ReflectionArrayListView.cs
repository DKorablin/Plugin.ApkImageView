using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using AlphaOmega.Debug;
using AlphaOmega.Windows.Forms;
using Plugin.ApkImageView.Bll;

namespace Plugin.ApkImageView.Controls
{
	internal class ReflectionArrayListView : SortableListView
	{
		public PluginWindows Plugin { get; set; }
		public ReflectionArrayListView()
		{
			base.View = View.Details;
			base.AllowColumnReorder = true;
			base.FullRowSelect = true;
			base.GridLines = true;
			base.MultiSelect = true;
			base.Sorting = SortOrder.None;//We don't need to sort reflection data, because it could break the indexing view
			base.UseCompatibleStateImageBehavior = false;
			base.HideSelection = false;
			base.HeaderStyle = ColumnHeaderStyle.Clickable;
		}

		public void DataBind(ITable table)
		{
			_ = table ?? throw new ArgumentNullException(nameof(table));
			if(this.Plugin == null)
				throw new InvalidOperationException("Plugin is null");

			base.SuspendLayout();
			try
			{
				base.ClearAll();

				List<ListViewItem> newItems = new List<ListViewItem>();
				List<ListViewItem> oldItems = new List<ListViewItem>(base.Items.Cast<ListViewItem>());
				IColumn[] columns = table.Columns;
				//this.SetColumns(columns.Select(p => p.Name).ToArray());

				foreach(IRow row in table.Rows)
				{
					Boolean added = false;
					ListViewItem item = oldItems.FirstOrDefault(p => p.Tag == row);
					if(item == null)
						item = new ListViewItem() { Tag = row, };
					else
					{
						added = true;
						oldItems.Remove(item);
					}

					foreach(IColumn column in columns)
					{
						Int32 index = this.GetColumn(column.Name).Index;

						while(item.SubItems.Count <= index)
							item.SubItems.Add(String.Empty);

						String text = this.Plugin.FormatValue(row[column.Index].Value);
						item.SubItems[index].Text = text;
					}

					if(!added)
						newItems.Add(item);
				}
				base.Items.AddRange(newItems.ToArray());
				//Удаление старых строк
				foreach(ListViewItem oldItem in oldItems)
					base.Items.Remove(oldItem);
				base.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			} finally
			{
				base.ResumeLayout(false);
			}
		}

		public void DataBind(IEnumerable rows)
		{
			_ = rows ?? throw new ArgumentNullException(nameof(rows));
			if(this.Plugin == null)
				throw new InvalidOperationException("Plugin is null");

			base.SuspendLayout();
			try
			{
				base.ClearAll();

				List<ListViewItem> newItems = new List<ListViewItem>();
				List<ListViewItem> oldItems = new List<ListViewItem>(base.Items.Cast<ListViewItem>());

				MemberInfo[] members = null;

				foreach(var row in rows)
				{
					//Код обновления ранее добавленной строки
					Boolean added = false;
					ListViewItem item = oldItems.FirstOrDefault(p => p.Tag == row);
					if(item == null)
						item = new ListViewItem() { Tag = row, };
					else
					{
						added = true;
						oldItems.Remove(item);
					}

					if(members == null)
					{
						members = row.GetType().GetMembers().Where(p => p.MemberType == MemberTypes.Field || p.MemberType == MemberTypes.Property).ToArray();
						//Установка колонок
						//this.SetColumns(members.Select(p => p.Name).ToArray());
					}

					foreach(MemberInfo member in members)
					{
						Int32 index = this.GetColumn(member.Name).Index;

						while(item.SubItems.Count <= index)
							item.SubItems.Add(String.Empty);

						String text;
						Boolean isException = false;
						try
						{
							text = this.Plugin.FormatValue(member, member.GetMemberValue(row));
						} catch(TargetInvocationException exc)
						{
							isException = true;
							text = exc.InnerException.Message;
						} catch(Exception exc)
						{
							isException = true;
							text = String.Format("{0}: \"{1}\"", exc, exc.Message);
						}
						if(isException)
							item.SetException();
						item.SubItems[index].Text = text;
					}

					if(!added)
						newItems.Add(item);
				}
				if(members == null)//Нет данных
					base.Columns.Clear();

				base.Items.AddRange(newItems.ToArray());
				//Удаление старых строк
				foreach(ListViewItem oldItem in oldItems)
					base.Items.Remove(oldItem);
				base.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			} finally
			{
				base.ResumeLayout(false);
			}
		}
		private ColumnHeader GetColumn(String name)
		{
			foreach(ColumnHeader column in base.Columns)
				if(column.Text == name)
					return column;

			//throw new ArgumentException(String.Format("Coumn {0} not found", name));
			return base.Columns.Add(name);
		}

		[Obsolete("Метод создаёт лютый писец с колонками при биндинге разных массивов", true)]
		private void SetColumns(String[] columns)
		{
			for(Int32 loop = base.Columns.Count - 1; loop >= 0; loop--)
			{//Удаляю колонки, которых нет в объекте
				ColumnHeader column = base.Columns[loop];
				if(!columns.Any(p => p == column.Text))
					column.Dispose();
			}

			foreach(String column in columns)
			{//Добавляю колонки, которых нет в списке
				Boolean found = false;
				foreach(ColumnHeader columnHeader in base.Columns)
					if(columnHeader.Text == column)
					{
						found = true;
						break;
					}
				if(!found)
					base.Columns.Add(column);
			}
		}
	}
}