using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Plugin.ApkImageView.Bll;
using Plugin.ApkImageView.Directory;
using SAL.Windows;

namespace Plugin.ApkImageView.Storage
{
	internal abstract class FileStorageBase<T> : IDisposable where T : class, IDisposable
	{
		private readonly Object _binLock = new Object();
		private readonly Dictionary<String, T> _binaries = new Dictionary<String, T>();
		private readonly Dictionary<String, FileSystemWatcher> _binaryWatcher = new Dictionary<String, FileSystemWatcher>();
		private readonly PluginWindows _plugin;

		public event EventHandler<PeListChangedEventArgs> PeListChanged;

		private protected FileStorageBase(PluginWindows plugin)
		{
			this._plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
			this._plugin.Settings.PropertyChanged += this.Settings_PropertyChanged;
		}

		protected abstract T LoadFilePath(String filePath);

		protected abstract T LoadFileMemory(String source, Byte[] payload);

		/// <summary>Check if the file exists among loaded binaries</summary>
		/// <param name="filePath">File key for search</param>
		/// <returns></returns>
		public Boolean Contains(String filePath)
			=> this._binaries.ContainsKey(filePath);

		/// <summary>Get information about the PE file. If the file is not open, open it</summary>
		/// <param name="filePath">Path to the file to read information for</param>
		/// <returns>Information about the PE/COFF file or null</returns>
		public T LoadFile(String filePath)
			=> this.LoadFile(filePath, false);

		/// <summary>Get information about a binary from the file system</summary>
		/// <param name="filePath">Path to the file to read information for</param>
		/// <param name="noLoad">Search the file among already loaded ones and if not found do not load it</param>
		/// <returns>Information about the PE/COFF file or null</returns>
		public T LoadFile(String filePath, Boolean noLoad)
		{
			if(String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException(nameof(filePath));

			if(this._binaries.TryGetValue(filePath, out T result))
				return result;
			else if(noLoad)
				return null;//Loading is not required, it is enough to check for file presence in storage

			if(result == null)
			{
				if(!File.Exists(filePath))
					return null;//If the file was loaded from memory, it cannot be found on the FS

				lock(this._binLock)
				{
					result = this.LoadFile(filePath, true);
					if(result == null)
					{
						result = this.LoadFilePath(filePath);
						this._binaries.Add(filePath, result);
						if(!this._binaryWatcher.ContainsKey(filePath)//When updating the file only the file is removed, not its monitor
							&& this._plugin.Settings.MonitorFileChange)
							this.RegisterFileWatcher(filePath);
					}
				}
			}
			return result;
		}

		/// <summary>Get information about a binary from memory</summary>
		/// <param name="key">Unique key</param>
		/// <param name="memFile">Binary from memory</param>
		/// <returns></returns>
		public T LoadFile(String key, Byte[] memFile)
		{
			if(memFile == null || memFile.Length == 0)
				throw new ArgumentNullException(nameof(memFile));

			if(key != null && this._binaries.TryGetValue(key, out T info))
				return info;

			lock(this._binLock)
			{
				if(String.IsNullOrEmpty(key))
					key = this.GetBinaryUniqueName(0);

				if(!this._binaries.TryGetValue(key, out info))
				{
					info = this.LoadFileMemory(key, memFile);
					this._binaries.Add(key, info);
				}
				return info;
			}
		}

		/// <summary>Close a previously opened file</summary>
		/// <param name="filePath">Path to the file to close</param>
		private void UnloadFile(String filePath)
		{
			if(String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException(nameof(filePath));

			T info = this.LoadFile(filePath, true);
			if(info == null)
				return;//File already unloaded

			try
			{
				IWindow[] windows = this._plugin.HostWindows.Windows.ToArray();
				for(Int32 loop = windows.Length - 1; loop >= 0; loop--)
				{
					IWindow wnd = windows[loop];
					DocumentBase ctrl = wnd.Control as DocumentBase;
					if(ctrl != null && Constant.CreatePathKey(ctrl.FilePath).StartsWith(filePath))
						wnd.Close();
				}
				if(filePath.StartsWith(Constant.BinaryFile))//The binary file is removed from the list immediately after closing
					this.OnPeListChanged(PeListChangeType.Removed, filePath);
			} finally
			{
				info.Dispose();
				this._binaries.Remove(filePath);
				this.UnregisterFileWatcher(filePath);
			}
		}

		/// <summary>Register file monitoring for delete or change</summary>
		/// <param name="filePath">The path to file register for watching</param>
		/// <exception cref="ArgumentNullException">filePath is null or empty string</exception>
		/// <exception cref="FileNotFoundException">File not found</exception>
		public void RegisterFileWatcher(String filePath)
		{
			if(String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException(nameof(filePath));
			if(!File.Exists(filePath))
				throw new FileNotFoundException("File not found", filePath);

			FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath), Path.GetFileName(filePath))
			{
				NotifyFilter = NotifyFilters.LastWrite,
			};
			watcher.Deleted += new FileSystemEventHandler(watcher_Changed);
			watcher.Changed += new FileSystemEventHandler(watcher_Changed);
			watcher.EnableRaisingEvents = true;
			this._binaryWatcher.Add(filePath, watcher);
		}

		public void UnregisterFileWatcher(String filePath)
		{
			if(String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException(nameof(filePath));

			if(this._binaryWatcher.TryGetValue(filePath, out FileSystemWatcher watcher))
			{
				watcher.Dispose();
				this._binaryWatcher.Remove(filePath);
			}
		}

		/// <summary>Add a file from memory to the list of open files</summary>
		/// <param name="memFile">File from memory</param>
		public void OpenFile(String key, Byte[] memFile)
		{
			if(memFile == null || memFile.Length == 0)
				throw new ArgumentNullException(nameof(memFile));

			if(key != null && this._binaries.TryGetValue(key, out _))
				return;

			lock(this._binLock)
			{
				if(key == null)
					key = this.GetBinaryUniqueName(0);

				if(this._binaries.TryGetValue(key, out T info))
					return;
				else
				{
					info = this.LoadFileMemory(key, memFile);
					this._binaries.Add(key, info);
				}
			}
			this.OnPeListChanged(PeListChangeType.Added, key);
		}

		/// <summary>Add a file to the list of open files</summary>
		/// <param name="filePath">Path to the file</param>
		public Boolean OpenFile(String filePath)
		{
			if(String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException(nameof(filePath));
			if(filePath.StartsWith(Constant.BinaryFile))
				return false;//This is necessary to filter out files that were loaded from memory

			String[] loadedFiles = this._plugin.Settings.LoadedFiles;
			if(loadedFiles.Contains(filePath))
				return false;
			else
			{
				List<String> files = new List<String>(loadedFiles) {
					filePath,
				};

				this._plugin.Settings.LoadedFiles = files.ToArray();
				this._plugin.HostWindows.Plugins.Settings(this._plugin).SaveAssemblyParameters();
				this.OnPeListChanged(PeListChangeType.Added, filePath);
				return true;
			}
		}

		public void CloseFile(String filePath)
		{
			if(String.IsNullOrEmpty(filePath))
				throw new ArgumentNullException(nameof(filePath));

			this.UnloadFile(filePath);

			String[] loadedFiles = this._plugin.Settings.LoadedFiles;
			List<String> files = new List<String>(loadedFiles);
			if(files.Remove(filePath))
			{//If this is a file from memory, then it is not in the file list
				this._plugin.Settings.LoadedFiles = files.ToArray();
				this._plugin.HostWindows.Plugins.Settings(this._plugin).SaveAssemblyParameters();
				this.OnPeListChanged(PeListChangeType.Removed, filePath);
			}
		}

		public void Dispose()
		{
			lock(this._binLock)
			{
				this._plugin.Settings.PropertyChanged -= Settings_PropertyChanged;
				foreach(String key in this._binaries.Keys.ToArray())
				{
					T info = this._binaries[key];
					info.Dispose();
				}
				this._binaries.Clear();
				foreach(String key in this._binaryWatcher.Keys)
					this._binaryWatcher[key].Dispose();
				this._binaryWatcher.Clear();
			}
		}

		/// <summary>The list of loaded files has changed</summary>
		/// <param name="type">Type of change</param>
		/// <param name="filePath">Path to the file on which the change occurred</param>
		private void OnPeListChanged(PeListChangeType type, String filePath)
			=> this.PeListChanged?.Invoke(this, new PeListChangedEventArgs(type, filePath));

		private void Settings_PropertyChanged(Object sender, PropertyChangedEventArgs e)
		{
			switch(e.PropertyName)
			{
			case nameof(PluginSettings.MonitorFileChange):
				if(this._plugin.Settings.MonitorFileChange)
				{
					if(this._binaryWatcher.Count == 0)
						lock(this._binLock)
						{
							if(this._binaryWatcher.Count == 0)
								foreach(String filePath in this._binaries.Keys)
									if(File.Exists(filePath))
										this.RegisterFileWatcher(filePath);
						}
				} else
				{
					if(this._binaryWatcher.Count > 0)
						lock(this._binLock)
						{
							if(this._binaryWatcher.Count > 0)
								foreach(String key in this._binaryWatcher.Keys)
									this._binaryWatcher[key].Dispose();
							this._binaryWatcher.Clear();
						}
				}
				break;
			}
		}

		private void watcher_Changed(Object sender, FileSystemEventArgs e)
		{
			FileSystemWatcher watcher = (FileSystemWatcher)sender;
			watcher.EnableRaisingEvents = false;
			try
			{
				switch(e.ChangeType)
				{
				case WatcherChangeTypes.Changed:
					FileInfo info = new FileInfo(e.FullPath);

					do
					{
						if(!info.Exists)
							goto case WatcherChangeTypes.Deleted;

						try
						{
							using(FileStream s = info.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
							{ }// File was modified and unlocked.

							lock(this._binLock)//Closing old file
							{
								this._binaries[e.FullPath].Dispose();
								this._binaries.Remove(e.FullPath);
							}

							this.OnPeListChanged(PeListChangeType.Changed, e.FullPath);
							break;
						} catch(IOException exc) when((exc.HResult & 0x0000FFFF) == 32)
						{//Sharing violation
							System.Threading.Thread.Sleep(1000);
						}
						info.Refresh();
					} while(true);
					break;
				case WatcherChangeTypes.Deleted:
				case WatcherChangeTypes.Renamed:
					lock(this._binLock)
					{
						this._binaries[e.FullPath].Dispose();
						this._binaries.Remove(e.FullPath);
					}
					this.OnPeListChanged(PeListChangeType.Removed, e.FullPath);
					break;
				}
			} finally
			{
				watcher.EnableRaisingEvents = true;
			}
		}

		/// <summary>Get a unique name for the binary file</summary>
		/// <param name="index">Index if a file with such name is already loaded</param>
		/// <returns>Unique file name</returns>
		private String GetBinaryUniqueName(UInt32 index)
		{
			String indexName = index > 0
				? $"{Constant.BinaryFile}[{index}]"
				: Constant.BinaryFile;

			return this._binaries.ContainsKey(indexName)
				? GetBinaryUniqueName(checked(index + 1))
				: indexName;
		}
	}
}