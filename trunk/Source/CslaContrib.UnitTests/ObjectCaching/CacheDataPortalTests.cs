using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CslaContrib.ObjectCaching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Principal;

namespace CslaContrib.UnitTests.ObjectCaching
{
    [TestClass]
    public class CacheDataPortalTests
    {
        public CacheDataPortalTests() { }
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext) { }

        //[ClassCleanup()]
        //public static void MyClassCleanup() { }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            CacheManager.SetCacheProvider(null);
            Csla.ApplicationContext.ClientContext.Remove(CacheDataPortal.CacheGroup);
        }

        //[TestCleanup()]
        //public void MyTestCleanup() { }
        #endregion

        [TestMethod]
        public void Portal_NoAttributePassThrough()
        {
            var id = 999;
            var test = TestInfo.GetTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestInfo).FullName)).Any());
        }

        [TestMethod]
        public void Portal_AttributeCachePortal_Get()
        {
            var id = 999;
            var test = TestCachedInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedInfo).FullName)).Any());
        }

        [TestMethod]
        public void Portal_AttributeCachePortal_Hit()
        {
            var id = 999;
            var test = TestCachedInfo.GetCacheTestInfo(id);
            var test2 = TestCachedInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedInfo).FullName)).Any());
            Assert.AreEqual(1, InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedInfo).FullName)).Count());
        }

        [TestMethod]
        public void Portal_AttributeCachePortalGroup()
        {
            Csla.ApplicationContext.ClientContext.Add(CacheDataPortal.CacheGroup, "group");
            var id = 999;
            var test = TestCachedGroupInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Single().Key.Contains("group"));
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Portal_AttributeCachePortalGroup_Exception()
        {
            var id = 999;
            var test = TestCachedGroupInfo.GetCacheTestInfo(id);
        }

        [TestMethod]
        public void Portal_AttributeCachePortalUser()
        {
            Csla.ApplicationContext.User = new GenericPrincipal(new GenericIdentity("foo"), new string[] { });
            var id = 999;
            var test = TestCachedUserInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedUserInfo).FullName)).Any());
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedUserInfo).FullName)).Single().Key.Contains("foo"));
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Portal_AttributeCachePortalUser_Exception()
        {
            var id = 999;
            var test = TestCachedUserInfo.GetCacheTestInfo(id);
        }

        [TestMethod]
        public void Portal_AttributeCachePortalGroupAndUser()
        {
            Csla.ApplicationContext.ClientContext.Add(CacheDataPortal.CacheGroup, "group");
            Csla.ApplicationContext.User = new GenericPrincipal(new GenericIdentity("foo"), new string[] { });
            var id = 999;
            var test = TestCachedUserInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedUserInfo).FullName)).Any());
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedUserInfo).FullName)).Single().Key.Contains("group"));
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedUserInfo).FullName)).Single().Key.Contains("foo"));
        }

        [TestMethod]
        public void Portal_CreateRemovesCachedItems_Global()
        {
            var id = 999;
            var test = TestCachedInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedInfo).FullName)).Any());
            var save = TestEdit.NewTestEdit();
            save.ID = id;
            save.Save();
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedInfo).FullName)).Any());
        }

        [TestMethod]
        public void Portal_UpdateRemovesCachedItems_Global()
        {
            var id = 999;
            var test = TestCachedInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedInfo).FullName)).Any());
            var save = TestEdit.GetTestEdit(0);
            save.ID = id;
            save.Save();
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedInfo).FullName)).Any());
        }

        [TestMethod]
        public void Portal_DeleteRemovesCachedItems_Global()
        {
            var id = 999;
            var test = TestCachedInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedInfo).FullName)).Any());
            TestEdit.DeleteTestEdit(id);
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedInfo).FullName)).Any());
        }

        [TestMethod]
        public void Portal_CreateRemovesCachedItems_Group()
        {
            Csla.ApplicationContext.ClientContext.Add(CacheDataPortal.CacheGroup, "group");
            var id = 999;
            var test = TestCachedGroupInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Single().Key.Contains("group"));
            var save = TestGroupEdit.NewTestGroupEdit();
            save.ID = id;
            save.Save();
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
        }

        [TestMethod]
        public void Portal_UpdateRemovesCachedItems_Group()
        {
            Csla.ApplicationContext.ClientContext.Add(CacheDataPortal.CacheGroup, "group");
            var id = 999;
            var test = TestCachedGroupInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Single().Key.Contains("group"));
            var save = TestGroupEdit.GetTestGroupEdit(0);
            save.ID = id;
            save.Save();
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
        }

        [TestMethod]
        public void Portal_DeleteRemovesCachedItems_Group()
        {
            Csla.ApplicationContext.ClientContext.Add(CacheDataPortal.CacheGroup, "group");
            var id = 999;
            var test = TestCachedGroupInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Single().Key.Contains("group"));
           TestGroupEdit.DeleteTestGroupEdit(id);
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
        }
        [TestMethod]
        public void Portal_CreateRemovesCachedItems_GroupWithoutGrouping()
        {
            Csla.ApplicationContext.ClientContext.Add(CacheDataPortal.CacheGroup, "group");
            var id = 999;
            var test = TestCachedGroupInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Single().Key.Contains("group"));
            Csla.ApplicationContext.ClientContext.Remove(CacheDataPortal.CacheGroup);
            var save = TestGroupEdit.NewTestGroupEdit();
            save.ID = id;
            save.Save();
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
        }

        [TestMethod]
        public void Portal_UpdateRemovesCachedItems_GroupWithoutGrouping()
        {
            Csla.ApplicationContext.ClientContext.Add(CacheDataPortal.CacheGroup, "group");
            var id = 999;
            var test = TestCachedGroupInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Single().Key.Contains("group"));
            Csla.ApplicationContext.ClientContext.Remove(CacheDataPortal.CacheGroup);
            var save = TestGroupEdit.GetTestGroupEdit(0);
            save.ID = id;
            save.Save();
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
        }

        [TestMethod]
        public void Portal_DeleteRemovesCachedItems_GroupWithoutGrouping()
        {
            Csla.ApplicationContext.ClientContext.Add(CacheDataPortal.CacheGroup, "group");
            var id = 999;
            var test = TestCachedGroupInfo.GetCacheTestInfo(id);
            Assert.AreEqual(id, test.ID);
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
            Assert.IsTrue(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Single().Key.Contains("group"));
            Csla.ApplicationContext.ClientContext.Remove(CacheDataPortal.CacheGroup);
            TestGroupEdit.DeleteTestGroupEdit(id);
            Assert.IsFalse(InMemoryCacheProvider.cache.Where(c => c.Key.StartsWith(typeof(TestCachedGroupInfo).FullName)).Any());
        }

    }
}
