using System;
using NHibernate;
using NUnit.Framework;
using ProjectTracker.Library.Tests.Framework;
using ProjectTracker.Library.Tests.ResourceTests;

namespace ProjectTracker.Library.Tests.ProjectTests
{
	#region Test factory class

	/// <summary>
	/// Represents a class that creates saved and unsaved <see cref="Project"/> objects.
	/// </summary>
	internal class ProjectFactory
	{
		/// <summary>
		/// Returns a populated new, unsaved <see cref="Project"/> object.
		/// </summary>
		/// <returns>A <see cref="Project"/> instance object.</returns>
		internal static Project NewProject()
		{
			Project project = Project.NewProject();
			project.Name = "The project name";
			project.Description = "Description of the project";
			return project;
		}

		/// <summary>
		/// Factory method to create a new, unsaved <see cref="Project"/> object
		/// with an associated <see cref="ProjectResource"/> object.
		/// </summary>
		/// <returns>A <see cref="Project"/> instance object.</returns>
		internal static Project NewProjectWithResource()
		{
			// Create a new project
			Project project = NewProject();
			Assert.IsNotNull(project);

			// Use the resource factory to get a resource (already in DB)
			Resource resource = ResourceFactory.ExistingResource();
			Assert.IsNotNull(resource);

			// Assign the resource to the project
			project.Resources.Assign(resource.Id);
			return project;
		}

		/// <summary>
		/// Returns an existing <see cref="Project"/> object.
		/// </summary>
		/// <returns>A <see cref="Project"/> instance object.</returns>
		internal static Project ExistingProject()
		{
			Project project = NewProject();
			project = project.Save();
			return project;
		}

		/// <summary>
		/// Returns an existing <see cref="Project"/> object.
		/// with an associated <see cref="ProjectResource"/> object.
		/// </summary>
		/// <returns>A <see cref="Project"/> instance object.</returns>
		internal static Project ExistingProjectWithResource()
		{
			Project project = NewProjectWithResource();
			project = project.Save();
			return project;
		}


		/// <summary>
		/// Tests for the existence of a <see cref="Project"/>.
		/// </summary>
		/// <param name="projectId">Unique identifier of the <see cref="Project"/>.</param>
		/// <returns>
		/// <lang>true</lang> if the <see cref="Project"/> exists; <lang>false</lang> otherwise.
		/// </returns>
		internal static bool ProjectExists(Guid projectId)
		{
			bool returnValue; 

			// Test if a project exists by trying to get it
			try
			{
				// Try and get the project
				Project project = Project.GetProject(projectId);

				// If we get here then the project is in the DB
				Assert.IsNotNull(project);
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

	#region DeleteProject() method tests

	[TestFixture]
	public class DeleteProject : AuthenticatedProjectManagerTestBase
	{
		[Test]
		public void Existing()
		{
			// Create an existing object
			Project existingProject = ProjectFactory.ExistingProject();

			// Delete the project
			Guid projectId = existingProject.Id;
			Project.DeleteProject(projectId);

			// Check if the Project exists now
			bool exists = ProjectFactory.ProjectExists(projectId);
			Assert.IsFalse(exists);
		}

		[Test]
		public void ExistingWithResource()
		{
			// Get an existing Resource with an Assignment
			Project existingProject = ProjectFactory.ExistingProjectWithResource();

			// Delete the project
			Guid projectId = existingProject.Id;
			Project.DeleteProject(projectId);

			// Check if the Project exists now
			bool exists = ProjectFactory.ProjectExists(projectId);
			Assert.IsFalse(exists);
		}
	}

	#endregion

	#region GetProject() method tests

	/// <summary>
	/// Unit tests for the <see cref="GetProject"/> method.
	/// </summary>
	[TestFixture]
	public class GetProject : AuthenticatedProjectManagerTestBase
	{
		private Project _project = null;

		/// <summary>
		/// Tests result when an invalid identifier is used.
		/// </summary>
		/// <remarks>
		/// Test written with the expectation of an <see cref="ObjectNotFoundException"/>
		/// even though a Data Portal exception will actually occur.
		/// </remarks>
		[Test]
		[ExpectedException(typeof(ObjectNotFoundException))]
		public void InvalidProjectId()
		{
			try
			{
				// Try and get the project using an invalid identifier
				Guid projectId = Guid.Empty;
				_project = Project.GetProject(projectId);
			}
			catch (Exception ex)
			{
				// The [ObjectNotFoundException] is nested inside 2 Data Portal exceptions
				if (!ReferenceEquals(ex.InnerException, null))
					if (!ReferenceEquals(ex.InnerException.InnerException, null))
						throw ex.InnerException.InnerException;
				
				// If there were no inner exceptions then something else went wrong...
				// ...so just throw the original Exception
				throw;
			}
		}

		[Test]
		public void Existing()
		{
			// Create an existing project
			Project existingProject = ProjectFactory.ExistingProject();

			// Get the Project to a different variable
			Guid projectId = existingProject.Id;
			_project = Project.GetProject(projectId);

			// Check objects match
			Assert.IsNotNull(_project);
			Assert.AreEqual(existingProject.Id, _project.Id);

			// Check CSLA properties
			Assert.IsFalse(_project.IsDeleted);
			Assert.IsFalse(_project.IsDirty);
			Assert.IsFalse(_project.IsNew);
			Assert.IsFalse(_project.IsSavable);
			Assert.IsTrue(_project.IsValid);
		}

		[Test]
		public void ExistingWithResource()
		{
			// Create an existing project
			Project existingProject = ProjectFactory.ExistingProjectWithResource();

			// Get the resource
			// Get the Project to a different variable
			Guid projectId = existingProject.Id;
			_project = Project.GetProject(projectId);

			// Check objects match
			Assert.IsNotNull(_project);
			Assert.AreEqual(existingProject.Id, _project.Id);

			// Check CSLA properties
			Assert.IsFalse(_project.IsDeleted);
			Assert.IsFalse(_project.IsDirty);
			Assert.IsFalse(_project.IsNew);
			Assert.IsFalse(_project.IsSavable);
			Assert.IsTrue(_project.IsValid);

			// Check child object
			Assert.IsNotNull(_project.Resources);
			Assert.Greater(_project.Resources.Count, 0);
		}
	}

	#endregion

	#region NewProject() method tests

	/// <summary>
	/// Unit tests for the <see cref="NewProject"/> method.
	/// </summary>
	[TestFixture]
	public class NewProject : AuthenticatedProjectManagerTestBase
	{
		/// <summary>
		/// Unit test for the parameterless <see cref="NewProject"/> factory method.
		/// </summary>
		/// <remarks>
		/// Checks some key CSLA properties are set correctly after initialization.
		/// Values for IsValid and IsSavable are specific to each BO based on the BusinessRules used.
		/// </remarks>
		[Test]
		public void Parameterless()
		{
			Project project = Project.NewProject();
			Assert.IsNotNull(project);
			Assert.IsTrue(project.IsNew);
			Assert.IsTrue(project.IsDirty);
			Assert.IsFalse(project.IsDeleted);
			Assert.IsFalse(project.IsValid);
			Assert.IsFalse(project.IsSavable);
		}
	}

	#endregion

	#region Save() method tests

	/// <summary>
	/// Unit tests for the <see cref="Save"/> method.
	/// </summary>
	[TestFixture]
	public class Save : BusinessTestBase<Project>
	{
		#region Save() method unit tests with a new instance

		[Test]
		public void WithNewInstanceWithNoAssignments()
		{
			BusinessObject = ProjectFactory.NewProject();
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
			BusinessObject.Description = "Description revised by NUnit";
			SaveBusinessObject();

			// Test that the version number has changed
			Assert.AreNotEqual(savedVersion, BusinessObject.Version);
		}

		[Test]
		public void WithNewInstanceWithResource()
		{
			BusinessObject = ProjectFactory.NewProjectWithResource();
			SaveBusinessObject();
		}

		[Test]
		public void WithNewInstanceWithAssignmentsThenEditSameInstance()
		{
			// Execute the code that adds a new instance with a resource
			WithNewInstanceWithResource();

			// Save the version number of the instances
			int savedBusinessObjectVersion = BusinessObject.Version;
			int savedAssignmentVersion = BusinessObject.Resources[0].Version;

			// Change the role of the Resource
			BusinessObject.Resources[0].Role += 1;

			// Check the objects are "dirty"
			Assert.IsTrue(BusinessObject.Resources[0].IsDirty);
			Assert.IsTrue(BusinessObject.IsDirty);

			// Save the BO again
			SaveBusinessObject();

			// Test that the version number is the same for the root BO...
			Assert.AreEqual(savedBusinessObjectVersion, BusinessObject.Version);

			// ... but different for the Assignment that changed
			Assert.AreNotEqual(savedAssignmentVersion, BusinessObject.Resources[0].Version);
		}

		#endregion

		#region Save() method tests with an existing instance with (child) resources doing a deferred delete

		[Test]
		public void ExistingThenDeferredDeleteParent()
		{
			// Get a Project that exists in the database (no children)
			Project project = ProjectFactory.ExistingProject();

			// Save the ID for use later
			Guid projectId = project.Id;

			// Mark the project for "deferred" deletion
			project.Delete();

			// Now call the Save(), which will actually delete the object 
			project = project.Save();
			Assert.IsNotNull(project);

			// Check the instance returned by the Save()...
			// ... has no (child) resources ...
			Assert.AreEqual(0, project.Resources.Count);

			// ... is marked as "new" ...
			Assert.IsTrue(project.IsNew);

			// Check the database to see if the Project exists there
			bool exists = ProjectFactory.ProjectExists(projectId);
			Assert.IsFalse(exists);
		}

		[Test]
		public void ExistingWithResourceThenDeferredDeleteParent()
		{
			// Get a Project that exists in the database (with children)
			Project project = ProjectFactory.ExistingProjectWithResource();

			// Save the ID for use later
			Guid projectId = project.Id;

			// Mark the project for "deferred" deletion
			project.Delete();

			// Now call the Save(), which will actually delete the object
			project = project.Save();
			Assert.IsNotNull(project);

			// Check the instance returned by the Save()...
			// ... has no (child) resources ...
			Assert.AreEqual(0, project.Resources.Count);

			// ... is marked as "new" ...
			Assert.IsTrue(project.IsNew);

			// Check the database to see if the Project exists there
			bool exists = ProjectFactory.ProjectExists(projectId);
			Assert.IsFalse(exists);
		}

		[Test]
		public void ExistingWithResourceThenDeferredDeleteChild()
		{
			// Get an existing object with assignments
			Project project = ProjectFactory.ExistingProjectWithResource();

			// Save the ID for use later
			Guid projectId = project.Id;

			// Remove the (child) Resource (i.e. a child deferred delete)
			int resourceId = project.Resources[0].ResourceId;
			project.Resources.Remove(resourceId);

			// Now call the Save(), which will actually delete the object 
			project = project.Save();
			Assert.IsNotNull(project);

			// Check the instance returned by the Save()...
			// ... has no (child) resources ...
			Assert.AreEqual(0, project.Resources.Count);

			// ... is marked as "old" ...
			Assert.IsFalse(project.IsNew);

			// And finally do a double check, by getting the project from the DB again ...
			project = Project.GetProject(projectId);
			Assert.IsNotNull(project);

			// ... and check there are no (child) resources
			Assert.AreEqual(0, project.Resources.Count);
		}

		#endregion
	}

	#endregion
}