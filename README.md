# MicroQueryOrm
Simplified Ado NET queries for Sql databases.

- For now it is compatible only with Sql Server Databases.

## Usage

Include the following namespaces:
```
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer;
```

### Basic Query:
```
DataBaseConfiguration dbConfig = new()
	{
		ConnectionString = "<your_connection_string>",
		CommandTimeout = 30,
		ConnectionStringName = "Default"
	};
	
	List<Product> result = await microQuery.QueryAsync(@"
            SELECT TOP (10)
            [Id]
            ,[ImageUrl]
            ,[Category]
            ,[Title]
            ,[Rating]
            ,[ReviewsCount]
            ,[Price]
            ,[ShopName]
            ,[CreatedAt]
            ,[RemoteId]
            FROM [dbo].[Products]
            ", System.Data.CommandType.Text)
            .MapAsync<Product>()
            .ToListAsync();
```
#### Test Model:
```
public class Product
{
    public Guid Id { get; set; }

    [Column("ImageUrl")]
    public string ImageUrl { get; set; }

    [Column("Category")]
    public string Category { get; set; }

    // With or without [Column("Title")] attribute
    public string Title { get; set; }

    [Column("Rating")]
    public string Rating { get; set; }

    [Column("ReviewsCount")]
    public string ReviewsCount { get; set; }

    [Column("Price")]
    public string Price { get; set; }

    [Column("ShopName")]
    public string ShopName { get; set; }

    [Ignore]
    public DateTime CreatedAt { get; set; }

    [Column("RemoteId")]
    public long RemoteId { get; set; }
}
```

### "Execute" Method:
```
var inventoryParams = (new Inventory
{
	WarehouseId = "1",
	CategoryId = "11",
	CategoryDesc = "Category Desc 1",
	Sku = "12345",
	ProductDesc = $"Product 12345",
	Price = 100,
	Quantity = 2,
	TotalPrice = 200,
	Updated = DateTime.UtcNow
})
.ToSqlParams(); // Automatically converts to SqlParameters

microQuery.Execute(@"
INSERT INTO [dbo].[Inventory]
([WarehouseId]
,[CategoryId]
,[CategoryDesc]
,[Sku]
,[ProductDesc]
,[Price]
,[Quantity]
,[TotalPrice]
,[Updated])
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
)", inventoryParams, System.Data.CommandType.Text);
```
#### Test Model:
```
public class Inventory
{
    public string WarehouseId { get; set; }
    public string CategoryId { get; set; }
    public string CategoryDesc { get; set; }
    public string Sku { get; set; }
    public string ProductDesc { get; set; }
    public decimal? Price { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? TotalPrice { get; set; }
    public DateTime Updated { get; set; }
}
```