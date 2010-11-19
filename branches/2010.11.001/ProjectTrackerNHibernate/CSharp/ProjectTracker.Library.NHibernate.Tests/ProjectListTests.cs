using NUnit.Framework;

namespace ProjectTracker.Library.Tests.ProjectListTests
{
	/// <summary>
	/// Unit tests for the <see cref="GetProjectList"/> method.
	/// </summary>
	[TestFixture]
	public class GetProjectList
	{
		[Test]
		public void Parameterless()
		{
			ProjectList projectList = ProjectList.GetProjectList();
			Assert.IsNotNull(projectList);
		}

		[Test]
		public void WithNameParameter()
		{
			string projectName = "A project name goes here";
			ProjectList projectList = ProjectList.GetProjectList(projectName);
			Assert.IsNotNull(projectList);
		}

		[Test]
		public void WithCriteriaParameter()
		{
			ProjectList.Criteria criteria = ProjectList.NewCriteria();
			criteria.Name = "Project 1";
			ProjectList projectList = ProjectList.GetProjectList(criteria);
			Assert.IsNotNull(projectList);
		}
	}
}
