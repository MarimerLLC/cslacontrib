using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Regions;

namespace OutlookStyle.Infrastructure.ModelVisualization
{
    /// <summary>
    ///     RegionBehavior that does absolutely nothing, but can be used to prevent other regionbehaviors to be added
    /// </summary>
    public class NullRegionBehavior : IRegionBehavior
    {
        public void Attach()
        {
            
        }

        public IRegion Region
        { get; set;
        }
    }
}
