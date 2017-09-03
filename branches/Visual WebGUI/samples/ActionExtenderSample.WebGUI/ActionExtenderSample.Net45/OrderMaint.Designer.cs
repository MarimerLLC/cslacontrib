//-----------------------------------------------------------------------
// <copyright file="OrderMaint.Designer.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary></summary>
//-----------------------------------------------------------------------

namespace ActionExtenderSample
{
  partial class OrderMaint
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
      Gizmox.WebGUI.Forms.Label cardHolderLabel;
      Gizmox.WebGUI.Forms.Label cardTypeLabel;
      Gizmox.WebGUI.Forms.Label creditCardLabel;
      Gizmox.WebGUI.Forms.Label expDateLabel;
      Gizmox.WebGUI.Forms.Label orderIDLabel;
      Gizmox.WebGUI.Forms.Label orderNumberLabel;
      Gizmox.WebGUI.Forms.Label userNameLabel;
      Gizmox.WebGUI.Forms.Label textLabel;
      Gizmox.WebGUI.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new Gizmox.WebGUI.Forms.DataGridViewCellStyle();
      Gizmox.WebGUI.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new Gizmox.WebGUI.Forms.DataGridViewCellStyle();
      this.cardHolderTextBox = new Gizmox.WebGUI.Forms.TextBox();
      this.orderBindingSource = new Gizmox.WebGUI.Forms.BindingSource(this.components);
      this.cardTypeTextBox = new Gizmox.WebGUI.Forms.TextBox();
      this.creditCardTextBox = new Gizmox.WebGUI.Forms.TextBox();
      this.expDateTextBox = new Gizmox.WebGUI.Forms.TextBox();
      this.orderIDLabel1 = new Gizmox.WebGUI.Forms.Label();
      this.orderNumberTextBox = new Gizmox.WebGUI.Forms.TextBox();
      this.userNameTextBox = new Gizmox.WebGUI.Forms.TextBox();
      this.orderDateTextBox = new Gizmox.WebGUI.Forms.TextBox();
      this.orderDetailListBindingSource = new Gizmox.WebGUI.Forms.BindingSource(this.components);
      this.orderDetailListDataGridView = new Gizmox.WebGUI.Forms.DataGridView();
      this.btnForceSave = new Gizmox.WebGUI.Forms.Button();
      this.btnValidate = new Gizmox.WebGUI.Forms.Button();
      this.btnClose = new Gizmox.WebGUI.Forms.Button();
      this.btnCancel = new Gizmox.WebGUI.Forms.Button();
      this.btnSaveClose = new Gizmox.WebGUI.Forms.Button();
      this.btnSaveNew = new Gizmox.WebGUI.Forms.Button();
      this.btnSave = new Gizmox.WebGUI.Forms.Button();
      this.cslaActionExtender1 = new CslaContrib.WebGUI.CslaActionExtender(this.components);
      this.errorProvider1 = new CslaContrib.WebGUI.ErrorProvider(this.components);
      this.bindingSourceRefresh1 = new CslaContrib.WebGUI.BindingSourceRefresh(this.components);
      this.dataGridViewTextBoxColumn1 = new Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn3 = new Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn4 = new Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn5 = new Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn();
      cardHolderLabel = new Gizmox.WebGUI.Forms.Label();
      cardTypeLabel = new Gizmox.WebGUI.Forms.Label();
      creditCardLabel = new Gizmox.WebGUI.Forms.Label();
      expDateLabel = new Gizmox.WebGUI.Forms.Label();
      orderIDLabel = new Gizmox.WebGUI.Forms.Label();
      orderNumberLabel = new Gizmox.WebGUI.Forms.Label();
      userNameLabel = new Gizmox.WebGUI.Forms.Label();
      textLabel = new Gizmox.WebGUI.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.orderDetailListBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.orderDetailListDataGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceRefresh1)).BeginInit();
      this.SuspendLayout();
      // 
      // cardHolderLabel
      // 
      cardHolderLabel.AutoSize = true;
      cardHolderLabel.Location = new System.Drawing.Point(36, 59);
      cardHolderLabel.Name = "cardHolderLabel";
      cardHolderLabel.Size = new System.Drawing.Size(66, 13);
      cardHolderLabel.TabIndex = 1;
      cardHolderLabel.Text = "Card Holder:";
      // 
      // cardTypeLabel
      // 
      cardTypeLabel.AutoSize = true;
      cardTypeLabel.Location = new System.Drawing.Point(36, 85);
      cardTypeLabel.Name = "cardTypeLabel";
      cardTypeLabel.Size = new System.Drawing.Size(59, 13);
      cardTypeLabel.TabIndex = 3;
      cardTypeLabel.Text = "Card Type:";
      // 
      // creditCardLabel
      // 
      creditCardLabel.AutoSize = true;
      creditCardLabel.Location = new System.Drawing.Point(36, 111);
      creditCardLabel.Name = "creditCardLabel";
      creditCardLabel.Size = new System.Drawing.Size(62, 13);
      creditCardLabel.TabIndex = 5;
      creditCardLabel.Text = "Credit Card:";
      // 
      // expDateLabel
      // 
      expDateLabel.AutoSize = true;
      expDateLabel.Location = new System.Drawing.Point(36, 137);
      expDateLabel.Name = "expDateLabel";
      expDateLabel.Size = new System.Drawing.Size(54, 13);
      expDateLabel.TabIndex = 7;
      expDateLabel.Text = "Exp Date:";
      // 
      // orderIDLabel
      // 
      orderIDLabel.AutoSize = true;
      orderIDLabel.Location = new System.Drawing.Point(36, 163);
      orderIDLabel.Name = "orderIDLabel";
      orderIDLabel.Size = new System.Drawing.Size(50, 13);
      orderIDLabel.TabIndex = 9;
      orderIDLabel.Text = "Order ID:";
      // 
      // orderNumberLabel
      // 
      orderNumberLabel.AutoSize = true;
      orderNumberLabel.Location = new System.Drawing.Point(36, 189);
      orderNumberLabel.Name = "orderNumberLabel";
      orderNumberLabel.Size = new System.Drawing.Size(76, 13);
      orderNumberLabel.TabIndex = 11;
      orderNumberLabel.Text = "Order Number:";
      // 
      // userNameLabel
      // 
      userNameLabel.AutoSize = true;
      userNameLabel.Location = new System.Drawing.Point(36, 215);
      userNameLabel.Name = "userNameLabel";
      userNameLabel.Size = new System.Drawing.Size(63, 13);
      userNameLabel.TabIndex = 13;
      userNameLabel.Text = "User Name:";
      // 
      // textLabel
      // 
      textLabel.AutoSize = true;
      textLabel.Location = new System.Drawing.Point(37, 241);
      textLabel.Name = "textLabel";
      textLabel.Size = new System.Drawing.Size(62, 13);
      textLabel.TabIndex = 15;
      textLabel.Text = "Order Date:";
      // 
      // cardHolderTextBox
      // 
      this.cardHolderTextBox.DataBindings.Add(new Gizmox.WebGUI.Forms.Binding("Text", this.orderBindingSource, "CardHolder", true));
      this.cardHolderTextBox.Location = new System.Drawing.Point(118, 56);
      this.cardHolderTextBox.Name = "cardHolderTextBox";
      this.cardHolderTextBox.Size = new System.Drawing.Size(292, 20);
      this.cardHolderTextBox.TabIndex = 2;
      // 
      // orderBindingSource
      // 
      this.orderBindingSource.DataSource = typeof(ActionExtenderSample.Business.Order);
      this.bindingSourceRefresh1.SetReadValuesOnChange(this.orderBindingSource, true);
      // 
      // cardTypeTextBox
      // 
      this.cardTypeTextBox.DataBindings.Add(new Gizmox.WebGUI.Forms.Binding("Text", this.orderBindingSource, "CardType", true));
      this.cardTypeTextBox.Location = new System.Drawing.Point(118, 82);
      this.cardTypeTextBox.Name = "cardTypeTextBox";
      this.cardTypeTextBox.Size = new System.Drawing.Size(77, 20);
      this.cardTypeTextBox.TabIndex = 4;
      // 
      // creditCardTextBox
      // 
      this.creditCardTextBox.DataBindings.Add(new Gizmox.WebGUI.Forms.Binding("Text", this.orderBindingSource, "CreditCard", true));
      this.creditCardTextBox.Location = new System.Drawing.Point(118, 108);
      this.creditCardTextBox.MaxLength = 20;
      this.creditCardTextBox.Name = "creditCardTextBox";
      this.creditCardTextBox.Size = new System.Drawing.Size(140, 20);
      this.creditCardTextBox.TabIndex = 6;
      // 
      // expDateTextBox
      // 
      this.expDateTextBox.DataBindings.Add(new Gizmox.WebGUI.Forms.Binding("Text", this.orderBindingSource, "ExpDate", true));
      this.expDateTextBox.Location = new System.Drawing.Point(118, 134);
      this.expDateTextBox.Name = "expDateTextBox";
      this.expDateTextBox.Size = new System.Drawing.Size(77, 20);
      this.expDateTextBox.TabIndex = 8;
      // 
      // orderIDLabel1
      // 
      this.orderIDLabel1.DataBindings.Add(new Gizmox.WebGUI.Forms.Binding("Text", this.orderBindingSource, "OrderID", true));
      this.orderIDLabel1.Location = new System.Drawing.Point(118, 163);
      this.orderIDLabel1.Name = "orderIDLabel1";
      this.orderIDLabel1.Size = new System.Drawing.Size(289, 13);
      this.orderIDLabel1.TabIndex = 10;
      this.orderIDLabel1.Text = "label1";
      // 
      // orderNumberTextBox
      // 
      this.orderNumberTextBox.DataBindings.Add(new Gizmox.WebGUI.Forms.Binding("Text", this.orderBindingSource, "OrderNumber", true));
      this.orderNumberTextBox.Location = new System.Drawing.Point(118, 186);
      this.orderNumberTextBox.MaxLength = 20;
      this.orderNumberTextBox.Name = "orderNumberTextBox";
      this.orderNumberTextBox.Size = new System.Drawing.Size(140, 20);
      this.orderNumberTextBox.TabIndex = 12;
      // 
      // userNameTextBox
      // 
      this.userNameTextBox.DataBindings.Add(new Gizmox.WebGUI.Forms.Binding("Text", this.orderBindingSource, "UserName", true));
      this.userNameTextBox.Location = new System.Drawing.Point(118, 212);
      this.userNameTextBox.MaxLength = 50;
      this.userNameTextBox.Name = "userNameTextBox";
      this.userNameTextBox.Size = new System.Drawing.Size(292, 20);
      this.userNameTextBox.TabIndex = 14;
      // 
      // orderDateTextBox
      // 
      this.orderDateTextBox.DataBindings.Add(new Gizmox.WebGUI.Forms.Binding("Text", this.orderBindingSource, "OrderDate", true));
      this.orderDateTextBox.Location = new System.Drawing.Point(118, 238);
      this.orderDateTextBox.Name = "orderDateTextBox";
      this.orderDateTextBox.Size = new System.Drawing.Size(77, 20);
      this.orderDateTextBox.TabIndex = 16;
      this.orderDateTextBox.Validating += new System.ComponentModel.CancelEventHandler(orderDateTextBox_Validating);
      // 
      // orderDetailListBindingSource
      // 
      this.orderDetailListBindingSource.DataMember = "OrderDetailList";
      this.orderDetailListBindingSource.DataSource = this.orderBindingSource;
      this.bindingSourceRefresh1.SetReadValuesOnChange(this.orderDetailListBindingSource, true);
      // 
      // orderDetailListDataGridView
      // 
      this.orderDetailListDataGridView.AutoGenerateColumns = false;
      this.orderDetailListDataGridView.ColumnHeadersHeightSizeMode = Gizmox.WebGUI.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.orderDetailListDataGridView.Columns.AddRange(new Gizmox.WebGUI.Forms.DataGridViewColumn[] {
      this.dataGridViewTextBoxColumn1,
      this.dataGridViewTextBoxColumn3,
      this.dataGridViewTextBoxColumn4,
      this.dataGridViewTextBoxColumn5});
      this.orderDetailListDataGridView.DataSource = this.orderDetailListBindingSource;
      this.orderDetailListDataGridView.Location = new System.Drawing.Point(76, 284);
      this.orderDetailListDataGridView.Name = "orderDetailListDataGridView";
      this.orderDetailListDataGridView.Size = new System.Drawing.Size(600, 220);
      this.orderDetailListDataGridView.TabIndex = 16;
      this.orderDetailListDataGridView.DataError += new Gizmox.WebGUI.Forms.DataGridViewDataErrorEventHandler(orderDetailListDataGridView_DataError);
      // 
      // btnForceSave
      // 
      this.cslaActionExtender1.SetActionType(this.btnForceSave, CslaContrib.WebGUI.CslaFormAction.Save);
      this.btnForceSave.Location = new System.Drawing.Point(518, 238);
      this.btnForceSave.Name = "btnForceSave";
      this.btnForceSave.Size = new System.Drawing.Size(117, 23);
      this.btnForceSave.TabIndex = 22;
      this.btnForceSave.Text = "Force Save";
      this.btnForceSave.UseVisualStyleBackColor = true;
      // 
      // btnValidate
      // 
      this.cslaActionExtender1.SetActionType(this.btnValidate, CslaContrib.WebGUI.CslaFormAction.Validate);
      this.btnValidate.Location = new System.Drawing.Point(518, 208);
      this.btnValidate.Name = "btnValidate";
      this.btnValidate.Size = new System.Drawing.Size(117, 23);
      this.btnValidate.TabIndex = 22;
      this.btnValidate.Text = "Validate";
      this.btnValidate.UseVisualStyleBackColor = true;
      // 
      // btnClose
      // 
      this.cslaActionExtender1.SetActionType(this.btnClose, CslaContrib.WebGUI.CslaFormAction.Close);
      this.btnClose.Location = new System.Drawing.Point(518, 158);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(117, 23);
      this.btnClose.TabIndex = 21;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.cslaActionExtender1.SetActionType(this.btnCancel, CslaContrib.WebGUI.CslaFormAction.Cancel);
      this.cslaActionExtender1.SetCommandName(this.btnCancel, "Undo");
      this.cslaActionExtender1.SetDisableWhenUseless(this.btnCancel, true);
      this.btnCancel.Location = new System.Drawing.Point(518, 128);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(117, 23);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnSaveClose
      // 
      this.cslaActionExtender1.SetActionType(this.btnSaveClose, CslaContrib.WebGUI.CslaFormAction.Save);
      this.cslaActionExtender1.SetDisableWhenUseless(this.btnSaveClose, true);
      this.btnSaveClose.Location = new System.Drawing.Point(518, 98);
      this.btnSaveClose.Name = "btnSaveClose";
      this.cslaActionExtender1.SetPostSaveAction(this.btnSaveClose, CslaContrib.WebGUI.PostSaveActionType.AndClose);
      this.cslaActionExtender1.SetRebindAfterSave(this.btnSaveClose, false);
      this.btnSaveClose.Size = new System.Drawing.Size(117, 23);
      this.btnSaveClose.TabIndex = 19;
      this.btnSaveClose.Text = "Save/Close";
      this.btnSaveClose.UseVisualStyleBackColor = true;
      // 
      // btnSaveNew
      // 
      this.cslaActionExtender1.SetActionType(this.btnSaveNew, CslaContrib.WebGUI.CslaFormAction.Save);
      this.cslaActionExtender1.SetDisableWhenUseless(this.btnSaveNew, true);
      this.btnSaveNew.Location = new System.Drawing.Point(518, 68);
      this.btnSaveNew.Name = "btnSaveNew";
      this.cslaActionExtender1.SetPostSaveAction(this.btnSaveNew, CslaContrib.WebGUI.PostSaveActionType.AndNew);
      this.btnSaveNew.Size = new System.Drawing.Size(117, 23);
      this.btnSaveNew.TabIndex = 18;
      this.btnSaveNew.Text = "Save/New";
      this.btnSaveNew.UseVisualStyleBackColor = true;
      // 
      // btnSave
      // 
      this.cslaActionExtender1.SetActionType(this.btnSave, CslaContrib.WebGUI.CslaFormAction.Save);
      this.cslaActionExtender1.SetDisableWhenUseless(this.btnSave, true);
      this.btnSave.Location = new System.Drawing.Point(518, 38);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(117, 23);
      this.btnSave.TabIndex = 17;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      // 
      // cslaActionExtender1
      // 
      this.cslaActionExtender1.DataSource = this.orderBindingSource;
      this.cslaActionExtender1.ObjectIsValidMessage = "Order is valid";
      this.cslaActionExtender1.SetForNew += new System.EventHandler<CslaContrib.WebGUI.CslaActionEventArgs>(this.cslaActionExtender1_SetForNew);
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      this.errorProvider1.DataSource = this.orderBindingSource;
      // 
      // bindingSourceRefresh1
      // 
      this.bindingSourceRefresh1.Host = this;
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.DataPropertyName = "OrderDetailID";
      this.dataGridViewTextBoxColumn1.HeaderText = "Order Detail ID";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = 185;
      // 
      // dataGridViewTextBoxColumn3
      // 
      this.dataGridViewTextBoxColumn3.DataPropertyName = "ProductID";
      this.dataGridViewTextBoxColumn3.HeaderText = "Product ID";
      this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
      this.dataGridViewTextBoxColumn3.Width = 185;
      // 
      // dataGridViewTextBoxColumn4
      // 
      this.dataGridViewTextBoxColumn4.DataPropertyName = "PurchaseUnitPrice";
      dataGridViewCellStyle1.Alignment = Gizmox.WebGUI.Forms.DataGridViewContentAlignment.MiddleRight;
      this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridViewTextBoxColumn4.HeaderText = "Purchase Unit Price";
      this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
      this.dataGridViewTextBoxColumn4.Width = 130;
      // 
      // dataGridViewTextBoxColumn5
      // 
      this.dataGridViewTextBoxColumn5.AutoSizeMode = Gizmox.WebGUI.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.dataGridViewTextBoxColumn5.DataPropertyName = "Quantity";
      dataGridViewCellStyle2.Alignment = Gizmox.WebGUI.Forms.DataGridViewContentAlignment.MiddleRight;
      this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle2;
      this.dataGridViewTextBoxColumn5.HeaderText = "Quantity";
      this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
      // 
      // OrderMaint
      // 
      this.ClientSize = new System.Drawing.Size(791, 571);
      this.Controls.Add(this.btnForceSave);
      this.Controls.Add(this.btnValidate);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnSaveClose);
      this.Controls.Add(this.btnSaveNew);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.orderDetailListDataGridView);
      this.Controls.Add(textLabel);
      this.Controls.Add(this.orderDateTextBox);
      this.Controls.Add(cardHolderLabel);
      this.Controls.Add(this.cardHolderTextBox);
      this.Controls.Add(cardTypeLabel);
      this.Controls.Add(this.cardTypeTextBox);
      this.Controls.Add(creditCardLabel);
      this.Controls.Add(this.creditCardTextBox);
      this.Controls.Add(expDateLabel);
      this.Controls.Add(this.expDateTextBox);
      this.Controls.Add(orderIDLabel);
      this.Controls.Add(this.orderIDLabel1);
      this.Controls.Add(orderNumberLabel);
      this.Controls.Add(this.orderNumberTextBox);
      this.Controls.Add(userNameLabel);
      this.Controls.Add(this.userNameTextBox);
      this.Name = "OrderMaint";
      this.Text = "Order Maintenance - Button";
      ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.orderDetailListBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.orderDetailListDataGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSourceRefresh1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Gizmox.WebGUI.Forms.BindingSource orderBindingSource;
    private Gizmox.WebGUI.Forms.TextBox cardHolderTextBox;
    private Gizmox.WebGUI.Forms.TextBox cardTypeTextBox;
    private Gizmox.WebGUI.Forms.TextBox creditCardTextBox;
    private Gizmox.WebGUI.Forms.TextBox expDateTextBox;
    private Gizmox.WebGUI.Forms.Label orderIDLabel1;
    private Gizmox.WebGUI.Forms.TextBox orderNumberTextBox;
    private Gizmox.WebGUI.Forms.TextBox userNameTextBox;
    private Gizmox.WebGUI.Forms.TextBox orderDateTextBox;
    private Gizmox.WebGUI.Forms.BindingSource orderDetailListBindingSource;
    private Gizmox.WebGUI.Forms.DataGridView orderDetailListDataGridView;
    private CslaContrib.WebGUI.CslaActionExtender cslaActionExtender1;
    private Gizmox.WebGUI.Forms.Button btnSave;
    private Gizmox.WebGUI.Forms.Button btnSaveNew;
    private Gizmox.WebGUI.Forms.Button btnSaveClose;
    private Gizmox.WebGUI.Forms.Button btnCancel;
    private Gizmox.WebGUI.Forms.Button btnClose;
    private Gizmox.WebGUI.Forms.Button btnForceSave;
    private Gizmox.WebGUI.Forms.Button btnValidate;
    private CslaContrib.WebGUI.ErrorProvider errorProvider1;
    private CslaContrib.WebGUI.BindingSourceRefresh bindingSourceRefresh1;
    private Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
  }
}