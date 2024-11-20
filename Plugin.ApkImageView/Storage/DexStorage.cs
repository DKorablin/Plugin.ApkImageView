using System;
using System.IO;
using AlphaOmega.Debug;

namespace Plugin.ApkImageView.Storage
{
	internal class DexStorage : FileStorageBase<DexFile>
	{
		public DexStorage(PluginWindows plugin)
			: base(plugin)
		{ }

		protected override DexFile LoadFilePath(String filePath)
		{
			IImageLoader loader = StreamLoader.FromFile(filePath);
			return new DexFile(loader);
		}

		protected override DexFile LoadFileMemory(String source, Byte[] payload)
			=> new DexFile(new StreamLoader(new MemoryStream(payload)));
	}
}