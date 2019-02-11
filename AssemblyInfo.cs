#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

[assembly: AssemblyTitle("GTA:LCS Save Editor")]
[assembly: AssemblyDescription("Grand Theft Auto: Liberty City Stories savegame editor.")]
[assembly: AssemblyProduct("GTA:LCS Save Editor")]
[assembly: AssemblyCopyright("Copyright (C) 2016-2019 Wes Hampson. All rights reserved.")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: ComVisible(false)]

/**
 * In order to begin building localizable applications, set
 * <UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
 * inside a <PropertyGroup>.  For example, if you are using US English
 * in your source files, set the <UICulture> to en-US.  Then uncomment
 * the NeutralResourceLanguage attribute below.  Update the "en-US" in
 * the line below to match the UICulture setting in the project file.
 */
//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,            // theme-specific resource dictionaries
    ResourceDictionaryLocation.SourceAssembly   // generic resource dictionary 
)]

[assembly: AssemblyVersion("0.4.0.966")]
[assembly: AssemblyFileVersion("0.4.0.966")]
[assembly: AssemblyInformationalVersion("0.4.0")]
