using System;
using Csla;
using CslaContrib.ObjectCaching;

namespace CslaContrib.UnitTests.ObjectCaching
{
    [Serializable]
    [ObjectCacheEviction(CachedTypes = new Type[] { typeof(TestCachedInfo) })]
    public class TestEdit : BusinessBase<TestEdit>
    {
        #region Business Methods

        public readonly static PropertyInfo<int> IDProperty = RegisterProperty<int>(o => o.ID, "ID");
        public int ID
        {
            get { return GetProperty(IDProperty); }
            set { SetProperty(IDProperty, value); }
        }

        #endregion

        #region Business Rules

        protected override void AddBusinessRules()
        {
            // TODO: add business rules
            //BusinessRules.AddRule(...);
        }

        #endregion

        #region Authorization Rules

        private static void AddObjectAuthorizationRules()
        {
            // TODO: add authorization rules
            //Csla.Rules.BusinessRules.AddRule(...);
        }

        #endregion

        #region Factory Methods

        public static TestEdit NewTestEdit()
        {
            return DataPortal.Create<TestEdit>();
        }

        public static TestEdit GetTestEdit(int id)
        {
            return DataPortal.Fetch<TestEdit>(new SingleCriteria<TestEdit, int>(id));
        }

        public static void DeleteTestEdit(int id)
        {
            DataPortal.Delete<TestEdit>(new SingleCriteria<TestEdit, int>(id));
        }

        public TestEdit()
        { /* Require use of factory methods */ }

        #endregion

        #region Data Access

        [RunLocal]
        protected override void DataPortal_Create()
        {
            base.DataPortal_Create();
        }

        private void DataPortal_Fetch(SingleCriteria<TestEdit, int> criteria)
        {
            LoadProperty(IDProperty, criteria.Value);
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        protected override void DataPortal_Insert()
        {
            // TODO: insert values
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        protected override void DataPortal_Update()
        {
            // TODO: update values
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(new SingleCriteria<TestEdit, int>(this.ID));
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        private void DataPortal_Delete(SingleCriteria<TestEdit, int> criteria)
        {
            // TODO: delete values
        }

        #endregion
    }
}
