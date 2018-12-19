using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MongoGenerator.Core
{
    public static class MongoGeneratorExtensions
    {
        public static IServiceCollection AddMongoGenerator(this IServiceCollection services)
        {
            return services.AddSingleton<IMongoClient, TestDbMongClient>()
                .AddSingleton<IIndexServices, IndexServices>();
        }
    }
}
