using Csla.NHibernate;
using NUnit.Framework;

namespace ProjectTracker.Library.Tests.Framework
{
	/// <summary>
	/// Abstract base class to provide some standard methods for testing editable Business Objects.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="NHibernateBusinessBase{T}"/>.</typeparam>
	public abstract class BusinessTestBase<T> : AuthenticatedProjectManagerTestBase
		where T: NHibernateBusinessBase<T>
	{
		#region fields

		/// <summary>
		/// Declare a field to hold the BO instance.
		/// </summary>
		private T _businessObject;

		#endregion

		#region properties

		/// <summary>
		/// Gets/sets the Business Object being tested.
		/// </summary>
		protected T BusinessObject
		{
			get { return _businessObject; }
			set { _businessObject = value;}
		}

		#endregion

		#region methods

		/// <summary>
		/// Saves the Business Object.
		/// </summary>
		/// <remarks>
		/// Checks ensure the properties of the BO are correct after the save.
		/// </remarks>
		protected virtual void SaveBusinessObject()
		{
			// Call the standard CSLA Save() method
			BusinessObject = BusinessObject.Save();

			// Make sure an instance BO was returned
			Assert.IsNotNull(BusinessObject);

			// Standard checks after save regardless of whether the original BO was "new" or "old"
			Assert.IsFalse(BusinessObject.IsDirty);
			Assert.IsFalse(BusinessObject.IsNew);
			Assert.IsFalse(BusinessObject.IsSavable);
			Assert.IsTrue(BusinessObject.IsValid);
		}

		#endregion
	}
}
