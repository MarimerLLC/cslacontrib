/****** Object:  StoredProcedure [GetOrder] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GetOrder]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [GetOrder]
GO

CREATE PROCEDURE [GetOrder]
    @OrderID uniqueidentifier
AS
    BEGIN

        SET NOCOUNT ON

        /* Get Order from table */
        SELECT
            [Order].[OrderID],
            [Order].[UserName],
            [Order].[OrderNumber],
            [Order].[OrderDate],
            [Order].[CardType],
            [Order].[CardHolder],
            [Order].[CreditCard],
            [Order].[ExpDate]
        FROM [Order]
        WHERE
            [Order].[OrderID] = @OrderID

        /* Get OrderDetail from table */
        SELECT
            [OrderDetail].[OrderDetailID],
            [OrderDetail].[ProductID],
            [OrderDetail].[PurchaseUnitPrice],
            [OrderDetail].[Quantity]
        FROM [OrderDetail]
            INNER JOIN [Order] ON [OrderDetail].[OrderID] = [Order].[OrderID]
        WHERE
            [Order].[OrderID] = @OrderID

    END
GO

/****** Object:  StoredProcedure [AddOrder] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AddOrder]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [AddOrder]
GO

CREATE PROCEDURE [AddOrder]
    @OrderID uniqueidentifier,
    @UserName varchar(50),
    @OrderNumber varchar(20),
    @OrderDate datetime,
    @CardType varchar(5),
    @CardHolder varchar(100),
    @CreditCard varchar(20),
    @ExpDate varchar(6)
AS
    BEGIN

        SET NOCOUNT ON

        /* Insert object into Order */
        INSERT INTO [Order]
        (
            [OrderID],
            [UserName],
            [OrderNumber],
            [OrderDate],
            [CardType],
            [CardHolder],
            [CreditCard],
            [ExpDate]
        )
        VALUES
        (
            @OrderID,
            @UserName,
            @OrderNumber,
            @OrderDate,
            @CardType,
            @CardHolder,
            @CreditCard,
            @ExpDate
        )

    END
GO

/****** Object:  StoredProcedure [UpdateOrder] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[UpdateOrder]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [UpdateOrder]
GO

CREATE PROCEDURE [UpdateOrder]
    @OrderID uniqueidentifier,
    @UserName varchar(50),
    @OrderNumber varchar(20),
    @OrderDate datetime,
    @CardType varchar(5),
    @CardHolder varchar(100),
    @CreditCard varchar(20),
    @ExpDate varchar(6)
AS
    BEGIN

        SET NOCOUNT ON

        /* Check for object existence */
        IF NOT EXISTS
        (
            SELECT [OrderID] FROM [Order]
            WHERE
                [OrderID] = @OrderID
        )
        BEGIN
            RAISERROR ('''Order'' object not found. It was probably removed by another user.', 16, 1)
            RETURN
        END

        /* Update object in Order */
        UPDATE [Order]
        SET
            [UserName] = @UserName,
            [OrderNumber] = @OrderNumber,
            [OrderDate] = @OrderDate,
            [CardType] = @CardType,
            [CardHolder] = @CardHolder,
            [CreditCard] = @CreditCard,
            [ExpDate] = @ExpDate
        WHERE
            [OrderID] = @OrderID

    END
GO

/****** Object:  StoredProcedure [DeleteOrder] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DeleteOrder]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [DeleteOrder]
GO

CREATE PROCEDURE [DeleteOrder]
    @OrderID uniqueidentifier
AS
    BEGIN

        SET NOCOUNT ON

        /* Check for object existence */
        IF NOT EXISTS
        (
            SELECT [OrderID] FROM [Order]
            WHERE
                [OrderID] = @OrderID
        )
        BEGIN
            RAISERROR ('''Order'' object not found. It was probably removed by another user.', 16, 1)
            RETURN
        END

        /* Delete child OrderDetail from OrderDetail */
        DELETE
            [OrderDetail]
        FROM [OrderDetail]
            INNER JOIN [Order] ON [OrderDetail].[OrderID] = [Order].[OrderID]
        WHERE
            [Order].[OrderID] = @OrderID

        /* Delete Order object from Order */
        DELETE
        FROM [Order]
        WHERE
            [Order].[OrderID] = @OrderID

    END
GO
