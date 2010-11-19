using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Diagnostics;
using OutlookStyleApp;

namespace PTWpf.Shell
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>

  public partial class App : System.Windows.Application
  {
      protected override void OnStartup(StartupEventArgs e)
      {
          base.OnStartup(e);
          new Bootstrapper().Run();
      }
  }
}