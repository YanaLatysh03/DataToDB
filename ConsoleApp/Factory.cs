using DataToDb.Models.DTO;
using DataToDb.Models;
using DataToDb.Repository;
using DataToDb.Service;
using DataToDb;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace UsingDataToDbService
{
    public class Factory
    {
        public static async Task CreateConfiguration(KindDb kindDb)
        {
            Daemon daemon = null;

            switch (kindDb)
            {
                case KindDb.Mongo:
                    ApplicationMongoContext MongoContext = new ApplicationMongoContext();

                    (IMongoCollection<MongoModel> blocks, IMongoCollection<ModelForLastRecordTable> lastRecord) collections = (null, null);

                    collections = MongoContext.CreateConfiguration();

                    IBlockIndexerRepository mongoRepository = new MongoBlockIndexerRepository(collections.blocks, collections.lastRecord);
                    daemon = new Daemon(mongoRepository);
                    break;
                case KindDb.Postgre:
                    IBlockIndexerRepository postgreRepository = new PostgreBlockIndexerRepository();
                    daemon = new Daemon(postgreRepository);
                    break;
                default:
                    break;
            }

            await daemon.GetBlockData();
        }

        public enum KindDb
        {
            Mongo,
            Postgre
        }
    }
}
