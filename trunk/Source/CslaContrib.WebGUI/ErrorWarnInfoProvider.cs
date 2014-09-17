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
* Revised      : 22/07/2012, Tiago Freitas Leal
*     Change: Adapt to Visual WebGUI. Use ResourceHandle instead of Icon.
*          The WebGUI's ErrorProvider doesn't allow simultaneous
*          Error / Warning / Infornation icons/messages.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Csla.Core;
using Csla.Rules;
using Gizmox.WebGUI.Common.Resources;
using Gizmox.WebGUI.Forms;
using Gizmox.WebGUI.Forms.Design;

namespace CslaContrib.WebGUI
{
  /// <summary>
  /// WebGUI extender control that automatically
  /// displays error, warning, or information icons and
  /// text for the form controls based on the
  /// BrokenRulesCollection of a CSLA .NET business object.
  /// </summary>
  [ProvideProperty("IconAlignment", typeof (Control))]
  [ProvideProperty("Error", typeof (Control))]
  [ProvideProperty("IconPadding", typeof (Control))]
  [ToolboxItemFilter("CslaContrib.WebGUI", ToolboxItemFilterType.Require)]
  [ToolboxItemCategory("Components")]
  [ToolboxItem(true)]
  [ComplexBindingProperties("DataSource", "DataMember")]
  [DesignerCategory("")]
  [ToolboxBitmap(typeof (ErrorWarnInfoProvider), "ErrorWarnInfoProvider.bmp")]
  public class ErrorWarnInfoProvider : Gizmox.WebGUI.Forms.ErrorProvider, IExtenderProvider, ISupportInitialize
  {
    #region Private variables

    private readonly IContainer components;
    private readonly Gizmox.WebGUI.Forms.ErrorProvider _errorProviderInfo;
    private readonly Gizmox.WebGUI.Forms.ErrorProvider _errorProviderWarn;
    private readonly List<Control> _controls = new List<Control>();
    private static readonly ResourceHandle DefaultIconInformation;
    private static readonly ResourceHandle DefaultIconWarning;
    private static readonly ResourceHandle DefaultIconError;
    private bool _showInformation = true;
    private bool _showWarning = true;
    private readonly Dictionary<string, string> _errorList = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _warningList = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _infoList = new Dictionary<string, string>();
    private bool _isInitializing;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes the <see cref="ErrorWarnInfoProvider"/> class.
    /// </summary>
    static ErrorWarnInfoProvider()
    {
      DefaultIconInformation = new AssemblyResourceHandle(typeof (ErrorWarnInfoProvider), "CslaContrib.WebGUI.Resources.InformationIcon.png");
      DefaultIconWarning = new AssemblyResourceHandle(typeof (ErrorWarnInfoProvider), "CslaContrib.WebGUI.Resources.WarningIcon.png");
      DefaultIconError = new AssemblyResourceHandle(typeof (ErrorWarnInfoProvider), "CslaContrib.WebGUI.Resources.ErrorIcon.png");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorWarnInfoProvider"/> class.
    /// </summary>
    public ErrorWarnInfoProvider()
    {
      components = new Container();
      _errorProviderInfo = new Gizmox.WebGUI.Forms.ErrorProvider(components);
      _errorProviderWarn = new Gizmox.WebGUI.Forms.ErrorProvider(components);
      BlinkRate = 0;
      Icon = DefaultIconError;

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

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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
      if ((extendee is Control && !(extendee is Form)))
      {
        return !(extendee is ToolBar);
      }
      return false;
    }

    #endregion

    #region Public properties

    /// <summary>
    /// Gets or sets the rate at which the error icon flashes.
    /// </summary>
    /// <returns>The rate, in milliseconds, at which the error icon should flash. The default is 250 milliseconds.</returns>
    ///   
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than zero. </exception>
    ///   
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [Category("Behavior")]
    [DefaultValue(0)]
    [Description("The rate in milliseconds at which the error icon blinks.")]
    public new int BlinkRate
    {
      get { return base.BlinkRate; }
      set
      {
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException(@"BlinkRate", value, @"Blink rate must be zero or more");
        }

        base.BlinkRate = value;
        if (value == 0)
        {
          BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }
      }
    }

    /// <summary>
    /// Gets or sets the rate at which the information icon flashes.
    /// </summary>
    /// <returns>The rate, in milliseconds, at which the information icon should flash. The default is 250 milliseconds.</returns>
    ///   
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than zero. </exception>
    ///   
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [Category("Behavior")]
    [DefaultValue(0)]
    [Description("The rate in milliseconds at which the information icon blinks.")]
    public int BlinkRateInformation
    {
      get { return _errorProviderInfo.BlinkRate; }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException(@"BlinkRateInformation", value, @"Blink rate must be zero or more");

        _errorProviderInfo.BlinkRate = value;

        if (value == 0)
          _errorProviderInfo.BlinkStyle = ErrorBlinkStyle.NeverBlink;
      }
    }

    /// <summary>
    /// Gets or sets the rate at which the warning icon flashes.
    /// </summary>
    /// <returns>The rate, in milliseconds, at which the warning icon should flash.
    /// The default is 250 milliseconds.</returns>
    ///   
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than zero. </exception>
    ///   
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [Category("Behavior")]
    [DefaultValue(0)]
    [Description("The rate in milliseconds at which the warning icon blinks.")]
    public int BlinkRateWarning
    {
      get { return _errorProviderWarn.BlinkRate; }
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException(@"BlinkRateWarning", value, @"Blink rate must be zero or more");

        _errorProviderWarn.BlinkRate = value;

        if (value == 0)
          _errorProviderWarn.BlinkStyle = ErrorBlinkStyle.NeverBlink;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating when the error icon flashes.
    /// </summary>
    /// <returns>One of the <see cref="T:Gizmox.WebGUI.Forms.ErrorBlinkStyle"/> values.
    /// The default is <see cref="F:Gizmox.WebGUI.Forms.ErrorBlinkStyle.BlinkIfDifferentError"/>.</returns>
    ///   
    /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:Gizmox.WebGUI.Forms.ErrorBlinkStyle"/> values. </exception>
    ///   
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [Category("Behavior")]
    [DefaultValue(ErrorBlinkStyle.NeverBlink)]
    [Description("Controls whether the error icon blinks when an error is set.")]
    public new ErrorBlinkStyle BlinkStyle
    {
      get { return base.BlinkStyle; }
      set { base.BlinkStyle = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating when the information icon flashes.
    /// </summary>
    /// <returns>One of the <see cref="T:Gizmox.WebGUI.Forms.ErrorBlinkStyle"/> values.
    /// The default is <see cref="F:Gizmox.WebGUI.Forms.ErrorBlinkStyle.BlinkIfDifferentError"/>.</returns>
    ///   
    /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:Gizmox.WebGUI.Forms.ErrorBlinkStyle"/> values. </exception>
    ///   
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [Category("Behavior")]
    [DefaultValue(ErrorBlinkStyle.NeverBlink)]
    [Description("Controls whether the information icon blinks when information is set.")]
    public ErrorBlinkStyle BlinkStyleInformation
    {
      get { return _errorProviderInfo.BlinkStyle; }
      set { _errorProviderWarn.BlinkStyle = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating when the warning icon flashes.
    /// </summary>
    /// <returns>One of the <see cref="T:Gizmox.WebGUI.Forms.ErrorBlinkStyle"/> values. The default is <see cref="F:Gizmox.WebGUI.Forms.ErrorBlinkStyle.BlinkIfDifferentError"/>.</returns>
    ///   
    /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:Gizmox.WebGUI.Forms.ErrorBlinkStyle"/> values. </exception>
    ///   
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [Category("Behavior")]
    [DefaultValue(ErrorBlinkStyle.NeverBlink)]
    [Description("Controls whether the warning icon blinks when a warning is set.")]
    public ErrorBlinkStyle BlinkStyleWarning
    {
      get { return _errorProviderWarn.BlinkStyle; }
      set { _errorProviderWarn.BlinkStyle = value; }
    }

    /// <summary>
    /// Gets or sets the data source that the <see cref="T:Gizmox.WebGUI.Forms.ErrorProvider"></see> monitors.
    /// </summary>
    /// <value></value>
    /// <returns>A data source based on the <see cref="T:System.Collections.IList"></see> interface to be monitored for errors. Typically, this is a <see cref="T:System.Data.DataSet"></see> to be monitored for errors.</returns>
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [DefaultValue((string) null)]
    public new object DataSource
    {
      get { return base.DataSource; }
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

    /*private void UpdateBindingsAndProcessAllControls()
    {
      if (ContainerControl != null)
      {
        InitializeAllControls(ContainerControl.Controls);
      }
      ProcessAllControls();
    }*/

    /// <summary>
    /// Gets or sets the information icon.
    /// </summary>
    /// <value>The information icon.</value>
    [Category("Behavior")]
    [Description("The icon used to indicate information.")]
    public ResourceHandle IconInformation
    {
      get { return _errorProviderInfo.Icon; }
      set
      {
        if (value == null)
          value = DefaultIconInformation;

        _errorProviderInfo.Icon = value;
      }
    }

    /// <summary>
    /// Gets or sets the warning icon.
    /// </summary>
    /// <value>The warning icon.</value>
    [Category("Behavior")]
    [Description("The icon used to indicate a warning.")]
    public ResourceHandle IconWarning
    {
      get { return _errorProviderWarn.Icon; }
      set
      {
        if (value == null)
          value = DefaultIconWarning;

        _errorProviderWarn.Icon = value;
      }
    }

    /// <summary>
    /// Gets or sets the error icon.
    /// </summary>
    /// <value>The error icon.</value>
    [Category("Behavior")]
    [Description("The icon used to indicate an error.")]
    public ResourceHandle IconError
    {
      get { return Icon; }
      set
      {
        if (value == null)
          value = DefaultIconError;

        Icon = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether broken rules with severity information should be visible.
    /// </summary>
    /// <value><c>true</c> if information is visible; otherwise, <c>false</c>.</value>
    [Category("Behavior")]
    [DefaultValue(true), Description("Determines if the information icon should be displayed when informations exists.")]
    public bool ShowInformation
    {
      get { return _showInformation; }
      set { _showInformation = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether broken rules with severity Warning should be visible.
    /// </summary>
    /// <value><c>true</c> if Warning is visible; otherwise, <c>false</c>.</value>
    [Category("Behavior")]
    [DefaultValue(true), Description("Determines if the warning icon should be displayed when warnings exist.")]
    public bool ShowWarning
    {
      get { return _showWarning; }
      set { _showWarning = value; }
    }

    #endregion

    #region Methods

    /*/// <summary>
    /// Clears all errors associated with this component.
    /// </summary>
    /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    public new void Clear()
    {
      base.Clear();
      _errorProviderInfo.Clear();
      _errorProviderWarn.Clear();
    }*/

    /// <summary>
    /// Gets the information message.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <returns></returns>
    public string GetInformation(Control control)
    {
      return _errorProviderInfo.GetError(control);
    }

    /// <summary>
    /// Gets the warning message.
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
      // We don't provide an extended property, so if the control is
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

    private void DataSource_CurrentItemChanged(object sender, EventArgs e)
    {
      Debug.Print("ErrorWarnInfo: CurrentItemChanged, {0}", DateTime.Now.Ticks);
      ProcessAllControls();
    }

    private void ProcessAllControls()
    {
      if (_isInitializing)
        return;

      // get error/warn/info list from business object
      GetWarnInfoList();
      // process controls in window
      ProcessControls();
    }

    private void GetWarnInfoList()
    {
      _infoList.Clear();
      _warningList.Clear();
      _errorList.Clear();

      var bs = (BindingSource) DataSource;
      if (bs == null)
        return;
      if (bs.Position == -1)
        return;

      // get the BusinessBase object
      var bb = bs.Current as BusinessBase;

      // we can only deal with CSLA BusinessBase objects
      if (bb != null)
      {
        foreach (BrokenRule br in bb.BrokenRulesCollection)
        {
          // we do not want to import result of object level broken rules
          if (br.Property == null)
            continue;

          switch (br.Severity)
          {
            case RuleSeverity.Error:
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
            case RuleSeverity.Warning:
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
      if (control == null)
        throw new ArgumentNullException("control");

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
      bWarn = _showWarning && bWarn;
      bInfo = _showInformation && bInfo;

      if (bError)
      {
        SetError(control, sbError.ToString());
      }
      else if (bWarn)
      {
        _errorProviderWarn.SetError(control, sbWarn.ToString());
        _errorProviderWarn.SetIconPadding(control, GetIconPadding(control));
        _errorProviderWarn.SetIconAlignment(control, GetIconAlignment(control));
      }
      else if (bInfo)
      {
        _errorProviderInfo.SetError(control, sbInfo.ToString());
        _errorProviderInfo.SetIconPadding(control, GetIconPadding(control));
        _errorProviderInfo.SetIconAlignment(control, GetIconAlignment(control));
      }
      else
        _errorProviderInfo.SetError(control, string.Empty);
    }

    /*private void ResetBlinkStyleInformation()
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

    private void ResetIconError()
    {
      Icon = DefaultIconError;
    }*/

    /// <summary>
    /// Sets the information message.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="message">The information message.</param>
    public void SetInformation(Control control, string message)
    {
      _errorProviderInfo.SetError(control, message);
    }

    /// <summary>
    /// Sets the warning message.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="message">The warning message.</param>
    public void SetWarning(Control control, string message)
    {
      _errorProviderWarn.SetError(control, message);
    }

    /*private bool ShouldSerializeIconInformation()
    {
      return (IconInformation != DefaultIconInformation);
    }

    private bool ShouldSerializeIconWarning()
    {
      return (IconWarning != DefaultIconWarning);
    }

    private bool ShouldSerializeIconError()
    {
      return (Icon != DefaultIconError);
    }

    private bool ShouldSerializeBlinkStyleInformation()
    {
      return (BlinkStyleInformation != ErrorBlinkStyle.BlinkIfDifferentError);
    }

    private bool ShouldSerializeBlinkStyleWarning()
    {
      return (BlinkStyleWarning != ErrorBlinkStyle.BlinkIfDifferentError);
    }*/

    /// <summary>
    /// Provides a method to update the bindings of the <see cref="P:Gizmox.WebGUI.Forms.ErrorProvider.DataSource"></see>, <see cref="P:Gizmox.WebGUI.Forms.ErrorProvider.DataMember"></see>, and the error text.
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
      if (ContainerControl != null)
      {
        InitializeAllControls(ContainerControl.Controls);
      }
    }

    #endregion
  }
}