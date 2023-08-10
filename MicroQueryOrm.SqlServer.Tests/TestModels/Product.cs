using MicroQueryOrm.SqlServer;

namespace MicroQueryOrm.SqlServer.Tests.TestModels
{
    public class Product
    {
        public Guid Id { get; set; }

        [Column("ImageUrl")]
        public string ImageUrl { get; set; }

        [Column("Category")]
        public string Category { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Rating")]
        public string Rating { get; set; }

        [Column("ReviewsCount")]
        public string ReviewsCount { get; set; }

        [Column("Price")]
        public string Price { get; set; }

        [Column("ShopName")]
        public string ShopName { get; set; }

        [Ignore]
        public DateTime CreatedAt { get; set; }

        [Column("RemoteId")]
        public long RemoteId { get; set; }
    }
}
