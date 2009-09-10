using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation;
using OutlookStyle.Infrastructure.RegionContext;

namespace OutlookStyle.Infrastructure.ModelVisualization
{
    /// <summary>
    /// Class that can visualize a specified model using the view provided. 
    /// The Model will be placed as the datacontext for the View. Also, the IActiveAware and IRegionContextAware
    /// are forwarded to the view and the viewmodel. 
    /// 
    /// this is similar to the ViewViewModelWrapper, only this class doesn't use generics and is used internally in the 
    /// <see cref="VisualizingRegion"/>. 
    /// </summary>
    public class ModelVisualizer : ContentControl, IModelVisualizer
    {
        public ModelVisualizer(object viewModel, FrameworkElement view)
        {
            this.View = view;
            this.ViewModel = viewModel;

            this.RegionContext = new ObservableObject<object>();
            this.RegionContext.PropertyChanged += RegionContext_PropertyChanged;

            this.DataContext = this.ViewModel;
            VisualizeView();

            RegisterIActiveAwareChanged(this.View as IActiveAware);
            RegisterIActiveAwareChanged(this.ViewModel as IActiveAware);
            RegisterRegionContextChanged(this.View as IRegionContextAware);
            RegisterRegionContextChanged(this.ViewModel as IRegionContextAware);
        }

        private void VisualizeView()
        {
            if (!(this.View is Window))
            {
                this.Content = this.View;
            }
        }

        public FrameworkElement View
        {
            get; private set;
        }

        public object ViewModel
        {
            get; private set;
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
            get;
            private set;
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

        private void RegionContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ForwardRegionContext(this.View as IRegionContextAware, RegionContext.Value);
            ForwardRegionContext(this.ViewModel as IRegionContextAware, RegionContext.Value);
        }

    }
}