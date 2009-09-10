using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation;
using Microsoft.Practices.ServiceLocation;
using OutlookStyle.Infrastructure.RegionContext;

namespace OutlookStyle.Infrastructure.ModelVisualization
{
    /// <summary>
    /// Wraps both a View and a View model and links the two together by setting the 
    /// datacontext of the view to the ViewModel. It also forwards
    /// RegionContext if the view or the model supports IRegionContextAware and it 
    /// forwards ISActive if the view or the model supports IActiveAware.
    /// 
    /// The view and the viewmodel are only created the fist time the View or ViewModel 
    /// properties are accessed. This allows for delayed creation of the view and viewmodel
    /// (IE, you can create this lightweight wrapper but the instances will only be created
    /// when they are needed. 
    /// </summary>
    /// <typeparam name="TView">The type of the view</typeparam>
    /// <typeparam name="TViewModel">the type of the viewmodel that will be set as the datacontext</typeparam>
    public class ViewViewModelWrapper<TView, TViewModel> : ContentControl, IModelVisualizer
        where TView:FrameworkElement
    {
        private readonly IServiceLocator serviceLocator;

        public ViewViewModelWrapper(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;

            this.RegionContext = new ObservableObject<object>();
            this.RegionContext.PropertyChanged += RegionContext_PropertyChanged;
        }

        protected virtual void Initialize()
        {
            this.View = serviceLocator.GetInstance<TView>() ;
            this.ViewModel = serviceLocator.GetInstance<TViewModel>();

            this.DataContext = this.ViewModel;
            this.Content = this.View;

            RegisterIActiveAwareChanged(this.View as IActiveAware);
            RegisterIActiveAwareChanged(this.ViewModel as IActiveAware);
            RegisterRegionContextChanged(this.View as IRegionContextAware);
            RegisterRegionContextChanged(this.ViewModel as IRegionContextAware);
        }

        private void RegionContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ForwardRegionContext(this.View as IRegionContextAware, RegionContext.Value);
            ForwardRegionContext(this.ViewModel as IRegionContextAware, RegionContext.Value);            
        }

        private TView view;

        public TView View
        {
            get
            {
                if (this.view == null)
                {
                    Initialize();
                }
                return view;
            }
            private set
            {
                view = value;
            }
        }

        private TViewModel viewModel;

        public TViewModel ViewModel
        {
            get
            {
                if (this.viewModel == null)
                {
                    Initialize();
                }
                return viewModel;
            }
            private set { viewModel = value; }
        }

        public event EventHandler IsActiveChanged;

        private void InvokeIsActiveChanged(EventArgs e)
        {
            EventHandler isActiveChangedHandler = IsActiveChanged;
            if (isActiveChangedHandler != null) isActiveChangedHandler(this, e);
        }

        private bool isActive;

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    ForwardActiveAware(this.View as IActiveAware, value);
                    ForwardActiveAware(this.ViewModel as IActiveAware, value);
                    InvokeIsActiveChanged(EventArgs.Empty);
                }
            }
        }

        private void ForwardActiveAware(IActiveAware activeAware, bool newValue)
        {
            if (activeAware == null)
                return;

            if (activeAware.IsActive != newValue)
            {
                activeAware.IsActive = newValue;
            }
        }

        private void RegisterIActiveAwareChanged(IActiveAware activeAware)
        {
            if (activeAware == null)
                return;

            activeAware.IsActiveChanged += delegate { this.IsActive = activeAware.IsActive; };
        }

        public event EventHandler<EventArgs> RegionContextChanged;

        private void InvokePayloadUpdated(EventArgs e)
        {
            EventHandler<EventArgs> payloadUpdatedHandler = RegionContextChanged;
            if (payloadUpdatedHandler != null) payloadUpdatedHandler(this, e);
        }

        public ObservableObject<object> RegionContext
        {
            get; private set;
        }

        private void ForwardRegionContext(IRegionContextAware regionContextAware, object payload)
        {
            if (regionContextAware == null)
                return;

            if (regionContextAware.RegionContext.Value != payload)
            {
                regionContextAware.RegionContext.Value = payload;
            }
        }

        private void RegisterRegionContextChanged(IRegionContextAware regionContextAware)
        {
            if (regionContextAware == null)
                return;

            regionContextAware.RegionContext.PropertyChanged += delegate
                                                                    {
                                                                        if (this.RegionContext.Value != regionContextAware.RegionContext.Value)
                                                                        {
                                                                            this.RegionContext.Value =
                                                                                regionContextAware.RegionContext.Value;
                                                                        }
                                                                    };
        }


        FrameworkElement IModelVisualizer.View
        {
            get { return this.View as FrameworkElement; }
        }

        object IModelVisualizer.ViewModel
        {
            get { return this.ViewModel; }
        }


    }

    public interface IModelVisualizer : IActiveAware, IRegionContextAware
    {
        FrameworkElement View
        { 
            get;
        }

        object ViewModel
        { 
            get;
        }
    }
}