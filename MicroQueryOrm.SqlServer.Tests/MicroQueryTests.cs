using FluentAssertions;
using Xunit;
using MicroQueryOrm.Core.Extensions;
using MicroQueryOrm.SqlServer.Tests.TestModels;

namespace MicroQueryOrm.SqlServer.Tests
{
    public class MicroQueryTests : MicroQueryBaseTests
    {
        [Fact]
        public void Query_WithoutTransaction_ManualMap_Success()
        {
            List<Product> result = _microQuery.Query(@"
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
            .Map<Product>()
            .ToList();

            result.Should().NotBeNull();
            result.Should().HaveCount(10);
        }
    }
}