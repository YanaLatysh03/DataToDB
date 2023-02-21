using DataToDb.Models;
using DataToDb.Models.DTO;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DataToDb.Repository
{
    public interface IDataToDbRepository
    {
        Task<PostgreModel> AddDataToPostgreDb(PostgreModel blockData, ModelForLastRecordTable lastOrderModel);

        Task<MongoModel> AddDataToMongoDb(
            IMongoCollection<MongoModel> blockCollection,
            IMongoCollection<ModelForLastRecordTable> lastRecordCollection,
            MongoModel blockData,
            ModelForLastRecordTable lastRecordModel);

        Task<ModelForLastRecordTable> GetRecordFromLastBlockTable();

        ModelForLastRecordTable GetRecordFromLastBlockTableMongo(IMongoCollection<ModelForLastRecordTable> collection);
    }
}
