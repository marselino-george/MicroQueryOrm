using FluentAssertions;
using Xunit;
using MicroQueryOrm.Core.Extensions;
using MicroQueryOrm.Sqlite.Tests.TestModels;

namespace MicroQueryOrm.Sqlite.Tests
{
    public class MicroQueryTests : MicroQueryBaseTests
    {
        [Fact]
        public void Query_WithoutTransaction_ManualMap_Success()
        {
            List<Product> result = _microQuery.Query(@"
            SELECT 
                [Id],
                [ImageUrl],
                [Category],
                [Title],
                [Rating],
                [ReviewsCount],
                [Price],
                [ShopName],
                [CreatedAt],
                [RemoteId]
            FROM [Products]
            LIMIT 1;
            ")
            .Map<Product>()
            .ToList();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }
    }
}