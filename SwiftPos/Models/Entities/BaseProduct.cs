using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SwiftPOS.Models
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(PhysicalProduct), typeof(ServiceItem))]
    public abstract class BaseProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = "";
        public string Category { get; set; } = "";

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => _price = value < 0 ? 0 : value; // Encapsulation: No negative price
        }

        public abstract string ProductType { get; }
    }

    public class PhysicalProduct : BaseProduct
    {
        public int StockQuantity { get; set; }
        public override string ProductType => "Physical";
    }

    public class ServiceItem : BaseProduct
    {
        public override string ProductType => "Service";
    }
}