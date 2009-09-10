using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation;
using Microsoft.Practices.Composite.Regions;
using OutlookStyle.Infrastructure.ViewToRegionBinding;

namespace OutlookStyle.Infrastructure.UseCase
{
    /// <summary>
    /// Baseclass that providees a default implementation of th IActiveAwareUseCaseController. 
    /// 
    /// This baseclass provides: 
    ///  - The ViewToRegionBinder to make it easy to add and remove views from regions when this usecase activates or deactivates. 
    ///  - the Implementaiton for IActiveAware
    ///  - A method for the first activation <see cref="OnFirstActivation"/>
    ///  - A way to set membervariables using objectfactories when this usecase first activates. <see cref="AddInitializationMethods"/>
    /// </summary>
    public abstract class ActiveAwareUseCaseController : IActiveAwareUseCaseController
    {
        
        private bool isActive;
        private bool initialized;
        private List<Action> onFirstActivationMethods = new List<Action>();

        protected ActiveAwareUseCaseController(IViewToRegionBinder viewToRegionBinder)
        {
            this.ViewToRegionBinder = viewToRegionBinder;
            this.ViewToRegionBinder.ObjectToMonitor = this;
        }

        /// <summary>
        /// Gets or sets the view to region binder, that will allow you to easily add or remove views from regions when this usecfase activates or deactivates.
        /// </summary>
        /// <value>The view to region binder.</value>
        protected IViewToRegionBinder ViewToRegionBinder { get; private set; }

        /// <summary>
        /// Override this method to implement your own activatoin logic
        /// </summary>
        protected virtual void OnActivate()
        {}

        /// <summary>
        /// Override this method to implement your own deactivation l ogic
        /// </summary>
        protected virtual void OnDeactivate()
        {}

        /// <summary>
        /// Event that notifies if the IsActive ahs changed
        /// </summary>
        public event EventHandler IsActiveChanged;

        private void InvokeIsActiveChanged(EventArgs e)
        {
            EventHandler isActiveChangedHandler = IsActiveChanged;
            if (isActiveChangedHandler != null) isActiveChangedHandler(this, e);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the object is active.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if the object is active; otherwise <see langword="false"/>.
        /// </value>
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                if (this.isActive == value)
                    return;

                this.isActive = value;

                CallInitAndActivation(value);


                this.InvokeIsActiveChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Calls the init and activation.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        protected virtual void CallInitAndActivation(bool value)
        {
            if (value)
            {
                if (!initialized)
                {
                    CallInitializationMethods();
                    OnFirstActivation();
                    initialized = true;
                }
                OnActivate();
            }
            else
            {
                OnDeactivate();
            }
        }

        /// <summary>
        /// Calls the initialization methods.
        /// </summary>
        protected virtual void CallInitializationMethods()
        {
            foreach (var action in onFirstActivationMethods)
                action();
        }

        /// <summary>
        /// Adds the initialization methods. this allows you to set membervariables to the result of ObjectFActory.CreateInstance() values.
        /// </summary>
        /// <example>
        /// () => this.myCustomObject = myCustomObjectFactory.CreateInstance();
        /// </example>
        /// <param name="methods">The methods.</param>
        protected virtual void AddInitializationMethods(params Action[] methods)
        {
            foreach(var method in methods)
            {
                this.onFirstActivationMethods.Add(method);
            }
        }


        /// <summary>
        /// Gets the name of the usecase.
        /// </summary>
        /// <value>The name.</value>
        public abstract string Name
        {
            get;
        }


        /// <summary>
        /// Override this method to provide your own 'first activation' logic. 
        /// </summary>
        protected virtual void OnFirstActivation()
        {
            
        }
    }
}