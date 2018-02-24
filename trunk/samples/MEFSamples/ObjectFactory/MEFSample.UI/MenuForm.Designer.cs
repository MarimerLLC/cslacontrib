namespace MEFSample.UI
{
  partial class MenuForm
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
      this.myRootBindingSource = new System.Windows.Forms.Button();
      this.myRootNotifyPropertyChanged = new System.Windows.Forms.Button();
      this.customerList = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // myRootBindingSource
      // 
      this.myRootBindingSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.myRootBindingSource.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.myRootBindingSource.Location = new System.Drawing.Point(45, 35);
      this.myRootBindingSource.Name = "myRootBindingSource";
      this.myRootBindingSource.Size = new System.Drawing.Size(289, 50);
      this.myRootBindingSource.TabIndex = 0;
      this.myRootBindingSource.Text = "Root data and validation\r\nBinding Source";
      this.myRootBindingSource.UseVisualStyleBackColor = true;
      this.myRootBindingSource.Click += new System.EventHandler(this.myRootBindingSource_Click);
      // 
      // myRootNotifyPropertyChanged
      // 
      this.myRootNotifyPropertyChanged.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.myRootNotifyPropertyChanged.Location = new System.Drawing.Point(45, 124);
      this.myRootNotifyPropertyChanged.Name = "myRootNotifyPropertyChanged";
      this.myRootNotifyPropertyChanged.Size = new System.Drawing.Size(289, 50);
      this.myRootNotifyPropertyChanged.TabIndex = 1;
      this.myRootNotifyPropertyChanged.Text = "Root data and validation\r\nINotifyPropertyChanged";
      this.myRootNotifyPropertyChanged.UseVisualStyleBackColor = true;
      this.myRootNotifyPropertyChanged.Click += new System.EventHandler(this.myRootNotifyPropertyChanged_Click);
      // 
      // customerList
      // 
      this.customerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.customerList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.customerList.Location = new System.Drawing.Point(45, 213);
      this.customerList.Name = "customerList";
      this.customerList.Size = new System.Drawing.Size(289, 50);
      this.customerList.TabIndex = 2;
      this.customerList.Text = "Customer List";
      this.customerList.UseVisualStyleBackColor = true;
      this.customerList.Click += new System.EventHandler(this.customerList_Click);
      // 
      // MenuForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(384, 309);
      this.Controls.Add(this.customerList);
      this.Controls.Add(this.myRootBindingSource);
      this.Controls.Add(this.myRootNotifyPropertyChanged);
      this.MinimumSize = new System.Drawing.Size(300, 200);
      this.Name = "MenuForm";
      this.Text = "Menu Form MEF Object Factory";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button myRootBindingSource;
    private System.Windows.Forms.Button myRootNotifyPropertyChanged;
    private System.Windows.Forms.Button customerList;

  }
}