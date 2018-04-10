﻿using System.Reflection;
#if DEBUG
using System.Runtime.CompilerServices;
#endif

[assembly: AssemblyTitle("CslaContrib in .NET 4.6.1")]
[assembly: AssemblyDescription("Extends CSLA .NET framework with ObjectCaching data portal implementation including in-memory cache provider for simple cache and additional generic business and authorization rules.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("CslaContrib.NET461")]
[assembly: AssemblyCulture("")]

#if DEBUG
// Allow unit test project access to internal types and members
// How to find thw PublicKey? sn.exe -Tp CslaContrib.WebGUI.dll
[assembly: InternalsVisibleTo("CslaContrib.UnitTests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100cf83b2ddb3b7363c911d441eaa8cb1f0a98f4dd29820e55efe50ccda13b3b446d08308da7136605b4d527b2075f9b67158ae8400ae4cc6a9761c99e778ce536e99b1d89ad3c0c203d462df49c86d34c5f22b5c6d7bc04f2c730ac0c7366f5da155b98ae900577dba1cf25d522888d1a70bf115a7c3a4feac93ed8c83f51eadae")]
#endif
