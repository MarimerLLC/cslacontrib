using System.ComponentModel;
using Gizmox.WebGUI.Forms;

namespace CslaContrib.WebGUI
{
  /// <summary>
  /// Helper class to manually do correct Bind and Unbind.
  /// </summary>
  public static class BindingSourceExtensions
  {
    /// <summary>
    /// Unbind the binding source and the Data object.
    /// Use this Method to safely disconnect the data object from a BindingSource before saving data.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="cancel">if set to <c>true</c> then call CancelEdit else call EndEdit.</param>
    /// <param name="isRoot">if set to <c>true</c> this BindingSource contains the Root object. Set to <c>false</c> for nested BindingSources</param>
    public static void Unbind(this BindingSource source, bool cancel, bool isRoot)
    {
      IEditableObject current = null;
      // position may be -1 when bindigsource is already unbound
      // and accessing source.Current will then throw Exception.
      if ((source.DataSource != null) && (source.Position > -1))
        current = source.Current as IEditableObject;

      // set Raise list changed to True
      source.RaiseListChangedEvents = false;
      // tell currency manager to suspend binding
      source.SuspendBinding();

      if (isRoot)
        source.DataSource = null;

      if (current == null)
        return;

      if (cancel)
        current.CancelEdit();
      else
        current.EndEdit();
    }

    /// <summary>
    /// Rebind the binding source and business object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="data">The data object.</param>
    public static void Rebind(this BindingSource source, object data)
    {
      source.Rebind(data, false);
    }

    /// <summary>
    /// Rebind the binding source and business object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="data">The data object.</param>
    /// <param name="metadataChanged">if set to <c>true</c> then metadata (type) was changed.</param>
    public static void Rebind(this BindingSource source, object data, bool metadataChanged)
    {
      source.RaiseListChangedEvents = true;
      source.SuspendBinding();

      if (data != null)
        source.DataSource = data;

      // set Raise list changed to True
      source.RaiseListChangedEvents = true;
      // tell currency manager to resume binding 
      source.ResumeBinding();
      // Notify UI controls that the dataobject/list was reset - but metadata was not changed 
      source.ResetBindings(metadataChanged);
    }
  }
}