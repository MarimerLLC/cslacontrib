namespace PTWisej
{
  partial class RolesEdit
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.CancelButton = new Wisej.Web.Button();
      this.SaveButton = new Wisej.Web.Button();
      this.rolesBindingSource = new Wisej.Web.BindingSource(this.components);
      this.RolesDataGridView = new Wisej.Web.DataGridView();
      this.DataGridViewTextBoxColumn1 = new Wisej.Web.DataGridViewTextBoxColumn();
      this.DataGridViewTextBoxColumn2 = new Wisej.Web.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.rolesBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.RolesDataGridView)).BeginInit();
      this.SuspendLayout();
      // 
      // CancelButton
      // 
      this.CancelButton.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Right)));
      this.CancelButton.Location = new System.Drawing.Point(453, 42);
      this.CancelButton.Name = "CancelButton";
      this.CancelButton.Size = new System.Drawing.Size(75, 23);
      this.CancelButton.TabIndex = 2;
      this.CancelButton.Text = "Cancel";
      //this.CancelButton.UseVisualStyleBackColor = true;
      this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // SaveButton
      // 
      this.SaveButton.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Right)));
      this.SaveButton.Location = new System.Drawing.Point(453, 13);
      this.SaveButton.Name = "SaveButton";
      this.SaveButton.Size = new System.Drawing.Size(75, 23);
      this.SaveButton.TabIndex = 1;
      this.SaveButton.Text = "Save";
      //this.SaveButton.UseVisualStyleBackColor = true;
      this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
      // 
      // rolesBindingSource
      // 
      this.rolesBindingSource.DataSource = typeof(ProjectTracker.Library.Admin.RoleEditList);
      // 
      // RolesDataGridView
      // 
      this.RolesDataGridView.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom)
                  | Wisej.Web.AnchorStyles.Left)
                  | Wisej.Web.AnchorStyles.Right)));
      this.RolesDataGridView.AutoGenerateColumns = false;
      this.RolesDataGridView.Columns.AddRange(new Wisej.Web.DataGridViewColumn[] {
            this.DataGridViewTextBoxColumn1,
            this.DataGridViewTextBoxColumn2});
      this.RolesDataGridView.DataSource = this.rolesBindingSource;
      this.RolesDataGridView.Location = new System.Drawing.Point(12, 13);
      this.RolesDataGridView.MultiSelect = false;
      this.RolesDataGridView.Name = "RolesDataGridView";
      this.RolesDataGridView.Size = new System.Drawing.Size(435, 323);
      this.RolesDataGridView.TabIndex = 0;
      // 
      // DataGridViewTextBoxColumn1
      // 
      this.DataGridViewTextBoxColumn1.DataPropertyName = "Id";
      this.DataGridViewTextBoxColumn1.HeaderText = "Id";
      this.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1";
      // 
      // DataGridViewTextBoxColumn2
      // 
      this.DataGridViewTextBoxColumn2.DataPropertyName = "Name";
      this.DataGridViewTextBoxColumn2.HeaderText = "Name";
      this.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2";
      // 
      // RolesEdit
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
      this.Controls.Add(this.RolesDataGridView);
      this.Controls.Add(this.CancelButton);
      this.Controls.Add(this.SaveButton);
      this.Name = "RolesEdit";
      this.Size = new System.Drawing.Size(541, 348);
      this.Load += new System.EventHandler(this.RolesEdit_Load);
      this.CurrentPrincipalChanged += new System.EventHandler(this.RolesEdit_CurrentPrincipalChanged);
      ((System.ComponentModel.ISupportInitialize)(this.rolesBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.RolesDataGridView)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    internal Wisej.Web.Button CancelButton;
    internal Wisej.Web.Button SaveButton;
    internal Wisej.Web.BindingSource rolesBindingSource;
    internal Wisej.Web.DataGridView RolesDataGridView;
    internal Wisej.Web.DataGridViewTextBoxColumn DataGridViewTextBoxColumn1;
    internal Wisej.Web.DataGridViewTextBoxColumn DataGridViewTextBoxColumn2;
  }
}
