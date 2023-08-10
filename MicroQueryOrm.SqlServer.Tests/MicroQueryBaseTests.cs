using Microsoft.Extensions.Configuration;
using NSubstitute;
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer.Tests.TestModels;

namespace MicroQueryOrm.SqlServer.Tests
{
    public abstract class MicroQueryBaseTests
    {
        private static Random _random = new Random();

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IDataBaseConfiguration _dbConfigSubstitute;
        protected readonly MicroQuery _microQuery;

        public MicroQueryBaseTests()
        {
            _configuration = new ConfigurationBuilder()
                            .AddUserSecrets<MicroQueryTests>()
                            .Build();

            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];

            _dbConfigSubstitute = Substitute.For<IDataBaseConfiguration>();
            _dbConfigSubstitute.ConnectionString.Returns(_connectionString);
            _dbConfigSubstitute.CommandTimeout.Returns(30);

            _microQuery = new MicroQuery(_dbConfigSubstitute);
        }

        protected static Inventory InventoryParameters()
        {
            var randomProductSku = Guid.NewGuid().ToString();
            decimal randomPrice = GetRandomPrice(10.00m, 1000.00m);
            int quantity = GetRandomQuantity(1, 100);
            return new()
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
            };
        }

        protected static decimal GetRandomPrice(decimal min, decimal max)
        {
            double randomValue = _random.NextDouble() * (double)(max - min) + (double)min;
            return Math.Round((decimal)randomValue, 2);
        }

        protected static int GetRandomQuantity(int min, int max)
        {
            return _random.Next(min, max + 1);
        }
    }
}