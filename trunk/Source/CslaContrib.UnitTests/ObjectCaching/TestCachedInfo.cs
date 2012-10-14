using System;
using Csla;
using CslaContrib.ObjectCaching;

namespace CslaContrib.UnitTests.ObjectCaching
{
    [Serializable]
    [ObjectCache(CacheByCriteria = true)]
    public class TestCachedInfo : ReadOnlyBase<TestCachedInfo>
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

        public static TestCachedInfo GetCacheTestInfo(int id)
        {
            return DataPortal.Fetch<TestCachedInfo>(new TestCriteria(id));
        }

        private TestCachedInfo()
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
