using System;
using System.Windows.Forms;
using MEFSample.Business;

namespace MEFSample.UI
{
  public partial class MyRootForm : Form
  {
    public MyRoot RootObject { get; set; }

    public MyRootForm()
    {
      InitializeComponent();
    }

    private void MyRootForm_Load(object sender, EventArgs e)
    {
      RootObject = MyRoot.GetRoot(5);
      Rebind();
    }

    private void Rebind()
    {
      nameTextBox.DataBindings.Add(new Binding("Text", RootObject, "Name", true));
      num1TextBox.DataBindings.Add(new Binding("Text", RootObject, "Num1", true));
      num2TextBox.DataBindings.Add(new Binding("Text", RootObject, "Num2", true));
      sumTextBox.DataBindings.Add(new Binding("Text", RootObject, "Sum", true));
      textBox2.DataBindings.Add(new Binding("Text", RootObject, "Id", true));

      errorProvider1.DataSource = RootObject;
    }
  }
}