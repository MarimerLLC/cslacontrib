![](https://raw.github.com/MarimerLLC/csla/master/Support/Logos/csla%20win8_mid.png)

# CSLA .NET public contribution project

[![NuGet](https://img.shields.io/nuget/v/CSLA-Core.svg)](https://www.nuget.org/packages/CslaContrib)

The use (and contribution to) cslacontrib is governed by the [LICENSE.md](https://github.com/MarimerLLC/cslacontrib/blob/master/LICENSE.md) file in this repo.

CSLA .NET Contrib is a contribution project to Rockford Lhotka's CSLA .NET framework. The project is focused on extending the features of CSLA .NET.

--

[CSLA .NET](http://www.cslanet.com) is copyright (c) Marimer LLC. Use of CSLA.NET and CslaContrib is governed by the [MIT license](https://github.com/MarimerLLC/cslacontrib/blob/master/LICENSE.md).

## News

#### Project Tracker Release mode build
When you build the solution in __Release__ mode, at the root level there will be 3 folders with ready to run versions:
- WinForms - Windows Forms desktop application
- WebSite - Wisej web site IIS ready
- WebStandalone - Wisej web site packed into a desktop .exe file

These applications use the remote AppServerHost hosted on Azure.

### [Release 4.6.606](https://github.com/MarimerLLC/cslacontrib/releases/tag/v4.6.606) published on NuGet (10-04-2018).

Maintenance release:
1. Promote .NET 4.6 projects to .NET 4.6.1
2. Fetch Wisej dependency from [NuGet](https://www.nuget.org/packages/Wisej/) (no need to intall Wisej)

All samples were updated to CSLA .Net 4.7.100 and CslaContrib 4.6.606

__N.B. - To run [ProjectTracker Wisej Web](https://github.com/MarimerLLC/cslacontrib/tree/master/trunk/samples/ProjectTracker) sample you don't need to install Wisej.__

### Release Notes

- [Releases 4.6.601 to 4.6.606](Release_Notes_4.6.606.md)
- [Release 4.6.600](Release_Notes_4.6.600.md)
