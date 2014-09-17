using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Csla;

// Copyright (c) 2005 Claudio Grazioli, http://www.grazioli.ch
//
// This code is free software; you can redistribute it and/or modify it.
// However, this header must remain intact and unchanged.  Additional
// information may be appended after this header.  Publications based on
// this code must also include an appropriate reference.
// 
// This code is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE.
//
// Thanx for contributions to this ReadOnlyComboBox to Alexandre Cunha, Brasil.

// Additional changes by Tiago to improve integration witn Csla 

namespace CslaContrib.Windows
{
  /// <summary>
  /// Represents a Windows combo box control. It enhances the .NET standard combo box control
  /// with a ReadOnly mode and works fine with Csla NameValueList.
  /// </summary>
  [Description("Csla ready ComboBox that supports Read Only property.")]
  [DefaultEvent("GetNameValueList")]
  [ToolboxItem(true)]
  [ToolboxBitmap(typeof (CslaComboBox), "CslaComboBox.bmp")]
  public class CslaComboBox : ComboBox, ISupportInitialize
  {
    #region Member variables

    // The embedded TextBox control that is used for the ReadOnly mode
    private readonly TextBox _textbox;

    // true, when the ComboBox is set to ReadOnly
    private bool _isReadOnly;
    private NameValueListBase<int, string> _nameValueList;

    // true, when the control is visible
    private bool _visible = true;

    #endregion

    #region Events

    /// <summary>
    /// Event that is raised when a data binding error occurs.
    /// </summary>
    [Browsable(true)]
    [Category("Data")]
    [Description("Event that is raised when a data binding error occurs.")]
    public event NameValueListEventHandler GetNameValueList = null;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the control is read-only.
    /// </summary>
    /// <value>
    /// 	<b>true</b> if the combo box is read-only; otherwise, <b>false</b>. The default is <b>false</b>.
    /// </value>
    /// <remarks>
    /// When this property is set to <b>true</b>, the contents of the control cannot be changed
    /// by the user at runtime. With this property set to <b>true</b>, you can still set the value
    /// in code. You can use this feature instead of disabling the control with the Enabled
    /// property to allow the contents to be copied.
    /// This property cannot be set at design time as it's intended to be handled only by ReadWriteAuthorization control.
    /// </remarks>
    [Browsable(false)]
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Controls whether the value in the combobox control can be changed or not.")]
    public bool ReadOnly
    {
      get { return _isReadOnly; }
      set
      {
        if (value != _isReadOnly)
        {
          _isReadOnly = value;
          UpdateControlDisplay();
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating wether the control is displayed.
    /// </summary>
    /// <value><b>true</b> if the control is displayed; otherwise, <b>false</b>. 
    /// The default is <b>true</b>.</value>
    public new bool Visible
    {
      get { return _visible; }
      set
      {
        _visible = value;
        UpdateControlDisplay();
      }
    }

    /// <summary>
    /// Conceals the control from the user.
    /// </summary>
    /// <remarks>
    /// Hiding the control is equvalent to setting the <see cref="Visible"/> property to <b>false</b>. 
    /// After the <b>Hide</b> method is called, the <b>Visible</b> property returns a value of 
    /// <b>false</b> until the <see cref="Show"/> method is called.
    /// </remarks>
    public new void Hide()
    {
      Visible = false;
    }

    /// <summary>
    /// Displays the control to the user.
    /// </summary>
    /// <remarks>
    /// Showing the control is equivalent to setting the <see cref="Visible"/> property to <b>true</b>.
    /// After the <b>Show</b> method is called, the <b>Visible</b> property returns a value of 
    /// <b>true</b> until the <see cref="Hide"/> method is called.
    /// </remarks>
    public new void Show()
    {
      Visible = true;
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Default Constructor
    /// </summary>
    public CslaComboBox()
    {
      _textbox = new TextBox();
      _textbox.Enter += OnTextBoxEnter;
    }

    private void OnTextBoxEnter(object sender, EventArgs e)
    {
      OnEnter(e);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      _textbox.Dispose();
    }

    #endregion

    #region ISupportInitialize Members

    /// <summary>Called by InitializeComponent()</summary>
    public void BeginInit()
    {
    }

    /// <summary>Called by InitializeComponent()</summary>
    public void EndInit()
    {
      if (GetNameValueList != null)
      {
        var argNameValueList = new NameValueListEventArgs(_nameValueList);
        GetNameValueList(this, argNameValueList);
        _nameValueList = argNameValueList.NameValueList;
        DataSource = _nameValueList;
      }
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Initializes the embedded TextBox with the default values from the ComboBox
    /// </summary>
    private void AddTextbox()
    {
      _textbox.ReadOnly = true;
      _textbox.Location = Location;
      _textbox.Size = Size;
      _textbox.Dock = Dock;
      _textbox.Anchor = Anchor;
      _textbox.Enabled = Enabled;
      _textbox.Visible = Visible;
      _textbox.RightToLeft = RightToLeft;
      _textbox.Font = Font;
      _textbox.Text = Text;
      _textbox.TabStop = TabStop;
      _textbox.TabIndex = TabIndex;
    }

    /// <summary>
    /// Shows either the ComboBox or the TextBox or nothing, depending on the state
    /// of the ReadOnly, Enable and Visible flags.
    /// </summary>
    private void UpdateControlDisplay()
    {
      if (DesignMode)
      {
        _textbox.Visible = false;
        base.Visible = true;
        return;
      }

      if (_isReadOnly)
      {
        _textbox.Visible = _visible && Enabled;
        base.Visible = _visible && !Enabled;
        _textbox.Text = Text;
      }
      else
      {
        _textbox.Visible = false;
        base.Visible = _visible;
      }
    }

    #endregion

    #region Event handlers

    /// <summary>
    /// This member overrides <see cref="System.Windows.Forms.Control.OnParentChanged"/>
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnParentChanged(EventArgs e)
    {
      base.OnParentChanged(e);

      if (Parent != null)
      {
        AddTextbox();
        _textbox.Parent = Parent;
      }
    }

    /// <summary>
    /// This member overrides <see cref="System.Windows.Forms.ComboBox.OnSelectedIndexChanged"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnSelectedIndexChanged(EventArgs e)
    {
      base.OnSelectedIndexChanged(e);
      if (SelectedItem == null)
        _textbox.Clear();
      else
        _textbox.Text = SelectedItem.ToString();
    }

    /// <summary>
    /// This member overrides <see cref="System.Windows.Forms.ComboBox.OnDropDownStyleChanged"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnDropDownStyleChanged(EventArgs e)
    {
      base.OnDropDownStyleChanged(e);
      _textbox.Text = Text;
    }

    /// <summary>
    /// This member overrides <see cref="System.Windows.Forms.ComboBox.OnFontChanged"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnFontChanged(EventArgs e)
    {
      base.OnFontChanged(e);
      _textbox.Font = Font;
    }

    /// <summary>
    /// This member overrides <see cref="System.Windows.Forms.ComboBox.OnResize"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      _textbox.Size = Size;
    }

    /// <summary>
    /// This member overrides <see cref="Control.OnDockChanged"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnDockChanged(EventArgs e)
    {
      base.OnDockChanged(e);
      _textbox.Dock = Dock;
    }

    /// <summary>
    /// This member overrides <see cref="System.Windows.Forms.Control.OnEnabledChanged"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnEnabledChanged(EventArgs e)
    {
      base.OnEnabledChanged(e);
      UpdateControlDisplay();
    }

    /// <summary>
    /// This member overrides <see cref="Control.OnRightToLeftChanged"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnRightToLeftChanged(EventArgs e)
    {
      base.OnRightToLeftChanged(e);
      _textbox.RightToLeft = RightToLeft;
    }

    /// <summary>
    /// This member overrides <see cref="Control.OnTextChanged"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnTextChanged(EventArgs e)
    {
      base.OnTextChanged(e);
      _textbox.Text = Text;
    }

    /// <summary>
    /// This member overrides <see cref="Control.OnLocationChanged"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnLocationChanged(EventArgs e)
    {
      base.OnLocationChanged(e);
      _textbox.Location = Location;
    }

    /// <summary>
    /// This member overrides <see cref="Control.OnTabIndexChanged"/>.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnTabIndexChanged(EventArgs e)
    {
      base.OnTabIndexChanged(e);
      _textbox.TabIndex = TabIndex;
    }

    #endregion
  }

  /// <summary>
  /// Represents the method that handles an event caused by data binding errors.
  /// </summary>
  /// <param name="sender">The object that triggered the event.</param>
  /// <param name="e">An <see cref="NameValueListEventArgs"/> that contains the event data.</param>
  public delegate void NameValueListEventHandler(object sender, NameValueListEventArgs e);

  /// <summary>
  /// NameValueListEventArgs defines the event arguments to report a data binding error.
  /// </summary>
  public class NameValueListEventArgs : EventArgs
  {
    #region Properties

    /// <summary>
    /// Gets the bound NameValueList that caused the exception.
    /// </summary>
    /// <value>The NameValueList.</value>
    public NameValueListBase<int, string> NameValueList { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor creates a new NameValueListEventArgs object instance using the information specified.
    /// </summary>
    /// <param name="nameValueList">The binding that caused th exception.</param>
    public NameValueListEventArgs(NameValueListBase<int, string> nameValueList)
    {
      NameValueList = nameValueList;
    }

    #endregion
  }
}