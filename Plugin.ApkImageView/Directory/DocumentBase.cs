using System;
using System.ComponentModel;
using System.Windows.Forms;
using AlphaOmega.Debug;
using Plugin.ApkImageView.Bll;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.ApkImageView.Directory
{
	public abstract partial class DocumentBase : UserControl, IPluginSettings<DocumentBaseSettings>
	{
		private readonly SectionNodeType _peType;
		private DocumentBaseSettings _settings;

		protected PluginWindows Plugin => (PluginWindows)this.Window.Plugin;
		protected IWindow Window => (IWindow)base.Parent;

		/// <summary>Путь к открытому файлу в текущем документе</summary>
		internal String[] FilePath => this.Settings.FilePath;

		Object IPluginSettings.Settings => this.Settings;

		public virtual DocumentBaseSettings Settings
			=> this._settings ?? (this._settings = new DocumentBaseSettings());

		protected virtual void SetCaption()
			=> this.Window.Caption = String.Join(" - ", new String[] { Constant.CreatePathKey(this.Settings.FilePath), Constant.GetHeaderName(this._peType), });

		public DocumentBase(SectionNodeType type)
		{
			this._peType = type;
			this.InitializeComponent();
		}

		protected override void OnCreateControl()
		{
			this.Window.Shown += this.Window_Shown;
			this.Window.Closed += this.Window_Closed;
			this.Plugin.Settings.PropertyChanged += this.Settings_PropertyChanged;
			this.Plugin.Binaries.PeListChanged += this.Plugin_PeListChanged;
			base.OnCreateControl();
			this.DataBind();
		}

		private void Window_Shown(Object sender, EventArgs e)
		{
			var info = this.GetFile();
			if(info == null)
			{
				this.Plugin.Trace.TraceInformation("File {0} not found", this.FilePath);
				this.Window.Close();
			}
		}

		private void Window_Closed(Object sender, EventArgs e)
		{
			this.Plugin.Settings.PropertyChanged -= this.Settings_PropertyChanged;
			this.Plugin.Binaries.PeListChanged -= this.Plugin_PeListChanged;
		}

		private void Plugin_PeListChanged(Object sender, PeListChangedEventArgs e)
		{
			if(base.InvokeRequired)
				base.Invoke((MethodInvoker)delegate { this.Plugin_PeListChanged(sender, e); });
			else
				switch(e.Type)
				{
				case PeListChangeType.Changed:
					if(Constant.CreatePathKey(this.FilePath).StartsWith(e.FilePath))
					{
						Object node = this.GetFile();
						this.ShowFile(node);
					}
					break;
				}
		}

		private void Settings_PropertyChanged(Object sender, PropertyChangedEventArgs e)
		{
			switch(e.PropertyName)
			{
			case nameof(PluginSettings.MaxArrayDisplay):
			case nameof(PluginSettings.ShowAsHexValue):
				var info = this.GetFile();
				this.ShowFile(info);
				break;
			}
		}

		/// <summary>Открыть файл, если его открывает окно, скажем, через Drag'n'Drop</summary>
		/// <param name="filePath">Путь к файлу для открытия</param>
		protected void OpenFile(String filePath)
		{
			if(this.FilePath != null && this.FilePath[0].Equals(filePath, StringComparison.OrdinalIgnoreCase))
				return;

			switch(Constant.GetSectionTypeByExtension(filePath))
			{
			case SectionNodeType.Dex:
				this.Plugin.Binaries.OpenFile(filePath);
				DexFile dex = this.Plugin.Binaries.LoadFile(filePath);
				this.Settings.FilePath = new String[] { filePath };

				this.SetCaption();
				this.ShowFile(dex);
				break;
			default:
				this.Plugin.Trace.TraceInformation("File {0} not supported", filePath);
				break;
			}
		}

		private void DataBind()
		{
			Object dex = this.GetFile();
			if(dex != null)
			{
				this.Plugin.Binaries.OpenFile(this.FilePath[0]);//Файл открыт. Необходимо обновить список открытых файлов (При необходимости)

				this.SetCaption();
				this.ShowFile(dex);
			}
		}

		/// <summary>Получить информацию о открытом файле</summary>
		/// <returns>Корневая директория описателя PE файла</returns>
		protected Object GetFile()
			=> this.Plugin.GetNodeDataRecursive(this.FilePath);

		/// <summary>Отобразить файл в окне</summary>
		/// <param name="info">Информация о файле</param>
		protected abstract void ShowFile(Object node);
	}
}