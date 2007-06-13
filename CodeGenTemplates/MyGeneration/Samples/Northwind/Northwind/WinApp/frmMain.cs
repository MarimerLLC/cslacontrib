using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Northwind.CSLA.Library;

namespace WinApp
{
	public partial class frmMain : Form
	{
		EmployeeInfoList _EmployeeInfoList = EmployeeInfoList.Get();
		Employee _Employee = null;
		object _LastObject = null;
		public frmMain()
		{
			InitializeComponent();
			lbEmployee.DataSource = _EmployeeInfoList;
			lbEmployee.DisplayMember = "FullNameSub";
			lbEmployee.ValueMember = "EmployeeID";
		}
		public void SaveChanges()
		{
			try
			{
				if (_Employee != null)
				{
					if (_Employee.IsSavable)
					{
						if (MessageBox.Show("Save Changes", "Empoyee Data Changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
							_Employee.Save();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		private void lbEmployee_SelectedValueChanged(object sender, EventArgs e)
		{
			try
			{
				SaveChanges();
				_Employee = Employee.Get((int)lbEmployee.SelectedValue);
				pg.SelectedObject = _Employee;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveChanges();
		}
	}
}