/****** Object:  StoredProcedure [AddOrderDetail] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AddOrderDetail]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [AddOrderDetail]
GO

CREATE PROCEDURE [AddOrderDetail]
    @OrderID uniqueidentifier,
    @OrderDetailID uniqueidentifier,
    @ProductID uniqueidentifier,
    @PurchaseUnitPrice money,
    @Quantity int
AS
    BEGIN

        SET NOCOUNT ON

        /* Insert object into OrderDetail */
        INSERT INTO [OrderDetail]
        (
            [OrderID],
            [OrderDetailID],
            [ProductID],
            [PurchaseUnitPrice],
            [Quantity]
        )
        VALUES
        (
            @OrderID,
            @OrderDetailID,
            @ProductID,
            @PurchaseUnitPrice,
            @Quantity
        )

    END
GO

/****** Object:  StoredProcedure [UpdateOrderDetail] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[UpdateOrderDetail]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [UpdateOrderDetail]
GO

CREATE PROCEDURE [UpdateOrderDetail]
    @OrderDetailID uniqueidentifier,
    @ProductID uniqueidentifier,
    @PurchaseUnitPrice money,
    @Quantity int
AS
    BEGIN

        SET NOCOUNT ON

        /* Check for object existence */
        IF NOT EXISTS
        (
            SELECT [OrderDetailID] FROM [OrderDetail]
            WHERE
                [OrderDetailID] = @OrderDetailID
        )
        BEGIN
            RAISERROR ('''OrderDetail'' object not found. It was probably removed by another user.', 16, 1)
            RETURN
        END

        /* Update object in OrderDetail */
        UPDATE [OrderDetail]
        SET
            [ProductID] = @ProductID,
            [PurchaseUnitPrice] = @PurchaseUnitPrice,
            [Quantity] = @Quantity
        WHERE
            [OrderDetailID] = @OrderDetailID

    END
GO

/****** Object:  StoredProcedure [DeleteOrderDetail] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DeleteOrderDetail]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [DeleteOrderDetail]
GO

CREATE PROCEDURE [DeleteOrderDetail]
    @OrderDetailID uniqueidentifier
AS
    BEGIN

        SET NOCOUNT ON

        /* Check for object existence */
        IF NOT EXISTS
        (
            SELECT [OrderDetailID] FROM [OrderDetail]
            WHERE
                [OrderDetailID] = @OrderDetailID
        )
        BEGIN
            RAISERROR ('''OrderDetail'' object not found. It was probably removed by another user.', 16, 1)
            RETURN
        END

        /* Delete OrderDetail object from OrderDetail */
        DELETE
        FROM [OrderDetail]
        WHERE
            [OrderDetail].[OrderDetailID] = @OrderDetailID

    END
GO
