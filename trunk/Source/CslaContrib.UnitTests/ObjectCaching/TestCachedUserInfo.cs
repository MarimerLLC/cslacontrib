using System;
using Csla;
using CslaContrib.ObjectCaching;

namespace CslaContrib.UnitTests.ObjectCaching
{
    [Serializable]
    [ObjectCache(Scope = CacheScope.User, CacheByCriteria = true)]
    public class TestCachedUserInfo : ReadOnlyBase<TestCachedUserInfo>
    {
        #region Business Methods

        public readonly static PropertyInfo<int> IDProperty = RegisterProperty<int>(o => o.ID, "ID");
        public int ID
        {
            get { return GetProperty(IDProperty); }
            private set { LoadProperty(IDProperty, value); }
        }

        #endregion

        #region Factory Methods

        public static TestCachedUserInfo GetCacheTestInfo(int id)
        {
            return DataPortal.Fetch<TestCachedUserInfo>(new TestCriteria(id));
        }

        public TestCachedUserInfo()
        { /* require use of factory methods */ }

        #endregion

        #region Data Access

        private void DataPortal_Fetch(TestCriteria criteria)
        {
            LoadProperty(IDProperty, criteria.Value);
        }

        #endregion
    }
}
