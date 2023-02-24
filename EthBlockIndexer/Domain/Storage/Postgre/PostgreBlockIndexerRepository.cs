using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using DataToDb.Repository;
using EthBlockIndexer.Models;
using EthBlockIndexer.Domain.Storage.Postgre.Model;

namespace EthBlockIndexer.Domain.Storage.Postgre
{
    public class PostgreBlockIndexerRepository : IBlockIndexerRepository
    {
        private PostgreConfiguration _context = new PostgreConfiguration();

        public PostgreBlockIndexerRepository()
        {
        }

        public async Task<Block> AddDataToDb(Block blockData, State lastRecordModel)
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

        public State GetRecordFromLastBlockTable()
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
