using System;
using System.ComponentModel;
using AlphaOmega.Debug.Manifest;

namespace Plugin.ApkImageView.ManifestDto
{
	internal class DtoManifest
	{
		private readonly AndroidManifest _manifest;

		[Description("A full Java-language-style package name for the Android app")]
		public String Package => this._manifest.Package;

		[DefaultValue(1)]
		[Description("The target sandbox for this app to use")]
		public Int32 TargetSandboxVersion => this._manifest.TargetSandboxVersion;

		[Description(@"An internal version number.
		This number is used only to determine whether one version is more recent than another, with higher numbers indicating more recent versions.
		This is not the version number shown to users; that number is set by the versionName attribute.")]
		public String VersionCode => this._manifest.VersionCode;

		public DtoManifest(AndroidManifest manifest)
			=> this._manifest = manifest;
	}
}