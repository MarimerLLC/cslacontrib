using System.Collections.Generic;
using Microsoft.Practices.Composite;
using OutlookStyle.Infrastructure.ViewToRegionBinding;

namespace OutlookStyleApp.Tests.Mocks
{
    internal class MockViewToRegionBinder : IViewToRegionBinder
    {
        public void Add(string regionName, object view)
        {
            
        }

        public IActiveAware ObjectToMonitor
        {
            get;
            set;
        }

        public IEnumerable<ViewRegionBinding> Bindings
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}