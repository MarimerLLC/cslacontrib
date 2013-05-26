using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Csla;

namespace SmartDateExtendedParserSample.Sl
{
  public partial class MainPage : UserControl
  {
    public MainPage()
    {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
      //      richTextBox1.LoadFile(Application.StartupPath + @"\Resources\SmartDateExtendedParser.rtf");
    }

    private void unchangedTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      SmartDate.CustomParser = null;
      try
      {
        ((TextBox) sender).Text = SmartDate.Parse(((TextBox) sender).Text);
      }
      catch (System.ArgumentException ex)
      {
      }
    }

    private void extendedTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      SmartDate.CustomParser = CslaContrib.SmartDateExtendedParser.ExtendedParser;
      try
      {
        ((TextBox) sender).Text = SmartDate.Parse(((TextBox) sender).Text);
      }
      catch (System.ArgumentException ex)
      {
      }
    }
  }
}
