using DataToDb.Models;
using DataToDb.Models.DTO;
using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks;
using static DataToDb.Service.DataToDbService;

namespace DataToDb.Service
{
    public interface IDataToDbService
    {
        Task GetBlockData(KindDb database);

        BlockHeader AddBlockHeaderDataToModel(BlockWithTransactions data);

        TransactionData[] AddTransactionDataToModel(BlockWithTransactions data);

        Models.Transaction AddTransactionToModel(TransactionData[] arrayTransData);

        PostgreModel GetPostgreDtoFromBlock(Models.Block block);

        Models.Block GetBlockModelFromPostgreDto(PostgreModel postgreModel);

        ModelForLastRecordTable CreateLastRecordModel(int number, string pair);
    }
}
