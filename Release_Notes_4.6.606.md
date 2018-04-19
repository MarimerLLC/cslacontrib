# CslaContrib 4.6.606 Release Notes

# Changes since release [4.6.600](Release_Notes_4.6.600.md)

## Release 4.6.606 - maintenance
- Promote 4.6 projects to 4.6.1 (tested against CSLA .NET 4.7.100 under .NET 4.6.1)
- Fetch Wisej dependency from [NuGet](https://www.nuget.org/packages/Wisej/) (no need to intall Wisej)

## Release 4.6.605 - ErrorWarnInfoProvider fixes and enhancements
- Fixes to CslaContrib-Windows and CslaContrib-Wisej
- ErrorWarnInfoProvider fixes and enhancements:
  - support INotifyPropertyChanged objects (used to support only BindingSource).
  - allow control discovery when the DataSource is set.
  - fix Information message that was showing only property name.

## Release 4.6.604 - critical fix
- Fix wrong ICloneable namespace

## Release 4.6.603 - maintenance
- bind to Csla 4.6.100 for backwards support

## Release 4.6.602 - minor rules fixing:
- allows rule NoDuplicates to be used with all kind of CSLA editable collections
- reorganize rules namespaces

## Release 4.6.601 - CslaContrib-Wisej update
- ApplicationContextManager now uses Wisej.Base.ApplicationBase.