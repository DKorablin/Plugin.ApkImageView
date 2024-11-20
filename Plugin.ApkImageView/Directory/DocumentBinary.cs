using System;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Forms;
using AlphaOmega.Debug;
using AlphaOmega.Debug.Manifest;

namespace Plugin.ApkImageView.Directory
{
	public partial class DocumentBinary : DocumentBase
	{
		internal static DisplayMode[] DisplayModes = (DisplayMode[])Enum.GetValues(typeof(DisplayMode));

		public DocumentBinary()
			: base(SectionNodeType.Sections)
		{
			InitializeComponent();
			tsddlView.Items.AddRange(Array.ConvertAll(DocumentBinary.DisplayModes, delegate(DisplayMode mode) { return mode.ToString(); }));
			tsddlView.SelectedIndex = 0;
		}

		protected override void ShowFile(Object node)
		{
			Byte[] payload = this.GetSectionData();
			//tsbnView.Enabled = this.Plugin.DirectoryViewers.ContainsKey(this.SettingsI.Header);

			bvBytes.SetBytes(payload);
		}

		protected override void SetCaption()
			=> base.Window.Caption = String.Join(" - ", new String[] { "Binary", Constant.CreatePathKey(this.Settings.FilePath), });

		private Byte[] GetSectionData()
		{
			Object node = base.GetFile();
			throw new NotImplementedException($"Type {node.GetType()} not supported in Binary viewer");
		}

		private void tsddlView_SelectedIndexChanged(Object sender, EventArgs e)
		{
			DisplayMode mode = DocumentBinary.DisplayModes[tsddlView.SelectedIndex];
			bvBytes.SetDisplayMode(mode);
		}

		private void tsbnSave_Click(Object sender, EventArgs e)
		{
			String peFilePath = Path.GetFullPath(this.Settings.FilePath[0]);
			using(SaveFileDialog dlg = new SaveFileDialog() { InitialDirectory = peFilePath, OverwritePrompt = true, AddExtension = true, DefaultExt = "bin", Filter = "BIN file (*.bin)|*.bin|All files (*.*)|*.*", })
				if(dlg.ShowDialog() == DialogResult.OK)
					bvBytes.SaveToFile(dlg.FileName);
		}
	}
}