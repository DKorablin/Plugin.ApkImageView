namespace Plugin.ApkImageView.Bll
{
	/// <summary>File change type</summary>
	public enum PeListChangeType
	{
		/// <summary>Unknown change type</summary>
		None = 0,
		/// <summary>File added</summary>
		Added = 1,
		/// <summary>File deleted</summary>
		Removed = 2,
		/// <summary>File modified externally</summary>
		Changed = 3,
	}
}