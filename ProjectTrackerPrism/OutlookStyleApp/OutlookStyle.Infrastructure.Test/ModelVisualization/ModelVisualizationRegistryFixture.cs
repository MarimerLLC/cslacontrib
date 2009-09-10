using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure.ModelVisualization;

namespace OutlookStyleApp.Tests.ModelVisualization
{
    [TestClass]
    public class ModelVisualizationRegistryFixture
    {
        [TestMethod]
        public void CanAddRegistration()
        {
            ModelVisualizationRegistry registry = new ModelVisualizationRegistry(null);

            registry.Register<MockViewModel, MockView>();

            Assert.IsNotNull(registry.ModelVisualizations.FirstOrDefault(
                (registration) => registration.ModelType == typeof(MockViewModel) 
                                    && registration.ViewType == typeof(MockView)));
        }

        [TestMethod]
        public void CanCreateVisualizationForModel()
        {
            ModelVisualizationRegistry registry = new ModelVisualizationRegistry(new UnityContainer());

            registry.Register<MockViewModel, MockView>();

            var mockViewModel = new MockViewModel();
            var visualization = registry.CreateVisualization(mockViewModel) as ModelVisualizer;

            Assert.IsNotNull(visualization);
            Assert.IsNotNull(visualization.View);
            Assert.IsNotNull(visualization.ViewModel);
            Assert.IsInstanceOfType(visualization.View, typeof(MockView));
            Assert.AreSame(visualization.ViewModel, mockViewModel);
        }

        [TestMethod]
        public void WontCreateVisualizationModelForFrameworkElements()
        {
            ModelVisualizationRegistry registry = new ModelVisualizationRegistry(new UnityContainer());

            var mockView = new MockView();
            var visualization = registry.CreateVisualization(mockView);

            Assert.IsNotNull(visualization);
            Assert.AreSame(visualization, mockView);

        }

        [TestMethod]
        public void CanRegisterVisualizationForInterface()
        {
            ModelVisualizationRegistry registry = new ModelVisualizationRegistry(new UnityContainer());

            registry.Register<IMockViewModel, MockView>();

            var visualization = registry.CreateVisualization(new MockViewModel()) as ModelVisualizer;
            Assert.IsNotNull(visualization);
            Assert.IsInstanceOfType(visualization.View, typeof(MockView));
        }
    }

    class MockViewModel : IMockViewModel
    {
        
    }

    public interface IMockViewModel
    {
    }
}
