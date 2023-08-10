using System.Data;
using FluentAssertions;
using Xunit;
using MicroQueryOrm.SqlServer.Extensions;
using MicroQueryOrm.SqlServer.Tests.TestModels;

namespace MicroQueryOrm.SqlServer.Tests
{
    public class MicroQueryExecuteTests : MicroQueryBaseTests
    {
        [Fact]
        public async Task Execute_Insert_ShouldNotThrowException_WhenCalledWithValidParameters_TParams()
        {
            var inventoryParams = InventoryParameters();
            Exception? capturedException = null;

            Action act = () => {
                try
                {
                    _microQuery.Execute<Inventory>(InsertInventoryQuery(), inventoryParams);
                }
                catch (Exception ex)
                {
                    capturedException = ex;
                }
            };

            act.Should().NotThrow();
            capturedException.Should().BeNull("because no errors should occur when executing the query with valid parameters");

            var retrievedData = _microQuery
                .Query($"SELECT * FROM [dbo].[Inventory] WHERE WarehouseId = 1 AND Sku = '{inventoryParams.Sku}'")
                .Rows[0];

            retrievedData.Should().NotBeNull();
            retrievedData["WarehouseId"].Should().Be("1");
            retrievedData["Sku"].Should().Be(inventoryParams.Sku);
        }

        [Fact]
        public async Task Execute_Insert_ShouldNotThrowException_WhenCalledWithValidParameters_IDbDataParameter()
        {
            var inventoryParams = InventoryParameters();
            IDbDataParameter[] dbInventoryParams = inventoryParams.ToSqlParams();
            Exception? capturedException = null;

            Action act = () => {
                try
                {
                    _microQuery.Execute(InsertInventoryQuery(), dbInventoryParams);
                }
                catch (Exception ex)
                {
                    capturedException = ex;
                }
            };

            act.Should().NotThrow();
            capturedException.Should().BeNull("because no errors should occur when executing the query with valid parameters");

            var retrievedData = _microQuery
                .Query($"SELECT * FROM [dbo].[Inventory] WHERE Sku = '{inventoryParams.Sku}'")
                .Rows[0];

            retrievedData.Should().NotBeNull();
            retrievedData["Sku"].Should().Be(inventoryParams.Sku);
        }

        [Fact]
        public async Task Execute_StoredProcedure_Insert_WithParams_Success()
        {
            var inventoryParams = InventoryParameters();
            Exception? capturedException = null;

            Action act = () => {
                try
                {
                    _microQuery.Execute("sp_InsertInventory", inventoryParams, CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    capturedException = ex;
                }
            };

            act?.Should().NotThrow();
            capturedException.Should().BeNull("because no errors should occur when executing the query with valid parameters");

            var retrievedData = _microQuery
                .Query($"SELECT * FROM [dbo].[Inventory] WHERE Sku = '{inventoryParams.Sku}'")
                .Rows[0];

            retrievedData.Should().NotBeNull();
            retrievedData["Sku"].Should().Be(inventoryParams.Sku);
        }

        [Fact]
        public async Task ExecuteAsync_StoredProcedure_Insert_WithParams_Success()
        {
            var inventoryParams = InventoryParameters();
            Exception? capturedException = null;

            Func<Task> actAsync = async () => {
                try
                {
                    await _microQuery.ExecuteAsync("sp_InsertInventory", inventoryParams, CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    capturedException = ex;
                }
            };

            actAsync?.Should().NotThrowAsync();
            capturedException.Should().BeNull("because no errors should occur when executing the query with valid parameters");

            var retrievedData = _microQuery
                .Query($"SELECT * FROM [dbo].[Inventory] WHERE Sku = '{inventoryParams.Sku}'")
                .Rows[0];

            retrievedData.Should().NotBeNull();
            retrievedData["Sku"].Should().Be(inventoryParams.Sku);
        }

        private static string InsertInventoryQuery() =>
            """
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
            )
            """;

    }
}