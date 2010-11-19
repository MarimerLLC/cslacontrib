using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure.ViewToRegionBinding;

namespace OutlookStyleApp.Tests
{
    [TestClass]
    public class ViewToRegionBinderFixture
    {
        [TestMethod]
        public void CanAddViewToRegionBinding()
        {
            var target = new ViewToRegionBinder(null);
            var view = new DependencyObject();
            target.Add("Region", view);

            Assert.AreEqual(1, target.Bindings.Count());
            Assert.AreEqual("Region", target.Bindings.FirstOrDefault().RegionName);
            Assert.AreEqual(view, target.Bindings.FirstOrDefault().View);
        }

        [TestMethod]
        public void SettingActiveWillAddViewsToRegion()
        {
            var regionManager = new RegionManager();

            var region = new Region()
                             {
                                 Name = "Region"
                             };
            regionManager.Regions.Add(region);

            var target = new ViewToRegionBinder(regionManager);
            target.ObjectToMonitor  = new MockActiveAware();

            var view = new DependencyObject();
            target.Add("Region", view);

            Assert.AreEqual(0, region.Views.Count());

            target.ObjectToMonitor.IsActive = true;

            Assert.AreEqual(1, region.Views.Count());
            Assert.AreEqual(view, region.Views.FirstOrDefault());

            target.ObjectToMonitor.IsActive = false;
            Assert.AreEqual(0, region.Views.Count());
            
        }

        internal class MockActiveAware : IActiveAware
        {
            public event EventHandler IsActiveChanged;

            private void InvokeIsActiveChanged(EventArgs e)
            {
                EventHandler isActiveChangedHandler = IsActiveChanged;
                if (isActiveChangedHandler != null) isActiveChangedHandler(this, e);
            }

            private bool isActive;

            public bool IsActive
            {
                get { return isActive; }
                set 
                { 
                    isActive = value;

                    InvokeIsActiveChanged(EventArgs.Empty);
                }
            }
        }
    }
}
