using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace MyCsla.Windows
{
    public static class BindingHelper
    {

        /// <summary>
        /// Unbinds the binding source and the Data object. Use this Method to safely disconnect the data object from a BindingSource before saving data.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="cancel">if set to <c>true</c> then call CancelEdit else call EndEdit.</param>
        /// <param name="isRoot">if set to <c>true</c> this BindingSource contains the Root object. Set to <c>false</c> for nested BindingSources</param>
        public static void UnbindBindingSource(BindingSource source, bool cancel, bool isRoot)
        {
            IEditableObject current = null;
            // position may be -1 if bindigsource is already unbound which results in Exception when trying to address current
            if ((source.DataSource != null) && (source.Position > -1)) {
                current = source.Current as IEditableObject;
            }

            // set Raise list changed to True
            source.RaiseListChangedEvents = false;
            // tell currency manager to suspend binding
            source.SuspendBinding();

            if (isRoot) source.DataSource = null;
            if (current == null) return;

            if (cancel)
            {
                current.CancelEdit();
            }
            else
            {
                current.EndEdit();
            }
        }

        /// <summary>
        /// Rebinds the binding source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="data">The data.</param>
        public static void RebindBindingSource(BindingSource source, object data)
        {
            RebindBindingSource(source, data, false);
        }


        /// <summary>
        /// Rebinds the binding source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="data">The data.</param>
        /// <param name="metadataChanged">if set to <c>true</c> then metadata (ovject/list type) was changed.</param>
        public static void RebindBindingSource(BindingSource source, object data, bool metadataChanged)
        {
            if (data != null)
            {
                source.DataSource = data;
            }

            // set Raise list changed to True
            source.RaiseListChangedEvents = true;
            // tell currency manager to resume binding 
            source.ResumeBinding();
            // Notify UI controls that the dataobject/list was reset - and if metadata was changed 
            source.ResetBindings(metadataChanged);
        }
    }
}
