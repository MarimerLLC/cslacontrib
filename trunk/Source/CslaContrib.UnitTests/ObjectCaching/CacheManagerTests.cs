using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using CslaContrib.ObjectCaching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CslaContrib.UnitTests.ObjectCaching
{
    [TestClass]
    public class CacheManagerTests
    {
        public CacheManagerTests() { }
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext) { }

        //[ClassCleanup()]
        //public static void MyClassCleanup() { }

        [TestInitialize]
        public void MyTestInitialize() 
        {
            CacheManager.SetCacheProvider(null);
        }

        [TestCleanup]
        public void MyTestCleanup() 
        {
            var cm = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cm.AppSettings.Settings.Remove("CachingProvider");
            cm.AppSettings.Settings.Add("CachingProvider", "CslaContrib.ObjectCaching.InMemoryCacheProvider, CslaContrib");
            cm.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion

        [TestMethod]
        public void Manager_IsConfigured_Positive()
        {
            Assert.IsTrue(CacheManager.IsCacheConfigured(typeof(TestCachedInfo)));
        }

        [TestMethod]
        public void Manager_IsConfigured_NoAttribute()
        {
            Assert.IsFalse(CacheManager.IsCacheConfigured(typeof(TestInfo)));
        }

        [TestMethod]
        public void Manager_IsConfigured_NoProvider()
        {
            var cm = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cm.AppSettings.Settings.Remove("CachingProvider");
            cm.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");

            Assert.IsFalse(CacheManager.IsCacheConfigured(typeof(TestCachedInfo)));
        }

        [TestMethod]
        public void Manager_SetCacheProvider()
        {
            CacheManager.SetCacheProvider(null);
            CacheManager.SetCacheProvider("CslaContrib.ObjectCaching.InMemoryCacheProvider, CslaContrib");
            Assert.IsNotNull(CacheManager.GetCacheProvider());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Manager_Get_InvalidConfig()
        {
            var cm = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cm.AppSettings.Settings.Remove("CachingProvider");
            cm.AppSettings.Settings.Add("CachingProvider", "foo, foo");
            cm.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");

            Assert.IsNull(CacheManager.GetCacheProvider());
        }

        [TestMethod]
        public void Manager_Get_EmptyConfig()
        {
            var cm = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cm.AppSettings.Settings.Remove("CachingProvider");
            cm.AppSettings.Settings.Add("CachingProvider", "");
            cm.Save(ConfigurationSaveMode.Modified, true);
            ConfigurationManager.RefreshSection("appSettings");

            Assert.IsNull(CacheManager.GetCacheProvider());
        }
    }
}
