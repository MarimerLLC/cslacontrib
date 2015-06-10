namespace MEFSample.UI
{
  partial class CustomerListForm
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
      this.components = new System.ComponentModel.Container();
      this.customerListBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.customerListDataGridView = new System.Windows.Forms.DataGridView();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.customerListBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.customerListDataGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // customerListBindingSource
      // 
      this.customerListBindingSource.DataSource = typeof(MEFSample.Business.CustomerInfo);
      // 
      // customerListDataGridView
      // 
      this.customerListDataGridView.AutoGenerateColumns = false;
      this.customerListDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.customerListDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
      this.customerListDataGridView.DataSource = this.customerListBindingSource;
      this.customerListDataGridView.Location = new System.Drawing.Point(12, 12);
      this.customerListDataGridView.Name = "customerListDataGridView";
      this.customerListDataGridView.Size = new System.Drawing.Size(581, 228);
      this.customerListDataGridView.TabIndex = 1;
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
      this.dataGridViewTextBoxColumn1.HeaderText = "Id";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      // 
      // dataGridViewTextBoxColumn2
      // 
      this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
      this.dataGridViewTextBoxColumn2.HeaderText = "Name";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.ReadOnly = true;
      // 
      // CustomerListForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(605, 252);
      this.Controls.Add(this.customerListDataGridView);
      this.Name = "CustomerListForm";
      this.Text = "CustomerListForm";
      this.Load += new System.EventHandler(this.CustomerListForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.customerListBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.customerListDataGridView)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource customerListBindingSource;
    private System.Windows.Forms.DataGridView customerListDataGridView;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
  }
}