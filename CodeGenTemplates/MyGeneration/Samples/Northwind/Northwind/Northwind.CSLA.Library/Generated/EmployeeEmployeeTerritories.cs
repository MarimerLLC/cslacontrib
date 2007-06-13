
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Configuration;
using System.IO;
using System.ComponentModel;
using Csla.Validation;
namespace Northwind.CSLA.Library
{
	/// <summary>
	///	EmployeeEmployeeTerritories Generated by MyGeneration using the CSLA Object Mapping template
	/// </summary>
	[Serializable()]
	[TypeConverter(typeof(EmployeeEmployeeTerritoriesConverter))]
	public partial class EmployeeEmployeeTerritories : BusinessListBase<EmployeeEmployeeTerritories, EmployeeEmployeeTerritory>, ICustomTypeDescriptor, IVEHasBrokenRules
	{
		#region Business Methods
		private string _ErrorMessage = string.Empty;
		public string ErrorMessage
		{
			get { return _ErrorMessage; }
		}
		// Many To Many
		public EmployeeEmployeeTerritory this[Territory myTerritory]
		{
			get
			{
				foreach (EmployeeEmployeeTerritory employeeTerritory in this)
					if (employeeTerritory.TerritoryID == myTerritory.TerritoryID)
						return employeeTerritory;
				return null;
			}
		}
		public new System.Collections.Generic.IList<EmployeeEmployeeTerritory> Items
		{
			get { return base.Items; }
		}
		public EmployeeEmployeeTerritory GetItem(Territory myTerritory)
		{
			foreach (EmployeeEmployeeTerritory employeeTerritory in this)
				if (employeeTerritory.TerritoryID == myTerritory.TerritoryID)
					return employeeTerritory;
			return null;
		}
		public EmployeeEmployeeTerritory Add(Territory myTerritory)// Many to Many with required fields
		{
			if (!Contains(myTerritory))
			{
				EmployeeEmployeeTerritory employeeTerritory =	EmployeeEmployeeTerritory.New(myTerritory);
				this.Add(employeeTerritory);
				return employeeTerritory;
			}
			else
				throw new InvalidOperationException("employeeTerritory already exists");
		}
		public void Remove(Territory myTerritory)
		{
			foreach (EmployeeEmployeeTerritory employeeTerritory in this)
			{
				if (employeeTerritory.TerritoryID == myTerritory.TerritoryID)
				{
					Remove(employeeTerritory);
					break;
				}
			}
		}
		public bool Contains(Territory myTerritory)
		{
			foreach (EmployeeEmployeeTerritory employeeTerritory in this)
				if (employeeTerritory.TerritoryID == myTerritory.TerritoryID)
					return true;
			return false;
		}
		public bool ContainsDeleted(Territory myTerritory)
		{
			foreach (EmployeeEmployeeTerritory employeeTerritory in DeletedList)
				if (employeeTerritory.TerritoryID == myTerritory.TerritoryID)
					return true;
			return false;
		}
		#endregion
		#region ValidationRules
		public IVEHasBrokenRules HasBrokenRules
		{
			get
			{
				IVEHasBrokenRules hasBrokenRules=null;
				foreach(EmployeeEmployeeTerritory employeeEmployeeTerritory in this)
					if ((hasBrokenRules = employeeEmployeeTerritory.HasBrokenRules) != null) return hasBrokenRules;
				return hasBrokenRules;
			}
		}
		public BrokenRulesCollection BrokenRules
		{
			get
			{
			IVEHasBrokenRules hasBrokenRules = HasBrokenRules;
			return (hasBrokenRules != null ? hasBrokenRules.BrokenRules : null);
			}
		}
		#endregion
		#region Factory Methods
		internal static EmployeeEmployeeTerritories New()
		{
			return new EmployeeEmployeeTerritories();
		}
		internal static EmployeeEmployeeTerritories Get(SafeDataReader dr)
		{
			return new EmployeeEmployeeTerritories(dr);
		}
		public static EmployeeEmployeeTerritories GetByEmployeeID(int employeeID)
		{
			try
			{
				return DataPortal.Fetch<EmployeeEmployeeTerritories>(new EmployeeIDCriteria(employeeID));
			}
			catch (Exception ex)
			{
				throw new DbCslaException("Error on EmployeeEmployeeTerritories.GetByEmployeeID", ex);
			}
		}
		private EmployeeEmployeeTerritories()
		{
			MarkAsChild();
		}
		internal EmployeeEmployeeTerritories(SafeDataReader dr)
		{
			MarkAsChild();
			Fetch(dr);
		}
		#endregion
		#region Data Access Portal
		// called to load data from the database
		private void Fetch(SafeDataReader dr)
		{
			this.RaiseListChangedEvents = false;
			while (dr.Read())
				this.Add(EmployeeEmployeeTerritory.Get(dr));
			this.RaiseListChangedEvents = true;
		}
		[Serializable()]
		private class EmployeeIDCriteria
		{
			public EmployeeIDCriteria(int employeeID)
			{
				_EmployeeID = employeeID;
			}
			private int _EmployeeID;
			public int EmployeeID
			{
				get { return _EmployeeID; }
				set { _EmployeeID = value; }
			}
		}
		private void DataPortal_Fetch(EmployeeIDCriteria criteria)
		{
			this.RaiseListChangedEvents = false;
			Database.LogInfo("EmployeeEmployeeTerritories.DataPortal_FetchEmployeeID", GetHashCode());
			try
			{
				using (SqlConnection cn = Database.Northwind_SqlConnection)
				{
					using (SqlCommand cm = cn.CreateCommand())
					{
						cm.CommandType = CommandType.StoredProcedure;
						cm.CommandText = "getEmployeeTerritoriesByEmployeeID";
						cm.Parameters.AddWithValue("@EmployeeID", criteria.EmployeeID);
						using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
						{
							while (dr.Read()) this.Add(new EmployeeEmployeeTerritory(dr));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Database.LogException("EmployeeEmployeeTerritories.DataPortal_FetchEmployeeID", ex);
				throw new DbCslaException("EmployeeEmployeeTerritories.DataPortal_Fetch", ex);
			}
			this.RaiseListChangedEvents = true;
		}
		internal void Update(Employee employee)
		{
			this.RaiseListChangedEvents = false;
			try
			{
				// update (thus deleting) any deleted child objects
				foreach (EmployeeEmployeeTerritory obj in DeletedList)
					obj.Delete();// TODO: Should this be SQLDelete
				// now that they are deleted, remove them from memory too
				DeletedList.Clear();
				// add/update any current child objects
				foreach (EmployeeEmployeeTerritory obj in this)
				{
					if (obj.IsNew)
						obj.Insert(employee);
					else
						obj.Update(employee);
				}
			}
			finally
			{
				this.RaiseListChangedEvents = true;
			}
		}
		#endregion
		#region ICustomTypeDescriptor impl
		public String GetClassName()
		{ return TypeDescriptor.GetClassName(this, true); }
		public AttributeCollection GetAttributes()
		{ return TypeDescriptor.GetAttributes(this, true); }
		public String GetComponentName()
		{ return TypeDescriptor.GetComponentName(this, true); }
		public TypeConverter GetConverter()
		{ return TypeDescriptor.GetConverter(this, true); }
		public EventDescriptor GetDefaultEvent()
		{ return TypeDescriptor.GetDefaultEvent(this, true); }
		public PropertyDescriptor GetDefaultProperty()
		{ return TypeDescriptor.GetDefaultProperty(this, true); }
		public object GetEditor(Type editorBaseType)
		{ return TypeDescriptor.GetEditor(this, editorBaseType, true); }
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{ return TypeDescriptor.GetEvents(this, attributes, true); }
		public EventDescriptorCollection GetEvents()
		{ return TypeDescriptor.GetEvents(this, true); }
		public object GetPropertyOwner(PropertyDescriptor pd)
		{ return this; }
		/// <summary>
		/// Called to get the properties of this type. Returns properties with certain
		/// attributes. this restriction is not implemented here.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{ return GetProperties(); }
		/// <summary>
		/// Called to get the properties of this type.
		/// </summary>
		/// <returns></returns>
		public PropertyDescriptorCollection GetProperties()
		{
			// Create a collection object to hold property descriptors
			PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
			// Iterate the list 
			for (int i = 0; i < this.Items.Count; i++)
			{
				// Create a property descriptor for the item and add to the property descriptor collection
				EmployeeEmployeeTerritoriesPropertyDescriptor pd = new EmployeeEmployeeTerritoriesPropertyDescriptor(this, i);
				pds.Add(pd);
			}
			// return the property descriptor collection
			return pds;
		}
		#endregion
	} // Class
	#region Property Descriptor
	/// <summary>
	/// Summary description for CollectionPropertyDescriptor.
	/// </summary>
	public partial class EmployeeEmployeeTerritoriesPropertyDescriptor : vlnListPropertyDescriptor
	{
		private EmployeeEmployeeTerritory Item { get { return (EmployeeEmployeeTerritory) _Item;} }
		public EmployeeEmployeeTerritoriesPropertyDescriptor(EmployeeEmployeeTerritories collection, int index):base(collection, index){;}
	}
	#endregion
	#region Converter
	internal class EmployeeEmployeeTerritoriesConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
		{
			if (destType == typeof(string) && value is EmployeeEmployeeTerritories)
			{
				// Return department and department role separated by comma.
				return ((EmployeeEmployeeTerritories) value).Items.Count.ToString() + " EmployeeTerritories";
			}
			return base.ConvertTo(context, culture, value, destType);
		}
	}
	#endregion
} // Namespace