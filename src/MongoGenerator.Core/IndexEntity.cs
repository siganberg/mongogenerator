using MongoDB.Bson;

namespace MongoGenerator.Core
{
    public class IndexEntity
    {
        public int Version { get; set; }
        public BsonDocument Key { get; set; }
        public string Name { get; set; }
        public string CollectionName { get; set; }
        public bool Background { get; set; }
    }
}