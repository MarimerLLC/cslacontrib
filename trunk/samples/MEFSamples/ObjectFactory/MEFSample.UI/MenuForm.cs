using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MEFSample.UI
{
  public partial class MenuForm : Form
  {
    public MenuForm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      var form = new MyRootForm();
      form.ShowDialog(this);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      var form = new CustomerListForm();
      form.ShowDialog(this);
    }
  }
}
