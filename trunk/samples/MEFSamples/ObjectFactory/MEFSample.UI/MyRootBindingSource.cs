using System.Windows.Forms;
using CslaContrib.Windows;
using MEFSample.Business;

namespace MEFSample.UI
{
  public partial class MyRootBindingSource : Form
  {
    public MyRootBindingSource()
    {
      InitializeComponent();
    }

    private void MyRootForm_Load(object sender, System.EventArgs e)
    {
      rootBindingSource.Rebind(MyRoot.GetRoot());
    }
  }
}