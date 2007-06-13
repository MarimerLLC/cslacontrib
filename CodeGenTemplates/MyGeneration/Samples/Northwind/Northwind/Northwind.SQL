use northwind
-- Indexes to support foreign keys
CREATE NONCLUSTERED INDEX IX_EmployeesReportsTo ON [dbo].[Employees]
	(ReportsTo)
CREATE NONCLUSTERED INDEX IX_TerritoriesRegionID ON [dbo].[Territories]
	(RegionID)

/****** Object:  StoredProcedure [purgeData] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[purgeData]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [purgeData];
GO
	
CREATE PROCEDURE [dbo].[purgeData]

AS
	delete from [CustomerCustomerDemo]
	delete from [CustomerDemographics]
	delete from [EmployeeTerritories]
	delete from [Order Details]
	delete from [Orders]
	dbcc checkident([Orders],reseed,0)
	delete from [Products]
	dbcc checkident([Products],reseed,0)
	delete from [Shippers]
	dbcc checkident([Shippers],reseed,0)
	delete from [Suppliers]
	dbcc checkident([Suppliers],reseed,0)
	delete from [Territories]
	delete from [Categories]
	dbcc checkident([Categories],reseed,0)
	delete from [Customers]
	delete from [Employees]
	dbcc checkident([Employees],reseed,0)
	delete from [Region]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: purgeData Succeeded'
ELSE PRINT 'Procedure Creation: purgeData Error on Creation'
GO

/****** Object:  StoredProcedure [getCategories] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCategories]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCategories];
GO
	
CREATE PROCEDURE [dbo].[getCategories]

AS
	SELECT
		[CategoryID],
		[CategoryName],
		[Description],
		[Picture],
		(SELECT COUNT(*) FROM [Products] WHERE [Products].[CategoryID]=[Categories].[CategoryID]) [ProductCount]
	FROM [Categories]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCategories Succeeded'
ELSE PRINT 'Procedure Creation: getCategories Error on Creation'
GO

/****** Object:  StoredProcedure [updateCategory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateCategory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateCategory];
GO
	
CREATE PROCEDURE [dbo].[updateCategory]

(
	@CategoryID int,
	@CategoryName nvarchar(15),
	@Description nvarchar(MAX)=null,
	@Picture image=null
)
AS
	UPDATE [Categories]
		SET
			[CategoryName]=@CategoryName,
			[Description]=@Description,
			[Picture]=@Picture
		WHERE [CategoryID]=@CategoryID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Categories] WHERE [CategoryID]=@CategoryID)
			RAISERROR('Category record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('Category has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateCategory Succeeded'
ELSE PRINT 'Procedure Creation: updateCategory Error on Creation'
GO

/****** Object:  StoredProcedure [addCategory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addCategory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addCategory];
GO
	
CREATE PROCEDURE [dbo].[addCategory]

(
	@CategoryName nvarchar(15),
	@Description nvarchar(MAX)=null,
	@Picture image=null,
	@newCategoryID int output
)
AS
	INSERT INTO [Categories]
	(
		[CategoryName],
		[Description],
		[Picture]
	)
	VALUES
	(
		@CategoryName,
		@Description,
		@Picture
	)
	SELECT @newCategoryID= SCOPE_IDENTITY()
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addCategory Succeeded'
ELSE PRINT 'Procedure Creation: addCategory Error on Creation'
GO

/****** Object:  StoredProcedure [deleteCategory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteCategory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteCategory];
GO
	
CREATE PROCEDURE [dbo].[deleteCategory]

(
	@CategoryID int
)
AS
	DELETE [Products]
		WHERE [CategoryID]=@CategoryID
	DELETE [Categories]
		WHERE [CategoryID] = @CategoryID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteCategory Succeeded'
ELSE PRINT 'Procedure Creation: deleteCategory Error on Creation'
GO

/****** Object:  StoredProcedure [getCategory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCategory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCategory];
GO
	
CREATE PROCEDURE [dbo].[getCategory]

(
	@CategoryID int
)
AS
	SELECT
		[CategoryID],
		[CategoryName],
		[Description],
		[Picture],
		(SELECT COUNT(*) FROM [Products] WHERE [Products].[CategoryID]=[Categories].[CategoryID]) [ProductCount]
	FROM [Categories]
	WHERE [CategoryID]=@CategoryID

	SELECT
		[Products].[ProductID],
		[Products].[ProductName],
		[Products].[SupplierID],
		[Products].[CategoryID],
		[Products].[QuantityPerUnit],
		[Products].[UnitPrice],
		[Products].[UnitsInStock],
		[Products].[UnitsOnOrder],
		[Products].[ReorderLevel],
		[Products].[Discontinued]
	FROM [Products]
	WHERE
		[Products].[CategoryID]=@CategoryID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCategory Succeeded'
ELSE PRINT 'Procedure Creation: getCategory Error on Creation'
GO

/****** Object:  StoredProcedure [existsCategory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsCategory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsCategory];
GO
	
CREATE PROCEDURE [dbo].[existsCategory]

(
	@CategoryID int
)
	AS
	SELECT COUNT(*)
	FROM [Categories] WHERE [CategoryID]=@CategoryID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsCategory Succeeded'
ELSE PRINT 'Procedure Creation: existsCategory Error on Creation'
GO

/****** Object:  StoredProcedure [getCustomerCustomerDemos] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCustomerCustomerDemos]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCustomerCustomerDemos];
GO
	
CREATE PROCEDURE [dbo].[getCustomerCustomerDemos]

AS
	SELECT
		[CustomerID],
		[CustomerTypeID]
	FROM [CustomerCustomerDemo]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCustomerCustomerDemos Succeeded'
ELSE PRINT 'Procedure Creation: getCustomerCustomerDemos Error on Creation'
GO

	-- Update cannot be performed on CustomerCustomerDemo since it doesn't contain
	-- any columns that are not part of the Primary Key

/****** Object:  StoredProcedure [addCustomerCustomerDemo] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addCustomerCustomerDemo]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addCustomerCustomerDemo];
GO
	
CREATE PROCEDURE [dbo].[addCustomerCustomerDemo]

(
	@CustomerID nchar(5),
	@CustomerTypeID nchar(10)
)
AS
	INSERT INTO [CustomerCustomerDemo]
	(
		[CustomerID],
		[CustomerTypeID]
	)
	VALUES
	(
		@CustomerID,
		@CustomerTypeID
	)

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addCustomerCustomerDemo Succeeded'
ELSE PRINT 'Procedure Creation: addCustomerCustomerDemo Error on Creation'
GO

/****** Object:  StoredProcedure [deleteCustomerCustomerDemo] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteCustomerCustomerDemo]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteCustomerCustomerDemo];
GO
	
CREATE PROCEDURE [dbo].[deleteCustomerCustomerDemo]

(
	@CustomerID nchar(5),
	@CustomerTypeID nchar(10)
)
AS
	DELETE [CustomerCustomerDemo]
		WHERE [CustomerID] = @CustomerID AND [CustomerTypeID] = @CustomerTypeID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteCustomerCustomerDemo Succeeded'
ELSE PRINT 'Procedure Creation: deleteCustomerCustomerDemo Error on Creation'
GO

/****** Object:  StoredProcedure [getCustomerCustomerDemo] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCustomerCustomerDemo]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCustomerCustomerDemo];
GO
	
CREATE PROCEDURE [dbo].[getCustomerCustomerDemo]

(
	@CustomerID nchar(5),
	@CustomerTypeID nchar(10)
)
AS
	SELECT
		[CustomerID],
		[CustomerTypeID]
	FROM [CustomerCustomerDemo]
	WHERE [CustomerID]=@CustomerID AND [CustomerTypeID]=@CustomerTypeID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCustomerCustomerDemo Succeeded'
ELSE PRINT 'Procedure Creation: getCustomerCustomerDemo Error on Creation'
GO

/****** Object:  StoredProcedure [existsCustomerCustomerDemo] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsCustomerCustomerDemo]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsCustomerCustomerDemo];
GO
	
CREATE PROCEDURE [dbo].[existsCustomerCustomerDemo]

(
	@CustomerID nchar(5),
	@CustomerTypeID nchar(10)
)
	AS
	SELECT COUNT(*)
	FROM [CustomerCustomerDemo] WHERE [CustomerID]=@CustomerID AND [CustomerTypeID]=@CustomerTypeID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsCustomerCustomerDemo Succeeded'
ELSE PRINT 'Procedure Creation: existsCustomerCustomerDemo Error on Creation'
GO

/****** Object:  StoredProcedure [getCustomerCustomerDemosByCustomerTypeID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCustomerCustomerDemosByCustomerTypeID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCustomerCustomerDemosByCustomerTypeID];
GO
	
CREATE PROCEDURE [dbo].[getCustomerCustomerDemosByCustomerTypeID]

(
	@CustomerTypeID nchar(10)
)
AS

	SELECT
		[CustomerCustomerDemo].[CustomerID],
		[CustomerCustomerDemo].[CustomerTypeID],
		[Customers].[CompanyName] [Customer_CompanyName],
		[Customers].[ContactName] [Customer_ContactName],
		[Customers].[ContactTitle] [Customer_ContactTitle],
		[Customers].[Address] [Customer_Address],
		[Customers].[City] [Customer_City],
		[Customers].[Region] [Customer_Region],
		[Customers].[PostalCode] [Customer_PostalCode],
		[Customers].[Country] [Customer_Country],
		[Customers].[Phone] [Customer_Phone],
		[Customers].[Fax] [Customer_Fax]
	FROM [CustomerCustomerDemo]
		JOIN [Customers]  ON
			[Customers].[CustomerID]=[CustomerCustomerDemo].[CustomerID]
	WHERE
		[CustomerCustomerDemo].[CustomerTypeID]=@CustomerTypeID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCustomerCustomerDemosByCustomerTypeID Succeeded'
ELSE PRINT 'Procedure Creation: getCustomerCustomerDemosByCustomerTypeID Error on Creation'
GO

/****** Object:  StoredProcedure [getCustomerCustomerDemosByCustomerID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCustomerCustomerDemosByCustomerID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCustomerCustomerDemosByCustomerID];
GO
	
CREATE PROCEDURE [dbo].[getCustomerCustomerDemosByCustomerID]

(
	@CustomerID nchar(5)
)
AS

	SELECT
		[CustomerCustomerDemo].[CustomerID],
		[CustomerCustomerDemo].[CustomerTypeID],
		[CustomerDemographics].[CustomerDesc] [CustomerDemographic_CustomerDesc]
	FROM [CustomerCustomerDemo]
		JOIN [CustomerDemographics]  ON
			[CustomerDemographics].[CustomerTypeID]=[CustomerCustomerDemo].[CustomerTypeID]
	WHERE
		[CustomerCustomerDemo].[CustomerID]=@CustomerID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCustomerCustomerDemosByCustomerID Succeeded'
ELSE PRINT 'Procedure Creation: getCustomerCustomerDemosByCustomerID Error on Creation'
GO

/****** Object:  StoredProcedure [getCustomerDemographics] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCustomerDemographics]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCustomerDemographics];
GO
	
CREATE PROCEDURE [dbo].[getCustomerDemographics]

AS
	SELECT
		[CustomerTypeID],
		[CustomerDesc],
		(SELECT COUNT(*) FROM [CustomerCustomerDemo] WHERE [CustomerCustomerDemo].[CustomerTypeID]=[CustomerDemographics].[CustomerTypeID]) [CustomerCustomerDemoCount]
	FROM [CustomerDemographics]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCustomerDemographics Succeeded'
ELSE PRINT 'Procedure Creation: getCustomerDemographics Error on Creation'
GO

/****** Object:  StoredProcedure [updateCustomerDemographic] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateCustomerDemographic]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateCustomerDemographic];
GO
	
CREATE PROCEDURE [dbo].[updateCustomerDemographic]

(
	@CustomerTypeID nchar(10),
	@CustomerDesc nvarchar(MAX)=null
)
AS
	UPDATE [CustomerDemographics]
		SET
			[CustomerDesc]=@CustomerDesc
		WHERE [CustomerTypeID]=@CustomerTypeID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [CustomerDemographics] WHERE [CustomerTypeID]=@CustomerTypeID)
			RAISERROR('CustomerDemographic record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('CustomerDemographic has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateCustomerDemographic Succeeded'
ELSE PRINT 'Procedure Creation: updateCustomerDemographic Error on Creation'
GO

/****** Object:  StoredProcedure [addCustomerDemographic] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addCustomerDemographic]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addCustomerDemographic];
GO
	
CREATE PROCEDURE [dbo].[addCustomerDemographic]

(
	@CustomerTypeID nchar(10),
	@CustomerDesc nvarchar(MAX)=null
)
AS
	INSERT INTO [CustomerDemographics]
	(
		[CustomerTypeID],
		[CustomerDesc]
	)
	VALUES
	(
		@CustomerTypeID,
		@CustomerDesc
	)

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addCustomerDemographic Succeeded'
ELSE PRINT 'Procedure Creation: addCustomerDemographic Error on Creation'
GO

/****** Object:  StoredProcedure [deleteCustomerDemographic] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteCustomerDemographic]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteCustomerDemographic];
GO
	
CREATE PROCEDURE [dbo].[deleteCustomerDemographic]

(
	@CustomerTypeID nchar(10)
)
AS
	DELETE [CustomerCustomerDemo]
		WHERE [CustomerTypeID]=@CustomerTypeID
	DELETE [CustomerDemographics]
		WHERE [CustomerTypeID] = @CustomerTypeID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteCustomerDemographic Succeeded'
ELSE PRINT 'Procedure Creation: deleteCustomerDemographic Error on Creation'
GO

/****** Object:  StoredProcedure [getCustomerDemographic] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCustomerDemographic]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCustomerDemographic];
GO
	
CREATE PROCEDURE [dbo].[getCustomerDemographic]

(
	@CustomerTypeID nchar(10)
)
AS
	SELECT
		[CustomerTypeID],
		[CustomerDesc],
		(SELECT COUNT(*) FROM [CustomerCustomerDemo] WHERE [CustomerCustomerDemo].[CustomerTypeID]=[CustomerDemographics].[CustomerTypeID]) [CustomerCustomerDemoCount]
	FROM [CustomerDemographics]
	WHERE [CustomerTypeID]=@CustomerTypeID

	SELECT
		[CustomerCustomerDemo].[CustomerID],
		[CustomerCustomerDemo].[CustomerTypeID],
		[Customers].[CompanyName] [Customer_CompanyName],
		[Customers].[ContactName] [Customer_ContactName],
		[Customers].[ContactTitle] [Customer_ContactTitle],
		[Customers].[Address] [Customer_Address],
		[Customers].[City] [Customer_City],
		[Customers].[Region] [Customer_Region],
		[Customers].[PostalCode] [Customer_PostalCode],
		[Customers].[Country] [Customer_Country],
		[Customers].[Phone] [Customer_Phone],
		[Customers].[Fax] [Customer_Fax]
	FROM [CustomerCustomerDemo]
		JOIN [Customers]  ON
			[Customers].[CustomerID]=[CustomerCustomerDemo].[CustomerID]
	WHERE
		[CustomerCustomerDemo].[CustomerTypeID]=@CustomerTypeID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCustomerDemographic Succeeded'
ELSE PRINT 'Procedure Creation: getCustomerDemographic Error on Creation'
GO

/****** Object:  StoredProcedure [existsCustomerDemographic] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsCustomerDemographic]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsCustomerDemographic];
GO
	
CREATE PROCEDURE [dbo].[existsCustomerDemographic]

(
	@CustomerTypeID nchar(10)
)
	AS
	SELECT COUNT(*)
	FROM [CustomerDemographics] WHERE [CustomerTypeID]=@CustomerTypeID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsCustomerDemographic Succeeded'
ELSE PRINT 'Procedure Creation: existsCustomerDemographic Error on Creation'
GO

/****** Object:  StoredProcedure [getCustomers] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCustomers]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCustomers];
GO
	
CREATE PROCEDURE [dbo].[getCustomers]

AS
	SELECT
		[CustomerID],
		[CompanyName],
		[ContactName],
		[ContactTitle],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[Phone],
		[Fax],
		(SELECT COUNT(*) FROM [CustomerCustomerDemo] WHERE [CustomerCustomerDemo].[CustomerID]=[Customers].[CustomerID]) [CustomerCustomerDemoCount],
		(SELECT COUNT(*) FROM [Orders] WHERE [Orders].[CustomerID]=[Customers].[CustomerID]) [OrderCount]
	FROM [Customers]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCustomers Succeeded'
ELSE PRINT 'Procedure Creation: getCustomers Error on Creation'
GO

/****** Object:  StoredProcedure [updateCustomer] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateCustomer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateCustomer];
GO
	
CREATE PROCEDURE [dbo].[updateCustomer]

(
	@CustomerID nchar(5),
	@CompanyName nvarchar(40),
	@ContactName nvarchar(30)=null,
	@ContactTitle nvarchar(30)=null,
	@Address nvarchar(60)=null,
	@City nvarchar(15)=null,
	@Region nvarchar(15)=null,
	@PostalCode nvarchar(10)=null,
	@Country nvarchar(15)=null,
	@Phone nvarchar(24)=null,
	@Fax nvarchar(24)=null
)
AS
	UPDATE [Customers]
		SET
			[CompanyName]=@CompanyName,
			[ContactName]=@ContactName,
			[ContactTitle]=@ContactTitle,
			[Address]=@Address,
			[City]=@City,
			[Region]=@Region,
			[PostalCode]=@PostalCode,
			[Country]=@Country,
			[Phone]=@Phone,
			[Fax]=@Fax
		WHERE [CustomerID]=@CustomerID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Customers] WHERE [CustomerID]=@CustomerID)
			RAISERROR('Customer record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('Customer has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateCustomer Succeeded'
ELSE PRINT 'Procedure Creation: updateCustomer Error on Creation'
GO

/****** Object:  StoredProcedure [addCustomer] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addCustomer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addCustomer];
GO
	
CREATE PROCEDURE [dbo].[addCustomer]

(
	@CustomerID nchar(5),
	@CompanyName nvarchar(40),
	@ContactName nvarchar(30)=null,
	@ContactTitle nvarchar(30)=null,
	@Address nvarchar(60)=null,
	@City nvarchar(15)=null,
	@Region nvarchar(15)=null,
	@PostalCode nvarchar(10)=null,
	@Country nvarchar(15)=null,
	@Phone nvarchar(24)=null,
	@Fax nvarchar(24)=null
)
AS
	INSERT INTO [Customers]
	(
		[CustomerID],
		[CompanyName],
		[ContactName],
		[ContactTitle],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[Phone],
		[Fax]
	)
	VALUES
	(
		@CustomerID,
		@CompanyName,
		@ContactName,
		@ContactTitle,
		@Address,
		@City,
		@Region,
		@PostalCode,
		@Country,
		@Phone,
		@Fax
	)

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addCustomer Succeeded'
ELSE PRINT 'Procedure Creation: addCustomer Error on Creation'
GO

/****** Object:  StoredProcedure [deleteCustomer] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteCustomer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteCustomer];
GO
	
CREATE PROCEDURE [dbo].[deleteCustomer]

(
	@CustomerID nchar(5)
)
AS
	DELETE [CustomerCustomerDemo]
		WHERE [CustomerID]=@CustomerID
	DELETE [Orders]
		WHERE [CustomerID]=@CustomerID
	DELETE [Customers]
		WHERE [CustomerID] = @CustomerID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteCustomer Succeeded'
ELSE PRINT 'Procedure Creation: deleteCustomer Error on Creation'
GO

/****** Object:  StoredProcedure [getCustomer] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getCustomer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getCustomer];
GO
	
CREATE PROCEDURE [dbo].[getCustomer]

(
	@CustomerID nchar(5)
)
AS
	SELECT
		[CustomerID],
		[CompanyName],
		[ContactName],
		[ContactTitle],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[Phone],
		[Fax],
		(SELECT COUNT(*) FROM [CustomerCustomerDemo] WHERE [CustomerCustomerDemo].[CustomerID]=[Customers].[CustomerID]) [CustomerCustomerDemoCount],
		(SELECT COUNT(*) FROM [Orders] WHERE [Orders].[CustomerID]=[Customers].[CustomerID]) [OrderCount]
	FROM [Customers]
	WHERE [CustomerID]=@CustomerID

	SELECT
		[CustomerCustomerDemo].[CustomerID],
		[CustomerCustomerDemo].[CustomerTypeID],
		[CustomerDemographics].[CustomerDesc] [CustomerDemographic_CustomerDesc]
	FROM [CustomerCustomerDemo]
		JOIN [CustomerDemographics]  ON
			[CustomerDemographics].[CustomerTypeID]=[CustomerCustomerDemo].[CustomerTypeID]
	WHERE
		[CustomerCustomerDemo].[CustomerID]=@CustomerID


	SELECT
		[Orders].[OrderID],
		[Orders].[CustomerID],
		[Orders].[EmployeeID],
		[Orders].[OrderDate],
		[Orders].[RequiredDate],
		[Orders].[ShippedDate],
		[Orders].[ShipVia],
		[Orders].[Freight],
		[Orders].[ShipName],
		[Orders].[ShipAddress],
		[Orders].[ShipCity],
		[Orders].[ShipRegion],
		[Orders].[ShipPostalCode],
		[Orders].[ShipCountry]
	FROM [Orders]
	WHERE
		[Orders].[CustomerID]=@CustomerID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getCustomer Succeeded'
ELSE PRINT 'Procedure Creation: getCustomer Error on Creation'
GO

/****** Object:  StoredProcedure [existsCustomer] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsCustomer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsCustomer];
GO
	
CREATE PROCEDURE [dbo].[existsCustomer]

(
	@CustomerID nchar(5)
)
	AS
	SELECT COUNT(*)
	FROM [Customers] WHERE [CustomerID]=@CustomerID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsCustomer Succeeded'
ELSE PRINT 'Procedure Creation: existsCustomer Error on Creation'
GO

/****** Object:  StoredProcedure [getEmployees] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getEmployees]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getEmployees];
GO
	
CREATE PROCEDURE [dbo].[getEmployees]

AS
	SELECT
		[EmployeeID],
		[LastName],
		[FirstName],
		[Title],
		[TitleOfCourtesy],
		[BirthDate],
		[HireDate],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[HomePhone],
		[Extension],
		[Photo],
		[Notes],
		[ReportsTo],
		[PhotoPath],
		(SELECT COUNT(*) FROM [Employees] [Children] WHERE [Children].[ReportsTo]=[Employees].[EmployeeID]) [ChildCount],
		(SELECT COUNT(*) FROM [EmployeeTerritories] WHERE [EmployeeTerritories].[EmployeeID]=[Employees].[EmployeeID]) [EmployeeTerritoryCount],
		(SELECT COUNT(*) FROM [Orders] WHERE [Orders].[EmployeeID]=[Employees].[EmployeeID]) [OrderCount]
	FROM [Employees]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getEmployees Succeeded'
ELSE PRINT 'Procedure Creation: getEmployees Error on Creation'
GO

/****** Object:  StoredProcedure [updateEmployee] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateEmployee]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateEmployee];
GO
	
CREATE PROCEDURE [dbo].[updateEmployee]

(
	@EmployeeID int,
	@LastName nvarchar(20),
	@FirstName nvarchar(10),
	@Title nvarchar(30)=null,
	@TitleOfCourtesy nvarchar(25)=null,
	@BirthDate datetime=null,
	@HireDate datetime=null,
	@Address nvarchar(60)=null,
	@City nvarchar(15)=null,
	@Region nvarchar(15)=null,
	@PostalCode nvarchar(10)=null,
	@Country nvarchar(15)=null,
	@HomePhone nvarchar(24)=null,
	@Extension nvarchar(4)=null,
	@Photo image=null,
	@Notes nvarchar(MAX)=null,
	@ReportsTo int=null,
	@PhotoPath nvarchar(255)=null
)
AS
	UPDATE [Employees]
		SET
			[LastName]=@LastName,
			[FirstName]=@FirstName,
			[Title]=@Title,
			[TitleOfCourtesy]=@TitleOfCourtesy,
			[BirthDate]=@BirthDate,
			[HireDate]=@HireDate,
			[Address]=@Address,
			[City]=@City,
			[Region]=@Region,
			[PostalCode]=@PostalCode,
			[Country]=@Country,
			[HomePhone]=@HomePhone,
			[Extension]=@Extension,
			[Photo]=@Photo,
			[Notes]=@Notes,
			[ReportsTo]=@ReportsTo,
			[PhotoPath]=@PhotoPath
		WHERE [EmployeeID]=@EmployeeID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Employees] WHERE [EmployeeID]=@EmployeeID)
			RAISERROR('Employee record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('Employee has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateEmployee Succeeded'
ELSE PRINT 'Procedure Creation: updateEmployee Error on Creation'
GO

/****** Object:  StoredProcedure [addEmployee] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addEmployee]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addEmployee];
GO
	
CREATE PROCEDURE [dbo].[addEmployee]

(
	@LastName nvarchar(20),
	@FirstName nvarchar(10),
	@Title nvarchar(30)=null,
	@TitleOfCourtesy nvarchar(25)=null,
	@BirthDate datetime=null,
	@HireDate datetime=null,
	@Address nvarchar(60)=null,
	@City nvarchar(15)=null,
	@Region nvarchar(15)=null,
	@PostalCode nvarchar(10)=null,
	@Country nvarchar(15)=null,
	@HomePhone nvarchar(24)=null,
	@Extension nvarchar(4)=null,
	@Photo image=null,
	@Notes nvarchar(MAX)=null,
	@ReportsTo int=null,
	@PhotoPath nvarchar(255)=null,
	@newEmployeeID int output
)
AS
	INSERT INTO [Employees]
	(
		[LastName],
		[FirstName],
		[Title],
		[TitleOfCourtesy],
		[BirthDate],
		[HireDate],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[HomePhone],
		[Extension],
		[Photo],
		[Notes],
		[ReportsTo],
		[PhotoPath]
	)
	VALUES
	(
		@LastName,
		@FirstName,
		@Title,
		@TitleOfCourtesy,
		@BirthDate,
		@HireDate,
		@Address,
		@City,
		@Region,
		@PostalCode,
		@Country,
		@HomePhone,
		@Extension,
		@Photo,
		@Notes,
		@ReportsTo,
		@PhotoPath
	)
	SELECT @newEmployeeID= SCOPE_IDENTITY()
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addEmployee Succeeded'
ELSE PRINT 'Procedure Creation: addEmployee Error on Creation'
GO

/****** Object:  StoredProcedure [deleteEmployee] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteEmployee]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteEmployee];
GO
	
CREATE PROCEDURE [dbo].[deleteEmployee]

(
	@EmployeeID int
)
AS
	DELETE [EmployeeTerritories]
		WHERE [EmployeeID]=@EmployeeID
	DELETE [Orders]
		WHERE [EmployeeID]=@EmployeeID
	DELETE [Employees]
		WHERE [EmployeeID] = @EmployeeID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteEmployee Succeeded'
ELSE PRINT 'Procedure Creation: deleteEmployee Error on Creation'
GO

/****** Object:  StoredProcedure [getEmployee] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getEmployee]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getEmployee];
GO
	
CREATE PROCEDURE [dbo].[getEmployee]

(
	@EmployeeID int
)
AS
	SELECT
		[EmployeeID],
		[LastName],
		[FirstName],
		[Title],
		[TitleOfCourtesy],
		[BirthDate],
		[HireDate],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[HomePhone],
		[Extension],
		[Photo],
		[Notes],
		[ReportsTo],
		[PhotoPath],
		(SELECT COUNT(*) FROM [Employees] [Children] WHERE [Children].[ReportsTo]=[Employees].[EmployeeID]) [ChildCount],
		(SELECT COUNT(*) FROM [EmployeeTerritories] WHERE [EmployeeTerritories].[EmployeeID]=[Employees].[EmployeeID]) [EmployeeTerritoryCount],
		(SELECT COUNT(*) FROM [Orders] WHERE [Orders].[EmployeeID]=[Employees].[EmployeeID]) [OrderCount]
	FROM [Employees]
	WHERE [EmployeeID]=@EmployeeID

	SELECT
		[EmployeeTerritories].[EmployeeID],
		[EmployeeTerritories].[TerritoryID],
		[Territories].[TerritoryDescription] [Territory_TerritoryDescription],
		[Territories].[RegionID] [Territory_RegionID]
	FROM [EmployeeTerritories]
		JOIN [Territories]  ON
			[Territories].[TerritoryID]=[EmployeeTerritories].[TerritoryID]
	WHERE
		[EmployeeTerritories].[EmployeeID]=@EmployeeID


	SELECT
		[Orders].[OrderID],
		[Orders].[CustomerID],
		[Orders].[EmployeeID],
		[Orders].[OrderDate],
		[Orders].[RequiredDate],
		[Orders].[ShippedDate],
		[Orders].[ShipVia],
		[Orders].[Freight],
		[Orders].[ShipName],
		[Orders].[ShipAddress],
		[Orders].[ShipCity],
		[Orders].[ShipRegion],
		[Orders].[ShipPostalCode],
		[Orders].[ShipCountry]
	FROM [Orders]
	WHERE
		[Orders].[EmployeeID]=@EmployeeID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getEmployee Succeeded'
ELSE PRINT 'Procedure Creation: getEmployee Error on Creation'
GO

/****** Object:  StoredProcedure [getParentEmployee] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getParentEmployee]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getParentEmployee];
GO
	
CREATE PROCEDURE [dbo].[getParentEmployee]
(
	@ReportsTo int
)
AS
	SELECT
		[EmployeeID],
		[LastName],
		[FirstName],
		[Title],
		[TitleOfCourtesy],
		[BirthDate],
		[HireDate],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[HomePhone],
		[Extension],
		[Photo],
		[Notes],
		[ReportsTo],
		[PhotoPath],
		(SELECT COUNT(*) FROM [Employees] [Children] WHERE [Children].[ReportsTo]=[Employees].[EmployeeID]) [ChildCount],
		(SELECT COUNT(*) FROM [EmployeeTerritories] WHERE [EmployeeTerritories].[EmployeeID]=[Employees].[EmployeeID]) [EmployeeTerritoryCount],
		(SELECT COUNT(*) FROM [Orders] WHERE [Orders].[EmployeeID]=[Employees].[EmployeeID]) [OrderCount]
	FROM [Employees]
	WHERE
		[EmployeeID]=@ReportsTo
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getParentEmployee Succeeded'
ELSE PRINT 'Procedure Creation: getParentEmployee Error on Creation'
GO

/****** Object:  StoredProcedure [existsEmployee] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsEmployee]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsEmployee];
GO
	
CREATE PROCEDURE [dbo].[existsEmployee]

(
	@EmployeeID int
)
	AS
	SELECT COUNT(*)
	FROM [Employees] WHERE [EmployeeID]=@EmployeeID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsEmployee Succeeded'
ELSE PRINT 'Procedure Creation: existsEmployee Error on Creation'
GO

/****** Object:  StoredProcedure [getChildEmployees] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getChildEmployees]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getChildEmployees];
GO
	
CREATE PROCEDURE [dbo].[getChildEmployees]

(
	@ReportsTo int
)
AS
	SELECT
		[EmployeeID],
		[LastName],
		[FirstName],
		[Title],
		[TitleOfCourtesy],
		[BirthDate],
		[HireDate],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[HomePhone],
		[Extension],
		[Photo],
		[Notes],
		[ReportsTo],
		[PhotoPath],
		(SELECT COUNT(*) FROM [Employees] [Children] WHERE [Children].[ReportsTo]=[Employees].[EmployeeID]) [ChildCount],
		(SELECT COUNT(*) FROM [EmployeeTerritories] WHERE [EmployeeTerritories].[EmployeeID]=[Employees].[EmployeeID]) [EmployeeTerritoryCount],
		(SELECT COUNT(*) FROM [Orders] WHERE [Orders].[EmployeeID]=[Employees].[EmployeeID]) [OrderCount]
	FROM [Employees] WHERE [ReportsTo]=@ReportsTo AND [EmployeeID]<>@ReportsTo
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getChildEmployees Succeeded'
ELSE PRINT 'Procedure Creation: getChildEmployees Error on Creation'
GO

/****** Object:  StoredProcedure [getEmployeeTerritories] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getEmployeeTerritories]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getEmployeeTerritories];
GO
	
CREATE PROCEDURE [dbo].[getEmployeeTerritories]

AS
	SELECT
		[EmployeeID],
		[TerritoryID]
	FROM [EmployeeTerritories]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getEmployeeTerritories Succeeded'
ELSE PRINT 'Procedure Creation: getEmployeeTerritories Error on Creation'
GO

	-- Update cannot be performed on EmployeeTerritories since it doesn't contain
	-- any columns that are not part of the Primary Key

/****** Object:  StoredProcedure [addEmployeeTerritory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addEmployeeTerritory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addEmployeeTerritory];
GO
	
CREATE PROCEDURE [dbo].[addEmployeeTerritory]

(
	@EmployeeID int,
	@TerritoryID nvarchar(20)
)
AS
	INSERT INTO [EmployeeTerritories]
	(
		[EmployeeID],
		[TerritoryID]
	)
	VALUES
	(
		@EmployeeID,
		@TerritoryID
	)

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addEmployeeTerritory Succeeded'
ELSE PRINT 'Procedure Creation: addEmployeeTerritory Error on Creation'
GO

/****** Object:  StoredProcedure [deleteEmployeeTerritory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteEmployeeTerritory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteEmployeeTerritory];
GO
	
CREATE PROCEDURE [dbo].[deleteEmployeeTerritory]

(
	@EmployeeID int,
	@TerritoryID nvarchar(20)
)
AS
	DELETE [EmployeeTerritories]
		WHERE [EmployeeID] = @EmployeeID AND [TerritoryID] = @TerritoryID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteEmployeeTerritory Succeeded'
ELSE PRINT 'Procedure Creation: deleteEmployeeTerritory Error on Creation'
GO

/****** Object:  StoredProcedure [getEmployeeTerritory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getEmployeeTerritory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getEmployeeTerritory];
GO
	
CREATE PROCEDURE [dbo].[getEmployeeTerritory]

(
	@EmployeeID int,
	@TerritoryID nvarchar(20)
)
AS
	SELECT
		[EmployeeID],
		[TerritoryID]
	FROM [EmployeeTerritories]
	WHERE [EmployeeID]=@EmployeeID AND [TerritoryID]=@TerritoryID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getEmployeeTerritory Succeeded'
ELSE PRINT 'Procedure Creation: getEmployeeTerritory Error on Creation'
GO

/****** Object:  StoredProcedure [existsEmployeeTerritory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsEmployeeTerritory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsEmployeeTerritory];
GO
	
CREATE PROCEDURE [dbo].[existsEmployeeTerritory]

(
	@EmployeeID int,
	@TerritoryID nvarchar(20)
)
	AS
	SELECT COUNT(*)
	FROM [EmployeeTerritories] WHERE [EmployeeID]=@EmployeeID AND [TerritoryID]=@TerritoryID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsEmployeeTerritory Succeeded'
ELSE PRINT 'Procedure Creation: existsEmployeeTerritory Error on Creation'
GO

/****** Object:  StoredProcedure [getEmployeeTerritoriesByEmployeeID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getEmployeeTerritoriesByEmployeeID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getEmployeeTerritoriesByEmployeeID];
GO
	
CREATE PROCEDURE [dbo].[getEmployeeTerritoriesByEmployeeID]

(
	@EmployeeID int
)
AS

	SELECT
		[EmployeeTerritories].[EmployeeID],
		[EmployeeTerritories].[TerritoryID],
		[Territories].[TerritoryDescription] [Territory_TerritoryDescription],
		[Territories].[RegionID] [Territory_RegionID]
	FROM [EmployeeTerritories]
		JOIN [Territories]  ON
			[Territories].[TerritoryID]=[EmployeeTerritories].[TerritoryID]
	WHERE
		[EmployeeTerritories].[EmployeeID]=@EmployeeID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getEmployeeTerritoriesByEmployeeID Succeeded'
ELSE PRINT 'Procedure Creation: getEmployeeTerritoriesByEmployeeID Error on Creation'
GO

/****** Object:  StoredProcedure [getEmployeeTerritoriesByTerritoryID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getEmployeeTerritoriesByTerritoryID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getEmployeeTerritoriesByTerritoryID];
GO
	
CREATE PROCEDURE [dbo].[getEmployeeTerritoriesByTerritoryID]

(
	@TerritoryID nvarchar(20)
)
AS

	SELECT
		[EmployeeTerritories].[EmployeeID],
		[EmployeeTerritories].[TerritoryID],
		[Employees].[LastName] [Employee_LastName],
		[Employees].[FirstName] [Employee_FirstName],
		[Employees].[Title] [Employee_Title],
		[Employees].[TitleOfCourtesy] [Employee_TitleOfCourtesy],
		[Employees].[BirthDate] [Employee_BirthDate],
		[Employees].[HireDate] [Employee_HireDate],
		[Employees].[Address] [Employee_Address],
		[Employees].[City] [Employee_City],
		[Employees].[Region] [Employee_Region],
		[Employees].[PostalCode] [Employee_PostalCode],
		[Employees].[Country] [Employee_Country],
		[Employees].[HomePhone] [Employee_HomePhone],
		[Employees].[Extension] [Employee_Extension],
		[Employees].[Photo] [Employee_Photo],
		[Employees].[Notes] [Employee_Notes],
		[Employees].[ReportsTo] [Employee_ReportsTo],
		[Employees].[PhotoPath] [Employee_PhotoPath]
	FROM [EmployeeTerritories]
		JOIN [Employees]  ON
			[Employees].[EmployeeID]=[EmployeeTerritories].[EmployeeID]
	WHERE
		[EmployeeTerritories].[TerritoryID]=@TerritoryID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getEmployeeTerritoriesByTerritoryID Succeeded'
ELSE PRINT 'Procedure Creation: getEmployeeTerritoriesByTerritoryID Error on Creation'
GO

/****** Object:  StoredProcedure [getOrderDetails] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getOrderDetails]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getOrderDetails];
GO
	
CREATE PROCEDURE [dbo].[getOrderDetails]

AS
	SELECT
		[OrderID],
		[ProductID],
		[UnitPrice],
		[Quantity],
		[Discount]
	FROM [Order Details]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getOrderDetails Succeeded'
ELSE PRINT 'Procedure Creation: getOrderDetails Error on Creation'
GO

/****** Object:  StoredProcedure [updateOrderDetail] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateOrderDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateOrderDetail];
GO
	
CREATE PROCEDURE [dbo].[updateOrderDetail]

(
	@OrderID int,
	@ProductID int,
	@UnitPrice money,
	@Quantity smallint,
	@Discount real
)
AS
	UPDATE [Order Details]
		SET
			[UnitPrice]=@UnitPrice,
			[Quantity]=@Quantity,
			[Discount]=@Discount
		WHERE [OrderID]=@OrderID AND [ProductID]=@ProductID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Order Details] WHERE [OrderID]=@OrderID AND [ProductID]=@ProductID)
			RAISERROR('OrderDetail record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('OrderDetail has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateOrderDetail Succeeded'
ELSE PRINT 'Procedure Creation: updateOrderDetail Error on Creation'
GO

/****** Object:  StoredProcedure [addOrderDetail] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addOrderDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addOrderDetail];
GO
	
CREATE PROCEDURE [dbo].[addOrderDetail]

(
	@OrderID int,
	@ProductID int,
	@UnitPrice money,
	@Quantity smallint,
	@Discount real
)
AS
	INSERT INTO [Order Details]
	(
		[OrderID],
		[ProductID],
		[UnitPrice],
		[Quantity],
		[Discount]
	)
	VALUES
	(
		@OrderID,
		@ProductID,
		@UnitPrice,
		@Quantity,
		@Discount
	)

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addOrderDetail Succeeded'
ELSE PRINT 'Procedure Creation: addOrderDetail Error on Creation'
GO

/****** Object:  StoredProcedure [deleteOrderDetail] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteOrderDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteOrderDetail];
GO
	
CREATE PROCEDURE [dbo].[deleteOrderDetail]

(
	@OrderID int,
	@ProductID int
)
AS
	DELETE [Order Details]
		WHERE [OrderID] = @OrderID AND [ProductID] = @ProductID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteOrderDetail Succeeded'
ELSE PRINT 'Procedure Creation: deleteOrderDetail Error on Creation'
GO

/****** Object:  StoredProcedure [getOrderDetail] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getOrderDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getOrderDetail];
GO
	
CREATE PROCEDURE [dbo].[getOrderDetail]

(
	@OrderID int,
	@ProductID int
)
AS
	SELECT
		[OrderID],
		[ProductID],
		[UnitPrice],
		[Quantity],
		[Discount]
	FROM [Order Details]
	WHERE [OrderID]=@OrderID AND [ProductID]=@ProductID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getOrderDetail Succeeded'
ELSE PRINT 'Procedure Creation: getOrderDetail Error on Creation'
GO

/****** Object:  StoredProcedure [existsOrderDetail] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsOrderDetail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsOrderDetail];
GO
	
CREATE PROCEDURE [dbo].[existsOrderDetail]

(
	@OrderID int,
	@ProductID int
)
	AS
	SELECT COUNT(*)
	FROM [Order Details] WHERE [OrderID]=@OrderID AND [ProductID]=@ProductID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsOrderDetail Succeeded'
ELSE PRINT 'Procedure Creation: existsOrderDetail Error on Creation'
GO

/****** Object:  StoredProcedure [getOrderDetailsByOrderID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getOrderDetailsByOrderID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getOrderDetailsByOrderID];
GO
	
CREATE PROCEDURE [dbo].[getOrderDetailsByOrderID]

(
	@OrderID int
)
AS

	SELECT
		[Order Details].[OrderID],
		[Order Details].[ProductID],
		[Order Details].[UnitPrice],
		[Order Details].[Quantity],
		[Order Details].[Discount],
		[Products].[ProductName] [Product_ProductName],
		[Products].[SupplierID] [Product_SupplierID],
		[Products].[CategoryID] [Product_CategoryID],
		[Products].[QuantityPerUnit] [Product_QuantityPerUnit],
		[Products].[UnitPrice] [Product_UnitPrice],
		[Products].[UnitsInStock] [Product_UnitsInStock],
		[Products].[UnitsOnOrder] [Product_UnitsOnOrder],
		[Products].[ReorderLevel] [Product_ReorderLevel],
		[Products].[Discontinued] [Product_Discontinued]
	FROM [Order Details]
		JOIN [Products]  ON
			[Products].[ProductID]=[Order Details].[ProductID]
	WHERE
		[Order Details].[OrderID]=@OrderID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getOrderDetailsByOrderID Succeeded'
ELSE PRINT 'Procedure Creation: getOrderDetailsByOrderID Error on Creation'
GO

/****** Object:  StoredProcedure [getOrderDetailsByProductID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getOrderDetailsByProductID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getOrderDetailsByProductID];
GO
	
CREATE PROCEDURE [dbo].[getOrderDetailsByProductID]

(
	@ProductID int
)
AS

	SELECT
		[Order Details].[OrderID],
		[Order Details].[ProductID],
		[Order Details].[UnitPrice],
		[Order Details].[Quantity],
		[Order Details].[Discount],
		[Orders].[CustomerID] [Order_CustomerID],
		[Orders].[EmployeeID] [Order_EmployeeID],
		[Orders].[OrderDate] [Order_OrderDate],
		[Orders].[RequiredDate] [Order_RequiredDate],
		[Orders].[ShippedDate] [Order_ShippedDate],
		[Orders].[ShipVia] [Order_ShipVia],
		[Orders].[Freight] [Order_Freight],
		[Orders].[ShipName] [Order_ShipName],
		[Orders].[ShipAddress] [Order_ShipAddress],
		[Orders].[ShipCity] [Order_ShipCity],
		[Orders].[ShipRegion] [Order_ShipRegion],
		[Orders].[ShipPostalCode] [Order_ShipPostalCode],
		[Orders].[ShipCountry] [Order_ShipCountry]
	FROM [Order Details]
		JOIN [Orders]  ON
			[Orders].[OrderID]=[Order Details].[OrderID]
	WHERE
		[Order Details].[ProductID]=@ProductID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getOrderDetailsByProductID Succeeded'
ELSE PRINT 'Procedure Creation: getOrderDetailsByProductID Error on Creation'
GO

/****** Object:  StoredProcedure [getOrders] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getOrders]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getOrders];
GO
	
CREATE PROCEDURE [dbo].[getOrders]

AS
	SELECT
		[OrderID],
		[CustomerID],
		[EmployeeID],
		[OrderDate],
		[RequiredDate],
		[ShippedDate],
		[ShipVia],
		[Freight],
		[ShipName],
		[ShipAddress],
		[ShipCity],
		[ShipRegion],
		[ShipPostalCode],
		[ShipCountry],
		(SELECT COUNT(*) FROM [Order Details] WHERE [Order Details].[OrderID]=[Orders].[OrderID]) [OrderDetailCount]
	FROM [Orders]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getOrders Succeeded'
ELSE PRINT 'Procedure Creation: getOrders Error on Creation'
GO

/****** Object:  StoredProcedure [updateOrder] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateOrder];
GO
	
CREATE PROCEDURE [dbo].[updateOrder]

(
	@OrderID int,
	@CustomerID nchar(5)=null,
	@EmployeeID int=null,
	@OrderDate datetime=null,
	@RequiredDate datetime=null,
	@ShippedDate datetime=null,
	@ShipVia int=null,
	@Freight money=null,
	@ShipName nvarchar(40)=null,
	@ShipAddress nvarchar(60)=null,
	@ShipCity nvarchar(15)=null,
	@ShipRegion nvarchar(15)=null,
	@ShipPostalCode nvarchar(10)=null,
	@ShipCountry nvarchar(15)=null
)
AS
	UPDATE [Orders]
		SET
			[CustomerID]=@CustomerID,
			[EmployeeID]=@EmployeeID,
			[OrderDate]=@OrderDate,
			[RequiredDate]=@RequiredDate,
			[ShippedDate]=@ShippedDate,
			[ShipVia]=@ShipVia,
			[Freight]=@Freight,
			[ShipName]=@ShipName,
			[ShipAddress]=@ShipAddress,
			[ShipCity]=@ShipCity,
			[ShipRegion]=@ShipRegion,
			[ShipPostalCode]=@ShipPostalCode,
			[ShipCountry]=@ShipCountry
		WHERE [OrderID]=@OrderID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Orders] WHERE [OrderID]=@OrderID)
			RAISERROR('Order record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('Order has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateOrder Succeeded'
ELSE PRINT 'Procedure Creation: updateOrder Error on Creation'
GO

/****** Object:  StoredProcedure [addOrder] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addOrder];
GO
	
CREATE PROCEDURE [dbo].[addOrder]

(
	@CustomerID nchar(5)=null,
	@EmployeeID int=null,
	@OrderDate datetime=null,
	@RequiredDate datetime=null,
	@ShippedDate datetime=null,
	@ShipVia int=null,
	@Freight money=null,
	@ShipName nvarchar(40)=null,
	@ShipAddress nvarchar(60)=null,
	@ShipCity nvarchar(15)=null,
	@ShipRegion nvarchar(15)=null,
	@ShipPostalCode nvarchar(10)=null,
	@ShipCountry nvarchar(15)=null,
	@newOrderID int output
)
AS
	INSERT INTO [Orders]
	(
		[CustomerID],
		[EmployeeID],
		[OrderDate],
		[RequiredDate],
		[ShippedDate],
		[ShipVia],
		[Freight],
		[ShipName],
		[ShipAddress],
		[ShipCity],
		[ShipRegion],
		[ShipPostalCode],
		[ShipCountry]
	)
	VALUES
	(
		@CustomerID,
		@EmployeeID,
		@OrderDate,
		@RequiredDate,
		@ShippedDate,
		@ShipVia,
		@Freight,
		@ShipName,
		@ShipAddress,
		@ShipCity,
		@ShipRegion,
		@ShipPostalCode,
		@ShipCountry
	)
	SELECT @newOrderID= SCOPE_IDENTITY()
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addOrder Succeeded'
ELSE PRINT 'Procedure Creation: addOrder Error on Creation'
GO

/****** Object:  StoredProcedure [deleteOrder] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteOrder];
GO
	
CREATE PROCEDURE [dbo].[deleteOrder]

(
	@OrderID int
)
AS
	DELETE [Order Details]
		WHERE [OrderID]=@OrderID
	DELETE [Orders]
		WHERE [OrderID] = @OrderID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteOrder Succeeded'
ELSE PRINT 'Procedure Creation: deleteOrder Error on Creation'
GO

/****** Object:  StoredProcedure [getOrder] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getOrder];
GO
	
CREATE PROCEDURE [dbo].[getOrder]

(
	@OrderID int
)
AS
	SELECT
		[OrderID],
		[CustomerID],
		[EmployeeID],
		[OrderDate],
		[RequiredDate],
		[ShippedDate],
		[ShipVia],
		[Freight],
		[ShipName],
		[ShipAddress],
		[ShipCity],
		[ShipRegion],
		[ShipPostalCode],
		[ShipCountry],
		(SELECT COUNT(*) FROM [Order Details] WHERE [Order Details].[OrderID]=[Orders].[OrderID]) [OrderDetailCount]
	FROM [Orders]
	WHERE [OrderID]=@OrderID

	SELECT
		[Order Details].[OrderID],
		[Order Details].[ProductID],
		[Order Details].[UnitPrice],
		[Order Details].[Quantity],
		[Order Details].[Discount],
		[Products].[ProductName] [Product_ProductName],
		[Products].[SupplierID] [Product_SupplierID],
		[Products].[CategoryID] [Product_CategoryID],
		[Products].[QuantityPerUnit] [Product_QuantityPerUnit],
		[Products].[UnitPrice] [Product_UnitPrice],
		[Products].[UnitsInStock] [Product_UnitsInStock],
		[Products].[UnitsOnOrder] [Product_UnitsOnOrder],
		[Products].[ReorderLevel] [Product_ReorderLevel],
		[Products].[Discontinued] [Product_Discontinued]
	FROM [Order Details]
		JOIN [Products]  ON
			[Products].[ProductID]=[Order Details].[ProductID]
	WHERE
		[Order Details].[OrderID]=@OrderID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getOrder Succeeded'
ELSE PRINT 'Procedure Creation: getOrder Error on Creation'
GO

/****** Object:  StoredProcedure [existsOrder] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsOrder];
GO
	
CREATE PROCEDURE [dbo].[existsOrder]

(
	@OrderID int
)
	AS
	SELECT COUNT(*)
	FROM [Orders] WHERE [OrderID]=@OrderID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsOrder Succeeded'
ELSE PRINT 'Procedure Creation: existsOrder Error on Creation'
GO

/****** Object:  StoredProcedure [getOrdersByCustomerID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getOrdersByCustomerID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getOrdersByCustomerID];
GO
	
CREATE PROCEDURE [dbo].[getOrdersByCustomerID]

(
	@CustomerID nchar(5)
)
AS

	SELECT
		[Orders].[OrderID],
		[Orders].[CustomerID],
		[Orders].[EmployeeID],
		[Orders].[OrderDate],
		[Orders].[RequiredDate],
		[Orders].[ShippedDate],
		[Orders].[ShipVia],
		[Orders].[Freight],
		[Orders].[ShipName],
		[Orders].[ShipAddress],
		[Orders].[ShipCity],
		[Orders].[ShipRegion],
		[Orders].[ShipPostalCode],
		[Orders].[ShipCountry],
		(SELECT COUNT(*) FROM [Order Details] WHERE [Order Details].[OrderID]=[Orders].[OrderID]) [OrderDetailCount]
	FROM [Orders]
	WHERE
		[Orders].[CustomerID]=@CustomerID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getOrdersByCustomerID Succeeded'
ELSE PRINT 'Procedure Creation: getOrdersByCustomerID Error on Creation'
GO

/****** Object:  StoredProcedure [getOrdersByEmployeeID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getOrdersByEmployeeID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getOrdersByEmployeeID];
GO
	
CREATE PROCEDURE [dbo].[getOrdersByEmployeeID]

(
	@EmployeeID int
)
AS

	SELECT
		[Orders].[OrderID],
		[Orders].[CustomerID],
		[Orders].[EmployeeID],
		[Orders].[OrderDate],
		[Orders].[RequiredDate],
		[Orders].[ShippedDate],
		[Orders].[ShipVia],
		[Orders].[Freight],
		[Orders].[ShipName],
		[Orders].[ShipAddress],
		[Orders].[ShipCity],
		[Orders].[ShipRegion],
		[Orders].[ShipPostalCode],
		[Orders].[ShipCountry],
		(SELECT COUNT(*) FROM [Order Details] WHERE [Order Details].[OrderID]=[Orders].[OrderID]) [OrderDetailCount]
	FROM [Orders]
	WHERE
		[Orders].[EmployeeID]=@EmployeeID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getOrdersByEmployeeID Succeeded'
ELSE PRINT 'Procedure Creation: getOrdersByEmployeeID Error on Creation'
GO

/****** Object:  StoredProcedure [getOrdersByShipVia] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getOrdersByShipVia]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getOrdersByShipVia];
GO
	
CREATE PROCEDURE [dbo].[getOrdersByShipVia]

(
	@ShipVia int
)
AS

	SELECT
		[Orders].[OrderID],
		[Orders].[CustomerID],
		[Orders].[EmployeeID],
		[Orders].[OrderDate],
		[Orders].[RequiredDate],
		[Orders].[ShippedDate],
		[Orders].[ShipVia],
		[Orders].[Freight],
		[Orders].[ShipName],
		[Orders].[ShipAddress],
		[Orders].[ShipCity],
		[Orders].[ShipRegion],
		[Orders].[ShipPostalCode],
		[Orders].[ShipCountry],
		(SELECT COUNT(*) FROM [Order Details] WHERE [Order Details].[OrderID]=[Orders].[OrderID]) [OrderDetailCount]
	FROM [Orders]
	WHERE
		[Orders].[ShipVia]=@ShipVia

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getOrdersByShipVia Succeeded'
ELSE PRINT 'Procedure Creation: getOrdersByShipVia Error on Creation'
GO

/****** Object:  StoredProcedure [getProducts] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getProducts]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getProducts];
GO
	
CREATE PROCEDURE [dbo].[getProducts]

AS
	SELECT
		[ProductID],
		[ProductName],
		[SupplierID],
		[CategoryID],
		[QuantityPerUnit],
		[UnitPrice],
		[UnitsInStock],
		[UnitsOnOrder],
		[ReorderLevel],
		[Discontinued],
		(SELECT COUNT(*) FROM [Order Details] WHERE [Order Details].[ProductID]=[Products].[ProductID]) [OrderDetailCount]
	FROM [Products]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getProducts Succeeded'
ELSE PRINT 'Procedure Creation: getProducts Error on Creation'
GO

/****** Object:  StoredProcedure [updateProduct] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateProduct]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateProduct];
GO
	
CREATE PROCEDURE [dbo].[updateProduct]

(
	@ProductID int,
	@ProductName nvarchar(40),
	@SupplierID int=null,
	@CategoryID int=null,
	@QuantityPerUnit nvarchar(20)=null,
	@UnitPrice money=null,
	@UnitsInStock smallint=null,
	@UnitsOnOrder smallint=null,
	@ReorderLevel smallint=null,
	@Discontinued bit
)
AS
	UPDATE [Products]
		SET
			[ProductName]=@ProductName,
			[SupplierID]=@SupplierID,
			[CategoryID]=@CategoryID,
			[QuantityPerUnit]=@QuantityPerUnit,
			[UnitPrice]=@UnitPrice,
			[UnitsInStock]=@UnitsInStock,
			[UnitsOnOrder]=@UnitsOnOrder,
			[ReorderLevel]=@ReorderLevel,
			[Discontinued]=@Discontinued
		WHERE [ProductID]=@ProductID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Products] WHERE [ProductID]=@ProductID)
			RAISERROR('Product record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('Product has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateProduct Succeeded'
ELSE PRINT 'Procedure Creation: updateProduct Error on Creation'
GO

/****** Object:  StoredProcedure [addProduct] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addProduct]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addProduct];
GO
	
CREATE PROCEDURE [dbo].[addProduct]

(
	@ProductName nvarchar(40),
	@SupplierID int=null,
	@CategoryID int=null,
	@QuantityPerUnit nvarchar(20)=null,
	@UnitPrice money=null,
	@UnitsInStock smallint=null,
	@UnitsOnOrder smallint=null,
	@ReorderLevel smallint=null,
	@Discontinued bit,
	@newProductID int output
)
AS
	INSERT INTO [Products]
	(
		[ProductName],
		[SupplierID],
		[CategoryID],
		[QuantityPerUnit],
		[UnitPrice],
		[UnitsInStock],
		[UnitsOnOrder],
		[ReorderLevel],
		[Discontinued]
	)
	VALUES
	(
		@ProductName,
		@SupplierID,
		@CategoryID,
		@QuantityPerUnit,
		@UnitPrice,
		@UnitsInStock,
		@UnitsOnOrder,
		@ReorderLevel,
		@Discontinued
	)
	SELECT @newProductID= SCOPE_IDENTITY()
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addProduct Succeeded'
ELSE PRINT 'Procedure Creation: addProduct Error on Creation'
GO

/****** Object:  StoredProcedure [deleteProduct] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteProduct]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteProduct];
GO
	
CREATE PROCEDURE [dbo].[deleteProduct]

(
	@ProductID int
)
AS
	DELETE [Order Details]
		WHERE [ProductID]=@ProductID
	DELETE [Products]
		WHERE [ProductID] = @ProductID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteProduct Succeeded'
ELSE PRINT 'Procedure Creation: deleteProduct Error on Creation'
GO

/****** Object:  StoredProcedure [getProduct] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getProduct]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getProduct];
GO
	
CREATE PROCEDURE [dbo].[getProduct]

(
	@ProductID int
)
AS
	SELECT
		[ProductID],
		[ProductName],
		[SupplierID],
		[CategoryID],
		[QuantityPerUnit],
		[UnitPrice],
		[UnitsInStock],
		[UnitsOnOrder],
		[ReorderLevel],
		[Discontinued],
		(SELECT COUNT(*) FROM [Order Details] WHERE [Order Details].[ProductID]=[Products].[ProductID]) [OrderDetailCount]
	FROM [Products]
	WHERE [ProductID]=@ProductID

	SELECT
		[Order Details].[OrderID],
		[Order Details].[ProductID],
		[Order Details].[UnitPrice],
		[Order Details].[Quantity],
		[Order Details].[Discount],
		[Orders].[CustomerID] [Order_CustomerID],
		[Orders].[EmployeeID] [Order_EmployeeID],
		[Orders].[OrderDate] [Order_OrderDate],
		[Orders].[RequiredDate] [Order_RequiredDate],
		[Orders].[ShippedDate] [Order_ShippedDate],
		[Orders].[ShipVia] [Order_ShipVia],
		[Orders].[Freight] [Order_Freight],
		[Orders].[ShipName] [Order_ShipName],
		[Orders].[ShipAddress] [Order_ShipAddress],
		[Orders].[ShipCity] [Order_ShipCity],
		[Orders].[ShipRegion] [Order_ShipRegion],
		[Orders].[ShipPostalCode] [Order_ShipPostalCode],
		[Orders].[ShipCountry] [Order_ShipCountry]
	FROM [Order Details]
		JOIN [Orders]  ON
			[Orders].[OrderID]=[Order Details].[OrderID]
	WHERE
		[Order Details].[ProductID]=@ProductID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getProduct Succeeded'
ELSE PRINT 'Procedure Creation: getProduct Error on Creation'
GO

/****** Object:  StoredProcedure [existsProduct] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsProduct]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsProduct];
GO
	
CREATE PROCEDURE [dbo].[existsProduct]

(
	@ProductID int
)
	AS
	SELECT COUNT(*)
	FROM [Products] WHERE [ProductID]=@ProductID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsProduct Succeeded'
ELSE PRINT 'Procedure Creation: existsProduct Error on Creation'
GO

/****** Object:  StoredProcedure [getProductsByCategoryID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getProductsByCategoryID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getProductsByCategoryID];
GO
	
CREATE PROCEDURE [dbo].[getProductsByCategoryID]

(
	@CategoryID int
)
AS

	SELECT
		[Products].[ProductID],
		[Products].[ProductName],
		[Products].[SupplierID],
		[Products].[CategoryID],
		[Products].[QuantityPerUnit],
		[Products].[UnitPrice],
		[Products].[UnitsInStock],
		[Products].[UnitsOnOrder],
		[Products].[ReorderLevel],
		[Products].[Discontinued],
		(SELECT COUNT(*) FROM [Order Details] WHERE [Order Details].[ProductID]=[Products].[ProductID]) [OrderDetailCount]
	FROM [Products]
	WHERE
		[Products].[CategoryID]=@CategoryID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getProductsByCategoryID Succeeded'
ELSE PRINT 'Procedure Creation: getProductsByCategoryID Error on Creation'
GO

/****** Object:  StoredProcedure [getProductsBySupplierID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getProductsBySupplierID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getProductsBySupplierID];
GO
	
CREATE PROCEDURE [dbo].[getProductsBySupplierID]

(
	@SupplierID int
)
AS

	SELECT
		[Products].[ProductID],
		[Products].[ProductName],
		[Products].[SupplierID],
		[Products].[CategoryID],
		[Products].[QuantityPerUnit],
		[Products].[UnitPrice],
		[Products].[UnitsInStock],
		[Products].[UnitsOnOrder],
		[Products].[ReorderLevel],
		[Products].[Discontinued],
		(SELECT COUNT(*) FROM [Order Details] WHERE [Order Details].[ProductID]=[Products].[ProductID]) [OrderDetailCount]
	FROM [Products]
	WHERE
		[Products].[SupplierID]=@SupplierID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getProductsBySupplierID Succeeded'
ELSE PRINT 'Procedure Creation: getProductsBySupplierID Error on Creation'
GO

/****** Object:  StoredProcedure [getRegions] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getRegions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getRegions];
GO
	
CREATE PROCEDURE [dbo].[getRegions]

AS
	SELECT
		[RegionID],
		[RegionDescription],
		(SELECT COUNT(*) FROM [Territories] WHERE [Territories].[RegionID]=[Region].[RegionID]) [TerritoryCount]
	FROM [Region]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getRegions Succeeded'
ELSE PRINT 'Procedure Creation: getRegions Error on Creation'
GO

/****** Object:  StoredProcedure [updateRegion] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateRegion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateRegion];
GO
	
CREATE PROCEDURE [dbo].[updateRegion]

(
	@RegionID int,
	@RegionDescription nchar(50)
)
AS
	UPDATE [Region]
		SET
			[RegionDescription]=@RegionDescription
		WHERE [RegionID]=@RegionID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Region] WHERE [RegionID]=@RegionID)
			RAISERROR('Region record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('Region has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateRegion Succeeded'
ELSE PRINT 'Procedure Creation: updateRegion Error on Creation'
GO

/****** Object:  StoredProcedure [addRegion] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addRegion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addRegion];
GO
	
CREATE PROCEDURE [dbo].[addRegion]

(
	@RegionID int,
	@RegionDescription nchar(50)
)
AS
	INSERT INTO [Region]
	(
		[RegionID],
		[RegionDescription]
	)
	VALUES
	(
		@RegionID,
		@RegionDescription
	)

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addRegion Succeeded'
ELSE PRINT 'Procedure Creation: addRegion Error on Creation'
GO

/****** Object:  StoredProcedure [deleteRegion] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteRegion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteRegion];
GO
	
CREATE PROCEDURE [dbo].[deleteRegion]

(
	@RegionID int
)
AS
	DELETE [Territories]
		WHERE [RegionID]=@RegionID
	DELETE [Region]
		WHERE [RegionID] = @RegionID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteRegion Succeeded'
ELSE PRINT 'Procedure Creation: deleteRegion Error on Creation'
GO

/****** Object:  StoredProcedure [getRegion] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getRegion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getRegion];
GO
	
CREATE PROCEDURE [dbo].[getRegion]

(
	@RegionID int
)
AS
	SELECT
		[RegionID],
		[RegionDescription],
		(SELECT COUNT(*) FROM [Territories] WHERE [Territories].[RegionID]=[Region].[RegionID]) [TerritoryCount]
	FROM [Region]
	WHERE [RegionID]=@RegionID

	SELECT
		[Territories].[TerritoryID],
		[Territories].[TerritoryDescription],
		[Territories].[RegionID]
	FROM [Territories]
	WHERE
		[Territories].[RegionID]=@RegionID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getRegion Succeeded'
ELSE PRINT 'Procedure Creation: getRegion Error on Creation'
GO

/****** Object:  StoredProcedure [existsRegion] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsRegion]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsRegion];
GO
	
CREATE PROCEDURE [dbo].[existsRegion]

(
	@RegionID int
)
	AS
	SELECT COUNT(*)
	FROM [Region] WHERE [RegionID]=@RegionID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsRegion Succeeded'
ELSE PRINT 'Procedure Creation: existsRegion Error on Creation'
GO

/****** Object:  StoredProcedure [getShippers] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getShippers]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getShippers];
GO
	
CREATE PROCEDURE [dbo].[getShippers]

AS
	SELECT
		[ShipperID],
		[CompanyName],
		[Phone],
		(SELECT COUNT(*) FROM [Orders] WHERE [Orders].[ShipVia]=[Shippers].[ShipperID]) [OrderCount]
	FROM [Shippers]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getShippers Succeeded'
ELSE PRINT 'Procedure Creation: getShippers Error on Creation'
GO

/****** Object:  StoredProcedure [updateShipper] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateShipper]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateShipper];
GO
	
CREATE PROCEDURE [dbo].[updateShipper]

(
	@ShipperID int,
	@CompanyName nvarchar(40),
	@Phone nvarchar(24)=null
)
AS
	UPDATE [Shippers]
		SET
			[CompanyName]=@CompanyName,
			[Phone]=@Phone
		WHERE [ShipperID]=@ShipperID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Shippers] WHERE [ShipperID]=@ShipperID)
			RAISERROR('Shipper record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('Shipper has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateShipper Succeeded'
ELSE PRINT 'Procedure Creation: updateShipper Error on Creation'
GO

/****** Object:  StoredProcedure [addShipper] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addShipper]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addShipper];
GO
	
CREATE PROCEDURE [dbo].[addShipper]

(
	@CompanyName nvarchar(40),
	@Phone nvarchar(24)=null,
	@newShipperID int output
)
AS
	INSERT INTO [Shippers]
	(
		[CompanyName],
		[Phone]
	)
	VALUES
	(
		@CompanyName,
		@Phone
	)
	SELECT @newShipperID= SCOPE_IDENTITY()
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addShipper Succeeded'
ELSE PRINT 'Procedure Creation: addShipper Error on Creation'
GO

/****** Object:  StoredProcedure [deleteShipper] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteShipper]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteShipper];
GO
	
CREATE PROCEDURE [dbo].[deleteShipper]

(
	@ShipperID int
)
AS
	DELETE [Orders]
		WHERE [ShipVia]=@ShipperID
	DELETE [Shippers]
		WHERE [ShipperID] = @ShipperID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteShipper Succeeded'
ELSE PRINT 'Procedure Creation: deleteShipper Error on Creation'
GO

/****** Object:  StoredProcedure [getShipper] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getShipper]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getShipper];
GO
	
CREATE PROCEDURE [dbo].[getShipper]

(
	@ShipperID int
)
AS
	SELECT
		[ShipperID],
		[CompanyName],
		[Phone],
		(SELECT COUNT(*) FROM [Orders] WHERE [Orders].[ShipVia]=[Shippers].[ShipperID]) [OrderCount]
	FROM [Shippers]
	WHERE [ShipperID]=@ShipperID

	SELECT
		[Orders].[OrderID],
		[Orders].[CustomerID],
		[Orders].[EmployeeID],
		[Orders].[OrderDate],
		[Orders].[RequiredDate],
		[Orders].[ShippedDate],
		[Orders].[ShipVia],
		[Orders].[Freight],
		[Orders].[ShipName],
		[Orders].[ShipAddress],
		[Orders].[ShipCity],
		[Orders].[ShipRegion],
		[Orders].[ShipPostalCode],
		[Orders].[ShipCountry]
	FROM [Orders]
	WHERE
		[Orders].[ShipVia]=@ShipperID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getShipper Succeeded'
ELSE PRINT 'Procedure Creation: getShipper Error on Creation'
GO

/****** Object:  StoredProcedure [existsShipper] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsShipper]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsShipper];
GO
	
CREATE PROCEDURE [dbo].[existsShipper]

(
	@ShipperID int
)
	AS
	SELECT COUNT(*)
	FROM [Shippers] WHERE [ShipperID]=@ShipperID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsShipper Succeeded'
ELSE PRINT 'Procedure Creation: existsShipper Error on Creation'
GO

/****** Object:  StoredProcedure [getSuppliers] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getSuppliers]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getSuppliers];
GO
	
CREATE PROCEDURE [dbo].[getSuppliers]

AS
	SELECT
		[SupplierID],
		[CompanyName],
		[ContactName],
		[ContactTitle],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[Phone],
		[Fax],
		[HomePage],
		(SELECT COUNT(*) FROM [Products] WHERE [Products].[SupplierID]=[Suppliers].[SupplierID]) [ProductCount]
	FROM [Suppliers]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getSuppliers Succeeded'
ELSE PRINT 'Procedure Creation: getSuppliers Error on Creation'
GO

/****** Object:  StoredProcedure [updateSupplier] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateSupplier]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateSupplier];
GO
	
CREATE PROCEDURE [dbo].[updateSupplier]

(
	@SupplierID int,
	@CompanyName nvarchar(40),
	@ContactName nvarchar(30)=null,
	@ContactTitle nvarchar(30)=null,
	@Address nvarchar(60)=null,
	@City nvarchar(15)=null,
	@Region nvarchar(15)=null,
	@PostalCode nvarchar(10)=null,
	@Country nvarchar(15)=null,
	@Phone nvarchar(24)=null,
	@Fax nvarchar(24)=null,
	@HomePage nvarchar(MAX)=null
)
AS
	UPDATE [Suppliers]
		SET
			[CompanyName]=@CompanyName,
			[ContactName]=@ContactName,
			[ContactTitle]=@ContactTitle,
			[Address]=@Address,
			[City]=@City,
			[Region]=@Region,
			[PostalCode]=@PostalCode,
			[Country]=@Country,
			[Phone]=@Phone,
			[Fax]=@Fax,
			[HomePage]=@HomePage
		WHERE [SupplierID]=@SupplierID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Suppliers] WHERE [SupplierID]=@SupplierID)
			RAISERROR('Supplier record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('Supplier has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateSupplier Succeeded'
ELSE PRINT 'Procedure Creation: updateSupplier Error on Creation'
GO

/****** Object:  StoredProcedure [addSupplier] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addSupplier]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addSupplier];
GO
	
CREATE PROCEDURE [dbo].[addSupplier]

(
	@CompanyName nvarchar(40),
	@ContactName nvarchar(30)=null,
	@ContactTitle nvarchar(30)=null,
	@Address nvarchar(60)=null,
	@City nvarchar(15)=null,
	@Region nvarchar(15)=null,
	@PostalCode nvarchar(10)=null,
	@Country nvarchar(15)=null,
	@Phone nvarchar(24)=null,
	@Fax nvarchar(24)=null,
	@HomePage nvarchar(MAX)=null,
	@newSupplierID int output
)
AS
	INSERT INTO [Suppliers]
	(
		[CompanyName],
		[ContactName],
		[ContactTitle],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[Phone],
		[Fax],
		[HomePage]
	)
	VALUES
	(
		@CompanyName,
		@ContactName,
		@ContactTitle,
		@Address,
		@City,
		@Region,
		@PostalCode,
		@Country,
		@Phone,
		@Fax,
		@HomePage
	)
	SELECT @newSupplierID= SCOPE_IDENTITY()
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addSupplier Succeeded'
ELSE PRINT 'Procedure Creation: addSupplier Error on Creation'
GO

/****** Object:  StoredProcedure [deleteSupplier] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteSupplier]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteSupplier];
GO
	
CREATE PROCEDURE [dbo].[deleteSupplier]

(
	@SupplierID int
)
AS
	DELETE [Products]
		WHERE [SupplierID]=@SupplierID
	DELETE [Suppliers]
		WHERE [SupplierID] = @SupplierID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteSupplier Succeeded'
ELSE PRINT 'Procedure Creation: deleteSupplier Error on Creation'
GO

/****** Object:  StoredProcedure [getSupplier] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getSupplier]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getSupplier];
GO
	
CREATE PROCEDURE [dbo].[getSupplier]

(
	@SupplierID int
)
AS
	SELECT
		[SupplierID],
		[CompanyName],
		[ContactName],
		[ContactTitle],
		[Address],
		[City],
		[Region],
		[PostalCode],
		[Country],
		[Phone],
		[Fax],
		[HomePage],
		(SELECT COUNT(*) FROM [Products] WHERE [Products].[SupplierID]=[Suppliers].[SupplierID]) [ProductCount]
	FROM [Suppliers]
	WHERE [SupplierID]=@SupplierID

	SELECT
		[Products].[ProductID],
		[Products].[ProductName],
		[Products].[SupplierID],
		[Products].[CategoryID],
		[Products].[QuantityPerUnit],
		[Products].[UnitPrice],
		[Products].[UnitsInStock],
		[Products].[UnitsOnOrder],
		[Products].[ReorderLevel],
		[Products].[Discontinued]
	FROM [Products]
	WHERE
		[Products].[SupplierID]=@SupplierID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getSupplier Succeeded'
ELSE PRINT 'Procedure Creation: getSupplier Error on Creation'
GO

/****** Object:  StoredProcedure [existsSupplier] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsSupplier]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsSupplier];
GO
	
CREATE PROCEDURE [dbo].[existsSupplier]

(
	@SupplierID int
)
	AS
	SELECT COUNT(*)
	FROM [Suppliers] WHERE [SupplierID]=@SupplierID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsSupplier Succeeded'
ELSE PRINT 'Procedure Creation: existsSupplier Error on Creation'
GO

/****** Object:  StoredProcedure [getTerritories] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getTerritories]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getTerritories];
GO
	
CREATE PROCEDURE [dbo].[getTerritories]

AS
	SELECT
		[TerritoryID],
		[TerritoryDescription],
		[RegionID],
		(SELECT COUNT(*) FROM [EmployeeTerritories] WHERE [EmployeeTerritories].[TerritoryID]=[Territories].[TerritoryID]) [EmployeeTerritoryCount]
	FROM [Territories]
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getTerritories Succeeded'
ELSE PRINT 'Procedure Creation: getTerritories Error on Creation'
GO

/****** Object:  StoredProcedure [updateTerritory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[updateTerritory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [updateTerritory];
GO
	
CREATE PROCEDURE [dbo].[updateTerritory]

(
	@TerritoryID nvarchar(20),
	@TerritoryDescription nchar(50),
	@RegionID int
)
AS
	UPDATE [Territories]
		SET
			[TerritoryDescription]=@TerritoryDescription,
			[RegionID]=@RegionID
		WHERE [TerritoryID]=@TerritoryID
	IF @@ROWCOUNT = 0
	BEGIN
		IF NOT exists(select * from [Territories] WHERE [TerritoryID]=@TerritoryID)
			RAISERROR('Territory record has been deleted by another user', 16, 1)
		ELSE           	
			RAISERROR('Territory has been edited by another user', 16, 1)            	
	END

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: updateTerritory Succeeded'
ELSE PRINT 'Procedure Creation: updateTerritory Error on Creation'
GO

/****** Object:  StoredProcedure [addTerritory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[addTerritory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [addTerritory];
GO
	
CREATE PROCEDURE [dbo].[addTerritory]

(
	@TerritoryID nvarchar(20),
	@TerritoryDescription nchar(50),
	@RegionID int
)
AS
	INSERT INTO [Territories]
	(
		[TerritoryID],
		[TerritoryDescription],
		[RegionID]
	)
	VALUES
	(
		@TerritoryID,
		@TerritoryDescription,
		@RegionID
	)

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: addTerritory Succeeded'
ELSE PRINT 'Procedure Creation: addTerritory Error on Creation'
GO

/****** Object:  StoredProcedure [deleteTerritory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[deleteTerritory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [deleteTerritory];
GO
	
CREATE PROCEDURE [dbo].[deleteTerritory]

(
	@TerritoryID nvarchar(20)
)
AS
	DELETE [EmployeeTerritories]
		WHERE [TerritoryID]=@TerritoryID
	DELETE [Territories]
		WHERE [TerritoryID] = @TerritoryID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: deleteTerritory Succeeded'
ELSE PRINT 'Procedure Creation: deleteTerritory Error on Creation'
GO

/****** Object:  StoredProcedure [getTerritory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getTerritory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getTerritory];
GO
	
CREATE PROCEDURE [dbo].[getTerritory]

(
	@TerritoryID nvarchar(20)
)
AS
	SELECT
		[TerritoryID],
		[TerritoryDescription],
		[RegionID],
		(SELECT COUNT(*) FROM [EmployeeTerritories] WHERE [EmployeeTerritories].[TerritoryID]=[Territories].[TerritoryID]) [EmployeeTerritoryCount]
	FROM [Territories]
	WHERE [TerritoryID]=@TerritoryID

	SELECT
		[EmployeeTerritories].[EmployeeID],
		[EmployeeTerritories].[TerritoryID],
		[Employees].[LastName] [Employee_LastName],
		[Employees].[FirstName] [Employee_FirstName],
		[Employees].[Title] [Employee_Title],
		[Employees].[TitleOfCourtesy] [Employee_TitleOfCourtesy],
		[Employees].[BirthDate] [Employee_BirthDate],
		[Employees].[HireDate] [Employee_HireDate],
		[Employees].[Address] [Employee_Address],
		[Employees].[City] [Employee_City],
		[Employees].[Region] [Employee_Region],
		[Employees].[PostalCode] [Employee_PostalCode],
		[Employees].[Country] [Employee_Country],
		[Employees].[HomePhone] [Employee_HomePhone],
		[Employees].[Extension] [Employee_Extension],
		[Employees].[Photo] [Employee_Photo],
		[Employees].[Notes] [Employee_Notes],
		[Employees].[ReportsTo] [Employee_ReportsTo],
		[Employees].[PhotoPath] [Employee_PhotoPath]
	FROM [EmployeeTerritories]
		JOIN [Employees]  ON
			[Employees].[EmployeeID]=[EmployeeTerritories].[EmployeeID]
	WHERE
		[EmployeeTerritories].[TerritoryID]=@TerritoryID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getTerritory Succeeded'
ELSE PRINT 'Procedure Creation: getTerritory Error on Creation'
GO

/****** Object:  StoredProcedure [existsTerritory] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[existsTerritory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [existsTerritory];
GO
	
CREATE PROCEDURE [dbo].[existsTerritory]

(
	@TerritoryID nvarchar(20)
)
	AS
	SELECT COUNT(*)
	FROM [Territories] WHERE [TerritoryID]=@TerritoryID
	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: existsTerritory Succeeded'
ELSE PRINT 'Procedure Creation: existsTerritory Error on Creation'
GO

/****** Object:  StoredProcedure [getTerritoriesByRegionID] ******/
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[getTerritoriesByRegionID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
    DROP PROCEDURE [getTerritoriesByRegionID];
GO
	
CREATE PROCEDURE [dbo].[getTerritoriesByRegionID]

(
	@RegionID int
)
AS

	SELECT
		[Territories].[TerritoryID],
		[Territories].[TerritoryDescription],
		[Territories].[RegionID],
		(SELECT COUNT(*) FROM [EmployeeTerritories] WHERE [EmployeeTerritories].[TerritoryID]=[Territories].[TerritoryID]) [EmployeeTerritoryCount]
	FROM [Territories]
	WHERE
		[Territories].[RegionID]=@RegionID

	RETURN
GO
-- Display the status of Proc creation
IF (@@Error = 0) PRINT 'Procedure Creation: getTerritoriesByRegionID Succeeded'
ELSE PRINT 'Procedure Creation: getTerritoriesByRegionID Error on Creation'
GO
