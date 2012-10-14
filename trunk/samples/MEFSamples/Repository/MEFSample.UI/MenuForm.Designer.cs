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
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.button1);
      this.flowLayoutPanel1.Controls.Add(this.button2);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(192, 643);
      this.flowLayoutPanel1.TabIndex = 0;
      // 
      // button1
      // 
      this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.button1.Location = new System.Drawing.Point(3, 3);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(189, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "Root data and validation";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(3, 32);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(189, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "Customer list";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // MenuForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(192, 643);
      this.Controls.Add(this.flowLayoutPanel1);
      this.Name = "MenuForm";
      this.Text = "MenuForm";
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
  }
}