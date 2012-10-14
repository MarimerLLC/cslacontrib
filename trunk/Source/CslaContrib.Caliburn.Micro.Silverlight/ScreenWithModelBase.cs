using Caliburn.Micro;

namespace CslaContrib.Caliburn.Micro
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Windows;
    using Csla;
    using Csla.Core;
    using Csla.Reflection;

    /// <summary>
    /// Base class used to create ScreenWithModel objects that
    /// implement their own commands/verbs/actions.
    /// </summary>
    /// <typeparam name="T">Type of the Model object.</typeparam>
    public abstract class ScreenWithModelBase<T> : DependencyObject, IScreen, IChild, IViewAware, IHaveModel
    {
        #region Constructor

        /// <summary>
        /// Create new instance of base class used to create ScreenWithModel objects that
        /// implement their own commands/verbs/actions.
        /// </summary>
        public ScreenWithModelBase()
        {
#if WINDOWS_PHONE
            ManageObjectLifetime = false;
#endif
            SetPropertiesAtObjectLevel();
            IsNotifying = true;
            DisplayName = GetType().FullName;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Model object.
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(T), typeof(ScreenWithModelBase<T>),
            new PropertyMetadata((o, e) =>
            {
                var screenWithModel = (ScreenWithModelBase<T>)o;
                screenWithModel.OnModelChanged((T)e.OldValue, (T)e.NewValue);
                if (screenWithModel.ManageObjectLifetime)
                {
                    var undo = e.NewValue as ISupportUndo;
                    if (undo != null)
                        undo.BeginEdit();
                }
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
                SetProperties();
            }
        }

        #endregion

        #region Can___ properties

        private bool _canSave;

        /// <summary>
        /// Gets a value indicating whether the Model
        /// can currently be saved.
        /// </summary>
        public bool CanSave
        {
            get
            {
                return _canSave;
            }
            private set
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
        public bool CanCancel
        {
            get
            {
                return _canCancel;
            }
            private set
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
        public bool CanCreate
        {
            get
            {
                return _canCreate;
            }
            private set
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
        public bool CanDelete
        {
            get
            {
                return _canDelete;
            }
            private set
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
        public bool CanFetch
        {
            get
            {
                return _canFetch;
            }
            private set
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
        public bool CanRemove
        {
            get
            {
                return _canRemove;
            }
            private set
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
        public bool CanAddNew
        {
            get
            {
                return _canAddNew;
            }
            private set
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
                CanSave = CanEditObject && targetObject.IsSavable && !isObjectBusy;
                CanCancel = CanEditObject && targetObject.IsDirty && !isObjectBusy;
                CanCreate = CanCreateObject && !targetObject.IsDirty && !isObjectBusy;
                CanDelete = CanDeleteObject && !isObjectBusy;
                CanFetch = CanGetObject && !targetObject.IsDirty && !isObjectBusy;

                // Set properties for List 
                if (list == null)
                {
                    CanRemove = false;
                    CanAddNew = false;
                }
                else
                {
                    Type itemType = Utilities.GetChildItemType(Model.GetType());
                    if (itemType == null)
                    {
                        CanAddNew = false;
                        CanRemove = false;
                    }
                    else
                    {
                        CanRemove = Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.DeleteObject, itemType) &&
                                    list.Count > 0 && !isObjectBusy;

                        CanAddNew = Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.CreateObject, itemType) &&
                                    !isObjectBusy;
                    }
                }
            }

            // Else if Model instance implement ICollection
            else if (list != null)
            {
                Type itemType = Utilities.GetChildItemType(Model.GetType());
                if (itemType == null)
                {
                    CanAddNew = false;
                    CanRemove = false;
                }
                else
                {
                    CanRemove = Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.DeleteObject, itemType) &&
                                list.Count > 0 && !isObjectBusy;

                    CanAddNew = Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.CreateObject, itemType) &&
                                !isObjectBusy;
                }
            }
            else
            {
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
        public bool CanCreateObject
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
        public bool CanGetObject
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
        public bool CanEditObject
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
        public bool CanDeleteObject
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

            CanCreateObject = Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.CreateObject, sourceType);
            CanGetObject = Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.GetObject, sourceType);
            CanEditObject = Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.EditObject, sourceType);
            CanDeleteObject = Csla.Rules.BusinessRules.HasPermission(Csla.Rules.AuthorizationActions.DeleteObject, sourceType);

            // call SetProperties to set "instance" values 
            SetProperties();
        }

        #endregion

        #region Verbs

#if !SILVERLIGHT
        /// <summary>
        ///  Creates or retrieves a new instance of the 
        ///  Model by invoking a static factory method.
        ///  </summary>
        ///  <param name="factoryMethod">Static factory method function.</param>
        ///  <example>DoRefresh(BusinessList.GetList)</example>
        ///  <example>DoRefresh(() => BusinessList.GetList())</example>
        ///  <example>DoRefresh(() => BusinessList.GetList(id))</example>
        protected void DoRefresh(Func<T> factoryMethod)
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
        protected void BeginRefresh(Action<EventHandler<DataPortalResult<T>>> factoryMethod)
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
            var args = typeof(DataPortalResult<>).MakeGenericType(objectType);
            System.Reflection.MethodInfo method = MethodCaller.GetNonPublicMethod(GetType(), "QueryCompleted");
            Delegate handler = Delegate.CreateDelegate(typeof(EventHandler<>).MakeGenericType(args), this, method);
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
                    undo.CancelEdit();
                    undo.BeginEdit();
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
            SetProperties();
        }
#else
        /// <summary>
        /// Adds a new item to the Model (if it
        /// is a collection).
        /// </summary>
        protected virtual object DoAddNew()
        {
            object result = null;
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
            SetProperties();
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
            SetProperties();
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
                var npc = oldValue as INotifyPropertyChanged;
                if (npc != null)
                    npc.PropertyChanged -= Model_PropertyChanged;
                var ncc = oldValue as INotifyChildChanged;
                if (ncc != null)
                    ncc.ChildChanged -= Model_ChildChanged;
                var nb = oldValue as INotifyBusy;
                if (nb != null)
                    nb.BusyChanged -= Model_BusyChanged;
                var cc = oldValue as INotifyCollectionChanged;
                if (cc != null)
                    cc.CollectionChanged -= Model_CollectionChanged;
            }

            // hook events on new value
            if (newValue != null)
            {
                var npc = newValue as INotifyPropertyChanged;
                if (npc != null)
                    npc.PropertyChanged += Model_PropertyChanged;
                var ncc = newValue as INotifyChildChanged;
                if (ncc != null)
                    ncc.ChildChanged += Model_ChildChanged;
                var nb = newValue as INotifyBusy;
                if (nb != null)
                    nb.BusyChanged += Model_BusyChanged;
                var cc = newValue as INotifyCollectionChanged;
                if (cc != null)
                    cc.CollectionChanged += Model_CollectionChanged;
            }

            SetProperties();
        }


        private void Model_BusyChanged(object sender, BusyChangedEventArgs e)
        {
            // only set busy state for entire object.  Ignore busy state based
            // on asynch rules being active
            if (e.PropertyName == string.Empty)
                IsBusy = e.Busy;
            else
                SetProperties();
        }


        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetProperties();
        }

        private void Model_ChildChanged(object sender, ChildChangedEventArgs e)
        {
            SetProperties();
        }

        private void Model_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetProperties();
        }

        #endregion

        #region IHaveModel Members

        object IHaveModel.Model
        {
            get { return Model; }
            set { Model = (T)value; }
        }

        #endregion

        #region Implementation of INotifyPropertyChangedEx

#if !SILVERLIGHT
        [field: NonSerialized]
#endif
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Enables/Disables property change notification.
        /// </summary>
        public bool IsNotifying { get; set; }

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
        public virtual void NotifyOfPropertyChange(string propertyName)
        {
            if (IsNotifying)
                Execute.OnUIThread(() => RaisePropertyChangedEventCore(propertyName));
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
        public virtual void RaisePropertyChangedEventImmediately(string propertyName)
        {
            if (IsNotifying)
                RaisePropertyChangedEventCore(propertyName);
        }

        void RaisePropertyChangedEventCore(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected static readonly ILog Log = LogManager.GetLog(typeof(Screen));

        bool isActive;
        bool isInitialized;
        object parent;
        string displayName;
        readonly Dictionary<object, object> views = new Dictionary<object, object>();

        #region Implementation of IHaveDisplayName

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

        #region Implementation of IActivate

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

        void IActivate.Activate()
        {
            if (IsActive)
                return;

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

        #region Implementation of IDeactivate

        /// <summary>
        /// Raised before deactivation.
        /// </summary>
        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation = delegate { };

        void IDeactivate.Deactivate(bool close)
        {
            if (!IsActive && !IsInitialized)
                return;

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
                views.Clear();
                Log.Info("Closed {0}.", this);
            }
        }

        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name="close">Inidicates whether this instance will be closed.</param>
        protected virtual void OnDeactivate(bool close) { }

        /// <summary>
        /// Raised after deactivation.
        /// </summary>
        public event EventHandler<DeactivationEventArgs> Deactivated = delegate { };

        #endregion

        #region Implementation of IClose

        /// <summary>
        /// Tries to close this instance by asking its Parent to initiate shutdown or by asking its corresponding view to close.
        /// </summary>
        public void TryClose()
        {
            Execute.OnUIThread(() =>
            {
                var closeAction = GetViewCloseAction(null);
                closeAction();
            });
        }

#if !SILVERLIGHT

        /// <summary>
        /// Closes this instance by asking its Parent to initiate shutdown or by asking it's corresponding view to close.
        /// This overload also provides an opportunity to pass a dialog result to it's corresponding view.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        public virtual void TryClose(bool? dialogResult)
        {
            Execute.OnUIThread(() =>
            {
                var closeAction = GetViewCloseAction(dialogResult);
                closeAction();
            });
        }

#endif
        System.Action GetViewCloseAction(bool? dialogResult)
        {
            var conductor = Parent as IConductor;
            if (conductor != null)
                return () => conductor.CloseItem(this);

            foreach (var contextualView in views.Values)
            {
                var viewType = contextualView.GetType();

                var closeMethod = viewType.GetMethod("Close");
                if (closeMethod != null)
                    return () =>
                    {
#if !SILVERLIGHT
                        if (dialogResult != null)
                        {
                            var resultProperty = contextualView.GetType().GetProperty("DialogResult");
                            if (resultProperty != null)
                                resultProperty.SetValue(contextualView, dialogResult, null);
                        }
#endif

                        closeMethod.Invoke(contextualView, null);
                    };

                var isOpenProperty = viewType.GetProperty("IsOpen");
                if (isOpenProperty != null)
                    return () => isOpenProperty.SetValue(contextualView, false, null);
            }

            return () =>
            {
                var ex = new NotSupportedException("TryClose requires a parent IConductor or a view with a Close method or IsOpen property.");
                Log.Error(ex);
                throw ex;
            };
        }

        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <param name="callback">The implementor calls this action with the result of the close check.</param>
        public virtual void CanClose(Action<bool> callback)
        {
            callback(true);
        }

        #endregion

        #region Implementation of IChild

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

        #region Implementation of IViewAware

        /// <summary>
        /// Attaches a view to this instance.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="context">The context in which the view appears.</param>
        public virtual void AttachView(object view, object context)
        {
            var loadWired = views.Values.Contains(view);
            views[context ?? View.DefaultContext] = view;

            var element = view as FrameworkElement;
            if (!loadWired && element != null)
                element.Loaded += delegate { OnViewLoaded(view); };

            if (!loadWired)
                ViewAttached(this, new ViewAttachedEventArgs { View = view, Context = context });
        }

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        /// <param name="view"></param>
        protected virtual void OnViewLoaded(object view) { }

        /// <summary>
        /// Gets a view previously attached to this instance.
        /// </summary>
        /// <param name="context">The context denoting which view to retrieve.</param>
        /// <returns>The view.</returns>
        public virtual object GetView(object context)
        {
            object view;
            views.TryGetValue(context ?? View.DefaultContext, out view);
            return view;
        }

        /// <summary>
        /// Raised when a view is attached.
        /// </summary>
        public event EventHandler<ViewAttachedEventArgs> ViewAttached = delegate { };

        #endregion
    }
}
