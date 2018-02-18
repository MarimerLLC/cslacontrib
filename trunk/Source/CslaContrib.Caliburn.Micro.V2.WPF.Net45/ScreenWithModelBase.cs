using Caliburn.Micro;

namespace CslaContrib.Caliburn.Micro.V2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;
#if NETFX_CORE
    using Windows.UI.Xaml;
    using System.Linq.Expressions;
#else
    using System.Windows;
#endif
    using Csla;
    using Csla.Core;
    using Csla.Reflection;
    using Csla.Rules;
#if NET45
    using System.Runtime.CompilerServices;
#endif
    using ICloneable = System.ICloneable;

    /// <summary>
    /// Base class used to create ScreenWithModel objects that
    /// implement their own commands/verbs/actions.
    /// </summary>
    /// <typeparam name="T">Type of the Model object.</typeparam>
    public abstract class ScreenWithModelBase<T> : DependencyObject, IScreen, IChild, IViewAware, IHaveModel, IClose
    {
        #region Constructor

        /// <summary>
        /// Create new instance of base class used to create ScreenWithModel objects that
        /// implement their own commands/verbs/actions.
        /// </summary>
        public ScreenWithModelBase()
        {          
            #if WINDOWS_PHONE
            ManageObjectLifetime = false; // ViewModelBase
#endif
            SetPropertiesAtObjectLevel(); // ViewModelBase
            IsNotifying = true; // PropertyChangedBase
            displayName = GetType().FullName; // Screen
            views = new CslaContrib.Caliburn.Micro.V2.WeakValueDictionary<object, object>(); //IViewAware
        }        
        #endregion

        #region CSLA .NET ViewModelBase

        #region InitAsync

#if !WINDOWS_PHONE

        /// <summary>
        /// Method used to perform async initialization of the
        /// viewmodel. This method is usually invoked immediately
        /// following construction of the object instance.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ScreenWithModelBase<T>> InitAsync()
        {
            try
            {
                IsBusy = true;
                Model = await DoInitAsync();
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Error = ex;
            }
            return this;
        }

#pragma warning disable 1998
        /// <summary>
        /// Override this method to implement async initialization of
        /// the model object. The result of this method is used
        /// to set the Model property of the viewmodel.
        /// </summary>
        /// <returns>A Task that creates the model object.</returns>
        protected async virtual System.Threading.Tasks.Task<T> DoInitAsync()
        {
            throw new NotImplementedException("DoInitAsync");
        }
#pragma warning restore 1998

#endif

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Model object.
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(T), typeof(ScreenWithModelBase<T>),
#if NETFX_CORE
            new PropertyMetadata(default(T), (o, e) =>
#else
            new PropertyMetadata((o, e) =>
#endif
            {
                var screenWithModel = (ScreenWithModelBase<T>)o;
                if (screenWithModel.ManageObjectLifetime)
                {
                    var undo = e.NewValue as ISupportUndo;
                    if (undo != null)
                        undo.BeginEdit();
                }
                screenWithModel.OnModelChanged((T)e.OldValue, (T)e.NewValue);
#if NETFX_CORE
                screenWithModel.OnPropertyChanged("Model");
#endif
            }));

        /// <summary>
        /// Gets or sets the Model object.
        /// </summary>
        public T Model
        {
            get { return (T)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the
        /// ScreenWithModel should automatically managed the
        /// lifetime of the Model.
        /// </summary>
        public static readonly DependencyProperty ManageObjectLifetimeProperty =
            DependencyProperty.Register("ManageObjectLifetime", typeof(bool),
            typeof(ScreenWithModelBase<T>), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether the
        /// ViewManageObjectLifetime should automatically managed the
        /// lifetime of the ManageObjectLifetime.
        /// </summary>
        [Browsable(false)]
        [Display(AutoGenerateField = false)]
        public bool ManageObjectLifetime
        {
            get { return (bool)GetValue(ManageObjectLifetimeProperty); }
            set { SetValue(ManageObjectLifetimeProperty, value); }
        }

        private Exception _error;

        /// <summary>
        /// Gets the Error object corresponding to the
        /// last asyncronous operation.
        /// </summary>
        [Browsable(false)]
        [Display(AutoGenerateField = false)]
        public Exception Error
        {
            get { return _error; }
            protected set
            {
                if (!ReferenceEquals(_error, value))
                {
                    _error = value;
                    NotifyOfPropertyChange("Error");
                    if (_error != null)
                        OnError(_error);
                }
            }
        }

        /// <summary>
        /// Event raised when an error occurs during processing.
        /// </summary>
        public event EventHandler<ErrorEventArgs> ErrorOccurred;

        /// <summary>
        /// Raises ErrorOccurred event when an error occurs
        /// during processing.
        /// </summary>
        /// <param name="error"></param>
        protected virtual void OnError(Exception error)
        {
            if (ErrorOccurred != null)
                ErrorOccurred(this, new ErrorEventArgs { Error = error });
        }

        private bool _isBusy;

        /// <summary>
        /// Gets a value indicating whether this object is
        /// executing an asynchronous process.
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            protected set
            {
                _isBusy = value;
                NotifyOfPropertyChange("IsBusy");
                OnSetProperties();
            }
        }

        #endregion

        #region Can___ properties

        private bool _isDirty;

        /// <summary>
        /// Gets a value indicating whether the Model
        /// has been changed.
        /// </summary>
        public virtual bool IsDirty
        {
            get { return _isDirty; }
            protected set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    NotifyOfPropertyChange("IsDirty");
                }
            }
        }

        private bool _isValid;

        /// <summary>
        /// Gets a value indicating whether the Model
        /// is currently valid (has no broken rules).
        /// </summary>
        public virtual bool IsValid
        {
            get { return _isValid; }
            protected set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    NotifyOfPropertyChange("IsValid");
                }
            }
        }

        private bool _canSave;

        /// <summary>
        /// Gets a value indicating whether the Model
        /// can currently be saved.
        /// </summary>
        public virtual bool CanSave
        {
            get { return _canSave; }
            protected set
            {
                if (_canSave != value)
                {
                    _canSave = value;
                    NotifyOfPropertyChange("CanSave");
                }
            }
        }

        private bool _canCancel;

        /// <summary>
        /// Gets a value indicating whether the Model
        /// can currently be canceled.
        /// </summary>
        public virtual bool CanCancel
        {
            get { return _canCancel; }
            protected set
            {
                if (_canCancel != value)
                {
                    _canCancel = value;
                    NotifyOfPropertyChange("CanCancel");
                }
            }
        }

        private bool _canCreate;

        /// <summary>
        /// Gets a value indicating whether an instance
        /// of the Model
        /// can currently be created.
        /// </summary>
        public virtual bool CanCreate
        {
            get { return _canCreate; }
            protected set
            {
                if (_canCreate != value)
                {
                    _canCreate = value;
                    NotifyOfPropertyChange("CanCreate");
                }
            }
        }

        private bool _canDelete;

        /// <summary>
        /// Gets a value indicating whether the Model
        /// can currently be deleted.
        /// </summary>
        public virtual bool CanDelete
        {
            get { return _canDelete; }
            protected set
            {
                if (_canDelete != value)
                {
                    _canDelete = value;
                    NotifyOfPropertyChange("CanDelete");
                }
            }
        }

        private bool _canFetch;

        /// <summary>
        /// Gets a value indicating whether an instance
        /// of the Model
        /// can currently be retrieved.
        /// </summary>
        public virtual bool CanFetch
        {
            get { return _canFetch; }
            protected set
            {
                if (_canFetch != value)
                {
                    _canFetch = value;
                    NotifyOfPropertyChange("CanFetch");
                }
            }
        }

        private bool _canRemove;

        /// <summary>
        /// Gets a value indicating whether the Model
        /// can currently be removed.
        /// </summary>
        public virtual bool CanRemove
        {
            get { return _canRemove; }
            protected set
            {
                if (_canRemove != value)
                {
                    _canRemove = value;
                    NotifyOfPropertyChange("CanRemove");
                }
            }
        }

        private bool _canAddNew;

        /// <summary>
        /// Gets a value indicating whether the Model
        /// can currently be added.
        /// </summary>
        public virtual bool CanAddNew
        {
            get { return _canAddNew; }
            protected set
            {
                if (_canAddNew != value)
                {
                    _canAddNew = value;
                    NotifyOfPropertyChange("CanAddNew");
                }
            }
        }

        private void SetProperties()
        {
            ITrackStatus targetObject = Model as ITrackStatus;
            ICollection list = Model as ICollection;
            INotifyBusy busyObject = Model as INotifyBusy;
            bool isObjectBusy = false;
            if (busyObject != null && busyObject.IsBusy)
                isObjectBusy = true;

            // Does Model instance implement ITrackStatus
            if (targetObject != null)
            {
                var canDeleteInstance = BusinessRules.HasPermission(AuthorizationActions.DeleteObject, targetObject);

                IsDirty = targetObject.IsDirty;
                IsValid = targetObject.IsValid;
                CanSave = CanEditObject && targetObject.IsSavable && !isObjectBusy;
                CanCancel = CanEditObject && targetObject.IsDirty && !isObjectBusy;
                CanCreate = CanCreateObject && !targetObject.IsDirty && !isObjectBusy;
                CanDelete = CanDeleteObject && !isObjectBusy && canDeleteInstance;
                CanFetch = CanGetObject && !targetObject.IsDirty && !isObjectBusy;

                // Set properties for List
                if (list == null)
                {
                    CanRemove = false;
                    CanAddNew = false;
                }
                else
                {
                    Type itemType = Csla.Utilities.GetChildItemType(Model.GetType());
                    if (itemType == null)
                    {
                        CanAddNew = false;
                        CanRemove = false;
                    }
                    else
                    {
                        CanRemove = BusinessRules.HasPermission(AuthorizationActions.DeleteObject, itemType) &&
                            list.Count > 0 && !isObjectBusy;

                        CanAddNew = BusinessRules.HasPermission(AuthorizationActions.CreateObject, itemType) &&
                            !isObjectBusy;
                    }
                }
            }

            // Else if Model instance implement ICollection
            else if (list != null)
            {
                Type itemType = Csla.Utilities.GetChildItemType(Model.GetType());
                if (itemType == null)
                {
                    CanAddNew = false;
                    CanRemove = false;
                }
                else
                {
                    CanRemove = BusinessRules.HasPermission(AuthorizationActions.DeleteObject, itemType) &&
                        list.Count > 0 && !isObjectBusy;

                    CanAddNew = BusinessRules.HasPermission(AuthorizationActions.CreateObject, itemType) &&
                        !isObjectBusy;
                }
            }
            else
            {
                IsDirty = false;
                IsValid = false;
                CanCancel = false;
                CanCreate = CanCreateObject;
                CanDelete = false;
                CanFetch = CanGetObject && !IsBusy;
                CanSave = false;
                CanRemove = false;
                CanAddNew = false;
            }
        }

        #endregion

        #region Can methods that only account for user rights

        private bool _canCreateObject;

        /// <summary>
        /// Gets a value indicating whether the current
        /// user is authorized to create a Model.
        /// </summary>
        public virtual bool CanCreateObject
        {
            get { return _canCreateObject; }
            protected set
            {
                if (_canCreateObject != value)
                {
                    _canCreateObject = value;
                    NotifyOfPropertyChange("CanCreateObject");
                }
            }
        }

        private bool _canGetObject;

        /// <summary>
        /// Gets a value indicating whether the current
        /// user is authorized to retrieve a Model.
        /// </summary>
        public virtual bool CanGetObject
        {
            get { return _canGetObject; }
            protected set
            {
                if (_canGetObject != value)
                {
                    _canGetObject = value;
                    NotifyOfPropertyChange("CanGetObject");
                }
            }
        }

        private bool _canEditObject;

        /// <summary>
        /// Gets a value indicating whether the current
        /// user is authorized to save (insert or update
        /// a Model.
        /// </summary>
        public virtual bool CanEditObject
        {
            get { return _canEditObject; }
            protected set
            {
                if (_canEditObject != value)
                {
                    _canEditObject = value;
                    NotifyOfPropertyChange("CanEditObject");
                }
            }
        }

        private bool _canDeleteObject;

        /// <summary>
        /// Gets a value indicating whether the current
        /// user is authorized to delete
        /// a Model.
        /// </summary>
        public virtual bool CanDeleteObject
        {
            get { return _canDeleteObject; }
            protected set
            {
                if (_canDeleteObject != value)
                {
                    _canDeleteObject = value;
                    NotifyOfPropertyChange("CanDeleteObject");
                }
            }
        }

        /// <summary>
        /// This method is only called from constuctor to set default values immediately.
        /// Sets the properties at object level.
        /// </summary>
        private void SetPropertiesAtObjectLevel()
        {
            Type sourceType = typeof(T);

            CanCreateObject = BusinessRules.HasPermission(AuthorizationActions.CreateObject, sourceType);
            CanGetObject = BusinessRules.HasPermission(AuthorizationActions.GetObject, sourceType);
            CanEditObject = BusinessRules.HasPermission(AuthorizationActions.EditObject, sourceType);
            CanDeleteObject = BusinessRules.HasPermission(AuthorizationActions.DeleteObject, sourceType);

            // call SetProperties to set "instance" values
            OnSetProperties();
        }

        #endregion

        #region Verbs

#if !SILVERLIGHT

        /// <summary>
        /// Creates or retrieves a new instance of the
        /// Model by invoking a static factory method.
        /// </summary>
        /// <param name="factoryMethod">Static factory method function.</param>
        /// <example>DoRefresh(BusinessList.GetList)</example>
        /// <example>DoRefresh(() => BusinessList.GetList())</example>
        /// <example>DoRefresh(() => BusinessList.GetList(id))</example>
        protected virtual void DoRefresh(Func<T> factoryMethod)
        {
            if (typeof(T) != null)
            {
                Error = null;
                try
                {
                    Model = factoryMethod.Invoke();
                }
                catch (Exception ex)
                {
                    Error = ex;
                }
                OnRefreshed();
            }
        }

        /// <summary>
        /// Creates or retrieves a new instance of the
        /// Model by invoking a static factory method.
        /// </summary>
        /// <param name="factoryMethod">Name of the static factory method.</param>
        /// <param name="factoryParameters">Factory method parameters.</param>
        protected virtual void DoRefresh(string factoryMethod, params object[] factoryParameters)
        {
            if (typeof(T) != null)
            {
                Error = null;
                try
                {
                    Model = (T)MethodCaller.CallFactoryMethod(typeof(T), factoryMethod, factoryParameters);
                }
                catch (Exception ex)
                {
                    Error = ex;
                }
                OnRefreshed();
            }
        }

        /// <summary>
        /// Creates or retrieves a new instance of the
        /// Model by invoking a static factory method.
        /// </summary>
        /// <param name="factoryMethod">Name of the static factory method.</param>
        protected virtual void DoRefresh(string factoryMethod)
        {
            DoRefresh(factoryMethod, new object[] { });
        }

#endif

        /// <summary>
        /// Creates or retrieves a new instance of the
        /// Model by invoking a static factory method.
        /// </summary>
        /// <param name="factoryMethod">Static factory method action.</param>
        /// <example>BeginRefresh(BusinessList.BeginGetList)</example>
        /// <example>BeginRefresh(handler => BusinessList.BeginGetList(handler))</example>
        /// <example>BeginRefresh(handler => BusinessList.BeginGetList(id, handler))</example>
        protected virtual void BeginRefresh(Action<EventHandler<DataPortalResult<T>>> factoryMethod)
        {
            if (typeof(T) != null)
                try
                {
                    Error = null;
                    IsBusy = true;

                    var handler = (EventHandler<DataPortalResult<T>>)CreateHandler(typeof(T));
                    factoryMethod(handler);
                }
                catch (Exception ex)
                {
                    Error = ex;
                    IsBusy = false;
                }
        }

        /// <summary>
        /// Creates or retrieves a new instance of the
        /// Model by invoking a static factory method.
        /// </summary>
        /// <param name="factoryMethod">Name of the static factory method.</param>
        /// <param name="factoryParameters">Factory method parameters.</param>
        protected virtual void BeginRefresh(string factoryMethod, params object[] factoryParameters)
        {
            if (typeof(T) != null)
                try
                {
                    Error = null;
                    IsBusy = true;
                    var parameters = new List<object>(factoryParameters);
                    parameters.Add(CreateHandler(typeof(T)));

                    MethodCaller.CallFactoryMethod(typeof(T), factoryMethod, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    Error = ex;
                    IsBusy = false;
                }
        }

        /// <summary>
        /// Creates or retrieves a new instance of the
        /// Model by invoking a static factory method.
        /// </summary>
        /// <param name="factoryMethod">Name of the static factory method.</param>
        protected virtual void BeginRefresh(string factoryMethod)
        {
            BeginRefresh(factoryMethod, new object[] { });
        }

        private Delegate CreateHandler(Type objectType)
        {
            System.Reflection.MethodInfo method = MethodCaller.GetNonPublicMethod(GetType(), "QueryCompleted");
            var innerType = typeof(DataPortalResult<>).MakeGenericType(objectType);
            var args = typeof(EventHandler<>).MakeGenericType(innerType);

#if NETFX_CORE
            var target = Expression.Constant(this);
            var p1 = new ParameterExpression[] { Expression.Parameter(typeof(object), "sender"), Expression.Parameter(typeof(EventArgs), "args") };
            var call = Expression.Call(target, method, p1);
            var lambda = Expression.Lambda(args, call, "QueryCompleted", p1);
            var handler = lambda.Compile();
#else
            Delegate handler = Delegate.CreateDelegate(args, this, method);
#endif
            return handler;
        }

        private void QueryCompleted(object sender, EventArgs e)
        {
            try
            {
                var eventArgs = (IDataPortalResult)e;
                if (eventArgs.Error == null)
                {
                    var model = (T)eventArgs.Object;
                    OnRefreshing(model);
                    Model = model;
                }
                else
                    Error = eventArgs.Error;
                OnRefreshed();
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Method called after a refresh operation
        /// has completed and before the model is updated
        /// (when successful).
        /// </summary>
        /// <param name="model">The model.</param>
        protected virtual void OnRefreshing(T model)
        { }

        /// <summary>
        /// Method called after a refresh operation
        /// has completed (whether successful or
        /// not).
        /// </summary>
        protected virtual void OnRefreshed()
        { }

#if !SILVERLIGHT

        /// <summary>
        /// Saves the Model, first committing changes
        /// if ManagedObjectLifetime is true.
        /// </summary>
        protected virtual T DoSave()
        {
            T result = (T)Model;
            Error = null;
            try
            {
                var savable = Model as ISavable;
                if (ManageObjectLifetime)
                {
                    // clone the object if possible
                    ICloneable clonable = Model as ICloneable;
                    if (clonable != null)
                        savable = (ISavable)clonable.Clone();

                    //apply changes
                    var undoable = savable as ISupportUndo;
                    if (undoable != null)
                        undoable.ApplyEdit();
                }

                result = (T)savable.Save();

                Model = result;
                OnSaved();
            }
            catch (Exception ex)
            {
                Error = ex;
                OnSaved();
            }
            return result;
        }

#endif

        /// <summary>
        /// Saves the Model, first committing changes
        /// if ManagedObjectLifetime is true.
        /// </summary>
        protected virtual async System.Threading.Tasks.Task<T> SaveAsync()
        {
          try
          {
            var savable = Model as ISavable;
            if (ManageObjectLifetime)
            {
              // clone the object if possible
              ICloneable clonable = Model as ICloneable;
              if (clonable != null)
                savable = (ISavable)clonable.Clone();

              //apply changes
              var undoable = savable as ISupportUndo;
              if (undoable != null)
                undoable.ApplyEdit();
            }

            Error = null;
            IsBusy = true;
            OnSaving(Model);
            Model = (T)await savable.SaveAsync();
            IsBusy = false;
            OnSaved();
          }
          catch (Exception ex)
          {
            IsBusy = false;
            Error = ex;
            OnSaved();
          }
          return Model;
        }

        /// <summary>
        /// Saves the Model, first committing changes
        /// if ManagedObjectLifetime is true.
        /// </summary>
        protected virtual void BeginSave()
        {
            try
            {
                var savable = Model as ISavable;
                if (ManageObjectLifetime)
                {
                    // clone the object if possible
                    ICloneable clonable = Model as ICloneable;
                    if (clonable != null)
                        savable = (ISavable)clonable.Clone();

                    //apply changes
                    var undoable = savable as ISupportUndo;
                    if (undoable != null)
                        undoable.ApplyEdit();
                }

                savable.Saved += (o, e) =>
                {
                    IsBusy = false;
                    if (e.Error == null)
                    {
                        var result = e.NewObject;
                        var model = (T)result;
                        OnSaving(model);
                        Model = model;
                    }
                    else
                    {
                        Error = e.Error;
                    }
                    OnSaved();
                };
                Error = null;
                IsBusy = true;
                savable.BeginSave();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Error = ex;
                OnSaved();
            }
        }

        /// <summary>
        /// Method called after a save operation
        /// has completed and before Model is updated
        /// (when successful).
        /// </summary>
        protected virtual void OnSaving(T model)
        { }

        /// <summary>
        /// Method called after a save operation
        /// has completed (whether successful or
        /// not).
        /// </summary>
        protected virtual void OnSaved()
        { }

        /// <summary>
        /// Cancels changes made to the model
        /// if ManagedObjectLifetime is true.
        /// </summary>
        protected virtual void DoCancel()
        {
          if (ManageObjectLifetime)
          {
            var undo = Model as ISupportUndo;
            if (undo != null)
            {
              UnhookChangedEvents(Model);
              try
              {
                undo.CancelEdit();
                undo.BeginEdit();
              }
              finally
              {
                HookChangedEvents(Model);
                OnSetProperties();
              }
            }
          }
        }

#if SILVERLIGHT

        /// <summary>
        /// Adds a new item to the Model (if it
        /// is a collection).
        /// </summary>
        protected virtual void BeginAddNew()
        {
            // In SL (for Csla 4.0.x) it will always be an IBindingList
            var ibl = (Model as IBindingList);
            if (ibl != null)
            {
                ibl.AddNew();
            }
            else
            {
                // else try to use as IObservableBindingList
                var iobl = ((IObservableBindingList)Model);
                iobl.AddNew();
            }
            OnSetProperties();
        }

#else

        /// <summary>
        /// Adds a new item to the Model (if it
        /// is a collection).
        /// </summary>
        protected virtual object DoAddNew()
        {
            object result;
            // typically use ObserableCollection
            var iobl = (Model as IObservableBindingList);
            if (iobl != null)
            {
                result = iobl.AddNew();
            }
            else
            {
                // else try to use as BindingList
                var ibl = ((IBindingList)Model);
                result = ibl.AddNew();
            }
            OnSetProperties();
            return result;
        }

#endif

        /// <summary>
        /// Removes an item from the Model (if it
        /// is a collection).
        /// </summary>
        protected virtual void DoRemove(object item)
        {
            ((IList)Model).Remove(item);
            OnSetProperties();
        }

        /// <summary>
        /// Marks the Model for deletion (if it is an
        /// editable root object).
        /// </summary>
        protected virtual void DoDelete()
        {
            ((IEditableBusinessObject)Model).Delete();
        }

        #endregion

        #region Model Changes Handling

        /// <summary>
        /// Invoked when the Model changes, allowing
        /// event handlers to be unhooked from the old
        /// object and hooked on the new object.
        /// </summary>
        /// <param name="oldValue">Previous Model reference.</param>
        /// <param name="newValue">New Model reference.</param>
        protected virtual void OnModelChanged(T oldValue, T newValue)
        {
          if (ReferenceEquals(oldValue, newValue)) return;

          // unhook events from old value
          if (oldValue != null)
          {
            UnhookChangedEvents(oldValue);

            var nb = oldValue as INotifyBusy;
            if (nb != null)
              nb.BusyChanged -= Model_BusyChanged;
          }

          // hook events on new value
          if (newValue != null)
          {
            HookChangedEvents(newValue);

            var nb = newValue as INotifyBusy;
            if (nb != null)
              nb.BusyChanged += Model_BusyChanged;
          }

          OnSetProperties();
        }

        private void UnhookChangedEvents(T model)
        {
          var npc = model as INotifyPropertyChanged;
          if (npc != null)
            npc.PropertyChanged -= Model_PropertyChanged;

          var ncc = model as INotifyChildChanged;
          if (ncc != null)
            ncc.ChildChanged -= Model_ChildChanged;

          var cc = model as INotifyCollectionChanged;
          if (cc != null)
            cc.CollectionChanged -= Model_CollectionChanged;
        }

        private void HookChangedEvents(T model)
        {
          var npc = model as INotifyPropertyChanged;
          if (npc != null)
            npc.PropertyChanged += Model_PropertyChanged;

          var ncc = model as INotifyChildChanged;
          if (ncc != null)
            ncc.ChildChanged += Model_ChildChanged;

          var cc = model as INotifyCollectionChanged;
          if (cc != null)
            cc.CollectionChanged += Model_CollectionChanged;
        }

        /// <summary>
        /// Override this method to hook into to logic of setting properties when model is changed or edited.
        /// </summary>
        protected virtual void OnSetProperties()
        {
            SetProperties();
        }

        private void Model_BusyChanged(object sender, BusyChangedEventArgs e)
        {
            // only set busy state for entire object.  Ignore busy state based
            // on asynch rules being active
            if (e.PropertyName == string.Empty)
                IsBusy = e.Busy;
            else
                OnSetProperties();
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnSetProperties();
        }

        private void Model_ChildChanged(object sender, ChildChangedEventArgs e)
        {
            OnSetProperties();
        }

        private void Model_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnSetProperties();
        }

        #endregion

        #endregion

        #region Implementation of IHaveModel

        object IHaveModel.Model
        {
            get { return Model; }
            set { Model = (T)value; }
        }

        #endregion

        #region Implementation of INotifyPropertyChangedEx (PropertyChangedBase)

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
#if !SILVERLIGHT && !NETFX_CORE
        [field: NonSerialized]
#endif
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

#if !SILVERLIGHT && !NETFX_CORE
        [field: NonSerialized]
#endif
        private bool isNotifying; //serializator try to serialize even autogenerated fields

        /// <summary>
        /// Enables/Disables property change notification.
        /// </summary>
#if !NETFX_CORE
        [Browsable(false)]
#endif
        public bool IsNotifying
        {
            get { return isNotifying; }
            set { isNotifying = value; }
        }

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        public void Refresh()
        {
            NotifyOfPropertyChange(string.Empty);
        }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
#if NETFX_CORE || NET45
        public virtual void NotifyOfPropertyChange([CallerMemberName]string propertyName = "")
#else
        public virtual void NotifyOfPropertyChange(string propertyName)
#endif
        {
            if (IsNotifying)
            {
                Execute.OnUIThread(() => OnPropertyChanged(new PropertyChangedEventArgs(propertyName)));
            }
        }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The property expression.</param>
        public virtual void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            NotifyOfPropertyChange(property.GetMemberInfo().Name);
        }

        /// <summary>
        /// Raises the property changed event immediately.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [Obsolete("Use NotifyOfPropertyChange instead.")]
        public virtual void RaisePropertyChangedEventImmediately(string propertyName)
        {
            NotifyOfPropertyChange(propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Called when the object is deserialized.
        /// </summary>
        /// <param name="c">The streaming context.</param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext c)
        {
            IsNotifying = true;
        }

        /// <summary>
        /// Used to indicate whether or not the IsNotifying property is serialized to Xml.
        /// </summary>
        /// <returns>Whether or not to serialize the IsNotifying property. The default is false.</returns>
        public virtual bool ShouldSerializeIsNotifying()
        {
            return false;
        }

        #endregion

        private static readonly ILog Log = LogManager.GetLog(typeof(Screen)); // Screen

        private bool isActive; // Screen
        private bool isInitialized; // Screen
        private object parent; // Screen
        private string displayName; // Screen

        /// <summary>
        /// The default view context.
        /// </summary>
        public static readonly object DefaultContext = new object();

        #region Implementation of IChild (Screen)

        /// <summary>
        /// Gets or Sets the Parent <see cref="IConductor"/>
        /// </summary>
        public virtual object Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                NotifyOfPropertyChange("Parent");
            }
        }

        #endregion

        #region Implementation of IHaveDisplayName (Screen)

        /// <summary>
        /// Gets or Sets the Display Name
        /// </summary>
        public virtual string DisplayName
        {
            get { return displayName; }
            set
            {
                displayName = value;
                NotifyOfPropertyChange("DisplayName");
            }
        }

        #endregion

        #region Implementation of IActivate (Screen)

        /// <summary>
        /// Indicates whether or not this instance is currently active.
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            private set
            {
                isActive = value;
                NotifyOfPropertyChange("IsActive");
            }
        }

        /// <summary>
        /// Indicates whether or not this instance is currently initialized.
        /// </summary>
        public bool IsInitialized
        {
            get { return isInitialized; }
            private set
            {
                isInitialized = value;
                NotifyOfPropertyChange("IsInitialized");
            }
        }

        /// <summary>
        /// Raised after activation occurs.
        /// </summary>
        public event EventHandler<ActivationEventArgs> Activated = delegate { };

        /// <summary>
        /// Activates this instance.
        /// </summary>
        void IActivate.Activate()
        {
            if (IsActive)
            {
                return;
            }

            var initialized = false;

            if (!IsInitialized)
            {
                IsInitialized = initialized = true;
                OnInitialize();
            }

            IsActive = true;
            Log.Info("Activating {0}.", this);
            OnActivate();

            Activated(this, new ActivationEventArgs
            {
                WasInitialized = initialized
            });
        }

        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when activating.
        /// </summary>
        protected virtual void OnActivate() { }

        #endregion

        #region Implementation of IDeactivate (Screen)

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        /// <param name="close">Indicates whether or not this instance is being closed.</param>
        void IDeactivate.Deactivate(bool close)
        {
            if (IsActive || (IsInitialized && close))
            {
                AttemptingDeactivation(this, new DeactivationEventArgs
                {
                    WasClosed = close
                });

                IsActive = false;
                Log.Info("Deactivating {0}.", this);
                OnDeactivate(close);

                Deactivated(this, new DeactivationEventArgs
                {
                    WasClosed = close
                });

                if (close)
                {
                    Views.Clear();
                    Log.Info("Closed {0}.", this);
                }
            }
        }

        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name="close">Inidicates whether this instance will be closed.</param>
        protected virtual void OnDeactivate(bool close) { }

        /// <summary>
        /// Raised before deactivation.
        /// </summary>
        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation = delegate { };

        /// <summary>
        /// Raised after deactivation.
        /// </summary>
        public event EventHandler<DeactivationEventArgs> Deactivated = delegate { };

        #endregion

        #region Implementation of IGuardClose (Screen)

        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <param name="callback">The implementor calls this action with the result of the close check.</param>
        public virtual void CanClose(Action<bool> callback)
        {
            callback(true);
        }

        #endregion

        #region Implementation of IClose (Screen)

        /// <summary>
        /// Tries to close this instance by asking its Parent to initiate shutdown or by asking its corresponding view to close.
        /// Also provides an opportunity to pass a dialog result to it's corresponding view.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        public virtual void TryClose(bool? dialogResult = null)
        {
            PlatformProvider.Current.GetViewCloseAction(this, Views.Values, dialogResult).OnUIThread();
        }

        #endregion

        #region Implementation of IViewAware (ViewAware)

        private static readonly DependencyProperty PreviouslyAttachedProperty = DependencyProperty.RegisterAttached(
            "PreviouslyAttached",
            typeof(bool),
            typeof(ScreenWithModelBase<T>),
            null
            );

        /// <summary>
        /// Indicates whether or not implementors of <see cref="IViewAware"/> should cache their views by default.
        /// </summary>
        public static bool CacheViewsByDefault = true;


        /// <summary>
        /// The view cache for this instance.
        /// </summary>
        /// 
        readonly IDictionary<object, object> views;
        protected IDictionary<object, object> Views
        {
            get { return views; }
        }

        /// <summary>
        /// Raised when a view is attached.
        /// </summary>
        public event EventHandler<ViewAttachedEventArgs> ViewAttached = delegate { };


        /// <summary>
        /// Attaches a view to this instance.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="context">The context in which the view appears.</param>
        void IViewAware.AttachView(object view, object context)
        {
            Views[context ?? DefaultContext] = view;

            var nonGeneratedView = PlatformProvider.Current.GetFirstNonGeneratedView(view);
            PlatformProvider.Current.ExecuteOnFirstLoad(nonGeneratedView, OnViewLoaded);
            OnViewAttached(nonGeneratedView, context);
            ViewAttached(this, new ViewAttachedEventArgs { View = nonGeneratedView, Context = context });

            var activatable = this as IActivate;
            if (activatable == null || activatable.IsActive)
            {
                PlatformProvider.Current.ExecuteOnLayoutUpdated(nonGeneratedView, OnViewReady);
            }
            else
            {
                AttachViewReadyOnActivated(activatable, nonGeneratedView);
            }
        }

        static void AttachViewReadyOnActivated(IActivate activatable, object nonGeneratedView)
        {
            var viewReference = new WeakReference(nonGeneratedView);
            EventHandler<ActivationEventArgs> handler = null;
            handler = (s, e) =>
            {
                ((IActivate)s).Activated -= handler;
                var view = viewReference.Target;
                if (view != null)
                {
                    PlatformProvider.Current.ExecuteOnLayoutUpdated(view, ((ScreenWithModelBase<T>)s).OnViewReady);
                }
            };
            activatable.Activated += handler;
        }

        /// <summary>
        /// Called when a view is attached.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="context">The context in which the view appears.</param>
        protected virtual void OnViewAttached(object view, object context) { }

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        /// <param name="view"></param>
        protected virtual void OnViewLoaded(object view) { }

        /// <summary>
        /// Called the first time the page's LayoutUpdated event fires after it is navigated to.
        /// </summary>
        /// <param name = "view"></param>
        protected virtual void OnViewReady(object view){ }

        /// <summary>
        /// Gets a view previously attached to this instance.
        /// </summary>
        /// <param name="context">The context denoting which view to retrieve.</param>
        /// <returns>The view.</returns>
        public virtual object GetView(object context = null)
        {
            object view;
            Views.TryGetValue(context ?? DefaultContext, out view);
            return view;
        }

        #endregion

    }
}
