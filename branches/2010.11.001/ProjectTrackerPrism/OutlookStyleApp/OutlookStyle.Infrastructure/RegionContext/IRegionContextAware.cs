using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation;
using Microsoft.Practices.Composite.Presentation.Regions;

namespace OutlookStyle.Infrastructure.RegionContext
{
    /// <summary>
    /// Interface for views and viewmodels that need the RegionContext. This is an alternative to the <see cref="Microsoft.Practices.Composite.Presentation.Regions.RegionContext.GetObservableContext"/> method in Prism.
    ///  </summary>
    public interface IRegionContextAware
    {
        ObservableObject<object> RegionContext{get;}
    }
}