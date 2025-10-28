using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using AlphaOmega.Debug;
using Plugin.ApkImageView.Bll;
using Plugin.ApkImageView.Directory;
using Plugin.ApkImageView.Storage;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.ApkImageView
{
	public class PluginWindows : IPlugin, IPluginSettings<PluginSettings>
	{
		#region Fields
		private TraceSource _trace;
		private PluginSettings _settings;
		private readonly Object _filesLock = new Object();
		private DexStorage _binaries;
		private ApkStorage _packages;
		private ManifestStorage _manifests;
		private Dictionary<String, DockState> _documentTypes;
		#endregion Fields

		#region Properties
		internal TraceSource Trace => this._trace ?? (this._trace = PluginWindows.CreateTraceSource<PluginWindows>());

		private IMenuItem MenuPeInfo { get; set; }
		private IMenuItem MenuWinApi { get; set; }

		/// <summary>Settings for interaction from the host</summary>
		Object IPluginSettings.Settings => this.Settings;

		/// <summary>Settings for interaction from the plugin</summary>
		public PluginSettings Settings
		{
			get
			{
				if(this._settings == null)
				{
					this._settings = new PluginSettings();
					this.HostWindows.Plugins.Settings(this).LoadAssemblyParameters(this._settings);
				}
				return this._settings;
			}
		}

		internal IHostWindows HostWindows { get; }

		/// <summary>Storage of open Dex files</summary>
		internal DexStorage Binaries
		{
			get
			{
				if(this._binaries == null)
					lock(this._filesLock)
						if(this._binaries == null)
							this._binaries = new DexStorage(this);
				return this._binaries;
			}
		}

		/// <summary>Storage of open APK files</summary>
		internal ApkStorage Packages
		{
			get
			{
				if(this._packages == null)
					lock(this._filesLock)
						if(this._packages == null)
							this._packages = new ApkStorage(this);
				return this._packages;
			}
		}

		internal ManifestStorage Manifests
		{
			get
			{
				if(this._manifests == null)
					lock(this._filesLock)
						if(this._manifests == null)
							this._manifests = new ManifestStorage(this);
				return this._manifests;
			}
		}

		internal static Dictionary<SectionNodeType, Type> DirectoryViewers
		{
			get => new Dictionary<SectionNodeType, Type>
				{
					{ SectionNodeType.Sections, typeof(DocumentTables) },
					{ SectionNodeType.ApkManifest, typeof(DocumentManifest) },
					{ SectionNodeType.Resource, typeof(DocumentResource) },
				};
		}

		internal static Dictionary<SectionNodeType, Type> BinaryViewers
		{
			get => new Dictionary<SectionNodeType, Type>
				{
					{ SectionNodeType.Resource, typeof(DocumentBinary) },
					{ SectionNodeType.Manifest, typeof(DocumentBinary) },
				};
		}

		private Dictionary<String, DockState> DocumentTypes
		{
			get
			{//TODO: List of supported windows
				if(this._documentTypes == null)
					this._documentTypes = new Dictionary<String, DockState>()
					{
						{ typeof(DocumentTables).ToString(), DockState.Document },
						{ typeof(PanelTOC).ToString(), DockState.DockRightAutoHide },
					};
				return this._documentTypes;
			}
		}
		#endregion Properties

		public PluginWindows(IHostWindows hostWindows)
			=> this.HostWindows = hostWindows ?? throw new ArgumentNullException(nameof(hostWindows));

		public IWindow GetPluginControl(String typeName, Object args)
			=> this.CreateWindow(typeName, false, args);

		/// <summary>Get the type of the object that is used to search for the object</summary>
		/// <returns>Reflection on the type of object used for searching</returns>
		public Type GetEntityType()
			=> typeof(AlphaOmega.Debug.DexFile);

		/// <summary>Create an instance of an object to search through reflection</summary>
		/// <remarks>To get a list, use <see cref="GetSearchObjects"/></remarks>
		/// <param name="filePath">The path to the element by which to create an instance for the search</param>
		/// <returns>The given object instance</returns>
		public Object CreateEntityInstance(String filePath)
		{
			DexFile result = new DexFile(StreamLoader.FromFile(filePath));
			return result;
		}

		/// <summary>Return objects for search, at the choice of the client, which will be searched</summary>
		/// <param name="folderPath">Path to folder where search for files</param>
		/// <returns>An array of files to search for, or null if the client didn't select anything</returns>
		public String[] GetSearchObjects(String folderPath)
		{
			List<String> result = new List<String>();
			foreach(String file in System.IO.Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories))//TODO: When migrating to .NET 4, change to Directory.EnumerateFiles
				if(Path.GetExtension(file).Equals(".dex", StringComparison.OrdinalIgnoreCase))
					result.Add(file);
			return result.ToArray();
		}

		public void OpenFile(String fileKey)
			=> this.OpenFile(fileKey, true);

		internal void OpenFile(String fileKey, Boolean isOpenPanel)
		{
			switch(Path.GetExtension(fileKey).ToLowerInvariant())
			{
			case ".apk":
			case ".xapk":
			case ".jar":
				this.Packages.OpenFile(fileKey);
				break;
			case ".xml":
				if(String.Equals("AndroidManifest.xml", Path.GetFileName(fileKey), StringComparison.OrdinalIgnoreCase))
					this.Packages.OpenFile(fileKey);
				else goto default;
				break;
			case ".dex":
				this.Binaries.OpenFile(fileKey);
				break;
			default:
				throw new NotImplementedException($"Unknown file extension: {fileKey}");
			}

			if(isOpenPanel)
				this.CreateWindow(typeof(PanelTOC).ToString(), true);
		}

		public void CloseFile(String fileKey)
		{
			switch(Path.GetExtension(fileKey).ToLowerInvariant())
			{
			case ".apk":
			case ".xapk":
			case ".jar":
				this.Packages.CloseFile(fileKey);
				break;
			case ".dex":
				this.Binaries.CloseFile(fileKey);
				break;
			default:
				throw new NotImplementedException($"Unknown file extension: {fileKey}");
			}
		}

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			IMenuItem menuView = this.HostWindows.MainMenu.FindMenuItem("View");
			if(menuView == null)
			{
				this.Trace.TraceEvent(TraceEventType.Error, 10, "Menu item 'View' not found");
				return false;
			}

			this.MenuWinApi = menuView.FindMenuItem("Executables");
			if(this.MenuWinApi == null)
			{
				this.MenuWinApi = menuView.Create("Executables");
				this.MenuWinApi.Name = "View.Executable";
				menuView.Items.Add(this.MenuWinApi);
			}

			this.MenuPeInfo = this.MenuWinApi.Create("&Apk View");
			this.MenuPeInfo.Name = "View.Executable.ApkView";
			this.MenuPeInfo.Click += (sender, e) => { this.CreateWindow(typeof(PanelTOC).ToString(), true); };

			this.MenuWinApi.Items.Add(this.MenuPeInfo);
			return true;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			if(this.MenuPeInfo != null)
				this.HostWindows.MainMenu.Items.Remove(this.MenuPeInfo);
			if(this.MenuWinApi != null && this.MenuWinApi.Items.Count == 0)
				this.HostWindows.MainMenu.Items.Remove(this.MenuWinApi);

			NodeExtender.DisposeFonts();

			if(this._binaries != null)
				this._binaries.Dispose();
			return true;
		}

		internal String FormatValue(Object value)
			=> value == null
				? null
				: this.FormatValue(value.GetType(), value);

		internal String FormatValue(MemberInfo info, Object value)
		{
			if(value == null)
				return null;

			Type type = info.GetMemberType();

			if(type.IsEnum)
				return value.ToString();
			else if(type == typeof(Char))
			{
				switch((Char)value)
				{
				case '\'':	return "\\\'";
				case '\"':	return "\\\"";
				case '\0':	return "\\0";
				case '\a':	return "\\a";
				case '\b':	return "\\b";
				case '\f':	return "\\b";
				case '\t':	return "\\t";
				case '\n':	return "\\n";
				case '\r':	return "\\r";
				case '\v':	return "\\v";
				default:	return value.ToString();
				}
			} else if(value is IFormattable)
			{
				type = type.GetRealType();//INullable<Enum>
				if(type.IsEnum)
					return value.ToString();

				switch(Convert.GetTypeCode(value))
				{
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					if(this.Settings.ShowAsHexValue)
						return "0x" + ((IFormattable)value).ToString("X", CultureInfo.CurrentCulture);
					else
						return ((IFormattable)value).ToString("n0", CultureInfo.CurrentCulture);
				default:
					return value.ToString();
				}
			} else
			{
				Type elementType = type.HasElementType ? type.GetElementType() : null;
				if(elementType != null && elementType.IsPrimitive && type.BaseType == typeof(Array))
				{
					Int32 index = 0;
					Array arr = (Array)value;
					StringBuilder values = new StringBuilder($"{elementType}[{this.FormatValue(arr.Length)}]");
					if(this.Settings.MaxArrayDisplay > 0)
					{
						values.Append(" { ");
						foreach(Object item in arr)
						{
							if(index++ > this.Settings.MaxArrayDisplay)
							{
								values.Append("...");
								break;
							}

							values.AppendFormat((this.FormatValue(item) ?? Properties.Resources.NullString) + ", ");
						}
						values.Append("}");
					}
					return values.ToString();
				} else
					return value.ToString();
			}
		}

		internal Object GetNodeDataRecursive(String[] path)
		{
			Object result = null;
			for(Int32 loop = 0; loop < path.Length; loop++)
			{
				String pathKey = Constant.CreatePathKey(path, loop);
				if(this.Binaries.Contains(pathKey))
					result = this.Binaries.LoadFile(pathKey);
				else if(this.Packages.Contains(pathKey))
					result = this.Packages.LoadFile(pathKey);
				else if(File.Exists(pathKey))
				{
					SectionNodeType node = Constant.GetSectionTypeByExtension(pathKey);
					switch(node)
					{
					case SectionNodeType.Dex:
						result = this.Binaries.LoadFile(pathKey);
						break;
					case SectionNodeType.Package:
						result = this.Packages.LoadFile(pathKey);
						break;
					case SectionNodeType.ApkManifest:
						result = this.Manifests.LoadFile(pathKey);
						break;
					default:
						result = File.ReadAllBytes(pathKey);
						break;
					}
				} else if(result is ApkFile zip)
				{
					Byte[] payload = zip.GetFile(path[loop]);

					SectionNodeType node = Constant.GetSectionTypeByExtension(path[loop]);
					switch(node)
					{
					case SectionNodeType.Dex:
						result = this.Binaries.LoadFile(Constant.CreatePathKey(path, loop + 1), payload);
						break;
					case SectionNodeType.Package:
						result = this.Packages.LoadFile(Constant.CreatePathKey(path, loop + 1), payload);
						break;
					case SectionNodeType.ApkManifest:
						result = zip.AndroidManifest;
						break;
					case SectionNodeType.Resource:
						result = new ArscFile(payload);
						break;
					default:
						result = payload;
						break;
					}
				} else if(result is DexFile dex)
				{
					SectionNodeType node = Constant.GetSectionTypeByExtension(path[loop]);
					switch(node)
					{
					case SectionNodeType.Header:
						result = dex.Header;
						break;
					case SectionNodeType.MapList:
						result = dex.map_list;
						break;
					case SectionNodeType.Sections:
						result = dex;
						break;
					default:
						throw new NotImplementedException($"Data retrieval for type '{node}' not found");
					}
				}
			}
			return result;
		}

		private IWindow CreateWindow(String typeName, Boolean searchForOpened, Object args = null)
			=> this.DocumentTypes.TryGetValue(typeName, out DockState state)
				? this.HostWindows.Windows.CreateWindow(this, typeName, searchForOpened, state, args)
				: null;

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}
	}
}