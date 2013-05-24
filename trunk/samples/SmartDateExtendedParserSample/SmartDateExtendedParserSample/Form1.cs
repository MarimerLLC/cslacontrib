using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Csla;
using CslaContrib;

namespace SmartDateExtendedParser.Test
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, System.EventArgs e)
    {
      Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
      richTextBox1.LoadFile(Application.StartupPath + @"\Resources\SmartDateExtendedParser.rtf");
    }

    private void unchangedTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
      SmartDate.CustomParser = null;
      try
      {
        ((TextBox)sender).Text = SmartDate.Parse(((TextBox)sender).Text);
      }
      catch (System.ArgumentException ex)
      {
      }
    }

    private void extendedTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
      SmartDate.CustomParser = CslaContrib.SmartDateExtendedParser.ExtendedParser;
      try
      {
        ((TextBox)sender).Text = SmartDate.Parse(((TextBox)sender).Text);
      }
      catch (System.ArgumentException ex)
      {
      }
    }

  }
}
