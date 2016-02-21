using System;
using Csla;
using CslaContrib.ObjectCaching;

namespace CslaContrib.UnitTests.ObjectCaching
{
    [Serializable]
    [ObjectCache(Scope = CacheScope.Group, CacheByCriteria = true)]
    public class TestCachedGroupInfo : ReadOnlyBase<TestCachedGroupInfo>
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

        public static TestCachedGroupInfo GetCacheTestInfo(int id)
        {
            return DataPortal.Fetch<TestCachedGroupInfo>(new TestCriteria(id));
        }

        public TestCachedGroupInfo()
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
