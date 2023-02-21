using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.IO;
using DataToDb.Models.DTO;
using DataToDb.Models;

namespace DataToDb
{
    public class ApplicationMongoContext
    {
        private readonly string DatabaseName = "blockData";
        private readonly string CollectionName = "data";
        private readonly string LastRecordCollection = "lastRecord";

        public (IMongoCollection<MongoModel> blocks, IMongoCollection<ModelForLastRecordTable> lastRecord) CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: false);

            IConfiguration config = builder.Build();

            var connectionString = config["MongoDb:ConnectionString"];

            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(DatabaseName);
            var blockCollection = db.GetCollection<MongoModel>(CollectionName);
            var lastRecorCollection = db.GetCollection<ModelForLastRecordTable>(LastRecordCollection);

            var result = (blocks: blockCollection, lastRecord: lastRecorCollection);

            return result;
        }
    }
}
