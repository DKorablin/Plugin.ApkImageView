using System;
using AlphaOmega.Debug;

namespace Plugin.ApkImageView.Storage
{
	internal class ManifestStorage : FileStorageBase<AxmlFile>
	{
		public ManifestStorage(PluginWindows plugin)
			: base(plugin)
		{ }

		protected override AxmlFile LoadFilePath(String filePath)
			=> new AxmlFile(StreamLoader.FromFile(filePath));

		protected override AxmlFile LoadFileMemory(String source, Byte[] payload)
			=> new AxmlFile(StreamLoader.FromMemory(payload));
	}
}