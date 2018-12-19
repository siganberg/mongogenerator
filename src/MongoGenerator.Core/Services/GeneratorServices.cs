using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoGenerator.Core.Data.Entities;

namespace MongoGenerator.Core.Services
{
    public class GeneratorServices : IGeneratorServices
    {
        private readonly IMongoClient _client;
        private readonly ILogger<GeneratorServices> _logger;
        public GeneratorServices(IMongoClient client, ILogger<GeneratorServices> logger)
        {
            _client = client;
            _logger = logger;
        }

        public string GenerateIndexes()
        {
            var db = _client.GetDatabase("ugc");
            var builder = new StringBuilder();
            foreach (var c in db.ListCollectionNamesAsync().Result.ToListAsync().Result)
            {
                var docs = db.GetCollection<BsonDocument>(c).Indexes.ListAsync().Result.ToListAsync().Result;
                var indexes = docs.Select(a => BsonSerializer.Deserialize<IndexEntity>(a));
                AppendAndLog(builder, $"//-- {c} collection");
                foreach (var index in indexes.Where(a => a.Name != "_id_"))
                    AppendAndLog(builder, $"db.{c}.createIndex({GetFields(index)}{GetOptions(index)});");
                AppendAndLog(builder, "");
            }
            return builder.ToString();
        }

        private string GetFields(IndexEntity index)
        {
            return index.Weights != null 
                ? index.Weights.ToString().Replace("1", "\"text\"") 
                : index.Key.ToString();
        }

        private string GetOptions(IndexEntity index)
        {
            var option = new List<string>();
            if (index.Background)
                option.Add("background : \"true\"");
            if (index.Unique)
                option.Add("unique : \"true\"");
            if (option.Count == 0) return "";
            return ", { " + string.Join(", ", option) + " }";
        }

        private void AppendAndLog(StringBuilder builder, string content, bool showLog = true)
        {
            builder.AppendLine(content);
            if (string.IsNullOrEmpty(content) || showLog == false) return;
            _logger.LogInformation(content);
        }
    }
}
