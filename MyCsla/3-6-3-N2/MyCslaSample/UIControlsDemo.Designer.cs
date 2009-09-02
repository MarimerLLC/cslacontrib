namespace MyCslaSample
{
  partial class UIControlsDemo
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
      System.Windows.Forms.Label address1Label;
      System.Windows.Forms.Label nameLabel;
      System.Windows.Forms.Label salaryLabel;
      System.Windows.Forms.Label countryCodeLabel;
      this.address1TextBox = new System.Windows.Forms.TextBox();
      this.testRootBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.nameTextBox = new System.Windows.Forms.TextBox();
      this.salaryTextBox = new System.Windows.Forms.TextBox();
      this.errorWarnInfoProvider1 = new Csla.Windows.ErrorWarnInfoProvider(this.components);
      this.button1 = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.bindableRadioButtons2 = new MyCsla.Windows.BindableRadioButtons();
      this.customerTypeNameValueListBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.bindableRadioButtons1 = new MyCsla.Windows.BindableRadioButtons();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.smartDateTimePicker1 = new Csla.Windows.SmartDateTimePicker();
      this.label3 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.textBox4 = new System.Windows.Forms.TextBox();
      this.countryCodeTextBox = new System.Windows.Forms.TextBox();
      this.button2 = new System.Windows.Forms.Button();
      this.textBox5 = new System.Windows.Forms.TextBox();
      address1Label = new System.Windows.Forms.Label();
      nameLabel = new System.Windows.Forms.Label();
      salaryLabel = new System.Windows.Forms.Label();
      countryCodeLabel = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.testRootBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorWarnInfoProvider1)).BeginInit();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.customerTypeNameValueListBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // address1Label
      // 
      address1Label.AutoSize = true;
      address1Label.Location = new System.Drawing.Point(33, 53);
      address1Label.Name = "address1Label";
      address1Label.Size = new System.Drawing.Size(54, 13);
      address1Label.TabIndex = 2;
      address1Label.Text = "Address1:";
      // 
      // nameLabel
      // 
      nameLabel.AutoSize = true;
      nameLabel.Location = new System.Drawing.Point(33, 27);
      nameLabel.Name = "nameLabel";
      nameLabel.Size = new System.Drawing.Size(38, 13);
      nameLabel.TabIndex = 0;
      nameLabel.Text = "Name:";
      // 
      // salaryLabel
      // 
      salaryLabel.AutoSize = true;
      salaryLabel.Location = new System.Drawing.Point(33, 79);
      salaryLabel.Name = "salaryLabel";
      salaryLabel.Size = new System.Drawing.Size(39, 13);
      salaryLabel.TabIndex = 4;
      salaryLabel.Text = "Salary:";
      // 
      // countryCodeLabel
      // 
      countryCodeLabel.AutoSize = true;
      countryCodeLabel.Location = new System.Drawing.Point(34, 138);
      countryCodeLabel.Name = "countryCodeLabel";
      countryCodeLabel.Size = new System.Drawing.Size(74, 13);
      countryCodeLabel.TabIndex = 14;
      countryCodeLabel.Text = "Country Code:";
      // 
      // address1TextBox
      // 
      this.address1TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.testRootBindingSource, "Address1", true));
      this.address1TextBox.Location = new System.Drawing.Point(93, 50);
      this.address1TextBox.Name = "address1TextBox";
      this.address1TextBox.Size = new System.Drawing.Size(200, 20);
      this.address1TextBox.TabIndex = 3;
      // 
      // testRootBindingSource
      // 
      this.testRootBindingSource.DataSource = typeof(MyCslaSample.Entities.TestRoot);
      this.testRootBindingSource.CurrentItemChanged += new System.EventHandler(this.testRootBindingSource_CurrentItemChanged);
      // 
      // nameTextBox
      // 
      this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.testRootBindingSource, "Name", true));
      this.nameTextBox.Location = new System.Drawing.Point(93, 24);
      this.nameTextBox.Name = "nameTextBox";
      this.nameTextBox.Size = new System.Drawing.Size(200, 20);
      this.nameTextBox.TabIndex = 1;
      // 
      // salaryTextBox
      // 
      this.salaryTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.testRootBindingSource, "Salary", true));
      this.salaryTextBox.Location = new System.Drawing.Point(93, 76);
      this.salaryTextBox.Name = "salaryTextBox";
      this.salaryTextBox.Size = new System.Drawing.Size(98, 20);
      this.salaryTextBox.TabIndex = 5;
      // 
      // errorWarnInfoProvider1
      // 
      this.errorWarnInfoProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.errorWarnInfoProvider1.BlinkStyleInformation = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.errorWarnInfoProvider1.BlinkStyleWarning = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.errorWarnInfoProvider1.ContainerControl = this;
      this.errorWarnInfoProvider1.DataSource = this.testRootBindingSource;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(36, 340);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 11;
      this.button1.Text = "Save";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.bindableRadioButtons2);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.bindableRadioButtons1);
      this.groupBox1.Controls.Add(this.comboBox1);
      this.groupBox1.Location = new System.Drawing.Point(36, 161);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(257, 173);
      this.groupBox1.TabIndex = 8;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "CustomerType";
      // 
      // bindableRadioButtons2
      // 
      this.bindableRadioButtons2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.bindableRadioButtons2.ButtonCount = 2;
      this.bindableRadioButtons2.ButtonPadding = new System.Drawing.Point(0, 0);
      this.bindableRadioButtons2.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.testRootBindingSource, "CustomerType", true));
      this.bindableRadioButtons2.DataSource = this.customerTypeNameValueListBindingSource;
      this.bindableRadioButtons2.DisplayMember = "Value";
      this.bindableRadioButtons2.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
      this.bindableRadioButtons2.Location = new System.Drawing.Point(45, 136);
      this.bindableRadioButtons2.Margin = new System.Windows.Forms.Padding(0);
      this.bindableRadioButtons2.Name = "bindableRadioButtons2";
      this.bindableRadioButtons2.SelectedValue = null;
      this.bindableRadioButtons2.Size = new System.Drawing.Size(197, 24);
      this.bindableRadioButtons2.TabIndex = 0;
      this.bindableRadioButtons2.UsedLookupCharacters = null;
      this.bindableRadioButtons2.ValueMember = "Key";
      // 
      // customerTypeNameValueListBindingSource
      // 
      this.customerTypeNameValueListBindingSource.DataSource = typeof(MyCslaSample.Entities.CustomerTypeNameValueList);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(15, 64);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(80, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "As radiobuttons";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(15, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(97, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "As combobox input";
      // 
      // bindableRadioButtons1
      // 
      this.bindableRadioButtons1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.bindableRadioButtons1.ButtonCount = 3;
      this.bindableRadioButtons1.ButtonPadding = new System.Drawing.Point(0, 0);
      this.bindableRadioButtons1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.testRootBindingSource, "CustomerType", true));
      this.bindableRadioButtons1.DataSource = this.customerTypeNameValueListBindingSource;
      this.bindableRadioButtons1.DisplayMember = "Value";
      this.bindableRadioButtons1.Location = new System.Drawing.Point(130, 64);
      this.bindableRadioButtons1.Margin = new System.Windows.Forms.Padding(0);
      this.bindableRadioButtons1.Name = "bindableRadioButtons1";
      this.bindableRadioButtons1.SelectedValue = null;
      this.bindableRadioButtons1.Size = new System.Drawing.Size(112, 60);
      this.bindableRadioButtons1.TabIndex = 3;
      this.bindableRadioButtons1.UsedLookupCharacters = null;
      this.bindableRadioButtons1.ValueMember = "Key";
      // 
      // comboBox1
      // 
      this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.testRootBindingSource, "CustomerType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.comboBox1.DataSource = this.customerTypeNameValueListBindingSource;
      this.comboBox1.DisplayMember = "Value";
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(130, 19);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(121, 21);
      this.comboBox1.TabIndex = 1;
      this.comboBox1.ValueMember = "Key";
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // smartDateTimePicker1
      // 
      this.smartDateTimePicker1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.testRootBindingSource, "Founded", true));
      this.smartDateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.smartDateTimePicker1.Location = new System.Drawing.Point(93, 104);
      this.smartDateTimePicker1.Name = "smartDateTimePicker1";
      this.smartDateTimePicker1.Size = new System.Drawing.Size(200, 20);
      this.smartDateTimePicker1.TabIndex = 7;
      this.smartDateTimePicker1.TabStop = false;
      this.smartDateTimePicker1.Value = new System.DateTime(((long)(0)));
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(33, 108);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(49, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Founded";
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(315, 88);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new System.Drawing.Size(247, 68);
      this.textBox1.TabIndex = 9;
      this.textBox1.TabStop = false;
      this.textBox1.Text = "SmartDatePicker allows text input (f.ex t = today, y=yesterday, +=tomorrow)\r\nThes" +
          "e texts are localizable in Csla and may vary between cultures. ";
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(315, 227);
      this.textBox2.Multiline = true;
      this.textBox2.Name = "textBox2";
      this.textBox2.ReadOnly = true;
      this.textBox2.Size = new System.Drawing.Size(247, 88);
      this.textBox2.TabIndex = 10;
      this.textBox2.TabStop = false;
      this.textBox2.Text = "BindableRadioButtons work just like a ComboBox in terms of databinding. Suitable " +
          "for a few items in list and may be vertical or horizontal. \r\n\r\nVery suitable for" +
          " questionnaires like Yes/No";
      // 
      // textBox3
      // 
      this.textBox3.Location = new System.Drawing.Point(315, 12);
      this.textBox3.Multiline = true;
      this.textBox3.Name = "textBox3";
      this.textBox3.ReadOnly = true;
      this.textBox3.Size = new System.Drawing.Size(247, 68);
      this.textBox3.TabIndex = 12;
      this.textBox3.TabStop = false;
      this.textBox3.Text = "Just for demo purposes: \r\nTry empty name, more than 30 chars and more than 50 cha" +
          "rs.\r\nTry salary below 100k, between 100k and 200k and over 200k";
      // 
      // textBox4
      // 
      this.textBox4.Location = new System.Drawing.Point(315, 340);
      this.textBox4.Multiline = true;
      this.textBox4.Name = "textBox4";
      this.textBox4.ReadOnly = true;
      this.textBox4.Size = new System.Drawing.Size(247, 35);
      this.textBox4.TabIndex = 13;
      this.textBox4.TabStop = false;
      this.textBox4.Text = "Save button is enabled/disabled based on IsSavable";
      // 
      // countryCodeTextBox
      // 
      this.countryCodeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.testRootBindingSource, "CountryCode", true));
      this.countryCodeTextBox.Location = new System.Drawing.Point(114, 135);
      this.countryCodeTextBox.Name = "countryCodeTextBox";
      this.countryCodeTextBox.ReadOnly = true;
      this.countryCodeTextBox.Size = new System.Drawing.Size(40, 20);
      this.countryCodeTextBox.TabIndex = 15;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(160, 133);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(31, 23);
      this.button2.TabIndex = 16;
      this.button2.Text = "...";
      this.button2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // textBox5
      // 
      this.textBox5.Location = new System.Drawing.Point(315, 162);
      this.textBox5.Multiline = true;
      this.textBox5.Name = "textBox5";
      this.textBox5.ReadOnly = true;
      this.textBox5.Size = new System.Drawing.Size(247, 55);
      this.textBox5.TabIndex = 17;
      this.textBox5.TabStop = false;
      this.textBox5.Text = "CountryCode searc button show a generic ListSearchForm with filtering for use whe" +
          "n too many items for a combo box";
      // 
      // UIControlsDemo
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(573, 400);
      this.Controls.Add(this.textBox5);
      this.Controls.Add(this.button2);
      this.Controls.Add(countryCodeLabel);
      this.Controls.Add(this.countryCodeTextBox);
      this.Controls.Add(this.textBox4);
      this.Controls.Add(this.textBox3);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.smartDateTimePicker1);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.button1);
      this.Controls.Add(address1Label);
      this.Controls.Add(this.address1TextBox);
      this.Controls.Add(nameLabel);
      this.Controls.Add(this.nameTextBox);
      this.Controls.Add(salaryLabel);
      this.Controls.Add(this.salaryTextBox);
      this.Name = "UIControlsDemo";
      this.Text = "UIControlsDemo";
      ((System.ComponentModel.ISupportInitialize)(this.testRootBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorWarnInfoProvider1)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.customerTypeNameValueListBindingSource)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.BindingSource testRootBindingSource;
    private System.Windows.Forms.TextBox address1TextBox;
    private System.Windows.Forms.TextBox nameTextBox;
    private System.Windows.Forms.TextBox salaryTextBox;
    private Csla.Windows.ErrorWarnInfoProvider errorWarnInfoProvider1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private MyCsla.Windows.BindableRadioButtons bindableRadioButtons1;
    private System.Windows.Forms.Label label3;
    private Csla.Windows.SmartDateTimePicker smartDateTimePicker1;
    private System.Windows.Forms.BindingSource customerTypeNameValueListBindingSource;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private MyCsla.Windows.BindableRadioButtons bindableRadioButtons2;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.TextBox textBox4;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.TextBox countryCodeTextBox;
    private System.Windows.Forms.TextBox textBox5;
  }
}