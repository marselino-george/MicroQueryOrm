# MicroQueryOrm
Simplified Ado NET queries for Sql Server databases.

## Usage

```
public class DataBaseConfiguration : IDataBaseConfiguration
{
	public string ConnectionStringName { get; set; }

	public string ConnectionString { get; set; }

	public int CommandTimeout { get; set; }
}

DataBaseConfiguration dbConfig = new()
	{
		ConnectionString = "<your_connection_string>",
		CommandTimeout = 30,
		ConnectionStringName = "Default"
	};
	
	var microQuery = new MicroQuery(dbConfig);
List<Product> result = microQuery.Query(@"
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
", commandType: System.Data.CommandType.Text)
                .Map<Product>()
                .ToList();
```

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
