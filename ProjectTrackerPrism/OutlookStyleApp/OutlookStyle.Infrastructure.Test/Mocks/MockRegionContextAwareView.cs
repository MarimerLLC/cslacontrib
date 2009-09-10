using Microsoft.Practices.Composite.Presentation;
using OutlookStyle.Infrastructure.RegionContext;

namespace OutlookStyleApp.Tests.Mocks
{
    internal class MockRegionContextAwareView : IRegionContextAware
    {
        public MockRegionContextAwareView()
        {
            this.RegionContext = new ObservableObject<object>();
        }

        public ObservableObject<object> RegionContext
        {
            get; set;
        }
    }
}