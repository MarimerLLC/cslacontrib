using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.ServiceLocation;
using OutlookStyle.Infrastructure.ModelVisualization;

namespace OutlookStyle.Infrastructure.ModelVisualization
{
    /// <summary>
    /// Class that wraps a region and will attemtp to visualize all the views that are placed into it. 
    /// 
    /// So if you add a viewmodel to the region, it will wrap it in a ModelVisualizer and find the registered
    /// visualization (viewtype) for it. This compensates for the lack of automatic datatemplates in silverlight. 
    /// </summary>
    public class VisualizingRegion : IRegion
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IModelVisualizationRegistry modelVisualizationRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualizingRegion"/> class.
        /// </summary>
        /// <param name="modelVisualizationRegistry">The model visualization registry.</param>
        public VisualizingRegion(IModelVisualizationRegistry modelVisualizationRegistry  )
        {
            this.modelVisualizationRegistry = modelVisualizationRegistry;
        }

        /// <summary>
        /// Gets or sets the inner region.
        /// </summary>
        /// <value>The inner region.</value>
        public IRegion InnerRegion { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Adds a new view to the region.
        /// </summary>
        /// <param name="view">The view to add.</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Practices.Composite.Regions.IRegionManager"/> that is set on the view if it is a <see cref="T:System.Windows.DependencyObject"/>. It will be the current region manager when using this overload.
        /// </returns>
        /// <overloads>Adds a new view to the region.</overloads>
        public IRegionManager Add(object view)
        {
            return InnerRegion.Add(this.modelVisualizationRegistry.CreateVisualization(view));
        }

        /// <summary>
        /// Adds a new view to the region.
        /// </summary>
        /// <param name="view">The view to add.</param>
        /// <param name="viewName">The name of the view. This can be used to retrieve it later by calling <see cref="M:Microsoft.Practices.Composite.Regions.IRegion.GetView(System.String)"/>.</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Practices.Composite.Regions.IRegionManager"/> that is set on the view if it is a <see cref="T:System.Windows.DependencyObject"/>. It will be the current region manager when using this overload.
        /// </returns>
        public IRegionManager Add(object view, string viewName)
        {
            return InnerRegion.Add(this.modelVisualizationRegistry.CreateVisualization(view), viewName);
        }

        /// <summary>
        /// Adds a new view to the region.
        /// </summary>
        /// <param name="view">The view to add.</param>
        /// <param name="viewName">The name of the view. This can be used to retrieve it later by calling <see cref="M:Microsoft.Practices.Composite.Regions.IRegion.GetView(System.String)"/>.</param>
        /// <param name="createRegionManagerScope">When <see langword="true"/>, the added view will receive a new instance of <see cref="T:Microsoft.Practices.Composite.Regions.IRegionManager"/>, otherwise it will use the current region manager for this region.</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Practices.Composite.Regions.IRegionManager"/> that is set on the view if it is a <see cref="T:System.Windows.DependencyObject"/>.
        /// </returns>
        public IRegionManager Add(object view, string viewName, bool createRegionManagerScope)
        {
            return InnerRegion.Add(this.modelVisualizationRegistry.CreateVisualization(view)
                                   , viewName, createRegionManagerScope);
        }

        /// <summary>
        /// Removes the specified view from the region.
        /// </summary>
        /// <param name="view">The view to remove.</param>
        public void Remove(object view)
        {
            var innerView = FindView(view);

            if (innerView == null)
            {
                throw new ArgumentException("View does not exist.");
            }

            InnerRegion.Remove(innerView);
        }

        /// <summary>
        /// Marks the specified view as active.
        /// </summary>
        /// <param name="view">The view to activate.</param>
        public void Activate(object view)
        {
            var innerView = FindView(view);

            if (innerView == null)
            {
                throw new ArgumentException("View does not exist.");
            }

            InnerRegion.Activate(innerView);
        }

        /// <summary>
        /// Marks the specified view as inactive.
        /// </summary>
        /// <param name="view">The view to deactivate.</param>
        public void Deactivate(object view)
        {
            var innerView = FindView(view);

            if (innerView == null)
            {
                throw new ArgumentException("View does not exist.");
            }

            InnerRegion.Deactivate(view);
        }

        public object GetView(string viewName)
        {
            return InnerRegion.GetView(viewName);
        }

        /// <summary>
        /// Gets a readonly view of the collection of views in the region.
        /// </summary>
        /// <value>
        /// An <see cref="T:Microsoft.Practices.Composite.Regions.IViewsCollection"/> of all the added views.
        /// </value>
        public IViewsCollection Views
        {
            get { return InnerRegion.Views; }
        }

        /// <summary>
        /// Gets a readonly view of the collection of all the active views in the region.
        /// </summary>
        /// <value>
        /// An <see cref="T:Microsoft.Practices.Composite.Regions.IViewsCollection"/> of all the active views.
        /// </value>
        public IViewsCollection ActiveViews
        {
            get { return InnerRegion.ActiveViews; }
        }

        /// <summary>
        /// Gets or sets a context for the region. This value can be used by the user to share context with the views.
        /// </summary>
        /// <value>The context value to be shared.</value>
        public object Context
        {
            get { return InnerRegion.Context; }
            set { InnerRegion.Context = value; }
        }

        /// <summary>
        /// Gets the name of the region that uniequely identifies the region within a <see cref="T:Microsoft.Practices.Composite.Regions.IRegionManager"/>.
        /// </summary>
        /// <value>The name of the region.</value>
        public string Name
        {
            get { return InnerRegion.Name; }
            set { InnerRegion.Name = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.Practices.Composite.Regions.IRegionManager"/> that will be passed to the views when adding them to the region, unless the view is added by specifying createRegionManagerScope as <see langword="true"/>.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.Practices.Composite.Regions.IRegionManager"/> where this <see cref="T:Microsoft.Practices.Composite.Regions.IRegion"/> is registered.
        /// </value>
        /// <remarks>This is usually used by implementations of <see cref="T:Microsoft.Practices.Composite.Regions.IRegionManager"/> and should not be
        /// used by the developer explicitely.</remarks>
        public IRegionManager RegionManager
        {
            get { return InnerRegion.RegionManager; }
            set { InnerRegion.RegionManager = value; }
        }

        /// <summary>
        /// Gets the collection of <see cref="T:Microsoft.Practices.Composite.Regions.IRegionBehavior"/>s that can extend the behavior of regions.
        /// </summary>
        /// <value></value>
        public IRegionBehaviorCollection Behaviors
        {
            get { return InnerRegion.Behaviors; }
        }

        /// <summary>
        /// Gets the framework element for view.
        /// </summary>
        /// <param name="innerView">The inner view.</param>
        /// <returns></returns>
        protected virtual FrameworkElement GetFrameworkElementForView(object innerView)
        {
            // See if the view already exists (either directly, or contained in an IModelVisualizer
            FrameworkElement returnValue = FindView(innerView);

            if (returnValue != null)
            {
                // The view already exists
                return returnValue;
            }

            // The view does not yet exist. Return it. 
            return this.modelVisualizationRegistry.CreateVisualization(innerView);
        }

        /// <summary>
        /// Attemps to find the visualization for a specific view. 
        /// </summary>
        /// <param name="innerView">The inner view.</param>
        /// <returns></returns>
        protected virtual  FrameworkElement FindView(object innerView)
        {
            return this.Views.FirstOrDefault((view) => 
                                             (view == innerView 
                                              || IModelVisualizerContainsInnerView(view, innerView))) as FrameworkElement;
        }

        private bool IModelVisualizerContainsInnerView(object view, object innerView)
        {
            IModelVisualizer visualizer = view as IModelVisualizer;
            if (visualizer == null)
                return false;

            return (visualizer.ViewModel == innerView);
        }
    }
}