namespace CslaContrib.WisejWeb
{
  partial class BindableRadioButtons
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
      this.radioButton1 = new Wisej.Web.RadioButton();
      this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
      this.radioButton2 = new Wisej.Web.RadioButton();
      this.radioButton3 = new Wisej.Web.RadioButton();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // radioButton1
      // 
      this.radioButton1.AutoSize = true;
      this.radioButton1.Location = new System.Drawing.Point(3, 3);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new System.Drawing.Size(85, 17);
      this.radioButton1.TabIndex = 0;
      this.radioButton1.TabStop = true;
      this.radioButton1.Text = "radioButton1";
      //this.radioButton1.UseVisualStyleBackColor = true;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.radioButton1);
      this.flowLayoutPanel1.Controls.Add(this.radioButton2);
      this.flowLayoutPanel1.Controls.Add(this.radioButton3);
      this.flowLayoutPanel1.Dock = Wisej.Web.DockStyle.Fill;
      this.flowLayoutPanel1.FlowDirection = Wisej.Web.FlowDirection.TopDown;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.Margin = new Wisej.Web.Padding(0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(112, 93);
      this.flowLayoutPanel1.TabIndex = 2;
      // 
      // radioButton2
      // 
      this.radioButton2.AutoSize = true;
      this.radioButton2.Location = new System.Drawing.Point(3, 26);
      this.radioButton2.Name = "radioButton2";
      this.radioButton2.Size = new System.Drawing.Size(85, 17);
      this.radioButton2.TabIndex = 1;
      this.radioButton2.TabStop = true;
      this.radioButton2.Text = "radioButton2";
      //this.radioButton2.UseVisualStyleBackColor = true;
      // 
      // radioButton3
      // 
      this.radioButton3.AutoSize = true;
      this.flowLayoutPanel1.SetFlowBreak(this.radioButton3, true);
      this.radioButton3.Location = new System.Drawing.Point(3, 49);
      this.radioButton3.Name = "radioButton3";
      this.radioButton3.Size = new System.Drawing.Size(85, 17);
      this.radioButton3.TabIndex = 2;
      this.radioButton3.TabStop = true;
      this.radioButton3.Text = "radioButton3";
      //this.radioButton3.UseVisualStyleBackColor = true;
      // 
      // BindableRadioButtons
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
      this.AutoSizeMode = Wisej.Web.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.flowLayoutPanel1);
      this.Margin = new Wisej.Web.Padding(0);
      this.Name = "BindableRadioButtons";
      this.Size = new System.Drawing.Size(112, 93);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private Wisej.Web.RadioButton radioButton1;
    private Wisej.Web.FlowLayoutPanel flowLayoutPanel1;
    private Wisej.Web.RadioButton radioButton2;
    private Wisej.Web.RadioButton radioButton3;
  }
}
