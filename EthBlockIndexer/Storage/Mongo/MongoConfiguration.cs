using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.IO;
using EthBlockIndexer.Storage.Mongo.Model;
using EthBlockIndexer.Domain;

namespace EthBlockIndexer.Storage.Mongo
{
    public class MongoConfiguration
    {
        private static readonly string DatabaseName = "blockData";
        private static readonly string CollectionName = "data";
        private static readonly string LastRecordCollection = "lastRecord";

        public static (IMongoCollection<MongoModel> blocks, IMongoCollection<State> lastRecord) CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: false);

            IConfiguration config = builder.Build();

            var connectionString = config["MongoDb:ConnectionString"];

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(DatabaseName);
            var blockCollection = db.GetCollection<MongoModel>(CollectionName);
            var lastRecorCollection = db.GetCollection<State>(LastRecordCollection);

            var result = (blocks: blockCollection, lastRecord: lastRecorCollection);

            return result;
        }
    }
}
