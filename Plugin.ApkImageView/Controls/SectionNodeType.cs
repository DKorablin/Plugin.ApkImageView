using System;

namespace Plugin.ApkImageView
{
	/// <summary>Tree node known type</summary>
	public enum SectionNodeType : Byte
	{
		/// <summary>Physical file path</summary>
		Path,
		/// <summary>DEX header</summary>
		Header,
		/// <summary>DEX map_list</summary>
		MapList,
		/// <summary>DEX sections</summary>
		Sections,
		/// <summary>Folder</summary>
		Folder,
		/// <summary>APK, XAPK file</summary>
		Package,
		/// <summary>DEX file</summary>
		Dex,
		/// <summary>ARSC file</summary>
		Resource,
		/// <summary>AndroidManifest.xml extended (+ARSC file)</summary>
		ApkManifest,
		/// <summary>Raw AndroidManifest.xml</summary>
		Manifest,
	}
}