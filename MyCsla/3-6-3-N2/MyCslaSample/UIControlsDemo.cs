using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Csla.Core;
using Csla.Windows;
using MyCsla.Windows;
using MyCslaSample.Entities;

namespace MyCslaSample
{
  public partial class UIControlsDemo : Form
  {
    private ISavable MyRoot { get; set; }

    public UIControlsDemo()
    {
      InitializeComponent();
      this.Closing += UIControlsDemo_Closing;

      MyRoot = TestRoot.NewEditableRoot();
      BindUI();
    }

    private void UIControlsDemo_Closing(object sender, CancelEventArgs e)
    {
      // if not unbound from data make sure to do it now 
      UnbindUI(true);
    }

    private void BindUI()
    {
      BindingHelper.RebindBindingSource(customerTypeNameValueListBindingSource,
                                        CustomerTypeNameValueList.GetNameValueList());
      BindingHelper.RebindBindingSource(testRootBindingSource, MyRoot);
    }

    private void UnbindUI(bool cancel)
    {
      BindingHelper.UnbindBindingSource(testRootBindingSource, cancel, true);
      BindingHelper.UnbindBindingSource(customerTypeNameValueListBindingSource, false, false);
    }

    private void testRootBindingSource_CurrentItemChanged(object sender, EventArgs e)
    {
      button1.Enabled = ((ITrackStatus) MyRoot).IsSavable;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void button2_Click(object sender, EventArgs e)
    {
      using (var selectForm = new MyCsla.Windows.ListSelectForm())
      {
   
        selectForm.SourceList =  CountryCodesNameValueList.GetNameValueList().ToArray();
        selectForm.ListLabel = "Select country";
        var result = selectForm.ShowDialog(this);
        if (result == DialogResult.OK)
        {
          countryCodeTextBox.Text = (string) selectForm.SelectedKey;
        }
      }
    }
  }
}
