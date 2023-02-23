using DataToDb.Models.DTO;
using DataToDb.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace DataToDb.Repository
{
    public class PostgreBlockIndexerRepository : IBlockIndexerRepository
    {
        private ApplicationPostgreContext _context = new ApplicationPostgreContext();

        public PostgreBlockIndexerRepository()
        {
        }

        public async Task<Block> AddDataToDb(Block blockData, ModelForLastRecordTable lastRecordModel)
        {
            var dtoModel = GetPostgreDtoFromBlock(blockData);

            await _context.Blocks.AddAsync(dtoModel);

            var lastRecord = GetRecordFromLastBlockTable();

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

        public ModelForLastRecordTable GetRecordFromLastBlockTable()
        {
            var lastOrder = _context.LastBlock.OrderBy(c => c.Id).LastOrDefault();

            return lastOrder;
        }

        private PostgreModel GetPostgreDtoFromBlock(Block block)
        {
            var jsonHeader = JsonSerializer.Serialize(block.Header);
            var jsonTransactions = JsonSerializer.Serialize(block.Transactions);

            var blockDto = new PostgreModel()
            {
                id = block.Id,
                header = jsonHeader,
                transactions = jsonTransactions
            };

            return blockDto;
        }
    }
}
