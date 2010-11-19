using System;
using NHibernate;
using NUnit.Framework;
using ProjectTracker.Library.Tests.Framework;
using ProjectTracker.Library.Tests.ProjectTests;

namespace ProjectTracker.Library.Tests.ResourceTests
{
	#region ResourceFactory class

	/// <summary>
	/// Represents a class that creates saved and unsaved <see cref="Resource"/> objects.
	/// </summary>
	internal class ResourceFactory
	{
		/// <summary>
		/// Returns a populated new, unsaved <see cref="Resource"/> object.
		/// </summary>
		/// <returns>A <see cref="Resource"/> instance object.</returns>
		internal static Resource NewResource()
		{
			Resource resource = Resource.NewResource();
			resource.FirstName = "Mickey";
			resource.LastName = "Mouse";
			return resource;
		}

		/// <summary>
		/// Factory method to create a new, unsaved <see cref="Resource"/> object
		/// with an associated <see cref="Assignment"/> object.
		/// </summary>
		/// <returns>A <see cref="Resource"/> instance object.</returns>
		internal static Resource NewResourceWithAssignment()
		{
			// Use the project factory to get an existing project
			Project project = ProjectFactory.ExistingProject();
			
			// Now create a resource and assign it to the project
			Resource resource = NewResource();
			resource.Assignments.AssignTo(project.Id);
			return resource;
		}

		/// <summary>
		/// Returns an existing <see cref="Resource"/> object.
		/// </summary>
		/// <returns>A <see cref="Resource"/> instance object.</returns>
		internal static Resource ExistingResource()
		{
			Resource resource = NewResource();
			resource = resource.Save();
			return resource;
		}

		/// <summary>
		/// Returns an existing <see cref="Resource"/> object.
		/// with an associated <see cref="Assignment"/> object.
		/// </summary>
		/// <returns>A <see cref="Resource"/> instance object.</returns>
		internal static Resource ExistingResourceWithAssignment()
		{
			Resource resource = NewResourceWithAssignment();
			resource = resource.Save();
			return resource;
		}

		/// <summary>
		/// Tests for the existence of a <see cref="Resource"/>.
		/// </summary>
		/// <param name="resourceId">Unique identifier of the <see cref="Resource"/>.</param>
		/// <returns>
		/// <lang>true</lang> if the <see cref="Resource"/> exists; <lang>false</lang> otherwise.
		/// </returns>
		internal static bool ResourceExists(int resourceId)
		{
			bool returnValue;

			// Test if a Resource exists by trying to get it
			try
			{
				// Try and get the project
				Resource resource = Resource.GetResource(resourceId);

				// If we get here then the project is in the DB
				Assert.IsNotNull(resource);
				returnValue = true;
			}
			catch (Exception ex)
			{
				// The [ObjectNotFoundException] is nested inside 2 Data Portal exceptions
				Exception actualException = ex.InnerException.InnerException;
				Type targetType = typeof(ObjectNotFoundException);

				// If the actual Exception is not the correct type then throw it again
				if (!targetType.IsInstanceOfType(actualException))
					throw;

				// If we're in here then we have the right exception we expected
				returnValue = false;
			}

			return returnValue;
		}
	}

	#endregion

	#region DeleteResource() method tests

	[TestFixture]
	public class DeleteResource : AuthenticatedProjectManagerTestBase
	{
		private Resource _resource = null;

		[Test]
		public void Existing()
		{
			// Get an existing object
			Resource existingResource = ResourceFactory.ExistingResource();
			int resourceId = existingResource.Id;

			Resource.DeleteResource(resourceId);

			// Test that it really was deleted by trying to get it again (which throws an Exception)
			try
			{
				_resource = Resource.GetResource(resourceId);

				// Shouldn't get here - do it means the delete didn't work
				Assert.Fail(String.Format("Resource '{0}' was not deleted", resourceId));
			}
			catch (AssertionException)
			{
				// If here then the Assert.Fail was triggered
				throw;
			}
			catch (Exception ex)
			{
				// The [ObjectNotFoundException] is nested inside 2 Data Portal exceptions
				Exception actualException = ex.InnerException.InnerException;
				Type targetType = typeof (ObjectNotFoundException);

				// If the actual Exception is not the correct type then throw it again
				if (!targetType.IsInstanceOfType(actualException))
					throw;
			}
		}

		[Test]
		public void ExistingWithAssignment()
		{
			// Get an existing Resource with an Assignment
			Resource existingResource = ResourceFactory.ExistingResourceWithAssignment();
			int resourceId = existingResource.Id;

			// Delete the Resource
			Resource.DeleteResource(resourceId);

			// Test that it really was deleted by trying to get it again (which throws an Exception)
			try
			{
				_resource = Resource.GetResource(resourceId);
				Assert.IsNull(_resource); //Prevents R# warning	
			}
			catch (Exception ex)
			{
				// The [ObjectNotFoundException] is nested inside 2 Data Portal exceptions
				Exception actualException = ex.InnerException.InnerException;
				Type targetType = typeof (ObjectNotFoundException);

				// If the actual Exception is not the correct type then throw it again
				if (!targetType.IsInstanceOfType(actualException))
					throw;
			}
		}
	}

	#endregion

	#region GetResource() method tests

	/// <summary>
	/// Unit tests for the <see cref="GetResource"/> method.
	/// </summary>
	[TestFixture]
	public class GetResource : AuthenticatedProjectManagerTestBase
	{
		private Resource _resource = null;

		[Test]
		public void Existing()
		{
			Resource existingResource = ResourceFactory.ExistingResource();

			// Get the Resource
			int resourceId = existingResource.Id;
			_resource = Resource.GetResource(resourceId);

			// Checks
			Assert.IsNotNull(_resource);
			Assert.AreEqual(existingResource.Id, _resource.Id);
		}

		[Test]
		public void ExistingWithAssignment()
		{
			Resource existingResource = ResourceFactory.ExistingResourceWithAssignment();
			int resourceId = existingResource.Id;

			// Get the resource
			_resource = Resource.GetResource(resourceId);

			// Checks
			Assert.IsNotNull(_resource);
			Assert.AreEqual(existingResource.Id, _resource.Id);
			Assert.IsNotNull(_resource.Assignments);
			Assert.Greater(_resource.Assignments.Count, 0);
		}
	}

	#endregion

	#region NewResource() method tests

	/// <summary>
	/// Unit tests for the <see cref="NewResource"/> method.
	/// </summary>
	[TestFixture]
	public class NewResource : BusinessTestBase<Resource>
	{
		/// <summary>
		/// Unit test for the parameterless <see cref="NewResource"/> factory method.
		/// </summary>
		/// <remarks>
		/// Checks some key CSLA properties are set correctly after initialization.
		/// Values for IsValid and IsSavable are specific to each BO based on the BusinessRules used.
		/// </remarks>
		[Test]
		public void Parameterless()
		{
			Resource resource = Resource.NewResource();
			Assert.IsNotNull(resource);
			Assert.IsTrue(resource.IsNew);
			Assert.IsTrue(resource.IsDirty);
			Assert.IsFalse(resource.IsDeleted);
			Assert.IsFalse(resource.IsValid);
			Assert.IsFalse(resource.IsSavable);
		}
	}

	#endregion

	#region Save() method tests

	/// <summary>
	/// Unit tests for the <see cref="Save"/> method.
	/// </summary>
	[TestFixture]
	public class Save : BusinessTestBase<Resource>
	{
		#region Save() method unit tests with a new instance

		[Test]
		public void WithNewInstanceWithNoAssignments()
		{
			// Get a new resource from the factory ...
			BusinessObject = ResourceFactory.NewResource();

			// ... and save it
			SaveBusinessObject();
		}

		/// <summary>
		/// This test verifies the NHibernate version control is working correctly.
		/// </summary>
		[Test]
		public void WithNewInstanceWithNoAssignmentsThenEditSameInstance()
		{
			// Execute the code that adds a new instance
			WithNewInstanceWithNoAssignments();

			// Save the version number
			int savedVersion = BusinessObject.Version;

			// Change some data and save it
			BusinessObject.FirstName = "Daffy";
			SaveBusinessObject();

			// Test that the version number has changed
			Assert.AreNotEqual(savedVersion, BusinessObject.Version);
		}

		[Test]
		public void WithNewInstanceWithAssignments()
		{
			// Get a new resource from the factory ...
			BusinessObject = ResourceFactory.NewResourceWithAssignment();
			
			// ... and save it
			SaveBusinessObject();
		}

		[Test]
		public void WithNewInstanceWithAssignmentsThenEditSameInstance()
		{
			// Execute the code that adds a new instance with assignments
			WithNewInstanceWithAssignments();

			// Save the version number of the instances
			int savedBusinessObjectVersion = BusinessObject.Version;
			int savedAssignmentVersion = BusinessObject.Assignments[0].Version;

			// Change an Assignment
			BusinessObject.Assignments[0].Role += 1;

			// Check the objects are "dirty"
			Assert.IsTrue(BusinessObject.Assignments[0].IsDirty);
			Assert.IsTrue(BusinessObject.IsDirty);

			// Save the BO again
			SaveBusinessObject();

			// Test that the version number is the same for the root BO...
			Assert.AreEqual(savedBusinessObjectVersion, BusinessObject.Version);

			// ... but different for the Assignment that changed
			Assert.AreNotEqual(savedAssignmentVersion, BusinessObject.Assignments[0].Version);
		}

		#endregion

		#region Save() method tests with an existing instance with (child) assignments doing a deferred delete

		[Test]
		public void ExistingThenDeferredDeleteParent()
		{
			// Get a Project that exists in the database (no children)
			Resource resource = ResourceFactory.ExistingResource();

			// Save the ID for use later
			int resourceId = resource.Id;

			// Mark the project for "deferred" deletion
			resource.Delete();

			// Now call the Save(), which will actually delete the object 
			resource = resource.Save();
			Assert.IsNotNull(resource);

			// Check the instance returned by the Save()...
			// ... has no (child) resources ...
			Assert.AreEqual(0, resource.Assignments.Count);

			// ... is marked as "new" ...
			Assert.IsTrue(resource.IsNew);

			// Check the database to see if the Resource exists there
			bool exists = ResourceFactory.ResourceExists(resourceId);
			Assert.IsFalse(exists);
		}

		[Test]
		public void ExistingWithAssignmentThenDeferredDeleteParent()
		{
			// Get a Resource that exists in the database (with children)
			Resource resource = ResourceFactory.ExistingResourceWithAssignment();

			// Save the ID for use later
			int resourceId = resource.Id;

			// Mark the project for "deferred" deletion
			resource.Delete();

			// Now call the Save(), which will actually delete the object
			resource = resource.Save();
			Assert.IsNotNull(resource);

			// Check the instance returned by the Save()...
			// ... has no (child) resources ...
			Assert.AreEqual(0, resource.Assignments.Count);

			// ... is marked as "new" ...
			Assert.IsTrue(resource.IsNew);

			// Check the database to see if the Resource exists there
			bool exists = ResourceFactory.ResourceExists(resourceId);
			Assert.IsFalse(exists);
		}

		[Test]
		public void ExistingWithAssignmentThenDeferredDeleteChild()
		{
			// Get a Resource that exists in the database (with children)
			Resource resource = ResourceFactory.ExistingResourceWithAssignment();

			// Save the ID for use later
			int resourceId = resource.Id;

			// Remove the (child) Project (i.e. a child deferred delete)
			Guid projectId = resource.Assignments[0].ProjectId;
			resource.Assignments.Remove(projectId);

			// Now call the Save(), which will actually delete the object 
			resource = resource.Save();
			Assert.IsNotNull(resource);

			// Check the instance returned by the Save()...
			// ... has no (child) assignments ...
			Assert.AreEqual(0, resource.Assignments.Count);

			// ... is marked as "old" ...
			Assert.IsFalse(resource.IsNew);

			// And finally do a double check, by getting the project from the DB again ...
			resource = Resource.GetResource(resourceId);
			Assert.IsNotNull(resource);

			// ... and check there are no (child) resources
			Assert.AreEqual(0, resource.Assignments.Count);
		}

		#endregion

	}

	#endregion
}