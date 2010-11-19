using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure.RegionContext;
using OutlookStyleApp.Tests.Mocks;

namespace OutlookStyleApp.Tests
{
    [TestClass]
    public class RegionContextAwareRegionBehaviorFixture
    {
        [TestMethod]
        public void WillSyncRegionContext()
        {
            RegionContextAwareRegionBehavior target = new RegionContextAwareRegionBehavior();
            target.Region = new Region();
            target.Attach();
            var view = new MockRegionContextAwareView();
            target.Region.Add(view);

            target.Region.Context = "Blurp";
            Assert.AreEqual("Blurp", view.RegionContext.Value);

            view.RegionContext.Value = "Slurp";
            Assert.AreEqual("Slurp", target.Region.Context);
        }

        [TestMethod]
        public void WillDetachEventsAfterRemove()
        {
            RegionContextAwareRegionBehavior target = new RegionContextAwareRegionBehavior();
            
            target.Region = new Region();
            target.Attach();
            var view = new MockRegionContextAwareView();
            WeakReference viewReference = new WeakReference(view);
            target.Region.Add(view);

            target.Region.Remove(view);

            target.Region.Context = "Blurp";
            Assert.IsNull(view.RegionContext.Value);

            view.RegionContext.Value = "Slurp";
            Assert.AreEqual("Blurp", target.Region.Context);
        }
    }
}
