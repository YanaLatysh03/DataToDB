using DataToDb.Repository;
using EthBlockIndexer.Domain.Storage.Mongo;
using EthBlockIndexer.Domain.Storage.Postgre;

namespace UsingDataToDbService
{
    public class StorageFactory
    {
        public static IBlockIndexerRepository CreateStorage(KindDb kindDb)
        {
            switch (kindDb)
            {
                case KindDb.Mongo:
                    IBlockIndexerRepository mongoRepository = new MongoBlockIndexerRepository();
                    return mongoRepository;
                case KindDb.Postgre:
                    IBlockIndexerRepository postgreRepository = new PostgreBlockIndexerRepository();
                    return postgreRepository;
                default:
                    return null;
            }
        }

        public enum KindDb
        {
            Mongo,
            Postgre
        }
    }
}
