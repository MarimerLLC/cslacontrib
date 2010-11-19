using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Presentation;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.RegionContext;

namespace OutlookStyleApp.Tests
{
    [TestClass]
    public class ViewViewModelWrapperFixture
    {
        [TestMethod]
        public void CanCreateWrapper()
        {
            var viewViewModelWrapper = new ViewViewModelWrapper<MockView, MockViewModel>(GetContainer());

            Assert.IsNotNull(viewViewModelWrapper.View);
            Assert.IsNotNull(viewViewModelWrapper.ViewModel);
            Assert.AreEqual(viewViewModelWrapper.View.DataContext, viewViewModelWrapper.ViewModel);
        }

        [TestMethod]
        public void ActiveAwareIsForwarded()
        {
            var viewViewModelWrapper = new ViewViewModelWrapper<MockActiveAwareView, MockActiveAwareViewModel>(GetContainer());

            viewViewModelWrapper.IsActive = true;

            Assert.IsTrue(viewViewModelWrapper.ViewModel.IsActive);
            Assert.IsTrue(viewViewModelWrapper.View.IsActive);
        }

        [TestMethod]
        public void ActiveAwareIsReturned()
        {
            var viewViewModelWrapper = new ViewViewModelWrapper<MockActiveAwareView, MockActiveAwareViewModel>(GetContainer());

            viewViewModelWrapper.IsActive = true;
            
            Assert.IsTrue(viewViewModelWrapper.IsActive);
            Assert.IsTrue(viewViewModelWrapper.View.IsActive);
        }

        [TestMethod]
        public void RegioncontextIsForwarded()
        {
            var viewViewModelWrapper = new ViewViewModelWrapper<MockContextAwareView, MockContextAwareView>(GetContainer());

            var payload = new MockPayload();
            viewViewModelWrapper.RegionContext.Value = payload;

            Assert.AreEqual(payload, viewViewModelWrapper.View.RegionContext.Value);
            Assert.AreEqual(payload, viewViewModelWrapper.ViewModel.RegionContext.Value);
        }

        [TestMethod]
        public void RegioncontextIsReturned()
        {
            var viewViewModelWrapper = new ViewViewModelWrapper<MockContextAwareView, MockContextAwareView>(GetContainer());

            var payload = new MockPayload();
            viewViewModelWrapper.View.RegionContext.Value = payload;

            Assert.AreEqual(payload, viewViewModelWrapper.RegionContext.Value);
            Assert.AreEqual(payload, viewViewModelWrapper.ViewModel.RegionContext.Value);
        }

        private IServiceLocator GetContainer()
        {
            return new UnityServiceLocatorAdapter(new UnityContainer());
        }

        [TestMethod]
        public void OnlyCreatesViewAndViewModelWhenCallingForTheProperties()
        {
            ConstructorCounter.CreatedCount = 0;

            var viewViewModelWrapper = new ViewViewModelWrapper<ConstructorCounter, ConstructorCounter>(GetContainer());

            Assert.AreEqual(0, ConstructorCounter.CreatedCount);

            var obj = viewViewModelWrapper.View;

            Assert.AreEqual(2, ConstructorCounter.CreatedCount);
        }
    }

    internal class ConstructorCounter : FrameworkElement
    {
        public static volatile int CreatedCount = 0;

        public ConstructorCounter()
        {
            CreatedCount++;
        }
    }

    internal class MockPayload
    {
        
    }

    internal class MockContextAwareView : FrameworkElement, IRegionContextAware
    {
        public MockContextAwareView()
        {
            RegionContext = new ObservableObject<object>();
        }
        public ObservableObject<object> RegionContext
        {
            get; private set;
        }
    }
}
