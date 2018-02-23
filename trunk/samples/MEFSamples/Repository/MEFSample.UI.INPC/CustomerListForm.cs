using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CslaContrib.Windows;
using MEFSample.Business;

namespace MEFSample.UI
{
  public partial class CustomerListForm : Form
  {
    public CustomerListForm()
    {
      InitializeComponent();
    }

    private void CustomerListForm_Load(object sender, EventArgs e)
    {
      // fetch data async 
      CustomerList.BeginGetReadOnlyList(string.Empty, (o, ev) =>
                                                        {
                                                          if (ev.Error != null)
                                                          {
                                                            MessageBox.Show(this, ev.Error.Message, "Error loading data",
                                                                            MessageBoxButtons.OKCancel);
                                                          }
                                                          else
                                                          {
                                                            customerListBindingSource.Rebind(ev.Object);
                                                          }

                                                        });
    }


  }
}
