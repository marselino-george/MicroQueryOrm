using FluentAssertions;
using Moq;
using Xunit;
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer;
using Microsoft.Extensions.Configuration;
using MicroQueryOrm.Tests.TestModels;
using MicroQueryOrm.SqlServer.Extensions;
using Microsoft.Data.SqlClient;

namespace MicroQueryOrm.Tests
{
    public class MicroQueryTests
    {
        private static Random _random = new Random();

        private readonly IConfiguration _configuration;

        public MicroQueryTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<MicroQueryTests>()
                .Build();
        }

        [Fact]
        public void Query_WithoutTransaction_Success()
        {
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var dbConfigMock = new Mock<IDataBaseConfiguration>();
            dbConfigMock.Setup(d => d.ConnectionString).Returns(connectionString);
            dbConfigMock.Setup(d => d.CommandTimeout).Returns(30);
            var microQuery = new MicroQuery(dbConfigMock.Object);

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
FROM [CrawlingDb].[dbo].[Products]
", commandType: System.Data.CommandType.Text)
                .Map<Product>()
                .ToList();

            result.Should().NotBeNull();
            result.Should().HaveCount(10);
        }

        [Fact]
        public async Task QueryAsync_WithoutTransaction_Success()
        {
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var dbConfigMock = new Mock<IDataBaseConfiguration>();
            dbConfigMock.Setup(d => d.ConnectionString).Returns(connectionString);
            dbConfigMock.Setup(d => d.CommandTimeout).Returns(30);
            var microQuery = new MicroQuery(dbConfigMock.Object);


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

            result.Should().NotBeNull();
            result.Should().HaveCount(10);
        }

        [Fact]
        public async Task Execute_Insert_ShouldNotThrowException_WhenCalledWithValidParameters()
        {
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            var dbConfigMock = new Mock<IDataBaseConfiguration>();
            dbConfigMock.Setup(d => d.ConnectionString).Returns(connectionString);
            dbConfigMock.Setup(d => d.CommandTimeout).Returns(30);

            var randomProductSku = Guid.NewGuid().ToString();
            decimal randomPrice = GetRandomPrice(10.00m, 1000.00m);
            int quantity = GetRandomQuantity(1, 100);
            var inventoryParams = (new Inventory
            {
                WarehouseId = "1",
                CategoryId = "11",
                CategoryDesc = "Category Desc 1",
                Sku = randomProductSku,
                ProductDesc = $"Product {randomProductSku}",
                Price = randomPrice,
                Quantity = quantity,
                TotalPrice = randomPrice * quantity,
                Updated = DateTime.UtcNow
            })
            .ToSqlParams();

            Exception? capturedException = null;

            var microQuery = new MicroQuery(dbConfigMock.Object);
            Action act = () => microQuery.Execute(@"
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
			)", inventoryParams, System.Data.CommandType.Text,
            onError: (ex) =>
            {
                capturedException = ex;
            });

            act.Should().NotThrow();
            capturedException.Should().BeNull("because no errors should occur when executing the query with valid parameters");

            var retrievedData = microQuery
                .Query($"SELECT * FROM [dbo].[Inventory] WHERE WarehouseId = 1 AND Sku = '{randomProductSku}'", System.Data.CommandType.Text)
                .Rows[0];

            retrievedData.Should().NotBeNull();
            retrievedData["WarehouseId"].Should().Be("1");
            retrievedData["Sku"].Should().Be(randomProductSku);
        }

        private static decimal GetRandomPrice(decimal min, decimal max)
        {
            double randomValue = _random.NextDouble() * (double)(max - min) + (double)min;
            return Math.Round((decimal)randomValue, 2);
        }

        private static int GetRandomQuantity(int min, int max)
        {
            return _random.Next(min, max + 1);
        }
    }
}