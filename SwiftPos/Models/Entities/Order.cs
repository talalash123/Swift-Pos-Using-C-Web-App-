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

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        // Isko update kiya taaki manually bhi total save ho sakay
        public decimal GrandTotal { get; set; }
    }

    public class OrderItem
    {
        public string ProductId { get; set; } = "";
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; } // Iska naam JS mein 'Quantity' hona chahiye
        public decimal UnitPrice { get; set; }
    }
}