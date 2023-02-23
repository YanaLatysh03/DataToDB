using DataToDb.Models;
using System.Threading.Tasks;

namespace DataToDb.Repository
{
    public interface IBlockIndexerRepository
    {
        Task<Block> AddDataToDb(Block blockData, ModelForLastRecordTable lastRecordModel);

        ModelForLastRecordTable GetRecordFromLastBlockTable();
    }
}
