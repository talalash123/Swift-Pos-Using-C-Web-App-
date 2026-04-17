// Models/BaseProduct.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SwiftPOS.Models
{
    [BsonIgnoreExtraElements]
    public class BaseProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }

        // Naya field stock track karne ke liye
        public int StockQuantity { get; set; }
    }
}