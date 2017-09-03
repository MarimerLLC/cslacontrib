﻿namespace CslaContrib.Caliburn.Micro
{
    using Caliburn.Micro;

    /// <summary>
    /// Base class used to create ScreenWithModel objects,
    /// with pre-existing verbs for use by
    /// InvokeMethod or Invoke.
    /// </summary>
    /// <typeparam name="T">Type of the Model object.</typeparam>
    public abstract class ScreenWithModel<T> : ScreenWithModelBase<T>
    {
        #region Verbs

        /// <summary>
        /// Saves the Model, first committing changes
        /// if ManagedObjectLifetime is true.
        /// </summary>
        public virtual void Save(object sender, ExecuteEventArgs e)
        {
            BeginSave();
        }

        /// <summary>
        /// Cancels changes made to the model
        /// if ManagedObjectLifetime is true.
        /// </summary>
        public virtual void Cancel(object sender, ExecuteEventArgs e)
        {
            DoCancel();
        }

        /// <summary>
        /// Adds a new item to the Model (if it
        /// is a collection).
        /// </summary>
        public virtual void AddNew(object sender, ExecuteEventArgs e)
        {
#if SILVERLIGHT
            BeginAddNew();
#else
            DoAddNew();
#endif
        }

        /// <summary>
        /// Removes an item from the Model (if it
        /// is a collection).
        /// </summary>
        public virtual void Remove(object sender, ExecuteEventArgs e)
        {
            DoRemove(e.MethodParameter);
        }

        /// <summary>
        /// Marks the Model for deletion (if it is an
        /// editable root object).
        /// </summary>
        public virtual void Delete(object sender, ExecuteEventArgs e)
        {
            DoDelete();
        }

        #endregion
    }
}
