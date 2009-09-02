using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyCslaSample
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      new StatusBarExtenderDemo().Show();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      new UIControlsDemo().Show();
    }
  }
}
