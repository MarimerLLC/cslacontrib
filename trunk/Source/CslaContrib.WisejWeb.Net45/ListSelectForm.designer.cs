namespace CslaContrib.WisejWeb
{
    partial class ListSelectForm
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
          this.sokLabel = new Wisej.Web.Label();
          this.sokTextBox = new Wisej.Web.TextBox();
          this.listSource = new Wisej.Web.BindingSource(this.components);
          this.acceptButton = new Wisej.Web.Button();
          this.cancelButton = new Wisej.Web.Button();
          this.resultListBox = new Wisej.Web.ListBox();
          this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
          this.listLabel = new Wisej.Web.Label();
          ((System.ComponentModel.ISupportInitialize)(this.listSource)).BeginInit();
          this.flowLayoutPanel1.SuspendLayout();
          this.SuspendLayout();
          // 
          // sokLabel
          // 
          this.sokLabel.Anchor = Wisej.Web.AnchorStyles.None;
          this.sokLabel.AutoSize = true;
          this.sokLabel.Location = new System.Drawing.Point(3, 6);
          this.sokLabel.Name = "sokLabel";
          this.sokLabel.Size = new System.Drawing.Size(111, 13);
          this.sokLabel.TabIndex = 0;
          this.sokLabel.Text = "Filter items containing:";
          this.sokLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // sokTextBox
          // 
          this.sokTextBox.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Left)
                      | Wisej.Web.AnchorStyles.Right)));
          this.sokTextBox.Location = new System.Drawing.Point(120, 3);
          this.sokTextBox.Name = "sokTextBox";
          this.sokTextBox.Size = new System.Drawing.Size(152, 20);
          this.sokTextBox.TabIndex = 1;
          this.sokTextBox.TextChanged += new System.EventHandler(this.sokTextBox_TextChanged);
          // 
          // listSource
          // 
          this.listSource.DataSource = typeof(CslaContrib.WisejWeb.ListSelectForm.MyNameValueList);
          // 
          // acceptButton
          // 
          this.acceptButton.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Bottom | Wisej.Web.AnchorStyles.Right)));
          this.acceptButton.Location = new System.Drawing.Point(130, 261);
          this.acceptButton.Name = "acceptButton";
          this.acceptButton.Size = new System.Drawing.Size(75, 23);
          this.acceptButton.TabIndex = 3;
          this.acceptButton.Text = "&Select";
          //this.acceptButton.UseVisualStyleBackColor = true;
          this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
          // 
          // cancelButton
          // 
          this.cancelButton.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Bottom | Wisej.Web.AnchorStyles.Right)));
          this.cancelButton.Cursor = Wisej.Web.Cursors.Arrow;
          this.cancelButton.DialogResult = Wisej.Web.DialogResult.Cancel;
          this.cancelButton.Location = new System.Drawing.Point(211, 261);
          this.cancelButton.Name = "cancelButton";
          this.cancelButton.Size = new System.Drawing.Size(75, 23);
          this.cancelButton.TabIndex = 4;
          this.cancelButton.Text = "&Cancel";
          //this.cancelButton.UseVisualStyleBackColor = true;
          this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
          // 
          // resultListBox
          // 
          this.resultListBox.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom)
                      | Wisej.Web.AnchorStyles.Left)
                      | Wisej.Web.AnchorStyles.Right)));
          this.resultListBox.DataSource = this.listSource;
          this.resultListBox.DisplayMember = "Value";
          this.resultListBox.FormattingEnabled = true;
          this.resultListBox.Location = new System.Drawing.Point(8, 64);
          this.resultListBox.Name = "resultListBox";
          this.resultListBox.Size = new System.Drawing.Size(278, 186);
          this.resultListBox.TabIndex = 2;
          this.resultListBox.ValueMember = "Key";
          this.resultListBox.DoubleClick += new System.EventHandler(this.acceptButton_Click);
          // 
          // flowLayoutPanel1
          // 
          this.flowLayoutPanel1.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Left)
                      | Wisej.Web.AnchorStyles.Right)));
          this.flowLayoutPanel1.Controls.Add(this.sokLabel);
          this.flowLayoutPanel1.Controls.Add(this.sokTextBox);
          this.flowLayoutPanel1.Location = new System.Drawing.Point(8, 12);
          this.flowLayoutPanel1.Name = "flowLayoutPanel1";
          this.flowLayoutPanel1.Size = new System.Drawing.Size(278, 26);
          this.flowLayoutPanel1.TabIndex = 0;
          this.flowLayoutPanel1.WrapContents = false;
          // 
          // listLabel
          // 
          this.listLabel.AutoSize = true;
          this.listLabel.Location = new System.Drawing.Point(11, 48);
          this.listLabel.Name = "listLabel";
          this.listLabel.Size = new System.Drawing.Size(62, 13);
          this.listLabel.TabIndex = 1;
          this.listLabel.Text = "Select item:";
          // 
          // ListSelectForm
          // 
          this.AcceptButton = this.acceptButton;
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
          this.CancelButton = this.cancelButton;
          this.ClientSize = new System.Drawing.Size(298, 296);
          this.ControlBox = false;
          this.Controls.Add(this.listLabel);
          this.Controls.Add(this.cancelButton);
          this.Controls.Add(this.flowLayoutPanel1);
          this.Controls.Add(this.acceptButton);
          this.Controls.Add(this.resultListBox);
          this.MinimumSize = new System.Drawing.Size(250, 190);
          this.Name = "ListSelectForm";
          //this.SizeGripStyle = Wisej.Web.SizeGripStyle.Hide;
          this.StartPosition = Wisej.Web.FormStartPosition.CenterParent;
          this.Text = "Select from list";
          ((System.ComponentModel.ISupportInitialize)(this.listSource)).EndInit();
          this.flowLayoutPanel1.ResumeLayout(false);
          this.flowLayoutPanel1.PerformLayout();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private Wisej.Web.Label sokLabel;
        private Wisej.Web.TextBox sokTextBox;
        private Wisej.Web.Button acceptButton;
        private Wisej.Web.Button cancelButton;
        private Wisej.Web.BindingSource listSource;
        private Wisej.Web.ListBox resultListBox;
        private Wisej.Web.FlowLayoutPanel flowLayoutPanel1;
        private Wisej.Web.Label listLabel;
    }
}