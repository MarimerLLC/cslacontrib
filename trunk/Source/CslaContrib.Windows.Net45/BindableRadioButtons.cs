using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace CslaContrib.Windows
{
  /// <summary>
  /// A control that encapsulates a list of radio buttons bindable in much the
  /// same way as a ComboBox
  /// </summary>
  [LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")]
  [DesignerCategory("")]
  [ToolboxItem(true), ToolboxBitmap(typeof (BindableRadioButtons), "BindableRadioButtons.bmp")]
  public partial class BindableRadioButtons : UserControl
  {
    private readonly Hashtable _controlTable = new Hashtable();
    private ArrayList _dataTable = new ArrayList();

    /// <summary>
    /// Creates an instance of the BindableRadioButtons control
    /// </summary>
    public BindableRadioButtons()
    {
      InitializeComponent();
      _buttonCount = 3;
      DoLayout();
    }

    #region Properties

    private int _buttonCount = 3;
    private Point _buttonPadding;
    private object _dataSource;
    private FlowDirection _flowDirection = FlowDirection.TopDown;
    private object _selectedValue;
    private bool _wrapContents = true;

    /// <summary>
    /// Indicates which radiobutton is checked
    /// </summary>
    [Category("Data")]
    [Description("The key of the data item that is selected at any time.")]
    [Bindable(true)]
    [Browsable(false)]
    public object SelectedValue
    {
      get { return _selectedValue; }
      set { if (Select(value)) _selectedValue = value; }
    }

    /// <summary>
    /// Indicates the list that this control will use to get its items
    /// </summary>
    [Category("Data")]
    [AttributeProvider(typeof (IListSource))]
    //[TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [Description("Indicates the list that this control will use to get its items")]
    [RefreshProperties(RefreshProperties.Repaint)]
    public object DataSource
    {
      get { return _dataSource; }
      set
      {
        _dataSource = value;
        PrepareDataSource();
        DoLayout();
      }
    }

    /// <summary>
    /// Indicates the property to display for the items in this control.
    /// </summary>
    //, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
    [Category("Data")]
    [Editor(
      "System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
      , typeof (UITypeEditor)), DefaultValue("")]
    public string DisplayMember { get; set; }

    /// <summary>
    /// Indicates the property to use as the actual value for the items in the control.
    /// </summary>
    [Editor(
      "System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
      , typeof (UITypeEditor)), DefaultValue("")]
    [Category("Data")]
    public string ValueMember { get; set; }

    /// <summary>
    /// Number of controls do display in the designer. Has no
    /// run-time functionality.
    /// </summary>
    [Category("Design"), Bindable(false),
     Description("Number of controls do display in the designer. Has no run-time functionality.")]
    public int ButtonCount
    {
      get { return _buttonCount; }
      set
      {
        _buttonCount = value;
        DoLayout();
      }
    }

    /// <summary>
    /// Generates lookup-keys automatically if true
    /// </summary>
    [DefaultValue(false)]
    public bool AutogenerateLookupKeys { get; set; }

    /// <summary>
    /// List a number of characters here that will be excluded from 
    /// automatic lookup character assignment.
    /// </summary>
    [DefaultValue("")]
    public string UsedLookupCharacters { get; set; }

    /// <summary>
    /// The number of pixels to pad between each radio button.
    /// </summary>
    [Description("The number of pixels to pad between each radio button.")]
    [DefaultValue("0;0")]
    [Category("Layout")]
    public Point ButtonPadding
    {
      get { return _buttonPadding; }
      set
      {
        _buttonPadding = value;
        DoLayout();
      }
    }

    /// <summary>
    /// In what direction the buttons are laid out
    /// </summary>
    [DefaultValue(FlowDirection.TopDown)]
    [Category("Layout")]
    public FlowDirection FlowDirection
    {
      get { return _flowDirection; }
      set
      {
        _flowDirection = value;
        DoLayout();
      }
    }

    /// <summary>
    /// Wrap the controls
    /// </summary>
    [DefaultValue(true)]
    [Category("Layout")]
    public bool WrapContents
    {
      get { return _wrapContents; }
      set
      {
        _wrapContents = value;
        DoLayout();
      }
    }

    #endregion

    #region Business Methods

    /// <summary>
    /// Returns a <see cref="T:System.String"/> containing the name of the <see cref="T:System.ComponentModel.Component"/>, if any. This method should not be overridden.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"/> containing the name of the <see cref="T:System.ComponentModel.Component"/>, if any, or null if the <see cref="T:System.ComponentModel.Component"/> is unnamed.
    /// </returns>
    public override string ToString()
    {
      return Name + " (" + _buttonCount + " buttons)";
    }

    private void DoLayout()
    {
      // Init default values            
      _dataTable = new ArrayList();
      for (var i = 1; i <= _buttonCount; i++)
      {
        _dataTable.Add(new DictionaryEntry(i, "Option " + i));
      }

      // Databind
      if (_dataSource != null)
      {
        var bs = (BindingSource) _dataSource;
        //object boundEntity = bs.DataSource;

        // If this is design-time & count=0, render default values instead
        var skipClear = false;
        if (bs.Count == 0)
        {
          if (GetService(typeof (IDesignerHost)) != null)
          {
            skipClear = true;
          }
        }

        // Fetch key-value-data
        if (!skipClear) _dataTable.Clear();
        foreach (var o in bs)
        {
          // use reflection to get properties
          var t = o.GetType();
          var text = t.GetProperty(DisplayMember);
          var value = t.GetProperty(ValueMember);
          if (text != null && value != null)
          {
            var oValue = value.GetValue(o, null);
            var oText = text.GetValue(o, null);
            _dataTable.Add(new DictionaryEntry(oValue, oText));
          }
        }
      }

      // Create controls
      Control container = flowLayoutPanel1;
      flowLayoutPanel1.FlowDirection = _flowDirection;
      flowLayoutPanel1.WrapContents = _wrapContents;

      container.Controls.Clear();
      _controlTable.Clear();
      foreach (DictionaryEntry entry in _dataTable)
      {
        var rb = new RadioButton();
        rb.Text = entry.Value.ToString();
        rb.Tag = entry.Key;
        rb.Margin = new Padding(3, 0, _buttonPadding.X, _buttonPadding.Y);
        rb.AutoSize = true;
        //Size s = rb.Size;

        rb.CheckedChanged += radioButton_CheckedChanged;
        if (_selectedValue != null && _selectedValue.Equals(entry.Key)) rb.Checked = true;
        container.Controls.Add(rb);

        // Hash the control
        _controlTable.Add(entry.Key, rb);
      }
    }

    /// <summary>
    /// Automatically generates lookup keys
    /// </summary>
    private void GenerateLookupKeys()
    {
      // TODO: Implement GenerateLookupKeys()
    }

    /// <summary>
    /// Add listener to change in datasource or datasource's child datasource
    /// </summary>
    private void PrepareDataSource()
    {
      if (_dataSource is BindingSource)
      {
        var bs = (BindingSource) _dataSource;
        bs.DataSourceChanged += BindingSource_DataSourceChanged;
        if (bs.DataSource is BindingSource)
        {
          // If datasource's datasource change
          ((BindingSource) bs.DataSource).DataSourceChanged += BindingSource_DataSourceChanged;
        }
      }
    }

    /// <summary>
    /// Selectes the radio button with the specified key.
    /// If the key cannot be found, false is returned
    /// </summary>
    /// <param name="key">The key for the radio button</param>
    /// <returns>True if selection is performed successfully</returns>
    private bool Select(object key)
    {
      if (key == null || _dataTable == null || _dataTable.Count == 0) return false;
      var value = _controlTable[key];
      if (value != null)
      {
        var rb = (RadioButton) value;
        rb.Checked = true;
        return true;
      }
      return false;
    }

    /// <summary>
    /// Writes to the databound objects
    /// </summary>
    private void NotifyDatabindings()
    {
      foreach (Binding binding in DataBindings)
      {
        binding.WriteValue();
      }
    }

    /// <summary>
    /// Returns a Hashtable of the buttons in this group with the key 
    /// set to whatever the databinding specifies in Value
    /// </summary>
    /// <returns>A hashtable of the buttons</returns>
    public Hashtable GetButtonTable()
    {
      return (Hashtable) _controlTable.Clone(); // Shallow copy
    }

    /// <summary>
    /// Returns a type-safe list of the buttons in this group
    /// </summary>
    /// <returns>a type-safe list of the buttons in this group</returns>
    public List<RadioButton> GetButtonList()
    {
      var list = new List<RadioButton>();

      foreach (RadioButton button in _controlTable.Values)
      {
        list.Add(button);
      }
      return list;
    }

    #endregion

    #region Event listeners

    private void BindingSource_DataSourceChanged(object sender, EventArgs e)
    {
      DoLayout();
    }

    private void radioButton_CheckedChanged(object sender, EventArgs e)
    {
      var rSender = (RadioButton) sender;
      if (rSender.Checked)
      {
        _selectedValue = ((RadioButton) sender).Tag;
        NotifyDatabindings();
        DoSelectedIndexChanged();
      }
    }

    private void DoSelectedIndexChanged()
    {
      if (SelectedIndexChanged != null)
        SelectedIndexChanged.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Determines whether the specified key is a regular input key or a special key that requires preprocessing.
    /// </summary>
    /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"/> values.</param>
    /// <returns>
    /// true if the specified key is a regular input key; otherwise, false.
    /// </returns>
    protected override bool IsInputKey(Keys keyData)
    {
      //switch (keyData)
      //{
      //    case Keys.Down: 
      //    case Keys.Left:
      //    case Keys.Up:  
      //    case Keys.Right: 
      //    case Keys.Enter: 
      //    case Keys.Tab:
      //    case Keys.ShiftKey:
      //        return true; 
      //}
      return base.IsInputKey(keyData);
    }

    //protected override void OnKeyDown(KeyEventArgs e)
    //{
    //    KeyPressed(e.KeyCode);
    //}

    //private void KeyPressed(Keys key)
    //{
    //    if (Parent == null) return;
    //    var bindingSource = DataSource as BindingSource;
    //    if (bindingSource == null) return;

    //    var index = ((BindingSource) DataSource).Position;

    //    switch (key)
    //    {
    //        case Keys.Up:
    //        case Keys.Left:
    //            this.flowLayoutPanel1.SelectNextControl(flowLayoutPanel1, false, false, true, false);
    //            break;
    //        case Keys.Down:
    //        case Keys.Right:
    //            index++;
    //            break;
    //    }

    //    //Debug.Print("FlowBreak: {0}", this.flowLayoutPanel1.);

    //    if (index < 0) index = 0;
    //    if (index >= bindingSource.Count) index = bindingSource.Count - 1;

    //    if (index != bindingSource.Position)
    //    {
    //        bindingSource.Position = index; 
    //    }
    //}

    #endregion

    /// <summary>
    /// Occurs when the selected radio button changes
    /// </summary>
    public event EventHandler SelectedIndexChanged;
  }
}