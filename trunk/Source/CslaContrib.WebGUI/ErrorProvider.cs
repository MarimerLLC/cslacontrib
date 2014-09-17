// code from Alexnaldo Santos (www.automato.org), used with permission.
// http://www.visualwebgui.com/Developers/Forums/tabid/364/forumid/29/threadid/11219/scope/posts/threadpage/2/Default.aspx
// http://www.visualwebgui.com/Developers/Forums/tabid/364/forumid/29/threadid/11219/scope/posts/threadpage/3/Default.aspx

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Gizmox.WebGUI.Common;
using Gizmox.WebGUI.Forms;
using Gizmox.WebGUI.Forms.Design;

namespace CslaContrib.WebGUI
{
  /// <summary>
  /// Provides a user interface for indicating that a control on a form has an error associated with it.
  /// </summary>
  [ProvideProperty("IconAlignment", typeof (Control))]
  [ProvideProperty("Error", typeof (Control))]
  [ProvideProperty("IconPadding", typeof (Control))]
  [ToolboxItemFilter("CslaContrib.WebGUI", ToolboxItemFilterType.Require)]
  [ToolboxItemCategory("Components")]
  [ToolboxItem(true)]
  [ToolboxBitmap(typeof (Global), "ErrorProvider.bmp")]
  [ComplexBindingProperties("DataSource", "DataMember")]
  public class ErrorProvider : Gizmox.WebGUI.Forms.ErrorProvider, ISupportInitialize
  {
    #region private fields

    private bool _loadOnEndInit;
    private BindingManagerBase _errorManager;
    private bool _isInitializing;
    private bool _isConfigurating;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CslaContrib.WebGUI.ErrorProvider"/> class and 
    /// initializes the default settings for <see cref="Gizmox.WebGUI.Forms.ErrorProvider.BlinkRate"/>, 
    /// <see cref="Gizmox.WebGUI.Forms.ErrorProvider.BlinkStyle"/>, and the 
    /// <see cref="Gizmox.WebGUI.Forms.ErrorProvider.Icon"/>.
    /// </summary>
    public ErrorProvider()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CslaContrib.WebGUI.ErrorProvider"/> class attached to a container.
    /// </summary>
    /// <param name="parentControl">The container of the control to monitor for errors.</param>
    public ErrorProvider(ContainerControl parentControl)
      : base(parentControl)
    {
      parentControl.BindingContextChanged += ParentControlBindingContextChanged;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CslaContrib.WebGUI.ErrorProvider"/> class attached 
    /// to an <see cref="System.ComponentModel.IContainer"/> implementation.
    /// </summary>
    /// <param name="container">The <see cref="System.ComponentModel.IContainer"/> to monitor for errors.</param>
    public ErrorProvider(IContainer container)
      : this()
    {
      container.Add(this);
    }

    private void ParentControlBindingContextChanged(object sender, EventArgs e)
    {
    }

    #endregion

    #region public Properties

    /// <summary>
    /// Gets or sets the data source that the <see cref="T:CslaContrib.WebGUI.ErrorProvider"/> monitors.
    /// </summary>
    /// <returns>A data source based on the <see cref="T:System.Collections.IList"/> interface to be monitored for errors.
    /// Typically, this is a <see cref="T:System.Data.DataSet"/> to be monitored for errors.</returns>
    ///   
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [DefaultValue("")]
    [AttributeProvider(typeof (IListSource))]
    public new object DataSource
    {
      get { return base.DataSource; }
      set
      {
        if (ContainerControl != null && value != null && !string.IsNullOrEmpty(DataMember))
        {
          try
          {
            _errorManager = ContainerControl.BindingContext[value, DataMember];
          }
          catch (ArgumentException)
          {
            base.DataMember = string.Empty;
          }
        }
        ConfigureDataSource(value, DataMember, false);
      }
    }

    /// <summary>
    /// Gets or sets the list within a data source to monitor.
    /// </summary>
    /// <returns>The string that represents a list within the data source specified by the
    /// <see cref="P:CslaContrib.WebGUI.ErrorProvider.DataSource"/> to be monitored.
    /// Typically, this will be a <see cref="T:System.Data.DataTable"/>.</returns>
    ///   
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
    public new string DataMember
    {
      get { return base.DataMember; }
      set
      {
        if (value == null)
        {
          base.DataMember = string.Empty;
        }
        ConfigureDataSource(DataSource, value, false);
      }
    }

    #endregion

    #region ISupportInitialize Members

    /// <summary>
    /// Signals the object that initialization is starting.
    /// </summary>
    void ISupportInitialize.BeginInit()
    {
      _isInitializing = true;
    }

    /// <summary>
    /// Signals the object that initialization is complete.
    /// </summary>
    void ISupportInitialize.EndInit()
    {
      var sn = DataSource as ISupportInitializeNotification;
      if (sn != null && !sn.IsInitialized)
      {
        sn.Initialized += HandlerInitialized;
      }
      else
      {
        EndInitCore();
      }
    }

    private void EndInitCore()
    {
      _isInitializing = false;
      if (_loadOnEndInit)
      {
        _loadOnEndInit = false;
        ConfigureDataSource(DataSource, DataMember, true);
      }
    }

    private void ConfigureDataSource(object newDataSource, string newDataMember, bool force)
    {
      if (!_isConfigurating)
      {
        _isConfigurating = true;
        try
        {
          if (DataSource == newDataSource && DataMember == newDataMember && !force)
          {
            return;
          }
          base.DataSource = newDataSource;
          base.DataMember = newDataMember;
          if (_isInitializing)
          {
            _loadOnEndInit = true;
          }
          else
          {
            UnwireEvents(_errorManager);
            if (ContainerControl != null && DataSource != null && ContainerControl.BindingContext != null)
            {
              _errorManager = ContainerControl.BindingContext[DataSource, DataMember];
            }
            else
            {
              _errorManager = null;
            }
            WireEvents(_errorManager);
            if (_errorManager != null)
            {
              UpdateBinding();
            }
          }
        }
        finally
        {
          _isConfigurating = false;
        }
      }
    }

    private void UnwireEvents(BindingManagerBase bindingManager)
    {
      if (bindingManager == null)
      {
        return;
      }
      bindingManager.CurrentChanged -= HandlerCurrentChanged;
      bindingManager.BindingComplete -= HandlerBindingComplete;
      var currencyManager = bindingManager as CurrencyManager;
      if (currencyManager != null)
      {
        currencyManager.ItemChanged -= HandlerItemChanged;
        currencyManager.Bindings.CollectionChanged -= HandlerBindingsChanged;
      }
    }

    private void WireEvents(BindingManagerBase bindingManager)
    {
      if (bindingManager == null)
      {
        return;
      }
      bindingManager.CurrentChanged += HandlerCurrentChanged;
      bindingManager.BindingComplete += HandlerBindingComplete;
      var currencyManager = bindingManager as CurrencyManager;
      if (currencyManager != null)
      {
        currencyManager.ItemChanged += HandlerItemChanged;
        currencyManager.Bindings.CollectionChanged += HandlerBindingsChanged;
      }
    }

    /// <summary>
    /// Provides a method to update the bindings of the <see cref="P:CslaContrib.WebGUI.ErrorProvider.DataSource"/>, 
    /// <see cref="P:CslaContrib.WebGUI.ErrorProvider.DataMember"/>, and the error text.
    /// </summary>
    /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    public new void UpdateBinding()
    {
      HandlerCurrentChanged(_errorManager, EventArgs.Empty);
    }

    private void HandlerBindingsChanged(object sender, CollectionChangeEventArgs e)
    {
      HandlerCurrentChanged(_errorManager, e);
    }

    private void HandlerItemChanged(object sender, ItemChangedEventArgs e)
    {
      BindingsCollection bc = _errorManager.Bindings;
      if (e.Index != -1 || _errorManager.Count != 0)
      {
        HandlerCurrentChanged(sender, e);
      }
      else
      {
        int count = bc.Count;
        for (int i = 0; i < count; i++)
        {
          if (bc[i].Control != null)
          {
            SetError(bc[i].Control, string.Empty);
          }
        }
      }
    }

    private void HandlerCurrentChanged(object sender, EventArgs e)
    {
      if (_errorManager.Count == 0)
      {
        return;
      }
      object obj = _errorManager.Current;
      var errorInfo = obj as IDataErrorInfo;
      if (errorInfo == null)
      {
        return;
      }
      var bindings = _errorManager.Bindings;
      int count = bindings.Count;
      var hashTable = new Hashtable(count);
      for (var i = 0; true; i++)
      {
        if (i >= count)
        {
          foreach (DictionaryEntry entry in hashTable)
          {
            SetError((Control) entry.Key, (string) entry.Value);
          }
          return;
        }

        if (bindings[i].Control != null)
        {
          var error = errorInfo[bindings[i].BindingMemberInfo.BindingField];
          if (error == null)
          {
            error = string.Empty;
          }
          var error2 = string.Empty;
          if (hashTable.Contains(bindings[i].Control))
          {
            error2 = hashTable[bindings[i].Control] as string;
          }
          if (string.IsNullOrEmpty(error2))
          {
            error2 = error;
          }
          else
          {
            error2 += Environment.NewLine + error;
          }
          hashTable[bindings[i].Control] = error2;
        }
      }
    }

    private void HandlerInitialized(object sender, EventArgs e)
    {
      var sn = DataSource as ISupportInitializeNotification;
      if (sn != null)
      {
        sn.Initialized -= HandlerInitialized;
      }
      EndInitCore();
    }

    private void HandlerBindingComplete(object sender, BindingCompleteEventArgs e)
    {
      Binding binding = e.Binding;
      if (binding != null && binding.Control != null)
      {
        SetError(binding.Control, (e.ErrorText == null ? string.Empty : e.ErrorText));
      }
    }

    #endregion
  }
}