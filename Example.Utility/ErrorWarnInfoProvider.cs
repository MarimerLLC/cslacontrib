/****************************************************************************
* Class Name   : ErrorWarnInfoProvider.cs
* Author       : Kenneth J. Koteles
* Created      : 10/04/2007 2:14 PM
* C# Version   : .NET 2.0
* Description  : This code is designed to create a new provider object to
*		         work specifically with CSLA BusinessBase objects.  In 
*		         addition to providing the red error icon for items in the 
*		         BrokenRulesCollection with Csla.Validation.RuleSeverity.Error,
*		         this object also provides a yellow warning icon for items
*		         with Csla.Validation.RuleSeverity.Warning and a blue
*		         information icon for items with 
*		         Csla.Validation.RuleSeverity.Information.  Since warnings
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
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Example.Utility
{
    /// <summary>
    /// Windows Forms extender control that automatically
    /// displays error, warning, or information icons and
    /// text for the form controls based on the 
    /// BrokenRulesCollection from a CSLA .NET business object.
    /// </summary>
    [DesignerCategory("")]
    [ToolboxItem(true), ToolboxBitmap(typeof(ErrorWarnInfoProvider), "Cascade.ico")]
    public partial class ErrorWarnInfoProvider : System.Windows.Forms.ErrorProvider, IExtenderProvider
    {
        private int _blinkRateInformation;
        private int _blinkRateWarning;
        private ErrorBlinkStyle _blinkStyleInformation = ErrorBlinkStyle.BlinkIfDifferentError;
        private ErrorBlinkStyle _blinkStyleWarning = ErrorBlinkStyle.BlinkIfDifferentError;
        private IContainer _container;
        private List<Control> _controls = new List<Control>();
        private object _dataSource;
        private const int _defaultBlinkRateInformation = 250;
        private const int _defaultBlinkRateWarning = 250;
        private static Icon _defaultIconInformation;
        private static Icon _defaultIconWarning;
        private Icon _iconInformation;
        private Icon _iconWarning;
        private int _offsetInformation = 32;
        private int _offsetWarning = 16;
        private ContainerControl _parentControl;
        private bool _processDependantProperties = false;
        private EventHandler _propChangedEvent;
        private bool _visibleInformation = true;
        private bool _visibleWarning = true;

        /// <summary>
        /// Creates an instance of the object.
        /// </summary>
        /// <param name="container">The container of the control.</param>
        public ErrorWarnInfoProvider(IContainer container)
        {
            container.Add(this);
            this._container = container;

            InitializeComponent();

            this._propChangedEvent = new EventHandler(this.ParentControl_BindingContextChanged);

            this._blinkRateInformation = _defaultBlinkRateInformation;
            this._iconInformation = DefaultIconInformation;
            this.errorProviderInfo.BlinkRate = this._blinkRateInformation;
            this.errorProviderInfo.Icon = this._iconInformation;

            this._blinkRateWarning = _defaultBlinkRateWarning;
            this._iconWarning = DefaultIconWarning;
            this.errorProviderWarn.BlinkRate = this._blinkRateWarning;
            this.errorProviderWarn.Icon = _iconWarning;
        }

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
            if ((extendee is Control && !( extendee is Form)))
            {
                return !( extendee is ToolBar);
            }
            else
            {
                return false;
            }
        }

        #endregion

        [DefaultValue(_defaultBlinkRateInformation), Description("The rate in milliseconds at which the information icon blinks.")]
        public int BlinkRateInformation
        {
            get
            {
                return this._blinkRateInformation;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("BlinkRateInformation", value, "Blink rate must be zero or more");
                }

                this._blinkRateInformation = value;
                this.errorProviderInfo.BlinkRate = this._blinkRateInformation;

                if (this._blinkRateInformation == 0)
                {
                    this.BlinkStyleInformation = ErrorBlinkStyle.NeverBlink;
                }
            }
        }

        [DefaultValue(_defaultBlinkRateWarning), Description("The rate in milliseconds at which the warning icon blinks.")]
        public int BlinkRateWarning
        {
            get
            {
                return this._blinkRateWarning;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("BlinkRateWarning", value, "Blink rate must be zero or more");
                }

                this._blinkRateWarning = value;
                this.errorProviderWarn.BlinkRate = this._blinkRateWarning;

                if (this._blinkRateWarning == 0)
                {
                    this.BlinkStyleWarning = ErrorBlinkStyle.NeverBlink;
                }
            }
        }

        [Description("Controls whether the information icon blinks when information is set.")]
        public ErrorBlinkStyle BlinkStyleInformation
        {
            get
            {
                if (this._blinkRateInformation == 0)
                {
                    return ErrorBlinkStyle.NeverBlink;
                }
                return this._blinkStyleInformation;
            }
            set
            {
                if (this._blinkRateInformation == 0)
                {
                    value = ErrorBlinkStyle.NeverBlink;
                }

                if (this._blinkStyleInformation != value)
                {
                    this._blinkStyleInformation = value;
                    this.errorProviderInfo.BlinkStyle = this._blinkStyleInformation;
                }
            }
        }

        [Description("Controls whether the warning icon blinks when a warning is set.")]
        public ErrorBlinkStyle BlinkStyleWarning
        {
            get
            {
                if (this._blinkRateWarning == 0)
                {
                    return ErrorBlinkStyle.NeverBlink;
                }
                return this._blinkStyleWarning;
            }
            set
            {
                if (this._blinkRateWarning == 0)
                {
                    value = ErrorBlinkStyle.NeverBlink;
                }

                if (this._blinkStyleWarning != value)
                {
                    this._blinkStyleWarning = value;
                    this.errorProviderWarn.BlinkStyle = this._blinkStyleWarning;
                }
            }
        }

        public new void Clear()
        {
            base.Clear();
            this.errorProviderInfo.Clear();
            this.errorProviderWarn.Clear();
        }

        [DefaultValue((string)null)]
        public new object DataSource
        {
            get
            {
                return this._dataSource;
            }
            set
            {
                if (value != null && this._dataSource != value)
                {
                    this._dataSource = value;
                    base.DataSource = value;

                    this._parentControl = this.ContainerControl;
                    this._parentControl.BindingContextChanged += this._propChangedEvent;

                    // This was added to ensure warnings and info messages get
                    // cleared when the DataSource of the BindingSource is changed
                    foreach (IComponent component in this._container.Components)
                    {
                        if (component is BindingSource)
                        {
                            BindingSource bs = (BindingSource)component;

                            if (bs == this._dataSource)
                            {
                                bs.DataSourceChanged += this.DataSourceChanged_Event;
                            }
                        }
                    }
                }
            }
        }

        private void DataSourceChanged_Event(object sender, EventArgs e)
        {
            this.errorProviderInfo.Clear();
            this.errorProviderWarn.Clear();
            //When DataSource changed, clear control list
            this._controls.Clear();
        }

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
                            Bitmap bitmap = (Bitmap)this.imageList1.Images[2];
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
                            Bitmap bitmap = (Bitmap)this.imageList1.Images[1];
                            _defaultIconWarning = Icon.FromHandle(bitmap.GetHicon());
                        }
                    }
                }

                return _defaultIconWarning;
            }
        }

        public string GetInformation(Control control)
        {
            return this.errorProviderInfo.GetError(control);
        }

        public string GetWarning(Control control)
        {
            return this.errorProviderWarn.GetError(control);
        }

        [Description("The icon used to indicate information.")]
        public Icon IconInformation
        {
            get
            {
                return this._iconInformation;
            }
            set
            {
                if (value == null)
                {
                    value = DefaultIconInformation;
                }

                this._iconInformation = value;
                this.errorProviderInfo.Icon = this._iconInformation;
            }
        }

        [Description("The icon used to indicate a warning.")]
        public Icon IconWarning
        {
            get
            {
                return this._iconWarning;
            }
            set
            {
                if (value == null)
                {
                    value = DefaultIconWarning;
                }

                this._iconWarning = value;
                this.errorProviderWarn.Icon = this._iconWarning;
            }
        }

        private void Initialize(Control.ControlCollection controls)
        {
            //We don't provide an extended property, so if the control is
            // not a Label then 'hook' the validating event here!
            foreach (Control control in controls)
            {
                if (control is Label)
                {
                }
                else if (control.Controls.Count > 0)
                {
                    Initialize(control.Controls);
                }
                else
                {
                    foreach (Binding binding in control.DataBindings)
                    {
                        // get the Binding if appropriate
                        if (binding.DataSource == this.DataSource)
                        {
                            if (binding.DataSourceUpdateMode == DataSourceUpdateMode.OnPropertyChanged)
                            {
                                control.KeyUp += this.Validation_Event;
                                //Add 'involved' control to control list
                                _controls.Add(control);
                            }
                            else if (binding.DataSourceUpdateMode == DataSourceUpdateMode.OnValidation)
                            {
                                control.Validated += this.Validation_Event;
                                //Add 'involved' control to control list
                                _controls.Add(control);
                            }
                        }
                    }
                }
            }
        }

        [DefaultValue(32), Description("The number of pixels the information icon will be offset from the error icon.")]
        public int OffsetInformation
        {
            get
            {
                return this._offsetInformation;
            }
            set
            {
                if (this._offsetInformation != value)
                {
                    this._offsetInformation = value;
                }
            }
        }

        [DefaultValue(16), Description("The number of pixels the warning icon will be offset from the error icon.")]
        public int OffsetWarning
        {
            get
            {
                return this._offsetWarning;
            }
            set
            {
                if (this._offsetWarning != value)
                {
                    this._offsetWarning = value;
                }
            }
        }

        private void ParentControl_BindingContextChanged(object sender, EventArgs e)
        {
            Initialize(this._parentControl.Controls);
        }

        private void ProcessControl(Control control)
        {
            foreach (Binding binding in control.DataBindings)
            {
                // get the Binding if appropriate
                if (binding.DataSource == this.DataSource)
                {
                    BindingSource bs =
                        (BindingSource)binding.DataSource;

                    // we can only deal with CSLA BusinessBase objects
                    if (bs.Current is Csla.Core.BusinessBase)
                    {
                        // get the BusinessBase object
                        Csla.Core.BusinessBase bb = bs.Current as Csla.Core.BusinessBase;
                        if (bb != null)
                        {
                            // get the object property name
                            string propertyName =
                                binding.BindingMemberInfo.BindingField;

                            //The errors are taking care of themselves in the base
                            //ErrorProvider - all we need to worry about are the
                            //Warnings and Information messages
                            StringBuilder warn = new StringBuilder();
                            StringBuilder info = new StringBuilder();

                            bool bError = false;
                            bool bWarn = false;

                            foreach (Csla.Validation.BrokenRule br in bb.BrokenRulesCollection)
                            {
                                if (br.Property == propertyName)
                                {
                                    if (this._visibleWarning == true &&
                                        br.Severity == Csla.Validation.RuleSeverity.Warning)
                                    {
                                        warn.AppendLine(br.Description);
                                        bWarn = true;
                                    }
                                    else if (this._visibleInformation == true &&
                                             br.Severity == Csla.Validation.RuleSeverity.Information)
                                    {
                                        info.AppendLine(br.Description);
                                    }
                                    else if (bError == false && br.Severity == Csla.Validation.RuleSeverity.Error)
                                    {
                                        bError = true;
                                    }
                                }
                            }

                            int offsetInformation = this._offsetInformation;
                            int offsetWarning = this._offsetWarning;
                            // Set / fix offsets

                            if (bError == false && bWarn == false)
                            {
                                offsetInformation = 0;
                            }
                            else if (bError == true && bWarn == false)
                            {
                                offsetInformation = this._offsetInformation - this._offsetWarning;
                            }
                            else if (bError == false && bWarn == true)
                            {
                                offsetInformation = this._offsetInformation - this._offsetWarning;
                                offsetWarning = 0;
                            }


                            if (this._visibleWarning == true && warn.Length > 0)
                            {
                                this.errorProviderWarn.SetError(binding.Control,
                                                                warn.ToString().Substring(0,
                                                                                          warn.ToString().Length -
                                                                                          2));
                                this.errorProviderWarn.SetIconPadding(binding.Control,
                                                                      base.GetIconPadding(binding.Control) +
                                                                      offsetWarning);
                                this.errorProviderWarn.SetIconAlignment(binding.Control,
                                                                        base.GetIconAlignment(binding.Control));
                            }
                            else
                            {
                                this.errorProviderWarn.SetError(binding.Control, string.Empty);
                            }

                            if (this._visibleInformation == true && info.Length > 0)
                            {
                                this.errorProviderInfo.SetError(binding.Control,
                                                                info.ToString().Substring(0,
                                                                                          info.ToString().Length -
                                                                                          2));
                                this.errorProviderInfo.SetIconPadding(binding.Control,
                                                                      base.GetIconPadding(binding.Control) +
                                                                      offsetInformation);
                                this.errorProviderInfo.SetIconAlignment(binding.Control,
                                                                        base.GetIconAlignment(binding.Control));
                            }
                            else
                            {
                                this.errorProviderInfo.SetError(binding.Control, string.Empty);
                            }
                        }
                    }
                }
            }
        }

        [DefaultValue(false), Description("Determines if all controls should be recursed through whenever any control value changes.")]
        public bool ProcessDependantProperties
        {
            get
            {
                return this._processDependantProperties;
            }
            set
            {
                if (this._processDependantProperties != value)
                {
                    this._processDependantProperties = value;
                }
            }
        }

        private void ResetBlinkStyleInformation()
        {
            this.BlinkStyleInformation = ErrorBlinkStyle.BlinkIfDifferentError;
        }

        private void ResetBlinkStyleWarning()
        {
            this.BlinkStyleWarning = ErrorBlinkStyle.BlinkIfDifferentError;
        }

        private void ResetIconInformation()
        {
            this.IconInformation = DefaultIconInformation;
        }

        private void ResetIconWarning()
        {
            this.IconWarning = DefaultIconWarning;
        }

        public void SetInformation(Control control, string value)
        {
            this.errorProviderInfo.SetError(control, value);
        }

        public void SetWarning(Control control, string value)
        {
            this.errorProviderWarn.SetError(control, value);
        }

        private bool ShouldSerializeIconInformation()
        {
            return (this.IconInformation != DefaultIconInformation);
        }

        private bool ShouldSerializeIconWarning()
        {
            return (this.IconWarning != DefaultIconWarning);
        }

        private bool ShouldSerializeBlinkStyleInformation()
        {
            return (this.BlinkStyleInformation != ErrorBlinkStyle.BlinkIfDifferentError);
        }

        private bool ShouldSerializeBlinkStyleWarning()
        {
            return (this.BlinkStyleWarning != ErrorBlinkStyle.BlinkIfDifferentError);
        }

        public new void UpdateBinding()
        {
            base.UpdateBinding();
            this.errorProviderInfo.UpdateBinding();
            this.errorProviderWarn.UpdateBinding();
        }

        // Following event is hooked for all controls, it sets an error message with the use of ErrorProvider
        private void Validation_Event(object sender, EventArgs e)
        {
            if (this._visibleInformation == true || this._visibleWarning == true)
            {
                if (ProcessDependantProperties)
                {
                    //Due to the possibility of dependant properties, 
                    //  need to check all controls in list
                    for (int i = 0; i < this._controls.Count; i++)
                    {
                        Control control = (Control)this._controls[i];
                        ProcessControl(control);
                    }
                }
                else
                {
                    Control control = (Control)sender;
                    ProcessControl(control);
                }
            }
            else
            {
                this.errorProviderWarn.Clear();
                this.errorProviderInfo.Clear();
            }
        }

        [DefaultValue(true), Description("Determines if the information icon should be displayed when information exists.")]
        public bool VisibleInformation
        {
            get
            {
                return this._visibleInformation;
            }
            set
            {
                if (this._visibleInformation != value)
                {
                    this._visibleInformation = value;
                }
            }
        }

        [DefaultValue(true), Description("Determines if the warning icon should be displayed when warnings exist.")]
        public bool VisibleWarning
        {
            get
            {
                return this._visibleWarning;
            }
            set
            {
                if (this._visibleWarning != value)
                {
                    this._visibleWarning = value;
                }
            }
        }

    }
}
