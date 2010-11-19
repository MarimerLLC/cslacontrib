using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure.NewWindow;

namespace OutlookStyleApp.Tests.NewWindow
{
    [TestClass]
    public class NewWindowRegionAdapterFixture
    {
        [TestMethod]
        public void InitializeCreatesRegionAndAttachesBehaviors()
        {
            NewWindowRegionAdapter adapter = new NewWindowRegionAdapter(null);
            var region = adapter.Initialize(new NewWindowControl(), "Region");

            Assert.IsInstanceOfType(region, typeof(Region));
            Assert.IsTrue(region.Behaviors.ContainsKey("NewWindowRegionBehavior"));
        }
    }
}
