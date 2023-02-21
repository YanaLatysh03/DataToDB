using AutoMapper;
using DataToDb.Models;
using DataToDb.Models.DTO;

namespace DataToDb
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
