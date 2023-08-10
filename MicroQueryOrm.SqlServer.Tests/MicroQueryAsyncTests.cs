using FluentAssertions;
using Xunit;
using MicroQueryOrm.SqlServer.Extensions;
using MicroQueryOrm.SqlServer.Tests.TestModels;

namespace MicroQueryOrm.SqlServer.Tests
{
    public class MicroQueryAsyncTests : MicroQueryBaseTests
    {
        [Fact]
        public async Task QueryAsync_WithoutTransaction_ManualMap_Success()
        {
            List<Product> result = await _microQuery.QueryAsync(@"
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
            ")
            .MapAsync<Product>()
            .ToListAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(10);
        }

        [Fact]
        public async Task QueryAsync_WithoutTransaction_AutoMap_Success()
        {
            List<Product> result = await _microQuery.QueryAsync<Product>(
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

            result.Should().NotBeNull();
            result.Should().HaveCount(10);
        }
    }
}