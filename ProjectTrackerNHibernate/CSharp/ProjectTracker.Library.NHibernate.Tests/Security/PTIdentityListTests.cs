using NUnit.Framework;
using ProjectTracker.Library.Security;
using ProjectTracker.Library.Tests;

namespace ProjectTracker.Library.NHibernate.Tests.Security.PTIdentityListTests
{
	#region GetPTIdentityList() method tests

	/// <summary>
	/// Unit tests for the <see cref="GetPTIdentityList"/> method.
	/// </summary>
	[TestFixture]
	public class GetPTIdentityList
	{
		/// <summary>
		/// Tests a valid username/password combination.
		/// </summary>
		/// <remarks>
		/// Relies on the fact that there is a username/password combination of admin/admin in the database.
		/// </remarks>
		[Test]
		public void ValidUsernameValidPassword()
		{
			PTIdentityList.Criteria criteria = PTIdentityList.NewCriteria();
			criteria.Username = Constants.User.ValidUsername;
			criteria.Password = Constants.User.ValidPassword;
			PTIdentityList identityList = PTIdentityList.GetPTIdentityList(criteria);
			Assert.IsNotNull(identityList);
			Assert.AreEqual(1, identityList.Count);
		}

		[Test]
		public void InvalidUsernameInvalidPassword()
		{
			PTIdentityList.Criteria criteria = PTIdentityList.NewCriteria();
			criteria.Username = Constants.User.InvalidUsername;
			criteria.Password = Constants.User.InvalidPassword;
			PTIdentityList identityList = PTIdentityList.GetPTIdentityList(criteria);
			Assert.IsNotNull(identityList);
			Assert.AreEqual(0, identityList.Count);
		}
	}

	#endregion
}