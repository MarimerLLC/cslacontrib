using System;
using Csla;
using CslaContrib.ObjectCaching;

namespace CslaContrib.UnitTests.ObjectCaching
{
    [Serializable]
    [ObjectCacheEviction(Scope = CacheScope.Group, CachedTypes = new Type[] { typeof(TestCachedGroupInfo) })]
    public class TestGroupEdit : BusinessBase<TestGroupEdit>
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

        public static TestGroupEdit NewTestGroupEdit()
        {
            return DataPortal.Create<TestGroupEdit>();
        }

        public static TestGroupEdit GetTestGroupEdit(int id)
        {
            return DataPortal.Fetch<TestGroupEdit>(new SingleCriteria<TestGroupEdit, int>(id));
        }

        public static void DeleteTestGroupEdit(int id)
        {
            DataPortal.Delete<TestGroupEdit>(new SingleCriteria<TestGroupEdit, int>(id));
        }

        public TestGroupEdit()
        { /* Require use of factory methods */ }

        #endregion

        #region Data Access

        [RunLocal]
        protected override void DataPortal_Create()
        {
            // TODO: load default values
            // omit this override if you have no defaults to set
            base.DataPortal_Create();
        }

        private void DataPortal_Fetch(SingleCriteria<TestGroupEdit, int> criteria)
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
            DataPortal_Delete(new SingleCriteria<TestGroupEdit, int>(this.ID));
        }

        [Transactional(TransactionalTypes.TransactionScope)]
        private void DataPortal_Delete(SingleCriteria<TestGroupEdit, int> criteria)
        {
            // TODO: delete values
        }

        #endregion
    }
}
