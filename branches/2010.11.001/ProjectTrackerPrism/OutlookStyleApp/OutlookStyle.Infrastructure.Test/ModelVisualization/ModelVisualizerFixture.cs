using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.RegionContext;

namespace OutlookStyleApp.Tests
{
    [TestClass]
    public class ModelVisualizerFixture
    {

        [TestMethod]
        public void ActiveAwareIsForwarded()
        {
            var viewViewModelWrapper = new ModelVisualizer(new MockActiveAwareViewModel(), new MockActiveAwareView());

            viewViewModelWrapper.IsActive = true;

            Assert.IsTrue((viewViewModelWrapper.View as IActiveAware).IsActive);
            Assert.IsTrue((viewViewModelWrapper.View as IActiveAware).IsActive);
        }

        [TestMethod]
        public void ActiveAwareIsReturned()
        {
            var viewViewModelWrapper = new ModelVisualizer(new MockActiveAwareViewModel(), new MockActiveAwareView());

            viewViewModelWrapper.IsActive = true;

            Assert.IsTrue(viewViewModelWrapper.IsActive);
            Assert.IsTrue((viewViewModelWrapper.View as IActiveAware).IsActive);
        }

        [TestMethod]
        public void RegioncontextIsForwarded()
        {
            var viewViewModelWrapper = new ModelVisualizer(new MockContextAwareView(), new MockContextAwareView());

            var payload = new MockPayload();
            viewViewModelWrapper.RegionContext.Value = payload;

            Assert.AreEqual(payload, (viewViewModelWrapper.View as IRegionContextAware).RegionContext.Value);
            Assert.AreEqual(payload, (viewViewModelWrapper.ViewModel as IRegionContextAware).RegionContext.Value);
        }

        [TestMethod]
        public void RegioncontextIsReturned()
        {
            var viewViewModelWrapper = new ModelVisualizer(new MockContextAwareView(), new MockContextAwareView());

            var payload = new MockPayload();
            (viewViewModelWrapper.View as IRegionContextAware).RegionContext.Value = payload;

            Assert.AreEqual(payload, viewViewModelWrapper.RegionContext.Value);
            Assert.AreEqual(payload, (viewViewModelWrapper.ViewModel as IRegionContextAware).RegionContext.Value);
        }

        [TestMethod]
        public void DontSetContentIfViewIsWindow()
        {
            var viewViewModelWrapper = new ModelVisualizer(new MockActiveAwareViewModel(), new Window());

            Assert.IsNull(viewViewModelWrapper.Content);
        }
    }
}
