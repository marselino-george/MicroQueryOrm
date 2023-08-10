CREATE PROCEDURE sp_GetInventoryBySku
    @Sku UNIQUEIDENTIFIER
AS
BEGIN
    SELECT 
        WarehouseId,
        CategoryId,
        CategoryDesc,
        Sku,
        ProductDesc,
        Price,
        Quantity,
        TotalPrice,
        Updated
    FROM dbo.Inventory
    WHERE Sku = @Sku
END