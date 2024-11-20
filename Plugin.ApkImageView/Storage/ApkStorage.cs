using System;
using AlphaOmega.Debug;

namespace Plugin.ApkImageView.Storage
{
	internal class ApkStorage : FileStorageBase<ApkFile>
	{
		public ApkStorage(PluginWindows plugin)
			: base(plugin)
		{ }

		protected override ApkFile LoadFilePath(String filePath)
			=> new ApkFile(filePath);

		protected override ApkFile LoadFileMemory(String source, Byte[] payload)
			=> new ApkFile(payload);
	}
}