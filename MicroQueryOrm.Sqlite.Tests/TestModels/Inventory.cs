
namespace MicroQueryOrm.Sqlite.Tests.TestModels
{
    public class Inventory
    {
        public string WarehouseId { get; set; }
        public string CategoryId { get; set; }
        public string CategoryDesc { get; set; }
        public string Sku { get; set; }
        public string ProductDesc { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime Updated { get; set; }
    }
}
