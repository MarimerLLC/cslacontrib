using System.Collections.Generic;
using Microsoft.Practices.Composite;

namespace OutlookStyle.Infrastructure.ViewToRegionBinding
{
    /// <summary>
    /// Interface for the ViewToRegionBinder. 
    /// 
    /// This class can bind an instance of a view to the name of a region. When the <see cref="ObjectToMonitor"/> becomes active
    /// it will add all registered view instances to the right regions. When it becomes in active, it will remove them again. 
    /// </summary>
    public interface IViewToRegionBinder
    {
        /// <summary>
        /// Adds a binding between a view and the name of a region.
        /// </summary>
        /// <param name="regionName">Name of the region.</param>
        /// <param name="view">The view.</param>
        void Add(string regionName, object view);


        /// <summary>
        /// Gets or sets the object to monitor for ISActive changes.
        /// </summary>
        /// <value>The object to monitor.</value>
        IActiveAware ObjectToMonitor { get; set; }

        /// <summary>
        /// The registered bindings. 
        /// </summary>
        IEnumerable<ViewRegionBinding> Bindings { get; }
    }
}