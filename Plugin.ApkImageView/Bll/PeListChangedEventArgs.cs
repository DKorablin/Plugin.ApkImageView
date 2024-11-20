using System;

namespace Plugin.ApkImageView.Bll
{
	public class PeListChangedEventArgs : EventArgs
	{
		public PeListChangeType Type { get; }
		public String FilePath { get; }

		public PeListChangedEventArgs(PeListChangeType type, String filePath)
		{
			this.Type = type;
			this.FilePath = filePath;
		}
	}
}