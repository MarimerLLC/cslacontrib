using Csla;
using System;

namespace CustomFieldData
{
	[Serializable]
	public sealed class Person : BusinessCore<Person>
	{
		private static PropertyInfo<int> ageProperty = 
      RegisterProperty<int>("Age");
		private static PropertyInfo<string> firstNameProperty = 
      RegisterProperty<string>("FirstName", "Friendly first name");
	  private static PropertyInfo<string> lastNameProperty = 
      RegisterProperty<string>("LastName", "Friendly lastname", String.Empty);

		private Person()
			: base()
		{
		}

// ReSharper disable UnusedMember.Local
		private void DataPortal_Fetch(SingleCriteria<Person, int> criteria)
// ReSharper restore UnusedMember.Local
		{
			if(criteria.Value != 2047)
			{
				throw new NotSupportedException();
			}
			
			using(this.BypassPropertyChecks)
			{
				this.Age = 22;
				this.FirstName = "Joe";
				this.LastName = "Smith";
			}
		}

		protected override void DataPortal_Update()
		{
		}

		public static Person Fetch(int id)
		{
			return DataPortal.Fetch<Person>(new SingleCriteria<Person, int>(id));
		}

		public int Age
		{
			get
			{
				return this.GetProperty(Person.ageProperty);
			}
			set
			{
				this.SetProperty(Person.ageProperty, value);
			}
		}

		public string FirstName
		{
			get
			{
				return this.GetProperty(Person.firstNameProperty);
			}
			set
			{
				this.SetProperty(Person.firstNameProperty, value);
			}
		}

		public string LastName
		{
			get
			{
				return this.GetProperty(Person.lastNameProperty);
			}
			set
			{
				this.SetProperty(Person.lastNameProperty, value);
			}
		}
	}
}
