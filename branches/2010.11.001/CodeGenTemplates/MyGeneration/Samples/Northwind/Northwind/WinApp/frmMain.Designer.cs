namespace WinApp
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lbEmployee = new System.Windows.Forms.ListBox();
			this.pg = new System.Windows.Forms.PropertyGrid();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lbEmployee);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pg);
			this.splitContainer1.Size = new System.Drawing.Size(549, 342);
			this.splitContainer1.SplitterDistance = 158;
			this.splitContainer1.TabIndex = 0;
			// 
			// lbEmployee
			// 
			this.lbEmployee.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbEmployee.FormattingEnabled = true;
			this.lbEmployee.Location = new System.Drawing.Point(0, 0);
			this.lbEmployee.Name = "lbEmployee";
			this.lbEmployee.Size = new System.Drawing.Size(158, 342);
			this.lbEmployee.TabIndex = 0;
			this.lbEmployee.SelectedValueChanged += new System.EventHandler(this.lbEmployee_SelectedValueChanged);
			// 
			// pg
			// 
			this.pg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pg.Location = new System.Drawing.Point(0, 0);
			this.pg.Name = "pg";
			this.pg.Size = new System.Drawing.Size(387, 342);
			this.pg.TabIndex = 0;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(549, 342);
			this.Controls.Add(this.splitContainer1);
			this.Name = "frmMain";
			this.Text = "Northwind CSLA MyGeneration Example";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox lbEmployee;
		private System.Windows.Forms.PropertyGrid pg;
	}
}

