using FluentAssertions;
using Xunit;

namespace MicroQueryOrm.SqlServer.Tests
{
    public class MicroQuerySingleResultTests : MicroQueryBaseTests
    {
        [Fact]
        public async Task SingleResultAsync_WithoutTransaction_ManualMap_Success()
        {
            decimal result = await _microQuery.SingleResultAsync<decimal>(@"
            SELECT TOP (1)
            [Price]
            FROM [dbo].[Inventory]
            ");

            result.Should().NotBe(0);
        }
    }
}