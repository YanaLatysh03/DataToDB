using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MongoDB.Driver;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using DataToDb.Models;
using Nethereum.RPC.Eth.DTOs;
using DataToDb.Repository;
using DataToDb.Models.DTO;
using System.Text.Json;
using AutoMapper;

namespace DataToDb.Service
{
    public class DataToDbService : IDataToDbService
    {
        private ApplicationMongoContext MongoContext = new ApplicationMongoContext();
        private ApplicationPostgreContext PostgreContext = new ApplicationPostgreContext();

        public DataToDbService()
        {
        }

        public async Task GetBlockData(KindDb database)
        {
            try
            {
                IDataToDbRepository repository = new DataToDbRepository(PostgreContext);
                int i = 1;
                (IMongoCollection<MongoModel> blocks, IMongoCollection<ModelForLastRecordTable> lastRecord) collections = (null, null);

                if (database == KindDb.Mongo)
                {
                    collections = MongoContext.CreateConfiguration();

                    var lastDoc = repository.GetRecordFromLastBlockTableMongo(collections.lastRecord);

                    if (lastDoc != null )
                    {
                        i = lastDoc.Value + 1;
                    }
                }
                else
                {
                    var lastRecord = await repository.GetRecordFromLastBlockTable();

                    if (lastRecord != null)
                    {
                        i = lastRecord.Value + 1;
                    }
                }

                while (true)
                {

                    var rpcClient = new RpcClient(new Uri("https://rpc.ankr.com/eth"));
                    var web3 = new Web3(rpcClient);
                    var data = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(i));

                    if (data == null)
                    {
                        break;
                    }

                    var block = AddBlockHeaderDataToModel(data);
                    var arrayTransData = AddTransactionDataToModel(data);
                    var transaction = AddTransactionToModel(arrayTransData);

                    var oneData = new Models.Block { 
                        Header = block, 
                        Transactions = transaction, 
                        Id = string.Concat(data.Number.Value, "-", data.Number.HexValue) };

                    if (database == KindDb.Postgre)
                    {
                        var dtoModel = GetPostgreDtoFromBlock(oneData);


                        var lastRecordModel = CreateLastRecordModel(oneData.Header.number, dtoModel.id);

                        var blockData = await repository.AddDataToPostgreDb(dtoModel, lastRecordModel);
                    }
                    else
                    {
                        var mapperConfig = MappingConfig.RegisterMaps();
                        var mapper = new Mapper(mapperConfig);
                        var dtoModel = mapper.Map<MongoModel>(oneData);

                        var lastRecordModel = CreateLastRecordModel(oneData.Header.number, dtoModel.id);

                        var blockData = await repository.AddDataToMongoDb(collections.blocks, collections.lastRecord, dtoModel, lastRecordModel);
                    }

                    i++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public BlockHeader AddBlockHeaderDataToModel(BlockWithTransactions data)
        {
            var block = new BlockHeader()
            {
                number = (int)data.Number.Value,
                timestamp = (uint)data.Timestamp.Value,
                hash = data.BlockHash,
                parentHash = data.ParentHash,
                receiptsRoot = data.ReceiptsRoot,
                transactionsRoot = data.TransactionsRoot,
                miner = data.Miner,
                gasUsed = (uint)data.GasUsed.Value,
                gasLimit = (uint)data.GasLimit.Value,
                difficulty = data.Difficulty.Value.ToString(),
                extraData = data.ExtraData,
                nonce = data.Nonce,
                sha3Uncles = data.Sha3Uncles,
                mixHash = data.MixHash,
                stateRoot = data.StateRoot,
                totalDifficulty = data.TotalDifficulty.Value.ToString(),
                size = (uint)data.Size.Value,
            };

            return block;
        }

        public TransactionData[] AddTransactionDataToModel(BlockWithTransactions data)
        {
            var listTransData = new List<TransactionData>();
            foreach (var item in data.Transactions)
            {
                var transactionData = new TransactionData()
                {
                    from = item.From,
                    to = item.To,
                    hash = item.BlockHash,
                    value = item.Value.ToString(),
                    gas = item.Gas.ToString(),
                    gasPrice = item.GasPrice.ToString(),
                    input = item.Input,
                };

                listTransData.Add(transactionData);
            }

            var arrayTransData = listTransData.ToArray();

            return arrayTransData;
        }

        public Models.Transaction AddTransactionToModel(TransactionData[] arrayTransData)
        {
            var transaction = new Models.Transaction()
            {
                data = arrayTransData,
            };

            return transaction;
        }

        public enum KindDb
        {
            Mongo,
            Postgre
        }

        public PostgreModel GetPostgreDtoFromBlock(Models.Block block)
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

        public Models.Block GetBlockModelFromPostgreDto(PostgreModel postgreModel)
        {
            var blockHeader = JsonSerializer.Deserialize<BlockHeader>(postgreModel.header);
            var transactions = JsonSerializer.Deserialize<Models.Transaction>(postgreModel.transactions);

            var blockModel = new Models.Block()
            {
                Id = postgreModel.id,
                Header = blockHeader,
                Transactions = transactions
            };

            return blockModel;
        }

        public ModelForLastRecordTable CreateLastRecordModel(int number, string pair)
        {
            var lastRecordModel = new ModelForLastRecordTable()
            {
                Id = 1,
                Value = number,
                Pair = pair
            };

            return lastRecordModel;
        }
    }
}
