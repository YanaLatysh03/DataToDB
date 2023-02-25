using EthBlockIndexer.Domain;

namespace EthBlockIndexer.Storage.Mongo.Model
{
    public class MongoModel
    {
        public string id { get; set; }

        public BlockHeader header { get; set; }

        public Transaction transactions { get; set; }
    }
}
