using EthBlockIndexer.Domain;
using System.Threading.Tasks;

namespace DataToDb.Repository
{
    public interface IBlockIndexerRepository
    {
        Task<Block> AddDataToDb(Block blockData, State lastRecordModel);

        State GetRecordFromLastBlockTable();
    }
}
