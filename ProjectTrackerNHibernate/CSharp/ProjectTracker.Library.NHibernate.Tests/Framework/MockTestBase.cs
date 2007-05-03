using NMock2;
using NUnit.Framework;

namespace Nfs.Test.Framework.NMock
{
	/// <summary>
	/// Represents an abstract test base class for tests that use NMock and NUnit.
	/// </summary>
	public abstract class MockTestBase
	{
		#region fields

		private Mockery _mockery = null;

		#endregion

		#region properties

		/// <summary>
		/// Gets the NMock <see cref="Mockery"/>.
		/// </summary>
		protected Mockery Mockery
		{
			get { return _mockery; }
		}

		#endregion

		#region virtual methods (with implementation)

		/// <summary>
		/// Creates a new private instance on an NMock <see cref="Mockery"/>.
		/// </summary>
		protected virtual void SetUpMockery()
		{
			_mockery = new Mockery();
		}

		/// <summary>
		/// Verifies all the NMock expectations have been met.
		/// </summary>
		protected virtual void TearDownExpectations()
		{
			Mockery.VerifyAllExpectationsHaveBeenMet();
		}

		#endregion

		#region pure virtual methods (no implementation)

		/// <summary>
		/// Sets up the objects that will be mocked.
		/// </summary>
		protected virtual void SetUpMockObjects() {}

		#endregion

		#region SetUp/TearDown

		/// <summary>
		/// Standard NUnit Setup fixture.
		/// </summary>
		/// <remarks>Calls the following methods:
		/// <see cref="SetUpMockery"/>
		/// <see cref="SetUpMockObjects"/>
		/// </remarks>
		[SetUp]
		public virtual void SetUp()
		{
			SetUpMockery();
			SetUpMockObjects();
		}

		/// <summary>
		/// Standard NUnit TearDown fixture.
		/// </summary>
		/// <remarks>Calls the <see cref="TearDownExpectations"/> method.</remarks>
		[TearDown]
		public virtual void TearDown()
		{
			TearDownExpectations();
		}

		#endregion
	}
}