using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Regions;

namespace OutlookStyle.Infrastructure.NewWindow
{
    /// <summary>
    /// Interface for classes that know about the regionmanager they are using. 
    /// 
    /// For example, a usecase that can be displayed in a popup (that needs it's own regionmanager scope) 
    /// </summary>
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; }
    }
}
