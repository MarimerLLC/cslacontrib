using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Composite.Presentation.Regions;
using Microsoft.Practices.Composite.Regions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure.UseCase;
using OutlookStyleApp.Tests.Mocks;

namespace OutlookStyleApp.Tests
{
    [TestClass]
    public class ActiveAwareUseCaseControllerFixture
    {
        [TestMethod]
        public void InitializeCalledOnFirstActivate()
        {
            var target = new TestableActiveAwareUseCaseController();

            Assert.AreEqual(0, target.InitCount);

            target.IsActive = true;
            Assert.AreEqual(1, target.InitCount);

            target.IsActive = false;
            Assert.AreEqual(1, target.InitCount);

            target.IsActive = true;
            Assert.AreEqual(1, target.InitCount);
        }

    }

    internal class TestableActiveAwareUseCaseController : ActiveAwareUseCaseController
    {
        public RegionManager RegionManager
        {
            get; set;
        }

        public TestableActiveAwareUseCaseController() : base(new MockViewToRegionBinder())
        {
            
        }

        public int InitCount = 0;

        protected override void OnFirstActivation()
        {
            InitCount++;
        }

        public override string Name
        {
            get { throw new System.NotImplementedException(); }
        }


    }
}
