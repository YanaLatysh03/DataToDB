using AutoMapper;
using EthBlockIndexer.Domain;
using EthBlockIndexer.Storage.Mongo.Model;

namespace EthBlockIndexer
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Block, MongoModel>();
                config.CreateMap<MongoModel, Block>();
            });

            return mappingConfig;
        }
    }
}
