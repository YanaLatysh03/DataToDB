using DataToDb.Repository;
using EthBlockIndexer.Storage.Mongo;
using EthBlockIndexer.Storage.Postgre;

namespace ConsoleApp
{
    public class StorageFactory
    {
        public static IBlockIndexerRepository CreateStorage(string typeDb)
        {
            switch (typeDb)
            {
                case "Postgre":
                    IBlockIndexerRepository mongoRepository = new MongoBlockIndexerRepository();
                    return mongoRepository;
                case "Mongo":
                    IBlockIndexerRepository postgreRepository = new PostgreBlockIndexerRepository();
                    return postgreRepository;
                default:
                    throw new System.ArgumentException("Database type is not set");
            }
        }
    }
}
