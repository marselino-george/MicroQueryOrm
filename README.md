# MicroQueryOrm
Simplified Ado NET queries for Sql databases.

- For now it is compatible only with Sql Server Databases.

## Usage

Include the following namespaces:
```
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer;
```

Additional namespaces for helper methods:
```
using MicroQueryOrm.Common.Extensions;
using MicroQueryOrm.SqlServer.Extensions;
```

### Basic query example:
```
DataBaseConfiguration dbConfig = new()
{
	ConnectionString = "<your_connection_string>",
	CommandTimeout = 30,
	ConnectionStringName = "Default"
};

var microQuery = new MicroQuery(dbConfig);
List<Product> result = await microQuery.QueryAsync<Product>(
    """
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
    """
)
.ToListAsync();
```
#### Test Model:
```
public class Product
{
    // With or without [Column("Id")] attribute
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

### "Execute" methods example:
For executing queries or StoredProcedures that do not return data, use the "Execute" methods.

```
var inventoryParams = new Inventory
{
	WarehouseId = "1",
	CategoryId = "11",
	CategoryDesc = "Category Desc 1",
	Sku = "12345",
	ProductDesc = $"Product 12345",
	Price = 100m,
	Quantity = 2m,
	TotalPrice = 200m,
	Updated = DateTime.UtcNow
};

await microQuery.ExecuteAsync(@"
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
)", inventoryParams);
```
or with Stored Procedure:
```
await microQuery.ExecuteAsync("sp_InsertInventory", inventoryParams, CommandType.StoredProcedure);
```

### "StoredProcedure" methods example:
StoredProcedure methods are used to execute Stored Procedures that return data.
```
IDbDataParameter[] sqlParams = (new { Sku = inventoryParams.Sku }).ToSqlParams();
IEnumerable<Inventory> data = microQuery.StoredProcedure<Inventory>("sp_GetInventoryBySku", sqlParams);
```

### "SingleResult" methods example:
"SingleResult" methods are used to execute queries that return a single result.
```
decimal result = await microQuery.SingleResultAsync<decimal>(@"
    SELECT TOP (1)
    [Price]
    FROM [dbo].[Inventory]
");
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