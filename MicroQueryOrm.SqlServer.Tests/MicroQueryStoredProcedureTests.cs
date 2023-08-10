using FluentAssertions;
using Xunit;
using MicroQueryOrm.SqlServer.Extensions;
using MicroQueryOrm.SqlServer.Tests.TestModels;
using System.Data;

namespace MicroQueryOrm.SqlServer.Tests
{
    public class MicroQueryStoredProcedureTests : MicroQueryBaseTests
    {
        [Fact]
        public async Task StoredProcedure_WithParams_Success()
        {
            var inventoryParams = InventoryParameters();
            await _microQuery.ExecuteAsync("sp_InsertInventory", inventoryParams, CommandType.StoredProcedure);

            Exception? capturedException = null;
            Func<IEnumerable<Inventory>?> act = () => {
                try
                {
                    IDbDataParameter[] sqlParams = (new { inventoryParams.Sku }).ToSqlParams();
                    var data = _microQuery.StoredProcedure<Inventory>("sp_GetInventoryBySku", sqlParams);
                    return data;
                }
                catch (Exception ex)
                {
                    capturedException = ex;
                }
                return null;
            };

            var result = act?.Invoke();
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().ContainSingle(item => item.Sku == inventoryParams.Sku);
            result.Should().NotContainNulls();

            capturedException.Should().BeNull("because no errors should occur when executing the query with valid parameters");
        }
    }
}