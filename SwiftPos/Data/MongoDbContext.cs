using MongoDB.Driver;
using SwiftPOS.Models;
using SwiftPOS.Pages.Settings;

namespace SwiftPOS.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;
        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetSection("MongoDbSettings")["ConnectionString"]);
            _db = client.GetDatabase(config.GetSection("MongoDbSettings")["DatabaseName"]);
        }

        public IMongoCollection<BaseProduct> Products => _db.GetCollection<BaseProduct>("Products");
        public IMongoCollection<Order> Orders => _db.GetCollection<Order>("Orders");

        // MongoDbContext.cs ke andar ye line lazmi add karein:

    }
}