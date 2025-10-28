using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Plugin.ApkImageView
{
	/// <summary>Shell extensions to open file in the Explorer window</summary>
	internal static class Shell32
	{
		/// <summary>Try to find opened window or open new explorer window and select required file</summary>
		/// <param name="filePath">Path to file to show in explorer window</param>
		public static void OpenFolderAndSelectItem(String filePath)
			=> Shell32.OpenFolderAndSelectItem(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));

		/// <summary>Try to find opened window or open new explorer window and select required file</summary>
		/// <param name="folderPath">Path to directory</param>
		/// <param name="fileName">Path to file to show in explorer window</param>
		public static void OpenFolderAndSelectItem(String folderPath, String fileName)
		{
			NativeMethods.SHParseDisplayName(folderPath, IntPtr.Zero, out IntPtr nativeFolder, 0, out _);

			if(nativeFolder == IntPtr.Zero)//Folder not found
				return;

			NativeMethods.SHParseDisplayName(Path.Combine(folderPath, fileName), IntPtr.Zero, out IntPtr nativeFile, 0, out _);

			IntPtr[] fileArray = nativeFile == IntPtr.Zero// Open the folder without the file selected if we can't find the file
				? new IntPtr[0]
				: new IntPtr[] { nativeFile };

			NativeMethods.SHOpenFolderAndSelectItems(nativeFolder, (UInt32)fileArray.Length, fileArray, 0);

			Marshal.FreeCoTaskMem(nativeFolder);
			if(nativeFile != IntPtr.Zero)
				Marshal.FreeCoTaskMem(nativeFile);
		}

		private static class NativeMethods
		{
			/// <summary>Opens a Windows Explorer window with specified items in a particular folder selected</summary>
			/// <param name="pidlFolder">A pointer to a fully qualified item ID list that specifies the folder</param>
			/// <param name="cidl">
			/// A count of items in the selection array, apidl.
			/// If cidl is zero, then pidlFolder must point to a fully specified ITEMIDLIST describing a single item to select.
			/// This function opens the parent folder and selects that item.
			/// </param>
			/// <param name="apidl">A pointer to an array of PIDL structures, each of which is an item to select in the target folder referenced by pidlFolder.</param>
			/// <param name="dwFlags">
			/// The optional flags.
			/// Under Windows XP this parameter is ignored.
			/// In Windows Vista, the following flags are defined.
			/// </param>
			/// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
			[DllImport("shell32.dll", SetLastError = true)]
			public static extern Int32 SHOpenFolderAndSelectItems(IntPtr pidlFolder, UInt32 cidl, [In, MarshalAs(UnmanagedType.LPArray)] IntPtr[] apidl, UInt32 dwFlags);

			/// <summary>
			/// Translates a Shell namespace object's display name into an item identifier list and returns the attributes of the object.
			/// This function is the preferred method to convert a string to a pointer to an item identifier list (PIDL).
			/// </summary>
			/// <param name="pszName">A pointer to a zero-terminated wide string that contains the display name to parse.</param>
			/// <param name="pbc">
			/// A bind context that controls the parsing operation.
			/// This parameter is normally set to NULL.
			/// </param>
			/// <param name="ppidl">
			/// The address of a pointer to a variable of type ITEMIDLIST that receives the item identifier list for the object.
			/// If an error occurs, then this parameter is set to NULL.
			/// </param>
			/// <param name="sfgaoIn">
			/// A ULONG value that specifies the attributes to query.
			/// To query for one or more attributes, initialize this parameter with the flags that represent the attributes of interest.
			/// For a list of available SFGAO flags, see SFGAO.
			/// </param>
			/// <param name="psfgaoOut">
			/// A pointer to a ULONG.
			/// On return, those attributes that are true for the object and were requested in sfgaoIn are set.
			/// An object's attribute flags can be zero or a combination of SFGAO flags.
			/// For a list of available SFGAO flags, see SFGAO.
			/// </param>
			/// <remarks>
			/// You should call this function from a background thread.
			/// Failure to do so could cause the UI to stop responding.
			/// </remarks>
			[DllImport("shell32.dll", SetLastError = true)]
			public static extern void SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] String pszName, IntPtr pbc, [Out] out IntPtr ppidl, UInt32 sfgaoIn, [Out] out UInt32 psfgaoOut);
		}
	}
}