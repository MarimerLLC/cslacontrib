using System;
using System.ComponentModel;
using System.Drawing;
using Csla;
using Wisej.Web;

namespace CslaContrib.WisejWeb
{
  /// <summary>
  /// Custom DateTime picker that understands additional commands.
  /// Ex: td, +, -
  /// </summary>
  [DesignerCategory("")]
  [ToolboxItem(true), ToolboxBitmap(typeof (SmartDateTimePicker), "SmartDateTimePicker.bmp")]
  public class SmartDateTimePicker : DateTimePicker
  {
    #region Private variables

    private const int checkWidth = 0;
    private const int buttonWidth = 16;

    private TextBox _myDateTextBox;
    private SmartDate _mySmartDate;
    private string _customFormat;

    #endregion

    #region Constructor and destructor

    /// <summary>
    /// Initializes a new instance of the <see cref="SmartDateTimePicker"/> class.
    /// </summary>
    public SmartDateTimePicker()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      base.TabStop = false;

      //Initialise base.Format to Custom, we only need Custom Format
      SmartDateTimePicker_Resize(this, null);
    }

    #endregion Constructor and destructor

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      _myDateTextBox = new Wisej.Web.TextBox();
      _mySmartDate = new Csla.SmartDate(true);
      SuspendLayout();
      // 
      // txtDateTime
      // 
      _myDateTextBox.Location = new System.Drawing.Point(20, 49);
      _myDateTextBox.MaxLength = 50;
      _myDateTextBox.Name = "myDateTextBox";
      _myDateTextBox.TabIndex = 0;
      _myDateTextBox.Text = "";
      _myDateTextBox.Leave += MyTextBox_Leave;
      _myDateTextBox.Enter += MyTextBox_Enter;
      // 
      // DateTimePicker
      // 
      Controls.Add(_myDateTextBox);

      // setup events 
      DropDown += SmartDateTimePicker_DropDown;
      CloseUp += SmartDateTimePicker_CloseUp;
      FontChanged += SmartDateTimePicker_FontChanged;
      ForeColorChanged += SmartDateTimePicker_ForeColorChanged;
      BackColorChanged += SmartDateTimePicker_BackColorChanged;
      Resize += SmartDateTimePicker_Resize;
      Enter += SmartDateTimePicker_Enter;
      FormatChanged += SmartDateTimePicker_FormatChanged;

      // set format 
      base.Format = DateTimePickerFormat.Short;
      _customFormat = "d";
      _mySmartDate.FormatString = _customFormat;

      ResumeLayout(false);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// A string that represents the text associated with this control.
    /// </returns>
    /// <PermissionSet>
    /// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
    /// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// </PermissionSet>
    /// 
    [Bindable(true)]
    public new string Text
    {
      get { return _mySmartDate.Text; }
      set
      {
        // set the Text property of _mySmartDate
        SetMyValue(value, true);
      }
    }

    /// <summary>
    /// Gets or sets the date/time value assigned to the control.
    /// </summary>
    /// <value></value>
    /// <returns>The <see cref="T:System.DateTime"/> value assign to the control.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The set value is less than <see cref="P:Wisej.Web.DateTimePicker.MinDate"/> or more than <see cref="P:Wisej.Web.DateTimePicker.MaxDate"/>.</exception>
    /// <PermissionSet>
    /// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
    /// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// </PermissionSet>
    [Bindable(true)]
    public new DateTime Value
    {
      get { return _mySmartDate.Date; }
      set
      {
        // set the Date property of _mySmartDate
        SetMyValue(value, true);
      }
    }

    /// <summary>
    /// Gets or sets the format of the date and time displayed in the control.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// One of the <see cref="T:Wisej.Web.DateTimePickerFormat"/> values. The default is <see cref="F:Wisej.Web.DateTimePickerFormat.Long"/>.
    /// </returns>
    /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
    /// The value assigned is not one of the <see cref="T:Wisej.Web.DateTimePickerFormat"/> values.
    /// </exception>
    /// <PermissionSet>
    /// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
    /// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// </PermissionSet>
    [Description("Constant - overridden and can only be DateTimePickerFormat.Custom."), DefaultValue(DateTimePickerFormat.Custom)]
    public new DateTimePickerFormat Format
    {
      get { return base.Format; }
      set { base.Format = DateTimePickerFormat.Custom; }
    }

    /// <summary>
    /// Gets or sets the custom date/time format string.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// A string that represents the custom date/time format. The default is null.
    /// </returns>
    /// <PermissionSet>
    /// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
    /// 	<IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
    /// </PermissionSet>
    [Description("Sets the display format for use in DateTimePicker"), DefaultValue("d"), Browsable(true)]
    public new string CustomFormat
    {
      get { return _customFormat; }
      set
      {
        _customFormat = value;
        _mySmartDate.FormatString = value;
        UpdateMyTextBox();
      }
    }

    [Category("Csla")]
    [Description("Set if Empty is min (true) or Empty is Max (false)"), DefaultValue(true), Browsable(true)]
    public bool EmptyIsMin
    {
      get { return _mySmartDate.EmptyIsMin; }
      set
      {
        if (_mySmartDate.EmptyIsMin != value)
        {
          _mySmartDate = new SmartDate(value) {Date = _mySmartDate.Date};
          _mySmartDate.FormatString = _customFormat;
        }
      }
    }

    #endregion

    #region DateTimePicker events

    /// <summary>
    /// Handles the Resize event of the DateTimePicker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void SmartDateTimePicker_Resize(object sender, EventArgs e)
    {
      _myDateTextBox.Location = new System.Drawing.Point(-2 + checkWidth, -2);
      _myDateTextBox.Size = new System.Drawing.Size(Width - buttonWidth - checkWidth, Height);
    }

    //private void MyDateTextBox_Validating(object sender, CancelEventArgs args)
    //{
    //    SetValue(_myDateTextBox.Text);
    //}

    private void SmartDateTimePicker_FontChanged(Object sender, EventArgs e)
    {
      _myDateTextBox.Font = Font;
    }

    private void SmartDateTimePicker_BackColorChanged(Object sender, EventArgs e)
    {
      _myDateTextBox.BackColor = BackColor;
    }

    private void SmartDateTimePicker_ForeColorChanged(Object sender, EventArgs e)
    {
      _myDateTextBox.ForeColor = BackColor;
    }

    private void SmartDateTimePicker_FormatChanged(Object sender, EventArgs e)
    {
      _mySmartDate.FormatString = base.CustomFormat;
      UpdateMyTextBox();
    }

    private void DateTimePicker_TextChanged(object sender, EventArgs e)
    {
      SetMyValue(base.Text, false);
    }

    private void MyTextBox_Enter(Object sender, EventArgs e)
    {
      if (_myDateTextBox.Text.Length <= 0) return;

      _myDateTextBox.SelectionStart = 0;
      _myDateTextBox.SelectionLength = _myDateTextBox.Text.Length;
    }

    private void MyTextBox_Leave(Object sender, EventArgs e)
    {
      SetMyValue(_myDateTextBox.Text, true);
    }

    private void SmartDateTimePicker_Enter(Object sender, EventArgs e)
    {
      _myDateTextBox.Focus();
    }

    private void SmartDateTimePicker_DropDown(Object sender, EventArgs e)
    {
      // hookup event for callback on selected value
      base.TextChanged += DateTimePicker_TextChanged;
    }

    private void SmartDateTimePicker_CloseUp(object sender, EventArgs e)
    {
      // unhook event for callback on selected value and focus myTextBox
      base.TextChanged -= DateTimePicker_TextChanged;
      _myDateTextBox.Focus();
    }

    private void SetMyValue(string text, bool updateBaseValue)
    {
      var tempdate = new SmartDate(true);
      if (SmartDate.TryParse(text, SmartDate.EmptyValue.MinDate, ref tempdate))
      {
        if (tempdate.CompareTo(_mySmartDate) != 0)
        {
          _mySmartDate.Text = tempdate.ToString(_customFormat);
        }
      }
      UpdateMyTextBox();
      if (updateBaseValue)
      {
        SetBaseValue();
      }
    }

    private void SetMyValue(DateTime value, bool updateBaseValue)
    {
      if (_mySmartDate.Date != value)
      {
        _mySmartDate.Text = value.ToString(_customFormat);
      }
      UpdateMyTextBox();
      if (updateBaseValue)
      {
        SetBaseValue();
      }
    }

    private void SetBaseValue()
    {
      if (_mySmartDate.IsEmpty || _mySmartDate.Date < base.MinDate || _mySmartDate.Date > base.MaxDate)
      {
        base.Text = string.Empty;
      }
      else
      {
        base.Value = _mySmartDate.Date;
      }
    }

    /// <summary>
    /// Formats the text box.
    /// </summary>
    private void UpdateMyTextBox()
    {
      if (_mySmartDate.Text == _myDateTextBox.Text) return;

      _myDateTextBox.Text = _mySmartDate.Text;
    }

    #endregion
  }
}