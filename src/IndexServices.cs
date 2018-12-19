using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoGenerator
{
    public class IndexEntity
    {
        public int Version { get; set; }
        public BsonDocument Key { get; set; }
        public string Name { get; set; }
        public string CollectionName { get; set; }
        public bool Background { get; set; }
    }

    public class IndexKeyEntity
    {
        public string Name { get; set; }

    }

    public interface IIndexServices
    {
        void Generate();
    }

    public class TestDbMongClient : MongoClient
    {
        private readonly IConfiguration _config; 
        public TestDbMongClient(IConfiguration config) : base(config.GetConnectionString("MongoDB"))
        {

        }
    }


    public class IndexServices : IIndexServices
    {
        private readonly IMongoClient _client;
        private readonly ILogger<IndexServices> _logger;
        public IndexServices(IMongoClient client, ILogger<IndexServices> logger)
        {
            _client = client;
            _logger = logger;
            RegisterMapping();
        }

        private void RegisterMapping()
        {
            BsonClassMap.RegisterClassMap<IndexEntity>(cm =>
            {
                cm.MapMember(c => c.Version).SetElementName("v");
                cm.MapMember(c => c.Key).SetElementName("key");
                cm.MapMember(c => c.CollectionName).SetElementName("ns");
                cm.MapMember(c => c.Name).SetElementName("name");
                cm.MapMember(c => c.Background).SetElementName("background");
            });
        }

        public void Generate()
        {
            var db = _client.GetDatabase("ugc");
            
            foreach (var c in db.ListCollectionNamesAsync().Result.ToListAsync().Result)
            {
                var docs = db.GetCollection<BsonDocument>(c).Indexes.ListAsync().Result.ToListAsync().Result;
                var indexes = docs.Select(a => BsonSerializer.Deserialize<IndexEntity>(a));

                _logger.LogInformation($"//-- {c} collection");
                foreach(var index in indexes.Where(a => a.Name != "_id_"))
                    _logger.LogInformation($"db.{c}.createIndex({{{index.Key}}}, {{ background:true }});");
            }
        }
    }
}
