using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoGenerator.Core
{
    public class TestDbMongClient : MongoClient
    {
        public TestDbMongClient(IConfiguration config) : base(config.GetConnectionString("MongoDB"))
        {
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
                cm.MapMember(c => c.TextIndexVersion).SetElementName("textIndexVersion");
                cm.MapMember(c => c.Weights).SetElementName("weights");
                cm.MapMember(c => c.Unique).SetElementName("unique");


            });
        }
    }
}