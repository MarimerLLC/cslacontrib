using System;
using Csla;
using CslaContrib.ObjectCaching;

namespace CslaContrib.UnitTests.ObjectCaching
{
    [Serializable]
    public class TestInfo : ReadOnlyBase<TestInfo>
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

        public static TestInfo GetTestInfo(int id)
        {
            return DataPortal.Fetch<TestInfo>(new SingleCriteria<TestInfo, int>(id));
        }

        private TestInfo()
        { /* require use of factory methods */ }

        #endregion

        #region Data Access

        private void DataPortal_Fetch(SingleCriteria<TestInfo, int> criteria)
        {
            LoadProperty(IDProperty, criteria.Value);
        }

        #endregion
    }
}
