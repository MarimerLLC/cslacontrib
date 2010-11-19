using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;
using OutlookStyle.Infrastructure.ViewToRegionBinding;

namespace OutlookStyle.Infrastructure.ViewToRegionBinding
{
    /// <summary>
    /// This class can bind an instance of a view to the name of a region. When the <see cref="ObjectToMonitor"/> becomes active
    /// it will add all registered view instances to the right regions. When it becomes in active, it will remove them again. 
    /// </summary>
    public class ViewToRegionBinder : IViewToRegionBinder
    {
        private readonly IRegionManager regionManager;
        private readonly IActiveAware activeAwareObservable;
        private List<ViewRegionBinding> bindings;
        private IActiveAware objectToMonitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewToRegionBinder"/> class.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        public ViewToRegionBinder(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            this.bindings = new List<ViewRegionBinding>();
        }

        /// <summary>
        /// Adds a binding between a view and the name of a region.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <param name="view">The view.</param>
        public void Add(string regionName, object view)
        {
            this.bindings.Add(new ViewRegionBinding()
                                  {
                                      RegionName = regionName
                                      , View = view
                                  });
        }

        /// <summary>
        /// Gets or sets the object to monitor for ISActive changes.
        /// </summary>
        /// <value>The object to monitor.</value>
        public IActiveAware ObjectToMonitor
        {
            get { return objectToMonitor; }
            set
            {
                if (objectToMonitor == value)
                    return;

                if (objectToMonitor != null)
                {
                    if (objectToMonitor.IsActive)
                    {
                        // Remove any views that were added
                        AddOrRemoveViews(false);
                    }
                    objectToMonitor.IsActiveChanged -= activeAware_IsActiveChanged;
                }

                objectToMonitor = value;

                if (objectToMonitor != null)
                {
                    objectToMonitor.IsActiveChanged += activeAware_IsActiveChanged;
                    if (objectToMonitor.IsActive)
                    {
                        AddOrRemoveViews(true);
                    }
                }
            }
        }

        void activeAware_IsActiveChanged(object sender, EventArgs e)
        {
            AddOrRemoveViews(this.ObjectToMonitor.IsActive);
        }

        private void AddOrRemoveViews(bool isActive)
        {
            foreach(var binding in this.bindings)
            {
                IRegion region = regionManager.Regions[binding.RegionName];
                if (isActive)
                {
                    region.Add(binding.View);
                }
                else
                {
                    region.Remove(binding.View);
                }
            }
        }

        /// <summary>
        /// The registered bindings.
        /// </summary>
        /// <value></value>
        public IEnumerable<ViewRegionBinding> Bindings
        {
            get
            {
                return bindings;
            }
        }
    }
}