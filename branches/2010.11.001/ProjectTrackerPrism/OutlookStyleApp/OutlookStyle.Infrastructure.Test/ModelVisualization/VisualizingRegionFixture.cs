using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Presentation;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure.ModelVisualization;

namespace OutlookStyleApp.Tests.ModelVisualization
{
    [TestClass]
    public class VisualizingRegionFixture
    {
        [TestMethod]
        public void AddingViewWillCallVisualizationRegistry()
        {
            var registry = new MockModelVisualizationRegistry();
            VisualizingRegion region = new VisualizingRegion(registry);

            registry.VisualizationToReturn = new FrameworkElement();

            region.InnerRegion = new Region();
            var mockView = new MockView();
            region.Add(mockView);

            Assert.IsTrue(region.Views.Contains(registry.VisualizationToReturn));
        }

        [TestMethod]
        public void CanRemoveView()
        {
            var registry = new MockModelVisualizationRegistry();
            var view = new MockView();
            var viewModel = new MockViewModel();
            registry.VisualizationToReturn = new MockModelVisualizer() {View = view, ViewModel = viewModel};

            VisualizingRegion region = new VisualizingRegion(registry);
            region.InnerRegion = new Region();

            region.Add(viewModel);
            Assert.IsTrue(region.Views.Contains(registry.VisualizationToReturn));

            region.Remove(viewModel);
            Assert.IsFalse(region.Views.Contains(registry.VisualizationToReturn));
        }


    }

    internal class MockModelVisualizer : FrameworkElement , IModelVisualizer
    {
        public event EventHandler IsActiveChanged;
        public bool IsActive
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public ObservableObject<object> RegionContext
        {
            get { throw new System.NotImplementedException(); }
        }

        public FrameworkElement View
        {
            get; set;
        }

        public object ViewModel
        {
            get; set;
        }
    }

    internal class MockModelVisualizationRegistry : IModelVisualizationRegistry
    {
        public FrameworkElement VisualizationToReturn;

        public void Register<TModel, TView>()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ModelVisualizationRegistration> ModelVisualizations
        {
            get { throw new System.NotImplementedException(); }
        }

        public FrameworkElement CreateVisualization(object objectToVisualize)
        {
            return VisualizationToReturn;
        }
    }
}
