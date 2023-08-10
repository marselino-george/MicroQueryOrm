CREATE PROCEDURE sp_InsertInventory
    @WarehouseId NVARCHAR(50),
    @CategoryId NVARCHAR(50),
    @CategoryDesc NVARCHAR(255),
    @Sku NVARCHAR(50),
    @ProductDesc NVARCHAR(255),
    @Price DECIMAL(18, 2) = NULL,
    @Quantity DECIMAL(18, 2) = NULL,
    @TotalPrice DECIMAL(18, 2) = NULL,
    @Updated DATETIME
AS
BEGIN
    INSERT INTO dbo.Inventory
    (
        WarehouseId,
        CategoryId,
        CategoryDesc,
        Sku,
        ProductDesc,
        Price,
        Quantity,
        TotalPrice,
        Updated
    )
    VALUES
    (
        @WarehouseId,
        @CategoryId,
        @CategoryDesc,
        @Sku,
        @ProductDesc,
        @Price,
        @Quantity,
        @TotalPrice,
        @Updated
    )
END
