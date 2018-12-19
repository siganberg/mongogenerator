using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoGenerator.Core
{
    public class IndexServices : IIndexServices
    {
        private readonly IMongoClient _client;
        private readonly ILogger<IndexServices> _logger;
        public IndexServices(IMongoClient client, ILogger<IndexServices> logger)
        {
            _client = client;
            _logger = logger;
        }

        public string Generate()
        {
            var db = _client.GetDatabase("ugc");
            var builder = new StringBuilder();
            foreach (var c in db.ListCollectionNamesAsync().Result.ToListAsync().Result)
            {
                var docs = db.GetCollection<BsonDocument>(c).Indexes.ListAsync().Result.ToListAsync().Result;
                var indexes = docs.Select(a => BsonSerializer.Deserialize<IndexEntity>(a));
                AppendAndLog(builder, $"//-- {c} collection");
                foreach (var index in indexes.Where(a => a.Name != "_id_"))
                {
                    if (index.Weights != null)
                    {
                        AppendAndLog(builder, $"db.{c}.createIndex({index.Weights.ToString().Replace("1","\"text\"")}, {{ background:true }});");

                    }
                    else
                    {
                        AppendAndLog(builder, $"db.{c}.createIndex({index.Key}, {{ background:true }});");

                    }

                }
                AppendAndLog(builder, "");
            }
            return builder.ToString();
        }

        private void AppendAndLog(StringBuilder builder, string content)
        {
            builder.AppendLine(content);
            if (string.IsNullOrEmpty(content)) return;
            _logger.LogInformation(content);
        }
    }
}
