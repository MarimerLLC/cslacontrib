using System;
using System.Windows.Forms;

namespace MEFSample.UI
{
  public partial class MenuForm : Form
  {
    public MenuForm()
    {
      InitializeComponent();
    }

    private void myRootBindingSource_Click(object sender, EventArgs e)
    {
      using (var form = new MyRootBindingSource())
      {
        form.ShowDialog(this);
      }
    }

    private void myRootNotifyPropertyChanged_Click(object sender, EventArgs e)
    {
      using (var form = new MyRootNotifyPropertyChanged())
      {
        form.ShowDialog(this);
      }
    }

    private void customerList_Click(object sender, EventArgs e)
    {
      using (var form = new CustomerListForm())
      {
        form.ShowDialog(this);
      }
    }
  }
}