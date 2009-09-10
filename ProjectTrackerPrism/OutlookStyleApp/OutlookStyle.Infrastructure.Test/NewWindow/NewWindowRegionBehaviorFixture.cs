using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure;
using OutlookStyle.Infrastructure.ModelVisualization;
using OutlookStyle.Infrastructure.NewWindow;

namespace OutlookStyleApp.Tests.NewWindow
{
    [TestClass]
    class NewWindowRegionBehaviorFixture
    {
        [TestMethod]
        public void ActivatingViewShowsWindow()
        {
            var target = new NewWindowRegionBehavior();
            target.Region = new Region();
            target.Region.Behaviors.Add("Key", new TwoWayActiveAwareBehavior());

            target.Attach();
            var mockWindow = new MockWindow();
            target.Region.Add(mockWindow);

            Assert.IsFalse(mockWindow.IsShown);

            mockWindow.IsActive = true;

            Assert.IsTrue(mockWindow.IsShown);

            mockWindow.IsActive = false;

            Assert.IsFalse(mockWindow.IsShown);
        }

        [TestMethod]
        public void ActivatingModelVisualizerThatHoldsWindowShowsWindow()
        {
            var target = new NewWindowRegionBehavior();
            target.Region = new Region();
            target.Region.Behaviors.Add("Key", new TwoWayActiveAwareBehavior());

            target.Attach();
            var mockWindow = new MockWindow();
            var modelVisualizer = new ModelVisualizer(null, mockWindow);

            target.Region.Add(modelVisualizer);

            Assert.IsFalse(mockWindow.IsShown);

            mockWindow.IsActive = true;

            Assert.IsTrue(mockWindow.IsShown);

            mockWindow.IsActive = false;

            Assert.IsFalse(mockWindow.IsShown);
        }

        [TestMethod]
        public void DeActivatingWindowRemovesViewFromRegion()
        {
            var target = new NewWindowRegionBehavior();
            target.Region = new Region();
            target.Region.Behaviors.Add("Key", new TwoWayActiveAwareBehavior());

            target.Attach();
            var mockWindow = new MockWindow();

            target.Region.Add(mockWindow);

            mockWindow.IsActive = true;
            mockWindow.IsActive = false;
            
            Assert.IsFalse(target.Region.Views.Contains(mockWindow));

        }
    }

    internal class MockWindow : FrameworkElement, IWindow, IActiveAware
    {
        public void Show()
        {
            IsShown = true;
        }

        public void Close()
        {
            IsShown = false;
        }

        private bool isActive;
        public bool IsShown;

        public bool IsActive
        {
            get { return isActive; }
            set { 
                isActive = value;
                InvokeIsActiveChanged(EventArgs.Empty);
            }
        }

        public event EventHandler IsActiveChanged;

        private void InvokeIsActiveChanged(EventArgs e)
        {
            EventHandler changed = IsActiveChanged;
            if (changed != null) changed(this, e);
        }
    }
}
