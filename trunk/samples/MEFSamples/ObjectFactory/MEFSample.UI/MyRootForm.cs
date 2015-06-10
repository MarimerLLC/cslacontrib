using System.Windows.Forms;
using CslaContrib.Windows;
using MEFSample.Business;

namespace MEFSample.UI
{
  public partial class MyRootForm : Form
  {
    public MyRootForm()
    {
      InitializeComponent();
    }

    private void MyRootForm_Load(object sender, System.EventArgs e)
    {
      rootBindingSource.Rebind(MyRoot.GetRoot());
    }
  }
}
