using FluentAssertions;
using Moq;
using Xunit;
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer;
using Microsoft.Extensions.Configuration;
using MicroQueryOrm.Tests.TestModels;

namespace MicroQueryOrm.Tests
{
    public class MicroQueryTests
    {
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


            List<Product> result = await (microQuery.QueryAsync(@"
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
", System.Data.CommandType.Text)
                .MapAsync<Product>()
                .ToListAsync());

            result.Should().NotBeNull();
            result.Should().HaveCount(10);
        }
    }
}