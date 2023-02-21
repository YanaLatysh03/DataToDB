using DataToDb.Models;
using DataToDb.Models.DTO;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace DataToDb.Repository
{
    public class DataToDbRepository : IDataToDbRepository
    {
        private ApplicationPostgreContext _context;

        public DataToDbRepository(ApplicationPostgreContext context)
        {
            _context = context;
        }

        public async Task<PostgreModel> AddDataToPostgreDb(PostgreModel blockData, ModelForLastRecordTable lastRecordModel)
        {
            await _context.Blocks.AddAsync(blockData);

            var lastRecord = await GetRecordFromLastBlockTable();

            if (lastRecord != null)
            {
                lastRecord.Value = lastRecordModel.Value;
                lastRecord.Pair = lastRecordModel.Pair;
            }
            else
            {
                await _context.LastBlock.AddAsync(lastRecordModel);
            }

            await _context.SaveChangesAsync();

            return blockData;
        }

        public async Task<MongoModel> AddDataToMongoDb(
            IMongoCollection<MongoModel> blockCollection, 
            IMongoCollection<ModelForLastRecordTable> lastRecordCollection,
            MongoModel blockData,
            ModelForLastRecordTable lastRecordModel)
        {
            await blockCollection.InsertOneAsync(blockData);
            var lastRecord = GetRecordFromLastBlockTableMongo(lastRecordCollection);

            if (lastRecord != null)
            {
                await lastRecordCollection.ReplaceOneAsync(item => item.Pair == lastRecord.Pair, lastRecordModel);
            }
            else
            {
                await lastRecordCollection.InsertOneAsync(lastRecordModel);
            }

            return blockData;
        }

        public async Task<ModelForLastRecordTable> GetRecordFromLastBlockTable()
        {
            var lastOrder = await _context.LastBlock.OrderBy(c => c.Id).LastOrDefaultAsync();

            return lastOrder;
        }

        public ModelForLastRecordTable GetRecordFromLastBlockTableMongo(IMongoCollection<ModelForLastRecordTable> collection)
        {
            var lastDoc = collection.AsQueryable().OrderBy(c => c.Id).FirstOrDefault();

            return lastDoc;
        }
    }
}
