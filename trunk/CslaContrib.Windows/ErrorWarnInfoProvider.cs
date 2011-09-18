/****************************************************************************
* Class Name   : ErrorWarnInfoProvider.cs
* Author       : Kenneth J. Koteles
* Created      : 10/04/2007 2:14 PM
* C# Version   : .NET 2.0
* Description  : This code is designed to create a new provider object to
*           work specifically with CSLA BusinessBase objects.  In
*           addition to providing the red error icon for items in the
*           BrokenRulesCollection with Csla.Rules.RuleSeverity.Error,
*           this object also provides a yellow warning icon for items
*           with Csla.Rules.RuleSeverity.Warning and a blue
*           information icon for items with
*           Csla.Rules.RuleSeverity.Information.  Since warnings
*           and information type items do not need to be fixed /
*           corrected prior to the object being saved, the tooltip
*           displayed when hovering over the respective icon contains
*           all the control's associated (by severity) broken rules.
* Revised      : 11/20/2007 8:32 AM
*     Change   : Warning and information icons were not being updated for
*           dependant properties (controls without the focus) when
*           changes were being made to a related property (control with
*           the focus).  Added a list of controls to be recursed
*           through each time a change was made to any control.  This
*           obviously could result in performance issues; however,
*           there is no consistent way to question the BusinessObject
*           in order to get a list of dependant properties based on a
*           property name.  It can be exposed to the UI (using
*           ValidationRules.GetRuleDescriptions()); however, it is up
*           to each developer to implement their own public method on
*           on the Business Object to do so.  To make this generic for
*           all CSLA Business Objects, I cannot assume the developer
*           always exposes the dependant properties (nor do I know what
*                they'll call the method); therefore, this is the best I can
*           do right now.
* Revised      : 11/23/2007 9:02 AM
*     Change   : Added new property ProcessDependantProperties to allow for
*           controlling when all controls are recursed through (for
*           dependant properties or not).  Default value is 'false'.
*           This allows the developer to ba able to choose whether or
*           not to use the control in this manner (which could have
*           performance implications).
* Revised      : 10/05/2009, Jonny Bekkum
*     Change: Added initialization of controls list (controls attached to BindingSource) 
*           and will update errors on all controls. Optimized retrieval of error, warn, info 
*           messages and setting these on the controls. 
****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CslaContrib.Windows
{
  [DesignerCategory("")]
  [ToolboxItem(true), ToolboxBitmap(typeof(ErrorWarnInfoProvider), "Cascade.ico")]
  public class ErrorWarnInfoProvider : ErrorProvider, IExtenderProvider, ISupportInitialize
  {
    #region internal variables

    private readonly System.ComponentModel.IContainer components = null;
    private readonly System.Windows.Forms.ErrorProvider _errorProviderInfo;
    private readonly System.Windows.Forms.ErrorProvider _errorProviderWarn;
    private readonly List<Control> _controls = new List<Control>();
    private static readonly Icon DefaultIconInformation;
    private static readonly Icon DefaultIconWarning;
    private int _offsetInformation = 32;
    private int _offsetWarning = 16;
    private bool _showInformation = true;
    private bool _showWarning = true;
    private bool _showMostSevereOnly = true;
    private readonly Dictionary<string, string> _errorList = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _warningList = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _infoList = new Dictionary<string, string>();
    private bool _isInitializing = false;

    #endregion

    #region Constructors

    static ErrorWarnInfoProvider()
    {
      DefaultIconInformation = CslaContrib.Windows.Properties.Resources.InformationIco16;
      DefaultIconWarning = CslaContrib.Windows.Properties.Resources.WarningIco16;
    }

    public ErrorWarnInfoProvider()
    {
      this.components = new System.ComponentModel.Container();
      this._errorProviderInfo = new System.Windows.Forms.ErrorProvider(this.components);
      this._errorProviderWarn = new System.Windows.Forms.ErrorProvider(this.components);
      BlinkRate = 0;

      _errorProviderInfo.BlinkRate = 0;
      _errorProviderInfo.Icon = DefaultIconInformation;

      _errorProviderWarn.BlinkRate = 0;
      _errorProviderWarn.Icon = DefaultIconWarning;
    }

    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    /// <param name="container">The container of the control.</param>
    public ErrorWarnInfoProvider(IContainer container)
      : this()
    {
      container.Add(this);
    }


    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
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
    [Category("Behavior")]
    [DefaultValue(0)]
    [Description("The rate in milliseconds at which the error icon blinks.")]
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

    [Category("Behavior")]
    [DefaultValue(0)]
    [Description("The rate in milliseconds at which the information icon blinks.")]
    public int BlinkRateInformation
    {
      get { return _errorProviderInfo.BlinkRate; }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException("BlinkRateInformation", value, "Blink rate must be zero or more");

        _errorProviderInfo.BlinkRate = value;

        if (value == 0)
          _errorProviderInfo.BlinkStyle = ErrorBlinkStyle.NeverBlink;
      }
    }

    [Category("Behavior")]
    [DefaultValue(0)]
    [Description("The rate in milliseconds at which the warning icon blinks.")]
    public int BlinkRateWarning
    {
      get { return _errorProviderWarn.BlinkRate; }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException("BlinkRateWarning", value, "Blink rate must be zero or more");

        _errorProviderWarn.BlinkRate = value;

        if (value == 0)
          _errorProviderWarn.BlinkStyle = ErrorBlinkStyle.NeverBlink;
      }
    }


    [Category("Behavior")]
    [DefaultValue(ErrorBlinkStyle.NeverBlink)]
    [Description("Controls whether the error icon blinks when an error is set.")]
    public new ErrorBlinkStyle BlinkStyle
    {
      get { return base.BlinkStyle; }
      set { base.BlinkStyle = value; }
    }

    [Category("Behavior")]
    [DefaultValue(ErrorBlinkStyle.NeverBlink)]
    [Description("Controls whether the information icon blinks when information is set.")]
    public ErrorBlinkStyle BlinkStyleInformation
    {
      get { return _errorProviderInfo.BlinkStyle; }
      set { _errorProviderWarn.BlinkStyle = value; }
    }

    [Category("Behavior")]
    [DefaultValue(ErrorBlinkStyle.NeverBlink)]
    [Description("Controls whether the warning icon blinks when a warning is set.")]
    public ErrorBlinkStyle BlinkStyleWarning
    {
      get { return _errorProviderWarn.BlinkStyle; }
      set { _errorProviderWarn.BlinkStyle = value; }
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
            bs1.CurrentItemChanged -= DataSource_CurrentItemChanged;
          }
        }


        base.DataSource = value;

        var bs = value as BindingSource;
        if (bs != null)
        {
          bs.CurrentItemChanged += DataSource_CurrentItemChanged;
        }
      }
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
    [Category("Behavior")]
    [Description("The icon used to indicate information.")]
    public Icon IconInformation
    {
      get
      {
        return _errorProviderInfo.Icon;
      }
      set
      {
        if (value == null)
          value = DefaultIconInformation;

        _errorProviderInfo.Icon = value;
      }
    }

    /// <summary>
    /// Gets or sets the icon warning.
    /// </summary>
    /// <value>The icon warning.</value>
    [Category("Behavior")]
    [Description("The icon used to indicate a warning.")]
    public Icon IconWarning
    {
      get
      {
        return _errorProviderWarn.Icon;
      }
      set
      {
        if (value == null)
          value = DefaultIconWarning;

        _errorProviderWarn.Icon = value;
      }
    }

    /// <summary>
    /// Gets or sets the offset information.
    /// </summary>
    /// <value>The offset information.</value>
    [Category("Behavior")]
    [DefaultValue(32), Description("The number of pixels the information icon will be offset from the error icon.")]
    public int OffsetInformation
    {
      get
      {
        return _offsetInformation;
      }
      set
      {
        _offsetInformation = value;
      }
    }

    /// <summary>
    /// Gets or sets the offset warning.
    /// </summary>
    /// <value>The offset warning.</value>
    [Category("Behavior")]
    [DefaultValue(16), Description("The number of pixels the warning icon will be offset from the error icon.")]
    public int OffsetWarning
    {
      get
      {
        return _offsetWarning;
      }
      set
      {
        _offsetWarning = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether broken rules with severity Infomation should be visible.
    /// </summary>
    /// <value><c>true</c> if Infomation is visible; otherwise, <c>false</c>.</value>
    [Category("Behavior")]
    [DefaultValue(true), Description("Determines if the information icon should be displayed when information exists.")]
    public bool ShowInformation
    {
      get
      {
        return _showInformation;
      }
      set
      {
        _showInformation = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether broken rules with severity Warning should be visible.
    /// </summary>
    /// <value><c>true</c> if Warning is visible; otherwise, <c>false</c>.</value>
    [Category("Behavior")]
    [DefaultValue(true), Description("Determines if the warning icon should be displayed when warnings exist.")]
    public bool ShowWarning
    {
      get
      {
        return _showWarning;
      }
      set
      {
        _showWarning = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether show only most severe broken rules message.
    /// </summary>
    /// <value><c>true</c> if show only most severe; otherwise, <c>false</c>.</value>
    [Category("Behavior")]
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

    #region Methods

    /// <summary>
    /// Clears all errors associated with this component.
    /// </summary>
    /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    public new void Clear()
    {
      base.Clear();
      _errorProviderInfo.Clear();
      _errorProviderWarn.Clear();
    }

    /// <summary>
    /// Gets the information.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <returns></returns>
    public string GetInformation(Control control)
    {
      return _errorProviderInfo.GetError(control);
    }

    /// <summary>
    /// Gets the warning.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <returns></returns>
    public string GetWarning(Control control)
    {
      return _errorProviderWarn.GetError(control);
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
      Debug.Print("ErrorWarnInfo: CurrentItemChanged, {0}", DateTime.Now.Ticks);
      ProcessAllControls();
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
            // we do not want to import result of object level broken rules 
            if (br.Property == null) continue; 

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
    private void ProcessControl(Control control)
    {
      if (control == null) throw new ArgumentNullException("control");

      bool hasWarning = false;
      bool hasInfo = false;

      var sbError = new StringBuilder();
      var sbWarn = new StringBuilder();
      var sbInfo = new StringBuilder();

      foreach (Binding binding in control.DataBindings)
      {
        // get the Binding if appropriate
        if (binding.DataSource == DataSource)
        {
          string propertyName = binding.BindingMemberInfo.BindingField;

          if (_errorList.ContainsKey(propertyName))
            sbError.AppendLine(_errorList[propertyName]);
          if (_warningList.ContainsKey(propertyName))
            sbWarn.AppendLine(_warningList[propertyName]);
          if (_infoList.ContainsKey(propertyName))
            sbInfo.AppendLine(propertyName);
        }
      }

      bool bError = sbError.Length > 0;
      bool bWarn = sbWarn.Length > 0;
      bool bInfo = sbInfo.Length > 0;

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
      if (_showWarning && bWarn)
      {
        _errorProviderWarn.SetError(control, sbWarn.ToString());
        _errorProviderWarn.SetIconPadding(control,
                                          base.GetIconPadding(control) +
                                              offsetWarning);
        _errorProviderWarn.SetIconAlignment(control,
                                                base.GetIconAlignment(control));
        hasWarning = true;
      }

      // should info be shown
      if (_showInformation && bInfo)
      {
        _errorProviderInfo.SetError(control, sbInfo.ToString());
        _errorProviderInfo.SetIconPadding(control,
                                              base.GetIconPadding(control) +
                                              offsetInformation);
        _errorProviderInfo.SetIconAlignment(control,
                                                base.GetIconAlignment(control));

        hasInfo = true;
      }

      if (!hasWarning) _errorProviderWarn.SetError((Control)control, string.Empty);
      if (!hasInfo) _errorProviderInfo.SetError((Control)control, string.Empty);
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
      _errorProviderInfo.SetError(control, value);
    }

    /// <summary>
    /// Sets the warning.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="value">The value.</param>
    public void SetWarning(Control control, string value)
    {
      _errorProviderWarn.SetError(control, value);
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
      _errorProviderInfo.UpdateBinding();
      _errorProviderWarn.UpdateBinding();
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