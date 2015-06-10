namespace MEFSample.UI
{
  partial class MyRootForm
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
      System.Windows.Forms.Label nameLabel;
      System.Windows.Forms.Label num1Label;
      System.Windows.Forms.Label num2Label;
      System.Windows.Forms.Label sumLabel;
      System.Windows.Forms.Label label1;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyRootForm));
      this.nameTextBox = new System.Windows.Forms.TextBox();
      this.rootBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.num1TextBox = new System.Windows.Forms.TextBox();
      this.num2TextBox = new System.Windows.Forms.TextBox();
      this.sumTextBox = new System.Windows.Forms.TextBox();
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      nameLabel = new System.Windows.Forms.Label();
      num1Label = new System.Windows.Forms.Label();
      num2Label = new System.Windows.Forms.Label();
      sumLabel = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.rootBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.SuspendLayout();
      // 
      // nameLabel
      // 
      nameLabel.AutoSize = true;
      nameLabel.Location = new System.Drawing.Point(23, 61);
      nameLabel.Name = "nameLabel";
      nameLabel.Size = new System.Drawing.Size(38, 13);
      nameLabel.TabIndex = 3;
      nameLabel.Text = "Name:";
      // 
      // num1Label
      // 
      num1Label.AutoSize = true;
      num1Label.Location = new System.Drawing.Point(23, 114);
      num1Label.Name = "num1Label";
      num1Label.Size = new System.Drawing.Size(38, 13);
      num1Label.TabIndex = 5;
      num1Label.Text = "Num1:";
      // 
      // num2Label
      // 
      num2Label.AutoSize = true;
      num2Label.Location = new System.Drawing.Point(23, 140);
      num2Label.Name = "num2Label";
      num2Label.Size = new System.Drawing.Size(38, 13);
      num2Label.TabIndex = 7;
      num2Label.Text = "Num2:";
      // 
      // sumLabel
      // 
      sumLabel.AutoSize = true;
      sumLabel.Location = new System.Drawing.Point(23, 166);
      sumLabel.Name = "sumLabel";
      sumLabel.Size = new System.Drawing.Size(31, 13);
      sumLabel.TabIndex = 9;
      sumLabel.Text = "Sum:";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(23, 35);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(19, 13);
      label1.TabIndex = 12;
      label1.Text = "Id:";
      // 
      // nameTextBox
      // 
      this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rootBindingSource, "Name", true));
      this.nameTextBox.Location = new System.Drawing.Point(73, 58);
      this.nameTextBox.Name = "nameTextBox";
      this.nameTextBox.Size = new System.Drawing.Size(104, 20);
      this.nameTextBox.TabIndex = 4;
      // 
      // rootBindingSource
      // 
      this.rootBindingSource.DataSource = typeof(MEFSample.Business.MyRoot);
      // 
      // num1TextBox
      // 
      this.num1TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rootBindingSource, "Num1", true));
      this.num1TextBox.Location = new System.Drawing.Point(73, 111);
      this.num1TextBox.Name = "num1TextBox";
      this.num1TextBox.Size = new System.Drawing.Size(104, 20);
      this.num1TextBox.TabIndex = 6;
      // 
      // num2TextBox
      // 
      this.num2TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rootBindingSource, "Num2", true));
      this.num2TextBox.Location = new System.Drawing.Point(73, 137);
      this.num2TextBox.Name = "num2TextBox";
      this.num2TextBox.Size = new System.Drawing.Size(104, 20);
      this.num2TextBox.TabIndex = 8;
      // 
      // sumTextBox
      // 
      this.sumTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rootBindingSource, "Sum", true));
      this.sumTextBox.Location = new System.Drawing.Point(73, 163);
      this.sumTextBox.Name = "sumTextBox";
      this.sumTextBox.Size = new System.Drawing.Size(104, 20);
      this.sumTextBox.TabIndex = 10;
      // 
      // errorProvider1
      // 
      this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.errorProvider1.ContainerControl = this;
      this.errorProvider1.DataSource = this.rootBindingSource;
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(207, 34);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new System.Drawing.Size(262, 194);
      this.textBox1.TabIndex = 11;
      this.textBox1.Text = resources.GetString("textBox1.Text");
      // 
      // textBox2
      // 
      this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rootBindingSource, "Id", true));
      this.textBox2.Location = new System.Drawing.Point(73, 32);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(104, 20);
      this.textBox2.TabIndex = 13;
      // 
      // MyRootForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(490, 266);
      this.Controls.Add(label1);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(nameLabel);
      this.Controls.Add(this.nameTextBox);
      this.Controls.Add(num1Label);
      this.Controls.Add(this.num1TextBox);
      this.Controls.Add(num2Label);
      this.Controls.Add(this.num2TextBox);
      this.Controls.Add(sumLabel);
      this.Controls.Add(this.sumTextBox);
      this.Name = "MyRootForm";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.MyRootForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.rootBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.BindingSource rootBindingSource;
    private System.Windows.Forms.TextBox nameTextBox;
    private System.Windows.Forms.TextBox num1TextBox;
    private System.Windows.Forms.TextBox num2TextBox;
    private System.Windows.Forms.TextBox sumTextBox;
    private System.Windows.Forms.ErrorProvider errorProvider1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
  }
}

