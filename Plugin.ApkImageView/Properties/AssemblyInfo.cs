using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("ca0bd89a-318f-4fa3-9a5e-49b3e1358a53")]
[assembly: System.CLSCompliant(false)]

[assembly: AssemblyDescription("Android (.APK) Package viewer")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2016-2025")]

/*
if $(ConfigurationName) == Release (
..\..\..\..\ILMerge.exe  "/out:$(ProjectDir)..\bin\$(TargetFileName)" "$(TargetPath)" "$(TargetDir)ApkReader.dll" "/lib:..\..\..\SAL\bin"
)
*/