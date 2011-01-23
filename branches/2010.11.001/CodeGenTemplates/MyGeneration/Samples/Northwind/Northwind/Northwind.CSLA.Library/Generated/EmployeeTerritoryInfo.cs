
using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Configuration;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
namespace Northwind.CSLA.Library
{
	public delegate void EmployeeTerritoryInfoEvent(object sender);
	/// <summary>
	///	EmployeeTerritoryInfo Generated by MyGeneration using the CSLA Object Mapping template
	/// </summary>
	[Serializable()]
	[TypeConverter(typeof(EmployeeTerritoryInfoConverter))]
	public partial class EmployeeTerritoryInfo : ReadOnlyBase<EmployeeTerritoryInfo>, IDisposable
	{
		public event EmployeeTerritoryInfoEvent Changed;
		private void OnChange()
		{
			if (Changed != null) Changed(this);
		}
		#region Collection
		protected static List<EmployeeTerritoryInfo> _AllList = new List<EmployeeTerritoryInfo>();
		private static Dictionary<string, EmployeeTerritoryInfo> _AllByPrimaryKey = new Dictionary<string, EmployeeTerritoryInfo>();
		private static void ConvertListToDictionary()
		{
			List<EmployeeTerritoryInfo> remove = new List<EmployeeTerritoryInfo>();
			foreach (EmployeeTerritoryInfo tmp in _AllList)
			{
				_AllByPrimaryKey[tmp.EmployeeID.ToString() + "_" + tmp.TerritoryID.ToString()]=tmp; // Primary Key
				remove.Add(tmp);
			}
			foreach (EmployeeTerritoryInfo tmp in remove)
				_AllList.Remove(tmp);
		}
		internal static void AddList(EmployeeTerritoryInfoList lst)
		{
			foreach (EmployeeTerritoryInfo item in lst) _AllList.Add(item);
		}
		public static EmployeeTerritoryInfo GetExistingByPrimaryKey(int employeeID, string territoryID)
		{
			ConvertListToDictionary();
			string key = employeeID.ToString() + "_" + territoryID.ToString();
			if (_AllByPrimaryKey.ContainsKey(key)) return _AllByPrimaryKey[key]; 
			return null;
		}
		#endregion
		#region Business Methods
		private string _ErrorMessage = string.Empty;
		public string ErrorMessage
		{
			get { return _ErrorMessage; }
		}
		protected EmployeeTerritory _Editable;
		private IVEHasBrokenRules HasBrokenRules
		{
			get
			{
				IVEHasBrokenRules hasBrokenRules = null;
				if (_Editable != null)
					hasBrokenRules = _Editable.HasBrokenRules;
				return hasBrokenRules;
			}
		}
		private int _EmployeeID;
		[System.ComponentModel.DataObjectField(true, true)]
		public int EmployeeID
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				if (_MyEmployee != null) _EmployeeID = _MyEmployee.EmployeeID;
				return _EmployeeID;
			}
		}
		private EmployeeInfo _MyEmployee;
		[System.ComponentModel.DataObjectField(true, true)]
		public EmployeeInfo MyEmployee
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				if (_MyEmployee == null && _EmployeeID != 0) _MyEmployee = EmployeeInfo.Get(_EmployeeID);
				return _MyEmployee;
			}
		}
		private string _TerritoryID = string.Empty;
		[System.ComponentModel.DataObjectField(true, true)]
		public string TerritoryID
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				if (_MyTerritory != null) _TerritoryID = _MyTerritory.TerritoryID;
				return _TerritoryID;
			}
		}
		private TerritoryInfo _MyTerritory;
		[System.ComponentModel.DataObjectField(true, true)]
		public TerritoryInfo MyTerritory
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				if (_MyTerritory == null && _TerritoryID != null) _MyTerritory = TerritoryInfo.Get(_TerritoryID);
				return _MyTerritory;
			}
		}
		// TODO: Replace base EmployeeTerritoryInfo.ToString function as necessary
		/// <summary>
		/// Overrides Base ToString
		/// </summary>
		/// <returns>A string representation of current EmployeeTerritoryInfo</returns>
		//public override string ToString()
		//{
		//  return base.ToString();
		//}
		// TODO: Check EmployeeTerritoryInfo.GetIdValue to assure that the ID returned is unique
		/// <summary>
		/// Overrides Base GetIdValue - Used internally by CSLA to determine equality
		/// </summary>
		/// <returns>A Unique ID for the current EmployeeTerritoryInfo</returns>
		protected override object GetIdValue()
		{
			return (_EmployeeID.ToString()+"."+_TerritoryID.ToString()).GetHashCode();
		}
		#endregion
		#region Factory Methods
		private EmployeeTerritoryInfo()
		{/* require use of factory methods */
			_AllList.Add(this);
		}
		public void Dispose()
		{
			_AllList.Remove(this);
			_AllByPrimaryKey.Remove(EmployeeID.ToString() + "_" + TerritoryID.ToString());
		}
		public virtual EmployeeTerritory Get()
		{
			return _Editable = EmployeeTerritory.Get(_EmployeeID, _TerritoryID);
		}
		public static void Refresh(EmployeeTerritory tmp)
		{
			EmployeeTerritoryInfo tmpInfo = GetExistingByPrimaryKey(tmp.EmployeeID, tmp.TerritoryID);
			if (tmpInfo == null) return;
			tmpInfo.RefreshFields(tmp);
		}
		private void RefreshFields(EmployeeTerritory tmp)
		{
			_EmployeeTerritoryInfoExtension.Refresh(this);
			_MyEmployee = null;
			_MyTerritory = null;
			OnChange();// raise an event
		}
		public static void Refresh(Employee myEmployee, EmployeeEmployeeTerritory tmp)
		{
			EmployeeTerritoryInfo tmpInfo = GetExistingByPrimaryKey(myEmployee.EmployeeID, tmp.TerritoryID);
			if (tmpInfo == null) return;
			tmpInfo.RefreshFields(tmp);
		}
		private void RefreshFields(EmployeeEmployeeTerritory tmp)
		{
			_EmployeeTerritoryInfoExtension.Refresh(this);
			_MyEmployee = null;
			_MyTerritory = null;
			OnChange();// raise an event
		}
		public static void Refresh(Territory myTerritory, TerritoryEmployeeTerritory tmp)
		{
			EmployeeTerritoryInfo tmpInfo = GetExistingByPrimaryKey(tmp.EmployeeID, myTerritory.TerritoryID);
			if (tmpInfo == null) return;
			tmpInfo.RefreshFields(tmp);
		}
		private void RefreshFields(TerritoryEmployeeTerritory tmp)
		{
			_EmployeeTerritoryInfoExtension.Refresh(this);
			_MyEmployee = null;
			_MyTerritory = null;
			OnChange();// raise an event
		}
		public static EmployeeTerritoryInfo Get(int employeeID, string territoryID)
		{
			//if (!CanGetObject())
			//  throw new System.Security.SecurityException("User not authorized to view a EmployeeTerritory");
			try
			{
				EmployeeTerritoryInfo tmp = GetExistingByPrimaryKey(employeeID, territoryID);
				if (tmp == null)
				{
					tmp = DataPortal.Fetch<EmployeeTerritoryInfo>(new PKCriteria(employeeID, territoryID));
					_AllList.Add(tmp);
				}
				if (tmp.ErrorMessage == "No Record Found") tmp = null;
				return tmp;
			}
			catch (Exception ex)
			{
				throw new DbCslaException("Error on EmployeeTerritoryInfo.Get", ex);
			}
		}
		#endregion
		#region Data Access Portal
		internal EmployeeTerritoryInfo(SafeDataReader dr)
		{
			Database.LogInfo("EmployeeTerritoryInfo.Constructor", GetHashCode());
			try
			{
				ReadData(dr);
			}
			catch (Exception ex)
			{
				Database.LogException("EmployeeTerritoryInfo.Constructor", ex);
				throw new DbCslaException("EmployeeTerritoryInfo.Constructor", ex);
			}
		}
		[Serializable()]
		protected class PKCriteria
		{
			private int _EmployeeID;
			public int EmployeeID
			{ get { return _EmployeeID; } }
			private string _TerritoryID;
			public string TerritoryID
			{ get { return _TerritoryID; } }
			public PKCriteria(int employeeID, string territoryID)
			{
				_EmployeeID = employeeID;
				_TerritoryID = territoryID;
			}
		}
		private void ReadData(SafeDataReader dr)
		{
			Database.LogInfo("EmployeeTerritoryInfo.ReadData", GetHashCode());
			try
			{
				_EmployeeID = dr.GetInt32("EmployeeID");
				_TerritoryID = dr.GetString("TerritoryID");
			}
			catch (Exception ex)
			{
				Database.LogException("EmployeeTerritoryInfo.ReadData", ex);
				_ErrorMessage = ex.Message;
				throw new DbCslaException("EmployeeTerritoryInfo.ReadData", ex);
			}
		}
		private void DataPortal_Fetch(PKCriteria criteria)
		{
			Database.LogInfo("EmployeeTerritoryInfo.DataPortal_Fetch", GetHashCode());
			try
			{
				using (SqlConnection cn = Database.Northwind_SqlConnection)
				{
					ApplicationContext.LocalContext["cn"] = cn;
					using (SqlCommand cm = cn.CreateCommand())
					{
						cm.CommandType = CommandType.StoredProcedure;
						cm.CommandText = "getEmployeeTerritory";
						cm.Parameters.AddWithValue("@EmployeeID", criteria.EmployeeID);
						cm.Parameters.AddWithValue("@TerritoryID", criteria.TerritoryID);
						using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
						{
							if (!dr.Read())
							{
								_ErrorMessage = "No Record Found";
								return;
							}
							ReadData(dr);
						}
					}
					// removing of item only needed for local data portal
					if (ApplicationContext.ExecutionLocation == ApplicationContext.ExecutionLocations.Client)
						ApplicationContext.LocalContext.Remove("cn");
				}
			}
			catch (Exception ex)
			{
				Database.LogException("EmployeeTerritoryInfo.DataPortal_Fetch", ex);
				_ErrorMessage = ex.Message;
				throw new DbCslaException("EmployeeTerritoryInfo.DataPortal_Fetch", ex);
			}
		}
		#endregion
		// Standard Refresh
		#region extension
		EmployeeTerritoryInfoExtension _EmployeeTerritoryInfoExtension = new EmployeeTerritoryInfoExtension();
		[Serializable()]
		partial class EmployeeTerritoryInfoExtension : extensionBase {}
		[Serializable()]
		class extensionBase
		{
			// Default Refresh
			public virtual void Refresh(EmployeeTerritoryInfo tmp) { }
		}
		#endregion
	} // Class
	#region Converter
	internal class EmployeeTerritoryInfoConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
		{
			if (destType == typeof(string) && value is EmployeeTerritoryInfo)
			{
				// Return the ToString value
				return ((EmployeeTerritoryInfo)value).ToString();
			}
			return base.ConvertTo(context, culture, value, destType);
		}
	}
	#endregion
} // Namespace