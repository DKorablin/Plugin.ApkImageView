using System;
using System.IO;
using Plugin.ApkImageView.Properties;

namespace Plugin.ApkImageView
{
	/// <summary>Plugin constants</summary>
	internal static class Constant
	{
		/// <summary>Binary file constant keyword</summary>
		public const String BinaryFile = "Binary";

		/// <summary>Get name of the header</summary>
		/// <param name="type">Type of the property in the DEX reader</param>
		/// <returns></returns>
		public static String GetHeaderName(SectionNodeType type)
		{
			switch(type)
			{
			case SectionNodeType.Header:	return Resources.Section_Header;
			case SectionNodeType.MapList:	return Resources.Section_MapList;
			case SectionNodeType.Sections:	return Resources.Section_Sections;
			default:						return type.ToString();
			}
		}

		public static String CreatePathKey(String[] path)
			=> String.Join("+", path);

		public static String CreatePathKey(String[] path, Int32 count)
		{
			if(path.Length == count)
				return Constant.CreatePathKey(path);
			else
			{
				String[] result = new String[count + 1];
				Array.Copy(path, result, result.Length);
				return Constant.CreatePathKey(result);
			}
		}

		public static SectionNodeType GetSectionTypeByExtension(String filePath)
		{
			switch(Path.GetExtension(filePath).ToLowerInvariant())
			{
			case ".apk":
			case ".xapk":
				return SectionNodeType.Package;
			case ".dex":
				return SectionNodeType.Dex;
			case ".arsc":
				return SectionNodeType.Resource;
			case ".xml":
				if(Path.GetFileName(filePath) == "AndroidManifest.xml")
					return SectionNodeType.ApkManifest;
				break;
			case "":
				if(filePath == Resources.Section_Header)
					return SectionNodeType.Header;
				else if(filePath == Resources.Section_MapList)
					return SectionNodeType.MapList;
				else if(filePath == Resources.Section_Sections)
					return SectionNodeType.Sections;
				else
					break;
			}
			return SectionNodeType.Path;
		}
	}
}