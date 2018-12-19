using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoGenerator.Core.Data.Client;
using MongoGenerator.Core.Services;

namespace MongoGenerator.Core
{
    public static class MongoGeneratorExtensions
    {
        public static IServiceCollection AddMongoGenerator(this IServiceCollection services)
        {
            return services.AddSingleton<IMongoClient, TestDbMongClient>()
                .AddSingleton<IGeneratorServices, GeneratorServices>();
        }
    }
}
