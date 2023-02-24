using AutoMapper;
using EthBlockIndexer.Domain.Storage.Mongo.Model;
using DataToDb.Repository;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using EthBlockIndexer.Models;

namespace EthBlockIndexer.Domain.Storage.Mongo
{
    public class MongoBlockIndexerRepository : IBlockIndexerRepository
    {
        (IMongoCollection<MongoModel> blocks, IMongoCollection<State> lastRecord) collections
            = MongoConfiguration.CreateConfiguration();

        public async Task<Block> AddDataToDb(Block blockData, State lastRecordModel)
        {
            var mapperConfig = MappingConfig.RegisterMaps();
            var mapper = new Mapper(mapperConfig);
            var dtoModel = mapper.Map<MongoModel>(blockData);

            await collections.blocks.InsertOneAsync(dtoModel);

            var lastRecord = GetRecordFromLastBlockTable();

            if (lastRecord != null)
            {
                await collections.lastRecord.ReplaceOneAsync(item => item.Pair == lastRecord.Pair, lastRecordModel);
            }
            else
            {
                await collections.lastRecord.InsertOneAsync(lastRecordModel);
            }

            return blockData;
        }

        public State GetRecordFromLastBlockTable()
        {
            var lastDoc = collections.lastRecord.AsQueryable().OrderBy(c => c.Id).FirstOrDefault();

            return lastDoc;
        }
    }
}
