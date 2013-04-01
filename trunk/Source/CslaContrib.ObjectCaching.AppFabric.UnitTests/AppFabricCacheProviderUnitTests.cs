using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CslaContrib.ObjectCaching.AppFabric.UnitTests
{
    [TestClass]
    public class AppFabricCacheProviderUnitTests
    {
        public AppFabricCacheProviderUnitTests() { }
        public TestContext TestContext { get; set; }
        private ICacheProvider provider = CacheManager.GetCacheProvider();

        #region Additional test attributes
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext) { }

        //[ClassCleanup()]
        //public static void MyClassCleanup() { }

        //[TestInitialize()]
        //public void MyTestInitialize() { }

        //[TestCleanup()]
        //public void MyTestCleanup() { }
        #endregion

        //NOTE: restart-cachecluster prior to test run to remove any cache data

        [TestMethod]
        public void Provider_Initialize()
        {
            Assert.IsNotNull(provider);
            var x = new CslaContrib.ObjectCaching.AppFabric.CacheProvider();
        }

        [TestMethod]
        public void Provider_IsEmpty()
        {
            Assert.AreEqual(0, provider.Entries.Count, "cache should not have entries entries");
        }

        [TestMethod]
        public void Provider_Put()
        {
            var data = "somedata";
            provider.Put("test", data);
            Assert.IsTrue(provider.Entries.ContainsKey("test"));
            var test = provider.Get("test");
            Assert.AreEqual(data, test);
            provider.Remove("test");
            Assert.IsFalse(provider.Entries.ContainsKey("test"));
        }

        [TestMethod]
        public void Provider_Put_Area()
        {
            var provider = CacheManager.GetCacheProvider();
            var data = "somedata";
            provider.Put("test", data, "area");
            Assert.IsTrue(provider.Entries.ContainsKey("test"));
            var test = provider.Get("test", "area");
            Assert.AreEqual(data, test);
            provider.Remove("test", "area");
            Assert.IsFalse(provider.Entries.ContainsKey("test"));
        }

        [TestMethod]
        public void Provider_Put_Timeout()
        {
            var provider = CacheManager.GetCacheProvider();
            var data = "somedata";
            provider.Put("test", data, new TimeSpan(0, 0, 10));
            Assert.IsTrue(provider.Entries.ContainsKey("test"));
            var test = provider.Get("test", new TimeSpan(0, 0, 10));
            Assert.AreEqual(data, test);
            provider.Remove("test");
            Assert.IsFalse(provider.Entries.ContainsKey("test"));
        }

        [TestMethod]
        public void Provider_Get_TimeoutExpired()
        {
            var provider = CacheManager.GetCacheProvider();
            var data = "somedata";
            provider.Put("test", data, new TimeSpan(0, 0, 1));
            Assert.IsTrue(provider.Entries.ContainsKey("test"));
            System.Threading.Thread.Sleep(2000);
            var test = provider.Get("test");
            Assert.IsNull(test);
            provider.Remove("test");
        }

        [TestMethod]
        public void Provider_Put_AreaAndTimeout()
        {
            var provider = CacheManager.GetCacheProvider();
            var data = "somedata";
            provider.Put("test", data, new TimeSpan(0, 0, 10), "area");
            Assert.IsTrue(provider.Entries.ContainsKey("test"));
            var test = provider.Get("test", new TimeSpan(0, 0, 10), "area");
            Assert.AreEqual(data, test);
            provider.Remove("test", "area");
            Assert.IsFalse(provider.Entries.ContainsKey("test"));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Provider_CreateArea()
        {
            var provider = CacheManager.GetCacheProvider();
            provider.CreateArea("area");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Provider_RemoveArea()
        {
            var provider = CacheManager.GetCacheProvider();
            provider.RemoveArea("area");
        }

        [TestMethod]
        public void Provider_RemoveAll()
        {
            var provider = CacheManager.GetCacheProvider();
            var data = "somedata";
            provider.Put("test1", data);
            provider.Put("test2", data);
            provider.Put("test1", data); //replace
            Assert.IsTrue(provider.Entries.ContainsKey("test1"));
            Assert.IsTrue(provider.Entries.ContainsKey("test2"));
            provider.RemoveAllByKeyPrefix("test");
            Assert.IsFalse(provider.Entries.ContainsKey("test1"));
            Assert.IsFalse(provider.Entries.ContainsKey("test2"));
            Assert.IsFalse(provider.Entries.Any());
        }
    }
}
