/****************************************************************************
* Class Name   : ErrorWarnInfoProvider.cs
* Author       : Kenneth J. Koteles
* Created      : 10/04/2007 2:14 PM
* C# Version   : .NET 2.0
* Description  : This code is designed to create a new provider object to
*		         work specifically with CSLA BusinessBase objects.  In 
*		         addition to providing the red error icon for items in the 
*		         BrokenRulesCollection with Csla.Rules.RuleSeverity.Error,
*		         this object also provides a yellow warning icon for items
*		         with Csla.Rules.RuleSeverity.Warning and a blue
*		         information icon for items with 
*		         Csla.Rules.RuleSeverity.Information.  Since warnings
*		         and information type items do not need to be fixed / 
*		         corrected prior to the object being saved, the tooltip
*		         displayed when hovering over the respective icon contains
*		         all the control's associated (by severity) broken rules. 
* Revised      : 11/20/2007 8:32 AM
*     Change   : Warning and information icons were not being updated for
*		         dependant properties (controls without the focus) when 
*		         changes were being made to a related property (control with
*		         the focus).  Added a list of controls to be recursed 
*		         through each time a change was made to any control.  This
*		         obviously could result in performance issues; however,
*		         there is no consistent way to question the BusinessObject 
*		         in order to get a list of dependant properties based on a
*		         property name.  It can be exposed to the UI (using
*		         ValidationRules.GetRuleDescriptions()); however, it is up
*		         to each developer to implement their own public method on
*		         on the Business Object to do so.  To make this generic for
*		         all CSLA Business Objects, I cannot assume the developer
*		         always exposes the dependant properties (nor do I know what
*                they'll call the method); therefore, this is the best I can
*		         do right now.
* Revised      : 11/23/2007 9:02 AM
*     Change   : Added new property ProcessDependantProperties to allow for
*		         controlling when all controls are recursed through (for 
*		         dependant properties or not).  Default value is 'false'.
*		         This allows the developer to ba able to choose whether or 
*		         not to use the control in this manner (which could have 
*		         performance implications).
****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CslaContrib.Windows
{
  /// <summary>
  /// Windows Forms extender control that automatically
  /// displays error, warning, or information icons and
  /// text for the form controls based on the
  /// BrokenRulesCollection from a CSLA .NET business object.
  /// </summary>
  [DesignerCategory("")]
  [ToolboxItem(true), ToolboxBitmap(typeof(ErrorWarnInfoProvider), "ErrorWarnInfoProvider.bmp")]
  public partial class ErrorWarnInfoProvider : ErrorProvider, IExtenderProvider, ISupportInitialize
  {
    #region internal variables

    private int _blinkRateInformation;
    private int _blinkRateWarning;
    private ErrorBlinkStyle _blinkStyleInformation = ErrorBlinkStyle.BlinkIfDifferentError;
    private ErrorBlinkStyle _blinkStyleWarning = ErrorBlinkStyle.BlinkIfDifferentError;
    private List<Control> _controls = new List<Control>();
    private const int _defaultBlinkRate = 0;
    private const int _defaultBlinkRateInformation = 0;
    private const int _defaultBlinkRateWarning = 0;
    private static Icon _defaultIconInformation;
    private static Icon _defaultIconWarning;
    private Icon _iconInformation;
    private Icon _iconWarning;
    private int _offsetInformation = 32;
    private int _offsetWarning = 16;
    private bool _visibleInformation = true;
    private bool _visibleWarning = true;
    private bool _showMostSevereOnly = true;
    private Dictionary<string, string> _errorList = new Dictionary<string, string>();
    private Dictionary<string, string> _warningList = new Dictionary<string, string>();
    private Dictionary<string, string> _infoList = new Dictionary<string, string>();
    private bool _isInitializing = false;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates an instance of the ErrorWarnInfoProvider.
    /// </summary>
    /// <param name="container">The container of the control.</param>
    public ErrorWarnInfoProvider(IContainer container)
    {
      container.Add(this);

      InitializeComponent();

      base.BlinkRate = 0;

      _blinkRateInformation = _defaultBlinkRateInformation;
      _iconInformation = DefaultIconInformation;
      errorProviderInfo.BlinkRate = _blinkRateInformation;
      errorProviderInfo.Icon = _iconInformation;

      _blinkRateWarning = _defaultBlinkRateWarning;
      _iconWarning = DefaultIconWarning;
      errorProviderWarn.BlinkRate = _blinkRateWarning;
      errorProviderWarn.Icon = _iconWarning;
    }

    #endregion

    #region IExtenderProvider Members

    /// <summary>
    /// Gets a value indicating whether the extender control
    /// can extend the specified control.
    /// </summary>
    /// <param name="extendee">The control to be extended.</param>
    /// <remarks>
    /// Any control implementing either a ReadOnly property or
    /// Enabled property can be extended.
    /// </remarks>
    bool IExtenderProvider.CanExtend(object extendee)
    {
      //if (extendee is ErrorProvider)
      //{
      //    return true;
      //}
      if ((extendee is Control && !(extendee is Form)))
      {
        return !(extendee is ToolBar);
      }
      else
      {
        return false;
      }
    }

    #endregion

    #region Public properties
    /// <summary>
    /// Gets or sets the blink rate information.
    /// </summary>
    /// <value>The blink rate information.</value>
    [DefaultValue(_defaultBlinkRate), Description("The rate in milliseconds at which the error icon blinks.")]
    public new int BlinkRate
    {
      get
      {
        return base.BlinkRate;
      }
      set
      {
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException("BlinkRate", value, "Blink rate must be zero or more");
        }

        base.BlinkRate = value;
        if (value == 0)
        {
          BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }
      }
    }


    /// <summary>
    /// Gets or sets the blink rate information.
    /// </summary>
    /// <value>The blink rate information.</value>
    [DefaultValue(_defaultBlinkRateInformation), Description("The rate in milliseconds at which the information icon blinks.")]
    public int BlinkRateInformation
    {
      get
      {
        return _blinkRateInformation;
      }
      set
      {
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException("BlinkRateInformation", value, "Blink rate must be zero or more");
        }

        _blinkRateInformation = value;
        errorProviderInfo.BlinkRate = _blinkRateInformation;

        if (_blinkRateInformation == 0)
        {
          BlinkStyleInformation = ErrorBlinkStyle.NeverBlink;
        }
      }
    }

    /// <summary>
    /// Gets or sets the blink rate warning.
    /// </summary>
    /// <value>The blink rate warning.</value>
    [DefaultValue(_defaultBlinkRateWarning), Description("The rate in milliseconds at which the warning icon blinks.")]
    public int BlinkRateWarning
    {
      get
      {
        return _blinkRateWarning;
      }
      set
      {
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException("BlinkRateWarning", value, "Blink rate must be zero or more");
        }

        _blinkRateWarning = value;
        errorProviderWarn.BlinkRate = _blinkRateWarning;

        if (_blinkRateWarning == 0)
        {
          BlinkStyleWarning = ErrorBlinkStyle.NeverBlink;
        }
      }
    }

    /// <summary>
    /// Gets or sets the blink style information.
    /// </summary>
    /// <value>The blink style information.</value>
    [Description("Controls whether the information icon blinks when information is set.")]
    public ErrorBlinkStyle BlinkStyleInformation
    {
      get
      {
        if (_blinkRateInformation == 0)
        {
          return ErrorBlinkStyle.NeverBlink;
        }
        return _blinkStyleInformation;
      }
      set
      {
        if (_blinkRateInformation == 0)
        {
          value = ErrorBlinkStyle.NeverBlink;
        }

        if (_blinkStyleInformation != value)
        {
          _blinkStyleInformation = value;
          errorProviderInfo.BlinkStyle = _blinkStyleInformation;
        }
      }
    }

    /// <summary>
    /// Gets or sets the blink style warning.
    /// </summary>
    /// <value>The blink style warning.</value>
    [Description("Controls whether the warning icon blinks when a warning is set.")]
    public ErrorBlinkStyle BlinkStyleWarning
    {
      get
      {
        if (_blinkRateWarning == 0)
        {
          return ErrorBlinkStyle.NeverBlink;
        }
        return _blinkStyleWarning;
      }
      set
      {
        if (_blinkRateWarning == 0)
        {
          value = ErrorBlinkStyle.NeverBlink;
        }

        if (_blinkStyleWarning != value)
        {
          _blinkStyleWarning = value;
          errorProviderWarn.BlinkStyle = _blinkStyleWarning;
        }
      }
    }

    /// <summary>
    /// Gets or sets the data source that the <see cref="T:System.Windows.Forms.ErrorProvider"></see> monitors.
    /// </summary>
    /// <value></value>
    /// <returns>A data source based on the <see cref="T:System.Collections.IList"></see> interface to be monitored for errors. Typically, this is a <see cref="T:System.Data.DataSet"></see> to be monitored for errors.</returns>
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [DefaultValue((string)null)]
    public new object DataSource
    {
      get
      {
        return base.DataSource;
      }
      set
      {

        if (base.DataSource != value)
        {
          var bs1 = base.DataSource as BindingSource;
          if (bs1 != null)
          {
            bs1.DataSourceChanged -= DataSource_DataSourceChanged;
            bs1.CurrentItemChanged -= DataSource_CurrentItemChanged;
            //bs1.BindingComplete -= DataSource_BindingComplete;
          }
        }


        base.DataSource = value;

        var bs = value as BindingSource;
        if (bs != null)
        {
          bs.DataSourceChanged += DataSource_DataSourceChanged;
          bs.CurrentItemChanged += DataSource_CurrentItemChanged;
          //bs.BindingComplete += DataSource_BindingComplete;

          //if (bs.DataSource != null) {
          //   UpdateBindingsAndProcessAllControls();
          //}
        }
      }
    }

    private void DataSource_BindingComplete(object sender, BindingCompleteEventArgs e)
    {
      UpdateBindingsAndProcessAllControls();
    }

    private void UpdateBindingsAndProcessAllControls()
    {
      if (ContainerControl != null)
      {
        InitializeAllControls(ContainerControl.Controls);
      }
      ProcessAllControls();
    }


    /// <summary>
    /// Gets or sets the icon information.
    /// </summary>
    /// <value>The icon information.</value>
    [Description("The icon used to indicate information.")]
    public Icon IconInformation
    {
      get
      {
        return _iconInformation;
      }
      set
      {
        if (value == null)
        {
          value = DefaultIconInformation;
        }

        _iconInformation = value;
        errorProviderInfo.Icon = _iconInformation;
      }
    }

    /// <summary>
    /// Gets or sets the icon warning.
    /// </summary>
    /// <value>The icon warning.</value>
    [Description("The icon used to indicate a warning.")]
    public Icon IconWarning
    {
      get
      {
        return _iconWarning;
      }
      set
      {
        if (value == null)
        {
          value = DefaultIconWarning;
        }

        _iconWarning = value;
        errorProviderWarn.Icon = _iconWarning;
      }
    }

    /// <summary>
    /// Gets or sets the offset information.
    /// </summary>
    /// <value>The offset information.</value>
    [DefaultValue(32), Description("The number of pixels the information icon will be offset from the error icon.")]
    public int OffsetInformation
    {
      get
      {
        return _offsetInformation;
      }
      set
      {
        if (_offsetInformation != value)
        {
          _offsetInformation = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the offset warning.
    /// </summary>
    /// <value>The offset warning.</value>
    [DefaultValue(16), Description("The number of pixels the warning icon will be offset from the error icon.")]
    public int OffsetWarning
    {
      get
      {
        return _offsetWarning;
      }
      set
      {
        if (_offsetWarning != value)
        {
          _offsetWarning = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether broken rules with severity Infomation should be visible.
    /// </summary>
    /// <value><c>true</c> if Infomation is visible; otherwise, <c>false</c>.</value>
    [DefaultValue(true), Description("Determines if the information icon should be displayed when information exists.")]
    public bool VisibleInformation
    {
      get
      {
        return _visibleInformation;
      }
      set
      {
        if (_visibleInformation != value)
        {
          _visibleInformation = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether broken rules with severity Warning should be visible.
    /// </summary>
    /// <value><c>true</c> if Warning is visible; otherwise, <c>false</c>.</value>
    [DefaultValue(true), Description("Determines if the warning icon should be displayed when warnings exist.")]
    public bool VisibleWarning
    {
      get
      {
        return _visibleWarning;
      }
      set
      {
        if (_visibleWarning != value)
        {
          _visibleWarning = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether show only most severe broken rules message.
    /// </summary>
    /// <value><c>true</c> if show only most severe; otherwise, <c>false</c>.</value>
    [DefaultValue(true), Description("Determines if the broken rules are show by severity - if true only most severe level is shown.")]
    public bool ShowOnlyMostSevere
    {
      get
      {
        return _showMostSevereOnly;
      }
      set
      {
        if (_showMostSevereOnly != value)
        {
          _showMostSevereOnly = value;
          //Refresh controls
          ProcessAllControls();
        }
      }
    }


    #endregion

    #region Private properties

    private Icon DefaultIconInformation
    {
      get
      {
        if (_defaultIconInformation == null)
        {
          lock (typeof(ErrorWarnInfoProvider))
          {
            if (_defaultIconInformation == null)
            {
              Bitmap bitmap = (Bitmap)imageList1.Images[2];
              _defaultIconInformation = Icon.FromHandle(bitmap.GetHicon());
            }
          }
        }

        return _defaultIconInformation;
      }
    }

    private Icon DefaultIconWarning
    {
      get
      {
        if (_defaultIconWarning == null)
        {
          lock (typeof(ErrorWarnInfoProvider))
          {
            if (_defaultIconWarning == null)
            {
              Bitmap bitmap = (Bitmap)imageList1.Images[1];
              _defaultIconWarning = Icon.FromHandle(bitmap.GetHicon());
            }
          }
        }

        return _defaultIconWarning;
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Clears all errors associated with this component.
    /// </summary>
    /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    public new void Clear()
    {
      base.Clear();
      errorProviderInfo.Clear();
      errorProviderWarn.Clear();
    }

    /// <summary>
    /// Gets the information.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <returns></returns>
    public string GetInformation(Control control)
    {
      return errorProviderInfo.GetError(control);
    }

    /// <summary>
    /// Gets the warning.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <returns></returns>
    public string GetWarning(Control control)
    {
      return errorProviderWarn.GetError(control);
    }

    private void InitializeAllControls(Control.ControlCollection controls)
    {
      // clear internal
      _controls.Clear();

      // run recursive initialize of controls
      Initialize(controls);
    }


    private void Initialize(Control.ControlCollection controls)
    {
      //We don't provide an extended property, so if the control is
      // not a Label then 'hook' the validating event here!
      foreach (Control control in controls)
      {
        if (control is Label) continue;
        // Initialize bindings
        foreach (Binding binding in control.DataBindings)
        {
          // get the Binding if appropriate
          if (binding.DataSource == DataSource)
          {
            _controls.Add(control);
          }
        }
        // Initialize any subcontrols
        if (control.Controls.Count > 0)
        {
          Initialize(control.Controls);
        }
      }

    }

    void DataSource_CurrentItemChanged(object sender, EventArgs e)
    {
      ProcessAllControls();
    }

    void DataSource_DataSourceChanged(object sender, EventArgs e)
    {
      //InitializeAllControls(ContainerControl.Controls);
      //ProcessAllControls();
    }

    private void ProcessAllControls()
    {
      if (_isInitializing) return;

      // get error/warn/info list from bussiness object
      GetWarnInfoList();
      // process controls in window
      ProcessControls();
    }


    private void GetWarnInfoList()
    {
      _infoList.Clear();
      _warningList.Clear();
      _errorList.Clear();

      BindingSource bs = (BindingSource)DataSource;
      if (bs == null) return;
      if (bs.Position == -1) return;

      // we can only deal with CSLA BusinessBase objects
      if (bs.Current is Csla.Core.BusinessBase)
      {
        // get the BusinessBase object
        Csla.Core.BusinessBase bb = bs.Current as Csla.Core.BusinessBase;

        if (bb != null)
        {
          foreach (Csla.Rules.BrokenRule br in bb.BrokenRulesCollection)
          {
            switch (br.Severity)
            {
              case Csla.Rules.RuleSeverity.Error:
                if (_errorList.ContainsKey(br.Property))
                {
                  _errorList[br.Property] =
                      String.Concat(_errorList[br.Property], Environment.NewLine, br.Description);
                }
                else
                {
                  _errorList.Add(br.Property, br.Description);
                }
                break;
              case Csla.Rules.RuleSeverity.Warning:
                if (_warningList.ContainsKey(br.Property))
                {
                  _warningList[br.Property] =
                      String.Concat(_warningList[br.Property], Environment.NewLine, br.Description);
                }
                else
                {
                  _warningList.Add(br.Property, br.Description);
                }
                break;
              default: // consider it an Info
                if (_infoList.ContainsKey(br.Property))
                {
                  _infoList[br.Property] =
                      String.Concat(_infoList[br.Property], Environment.NewLine, br.Description);
                }
                else
                {
                  _infoList.Add(br.Property, br.Description);
                }
                break;
            }
          }
        }
      }
    }

    private void ProcessControls()
    {
      foreach (Control control in _controls)
      {
        ProcessControl(control);
      }

    }


    /// <summary>
    /// Processes the control.
    /// </summary>
    /// <param name="control">The control.</param>
    private void ProcessControl(IBindableComponent control)
    {
      if (control == null) throw new ArgumentNullException("control");

      bool hasWarning = false;
      bool hasInfo = false;

      foreach (Binding binding in control.DataBindings)
      {
        // get the Binding if appropriate
        if (binding.DataSource == DataSource)
        {
          string propertyName = binding.BindingMemberInfo.BindingField;

          bool bError = _errorList.ContainsKey(propertyName);
          bool bWarn = _warningList.ContainsKey(propertyName);
          bool bInfo = _infoList.ContainsKey(propertyName);

          // set flags to indicat if Warning or Info is highest severity; else false
          if (_showMostSevereOnly)
          {
            bInfo = bInfo && !bWarn && !bError;
            bWarn = bWarn && !bError;
          }

          int offsetInformation = _offsetInformation;
          int offsetWarning = _offsetWarning;

          // Set / fix offsets
          // by default the setting are correct for Error (0), Warning and Info
          if (!bError)
          {
            if (bWarn)
            {
              // warning and possibly info, no error
              offsetInformation = _offsetInformation - _offsetWarning;
              offsetWarning = 0;
            }
            else
            {
              // Info only
              offsetInformation = 0;
            }
          }
          else if (!bWarn)
          {
            offsetInformation = _offsetInformation - _offsetWarning;
          }


          // should warning be visible
          if (_visibleWarning && bWarn)
          {
            errorProviderWarn.SetError(binding.Control, _warningList[propertyName]);
            errorProviderWarn.SetIconPadding(binding.Control,
                                                  base.GetIconPadding(binding.Control) +
                                                  offsetWarning);
            errorProviderWarn.SetIconAlignment(binding.Control,
                                                    base.GetIconAlignment(binding.Control));
            hasWarning = true;
          }

          // should info be shown
          if (_visibleInformation && bInfo)
          {
            errorProviderInfo.SetError(binding.Control, _infoList[propertyName]);
            errorProviderInfo.SetIconPadding(binding.Control,
                                                  base.GetIconPadding(binding.Control) +
                                                  offsetInformation);
            errorProviderInfo.SetIconAlignment(binding.Control,
                                                    base.GetIconAlignment(binding.Control));

            hasInfo = true;
          }
        }
      }

      if (!hasWarning) errorProviderWarn.SetError((Control)control, string.Empty);
      if (!hasInfo) errorProviderInfo.SetError((Control)control, string.Empty);
    }

    private void ResetBlinkStyleInformation()
    {
      BlinkStyleInformation = ErrorBlinkStyle.BlinkIfDifferentError;
    }

    private void ResetBlinkStyleWarning()
    {
      BlinkStyleWarning = ErrorBlinkStyle.BlinkIfDifferentError;
    }

    private void ResetIconInformation()
    {
      IconInformation = DefaultIconInformation;
    }

    private void ResetIconWarning()
    {
      IconWarning = DefaultIconWarning;
    }

    /// <summary>
    /// Sets the information.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="value">The value.</param>
    public void SetInformation(Control control, string value)
    {
      errorProviderInfo.SetError(control, value);
    }

    /// <summary>
    /// Sets the warning.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="value">The value.</param>
    public void SetWarning(Control control, string value)
    {
      errorProviderWarn.SetError(control, value);
    }

    private bool ShouldSerializeIconInformation()
    {
      return (IconInformation != DefaultIconInformation);
    }

    private bool ShouldSerializeIconWarning()
    {
      return (IconWarning != DefaultIconWarning);
    }

    private bool ShouldSerializeBlinkStyleInformation()
    {
      return (BlinkStyleInformation != ErrorBlinkStyle.BlinkIfDifferentError);
    }

    private bool ShouldSerializeBlinkStyleWarning()
    {
      return (BlinkStyleWarning != ErrorBlinkStyle.BlinkIfDifferentError);
    }

    /// <summary>
    /// Provides a method to update the bindings of the <see cref="P:System.Windows.Forms.ErrorProvider.DataSource"></see>, <see cref="P:System.Windows.Forms.ErrorProvider.DataMember"></see>, and the error text.
    /// </summary>
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    public new void UpdateBinding()
    {
      base.UpdateBinding();
      errorProviderInfo.UpdateBinding();
      errorProviderWarn.UpdateBinding();
    }

    #endregion

    #region ISupportInitialize Members

    void ISupportInitialize.BeginInit()
    {
      _isInitializing = true;
    }

    void ISupportInitialize.EndInit()
    {
      _isInitializing = false;
      if (this.ContainerControl != null)
      {
        InitializeAllControls(this.ContainerControl.Controls);
      }
    }

    #endregion
  }
}
