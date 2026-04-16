using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SwiftPOS.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string CustomerName { get; set; } = "Guest";
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Composition: Order "Has-a" List of OrderItems
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public decimal GrandTotal => Items.Sum(x => x.UnitPrice * x.Quantity);
    }

    public class OrderItem
    {
        public string ProductId { get; set; } = "";
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}