using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.CSLA.Library
{
// The following are samples of ToString overrides
	public partial class Category
	{ public override string ToString() { return string.Format("{0}",_CategoryName); } }
	public partial class CategoryInfo
	{ public override string ToString() { return string.Format("{0}", _CategoryName); } }

	public partial class CustomerCustomerDemo
	{ public override string ToString() { return string.Format("#{0}", _CustomerID); } }
	public partial class CustomerCustomerDemoInfo
	{ public override string ToString() { return string.Format("{0}", _CustomerID); } }
	public partial class CustomerDemographicCustomerCustomerDemo
	{ public override string ToString() { return string.Format("{0}", _Customer_CompanyName); } }
	public partial class CustomerCustomerCustomerDemo
	{ public override string ToString() { return string.Format("{0}", _CustomerDemographic_CustomerDesc); } }

	public partial class CustomerDemographic
	{ public override string ToString() { return string.Format("{0}", _CustomerDesc); } }
	public partial class CustomerDemographicInfo
		{ public override string ToString() { return string.Format("{0}", _CustomerDesc); } }

	public partial class Customer
	{ public override string ToString() { return string.Format("{0}", _CompanyName); } }
	public partial class CustomerInfo
	{ public override string ToString() { return string.Format("{0}", _CompanyName); } }

	public partial class Employee
	{
		public string FullNameSub	{ get	{ return string.Format("{0}, {1} {2}{3}", _LastName, _FirstName, _Title, 
			(_ChildEmployeeCount == 0 ? "" : string.Format(" ({0})", _ChildEmployeeCount)));}}
		public override string ToString() { return string.Format("{0}", FullNameSub); }
	}
	public partial class EmployeeInfo
	{
		public string FullNameSub { get	{ return string.Format("{0}, {1} {2}{3}", _LastName, _FirstName, _Title,
			(_ChildEmployeeCount == 0 ? "" : string.Format(" ({0})", _ChildEmployeeCount)));}}
		public override string ToString() { return string.Format("{0}", FullNameSub); } 
	}

	public partial class EmployeeTerritory
	{ public override string ToString() { return string.Format("{0}, {1}", _EmployeeID,_TerritoryID); } }
	public partial class EmployeeTerritoryInfo
	{ public override string ToString() { return string.Format("{0}, {1}", _EmployeeID, _TerritoryID); } }
	public partial class EmployeeEmployeeTerritory
	{ public override string ToString() { return string.Format("{0}", _Territory_TerritoryDescription); } }
	public partial class TerritoryEmployeeTerritory
	{ public override string ToString() { return string.Format("{0}, {1}", _Employee_LastName, _Employee_FirstName); } }

	public partial class OrderDetail
	{ public override string ToString() { return string.Format("{0} {1}", _Quantity,MyProduct.ProductName); } }
	public partial class OrderDetailInfo
	{ public override string ToString() { return string.Format("{0} {1}", _Quantity, MyProduct.ProductName); } }
	public partial class OrderOrderDetail
	{ public override string ToString() { return string.Format("{0} {1}", _Quantity, MyProduct.ProductName); } }
	public partial class ProductOrderDetail
	{ public override string ToString() { return string.Format("{0} {1} {2}", _Quantity, _Order_ShipName, _Order_RequiredDate); } }

	public partial class Order
	{ public override string ToString() { return string.Format("{0} {1}", MyCustomer.CompanyName,_OrderDate); } }
	public partial class OrderInfo
	{ public override string ToString() { return string.Format("{0} {1}", MyCustomer.CompanyName, _OrderDate); } }
	public partial class CustomerOrder
	{ public override string ToString() { return string.Format("{0} {1}", MyEmployee.FullNameSub, _OrderDate); } }
	public partial class EmployeeOrder
	{ public override string ToString() { return string.Format("{0} {1}", MyCustomer.CompanyName, _OrderDate); } }
	public partial class ShipperOrder
	{ public override string ToString() { return string.Format("{0} {1}", _ShipName, _OrderDate); } }

	public partial class Product
	{ public override string ToString() { return string.Format("{0}", _ProductName); } }
	public partial class ProductInfo
	{ public override string ToString() { return string.Format("{0}", _ProductName); } }
	public partial class SupplierProduct
	{ public override string ToString() { return string.Format("{0}", _ProductName); } }
	public partial class CategoryProduct
	{ public override string ToString() { return string.Format("{0}", _ProductName); } }

	public partial class Region
	{ public override string ToString() { return string.Format("{0}", _RegionDescription); } }
	public partial class RegionInfo
	{ public override string ToString() { return string.Format("{0}", _RegionDescription); } }


	public partial class Shipper
	{ public override string ToString() { return string.Format("{0}", _CompanyName); } }
	public partial class ShipperInfo
	{ public override string ToString() { return string.Format("{0}", _CompanyName); } }

	public partial class Supplier
	{ public override string ToString() { return string.Format("{0}", _CompanyName); } }
	public partial class SupplierInfo
	{ public override string ToString() { return string.Format("{0}", _CompanyName); } }

	public partial class Territory
	{ public override string ToString() { return string.Format("{0}", _TerritoryDescription); } }
	public partial class TerritoryInfo
	{ public override string ToString() { return string.Format("{0}", _TerritoryDescription); } }
	public partial class RegionTerritory
	{ public override string ToString() { return string.Format("{0}", _TerritoryDescription); } }

}
