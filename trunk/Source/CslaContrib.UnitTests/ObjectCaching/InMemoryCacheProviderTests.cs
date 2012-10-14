using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CslaContrib.ObjectCaching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CslaContrib.UnitTests.ObjectCaching
{
    [TestClass]
    public class InMemoryCacheProviderTests
    {
        public InMemoryCacheProviderTests() { }
        public TestContext TestContext { get; set; }
        private ICacheProvider provider = CacheManager.GetCacheProvider();

        #region Additional test attributes
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext) { }

        //[ClassCleanup()]
        //public static void MyClassCleanup() { }

        //[TestInitialize()]
        public void MyTestInitialize() 
        {
            InMemoryCacheProvider.cache.Clear();
        }

        //[TestCleanup()]
        //public void MyTestCleanup() { }
        #endregion

        [TestMethod]
        public void Provider_Initialize()
        {
            Assert.IsNotNull(provider);
        }

        [TestMethod]
        public void Provider_IsEmpty()
        {
            Assert.IsFalse(InMemoryCacheProvider.cache.Any());
        }

        [TestMethod]
        public void Provider_Put()
        {
            var data = "somedata";
            provider.Put("test", data);
            Assert.IsTrue(InMemoryCacheProvider.cache.ContainsKey("test"));
            var test = provider.Get("test");
            Assert.AreEqual(data, test);
            provider.Remove("test");
            Assert.IsFalse(InMemoryCacheProvider.cache.ContainsKey("test"));
        }

        [TestMethod]
        public void Provider_Put_Area()
        {
            var data = "somedata";
            provider.Put("test", data, "area");
            Assert.IsTrue(InMemoryCacheProvider.cache.ContainsKey("test"));
            var test = provider.Get("test", "area");
            Assert.AreEqual(data, test);
            provider.Remove("test", "area");
            Assert.IsFalse(InMemoryCacheProvider.cache.ContainsKey("test"));
        }

        [TestMethod]
        public void Provider_Put_Timeout()
        {
            var data = "somedata";
            provider.Put("test", data, new TimeSpan(0,0,10));
            Assert.IsTrue(InMemoryCacheProvider.cache.ContainsKey("test"));
            var test = provider.Get("test", new TimeSpan(0, 0, 10));
            Assert.AreEqual(data, test);
            provider.Remove("test");
            Assert.IsFalse(InMemoryCacheProvider.cache.ContainsKey("test"));
        }

        [TestMethod]
        public void Provider_Get_TimeoutExpired()
        {
            var data = "somedata";
            provider.Put("test", data, new TimeSpan(0, 0, 1));
            Assert.IsTrue(InMemoryCacheProvider.cache.ContainsKey("test"));
            System.Threading.Thread.Sleep(2000);
            var test = provider.Get("test");
            Assert.IsNull(test);
            provider.Remove("test");
        }

        [TestMethod]
        public void Provider_Put_AreaAndTimeout()
        {
            var data = "somedata";
            provider.Put("test", data, new TimeSpan(0, 0, 10), "area");
            Assert.IsTrue(InMemoryCacheProvider.cache.ContainsKey("test"));
            var test = provider.Get("test", new TimeSpan(0, 0, 10), "area");
            Assert.AreEqual(data, test);
            provider.Remove("test", "area");
            Assert.IsFalse(InMemoryCacheProvider.cache.ContainsKey("test"));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Provider_CreateArea()
        {
            provider.CreateArea("area");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Provider_RemoveArea()
        {
            provider.RemoveArea("area");
        }

        [TestMethod]
        public void Provider_RemoveAll()
        {
            var data = "somedata";
            provider.Put("test1", data);
            provider.Put("test2", data);
            provider.Put("test1", data); //replace
            Assert.IsTrue(InMemoryCacheProvider.cache.ContainsKey("test1"));
            Assert.IsTrue(InMemoryCacheProvider.cache.ContainsKey("test2"));
            provider.RemoveAllByKeyPrefix("test");
            Assert.IsFalse(InMemoryCacheProvider.cache.ContainsKey("test1"));
            Assert.IsFalse(InMemoryCacheProvider.cache.ContainsKey("test2"));
        }
    }
}
