
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
	public delegate void ShipperInfoEvent(object sender);
	/// <summary>
	///	ShipperInfo Generated by MyGeneration using the CSLA Object Mapping template
	/// </summary>
	[Serializable()]
	[TypeConverter(typeof(ShipperInfoConverter))]
	public partial class ShipperInfo : ReadOnlyBase<ShipperInfo>, IDisposable
	{
		public event ShipperInfoEvent Changed;
		private void OnChange()
		{
			if (Changed != null) Changed(this);
		}
		#region Collection
		protected static List<ShipperInfo> _AllList = new List<ShipperInfo>();
		private static Dictionary<string, ShipperInfo> _AllByPrimaryKey = new Dictionary<string, ShipperInfo>();
		private static void ConvertListToDictionary()
		{
			List<ShipperInfo> remove = new List<ShipperInfo>();
			foreach (ShipperInfo tmp in _AllList)
			{
				_AllByPrimaryKey[tmp.ShipperID.ToString()]=tmp; // Primary Key
				remove.Add(tmp);
			}
			foreach (ShipperInfo tmp in remove)
				_AllList.Remove(tmp);
		}
		internal static void AddList(ShipperInfoList lst)
		{
			foreach (ShipperInfo item in lst) _AllList.Add(item);
		}
		public static ShipperInfo GetExistingByPrimaryKey(int shipperID)
		{
			ConvertListToDictionary();
			string key = shipperID.ToString();
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
		protected Shipper _Editable;
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
		private int _ShipperID;
		[System.ComponentModel.DataObjectField(true, true)]
		public int ShipperID
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _ShipperID;
			}
		}
		private string _CompanyName = string.Empty;
		public string CompanyName
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _CompanyName;
			}
		}
		private string _Phone = string.Empty;
		public string Phone
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _Phone;
			}
		}
		private int _ShipperOrderCount = 0;
		/// <summary>
		/// Count of ShipperOrders for this Shipper
		/// </summary>
		public int ShipperOrderCount
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _ShipperOrderCount;
			}
		}
		private OrderInfoList _ShipperOrders = null;
		[TypeConverter(typeof(OrderInfoListConverter))]
		public OrderInfoList ShipperOrders
		{
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				if (_ShipperOrderCount > 0 && _ShipperOrders == null)
					_ShipperOrders = OrderInfoList.GetByShipVia(_ShipperID);
				return _ShipperOrders;
			}
		}
		// TODO: Replace base ShipperInfo.ToString function as necessary
		/// <summary>
		/// Overrides Base ToString
		/// </summary>
		/// <returns>A string representation of current ShipperInfo</returns>
		//public override string ToString()
		//{
		//  return base.ToString();
		//}
		// TODO: Check ShipperInfo.GetIdValue to assure that the ID returned is unique
		/// <summary>
		/// Overrides Base GetIdValue - Used internally by CSLA to determine equality
		/// </summary>
		/// <returns>A Unique ID for the current ShipperInfo</returns>
		protected override object GetIdValue()
		{
			return _ShipperID;
		}
		#endregion
		#region Factory Methods
		private ShipperInfo()
		{/* require use of factory methods */
			_AllList.Add(this);
		}
		public void Dispose()
		{
			_AllList.Remove(this);
			_AllByPrimaryKey.Remove(ShipperID.ToString());
		}
		public virtual Shipper Get()
		{
			return _Editable = Shipper.Get(_ShipperID);
		}
		public static void Refresh(Shipper tmp)
		{
			ShipperInfo tmpInfo = GetExistingByPrimaryKey(tmp.ShipperID);
			if (tmpInfo == null) return;
			tmpInfo.RefreshFields(tmp);
		}
		private void RefreshFields(Shipper tmp)
		{
			_CompanyName = tmp.CompanyName;
			_Phone = tmp.Phone;
			_ShipperInfoExtension.Refresh(this);
			OnChange();// raise an event
		}
		public static ShipperInfo Get(int shipperID)
		{
			//if (!CanGetObject())
			//  throw new System.Security.SecurityException("User not authorized to view a Shipper");
			try
			{
				ShipperInfo tmp = GetExistingByPrimaryKey(shipperID);
				if (tmp == null)
				{
					tmp = DataPortal.Fetch<ShipperInfo>(new PKCriteria(shipperID));
					_AllList.Add(tmp);
				}
				if (tmp.ErrorMessage == "No Record Found") tmp = null;
				return tmp;
			}
			catch (Exception ex)
			{
				throw new DbCslaException("Error on ShipperInfo.Get", ex);
			}
		}
		#endregion
		#region Data Access Portal
		internal ShipperInfo(SafeDataReader dr)
		{
			Database.LogInfo("ShipperInfo.Constructor", GetHashCode());
			try
			{
				ReadData(dr);
			}
			catch (Exception ex)
			{
				Database.LogException("ShipperInfo.Constructor", ex);
				throw new DbCslaException("ShipperInfo.Constructor", ex);
			}
		}
		[Serializable()]
		protected class PKCriteria
		{
			private int _ShipperID;
			public int ShipperID
			{ get { return _ShipperID; } }
			public PKCriteria(int shipperID)
			{
				_ShipperID = shipperID;
			}
		}
		private void ReadData(SafeDataReader dr)
		{
			Database.LogInfo("ShipperInfo.ReadData", GetHashCode());
			try
			{
				_ShipperID = dr.GetInt32("ShipperID");
				_CompanyName = dr.GetString("CompanyName");
				_Phone = dr.GetString("Phone");
				_ShipperOrderCount = dr.GetInt32("OrderCount");
			}
			catch (Exception ex)
			{
				Database.LogException("ShipperInfo.ReadData", ex);
				_ErrorMessage = ex.Message;
				throw new DbCslaException("ShipperInfo.ReadData", ex);
			}
		}
		private void DataPortal_Fetch(PKCriteria criteria)
		{
			Database.LogInfo("ShipperInfo.DataPortal_Fetch", GetHashCode());
			try
			{
				using (SqlConnection cn = Database.Northwind_SqlConnection)
				{
					ApplicationContext.LocalContext["cn"] = cn;
					using (SqlCommand cm = cn.CreateCommand())
					{
						cm.CommandType = CommandType.StoredProcedure;
						cm.CommandText = "getShipper";
						cm.Parameters.AddWithValue("@ShipperID", criteria.ShipperID);
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
				Database.LogException("ShipperInfo.DataPortal_Fetch", ex);
				_ErrorMessage = ex.Message;
				throw new DbCslaException("ShipperInfo.DataPortal_Fetch", ex);
			}
		}
		#endregion
		// Standard Refresh
		#region extension
		ShipperInfoExtension _ShipperInfoExtension = new ShipperInfoExtension();
		[Serializable()]
		partial class ShipperInfoExtension : extensionBase {}
		[Serializable()]
		class extensionBase
		{
			// Default Refresh
			public virtual void Refresh(ShipperInfo tmp) { }
		}
		#endregion
	} // Class
	#region Converter
	internal class ShipperInfoConverter : ExpandableObjectConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
		{
			if (destType == typeof(string) && value is ShipperInfo)
			{
				// Return the ToString value
				return ((ShipperInfo)value).ToString();
			}
			return base.ConvertTo(context, culture, value, destType);
		}
	}
	#endregion
} // Namespace
