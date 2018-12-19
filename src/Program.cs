using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Serilog;

namespace MongoGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(a => CreateConfiguration())
                .AddSingleton<IMongoClient, TestDbMongClient>()
                .AddSingleton<IIndexServices, IndexServices>()
                .AddLogging()
                .BuildServiceProvider();

            ConfigureLogger(serviceProvider);

            var indexer = serviceProvider.GetService<IIndexServices>();
            indexer.Generate();

            Console.ReadLine();

        }

        private static IConfigurationRoot CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", false, true);

            var config = builder.Build();
            return config;
        }

        private static void ConfigureLogger(ServiceProvider serviceProvider)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddSerilog();
        }
    }
}
