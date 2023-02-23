using AutoMapper;
using DataToDb.Models;
using DataToDb.Models.DTO;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace DataToDb.Repository
{
    public class MongoBlockIndexerRepository : IBlockIndexerRepository
    {
        private IMongoCollection<MongoModel> _blockCollection;
        private IMongoCollection<ModelForLastRecordTable> _lastRecordCollection;

        public MongoBlockIndexerRepository(IMongoCollection<MongoModel> blockCollection, IMongoCollection<ModelForLastRecordTable> lastRecordCollection)
        {
            _blockCollection = blockCollection;
            _lastRecordCollection = lastRecordCollection;
        }

        public async Task<Block> AddDataToDb(Block blockData, ModelForLastRecordTable lastRecordModel)
        {

            var mapperConfig = MappingConfig.RegisterMaps();
            var mapper = new Mapper(mapperConfig);
            var dtoModel = mapper.Map<MongoModel>(blockData);

            await _blockCollection.InsertOneAsync(dtoModel);

            var lastRecord = GetRecordFromLastBlockTable();

            if (lastRecord != null)
            {
                await _lastRecordCollection.ReplaceOneAsync(item => item.Pair == lastRecord.Pair, lastRecordModel);
            }
            else
            {
                await _lastRecordCollection.InsertOneAsync(lastRecordModel);
            }

            return blockData;
        }

        public ModelForLastRecordTable GetRecordFromLastBlockTable()
        {
            var lastDoc = _lastRecordCollection.AsQueryable().OrderBy(c => c.Id).FirstOrDefault();

            return lastDoc;
        }
    }
}
