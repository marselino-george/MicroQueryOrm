CREATE TABLE Inventory
(
    WarehouseId NVARCHAR(255),
    CategoryId NVARCHAR(255),
    CategoryDesc NVARCHAR(255),
    Sku NVARCHAR(255),
    ProductDesc NVARCHAR(255),
    Price DECIMAL(18, 2) NULL,
    Quantity DECIMAL(18, 2) NULL,
    TotalPrice DECIMAL(18, 2) NULL,
    Updated DATETIME
);