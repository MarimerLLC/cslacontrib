namespace SmartDateExtendedParser.Test
{
  partial class Form1
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
      this.unchangedLabel = new System.Windows.Forms.Label();
      this.unchangedTextBox = new System.Windows.Forms.TextBox();
      this.extendedTextBox = new System.Windows.Forms.TextBox();
      this.extendedLabel = new System.Windows.Forms.Label();
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.documentationLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // unchangedLabel
      // 
      this.unchangedLabel.AutoSize = true;
      this.unchangedLabel.Location = new System.Drawing.Point(42, 23);
      this.unchangedLabel.Name = "unchangedLabel";
      this.unchangedLabel.Size = new System.Drawing.Size(63, 13);
      this.unchangedLabel.TabIndex = 0;
      this.unchangedLabel.Text = "Standard parsing";
      // 
      // unchangedTextBox
      // 
      this.unchangedTextBox.Location = new System.Drawing.Point(142, 20);
      this.unchangedTextBox.Name = "unchangedTextBox";
      this.unchangedTextBox.Size = new System.Drawing.Size(100, 20);
      this.unchangedTextBox.TabIndex = 1;
      this.unchangedTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.unchangedTextBox_Validating);
      // 
      // extendedTextBox
      // 
      this.extendedTextBox.Location = new System.Drawing.Point(428, 20);
      this.extendedTextBox.Name = "extendedTextBox";
      this.extendedTextBox.Size = new System.Drawing.Size(100, 20);
      this.extendedTextBox.TabIndex = 3;
      this.extendedTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.extendedTextBox_Validating);
      // 
      // extendedLabel
      // 
      this.extendedLabel.AutoSize = true;
      this.extendedLabel.Location = new System.Drawing.Point(328, 23);
      this.extendedLabel.Name = "extendedLabel";
      this.extendedLabel.Size = new System.Drawing.Size(52, 13);
      this.extendedLabel.TabIndex = 2;
      this.extendedLabel.Text = "Extended parsing";
      // 
      // richTextBox1
      // 
      this.richTextBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.richTextBox1.Location = new System.Drawing.Point(12, 86);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ReadOnly = true;
      this.richTextBox1.Size = new System.Drawing.Size(568, 325);
      this.richTextBox1.TabIndex = 4;
      this.richTextBox1.Text = "";
      // 
      // documentationLabel
      // 
      this.documentationLabel.AutoSize = true;
      this.documentationLabel.Location = new System.Drawing.Point(12, 67);
      this.documentationLabel.Name = "documentationLabel";
      this.documentationLabel.Size = new System.Drawing.Size(79, 13);
      this.documentationLabel.TabIndex = 5;
      this.documentationLabel.Text = "Specifications";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(592, 423);
      this.Controls.Add(this.documentationLabel);
      this.Controls.Add(this.richTextBox1);
      this.Controls.Add(this.extendedTextBox);
      this.Controls.Add(this.extendedLabel);
      this.Controls.Add(this.unchangedTextBox);
      this.Controls.Add(this.unchangedLabel);
      this.Name = "Form1";
      this.Text = "SmartDate Extended Parser sample";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label unchangedLabel;
    private System.Windows.Forms.TextBox unchangedTextBox;
    private System.Windows.Forms.TextBox extendedTextBox;
    private System.Windows.Forms.Label extendedLabel;
    private System.Windows.Forms.RichTextBox richTextBox1;
    private System.Windows.Forms.Label documentationLabel;
  }
}

