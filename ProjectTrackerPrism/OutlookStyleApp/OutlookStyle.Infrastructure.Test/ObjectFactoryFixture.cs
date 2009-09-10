using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutlookStyle.Infrastructure;

namespace OutlookStyleApp.Tests
{
    [TestClass]
    public class ObjectFactoryFixture
    {
        [TestMethod]
        public void CanCreateView()
        {
            var serviceLocator = new UnityServiceLocatorAdapter(new UnityContainer());
            var objectFactory = new ObjectFactory<MockView>(serviceLocator);
            
            Assert.IsInstanceOfType(objectFactory.CreateInstance(), typeof(MockView));
            Assert.IsInstanceOfType(objectFactory.Value, typeof(MockView));
        }

        [TestMethod]
        public void CanImplicitlyConvertToType()
        {
            var serviceLocator = new UnityServiceLocatorAdapter(new UnityContainer());
            var objectFactory = new ObjectFactory<MockView>(serviceLocator);

            var expected = objectFactory.CreateInstance();
            MockView view = objectFactory;

            Assert.AreEqual(expected, view);
        }

    }
}
