using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using DataToDb.Repository;
using EthBlockIndexer.Models;

namespace EthBlockIndexer
{
    public class Daemon
    {

        private IBlockIndexerRepository _repository;

        public Daemon(IBlockIndexerRepository postgreRepository)
        {
            _repository = postgreRepository;
        }

        public async Task GetBlockData()
        {
            try
            {
                int i = 1;

                var lastRecord = _repository.GetRecordFromLastBlockTable();

                if (lastRecord != null)
                {
                    i = lastRecord.Value + 1;
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

                    var oneData = new Models.Block
                    {
                        Header = block,
                        Transactions = transaction,
                        Id = string.Concat(data.Number.Value, "-", data.Number.HexValue)
                    };

                    var lastRecordModel = CreateLastRecordModel(oneData.Header.number, oneData.Id);

                    var blockData = await _repository.AddDataToDb(oneData, lastRecordModel);

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

        public State CreateLastRecordModel(int number, string pair)
        {
            var lastRecordModel = new State()
            {
                Id = 1,
                Value = number,
                Pair = pair
            };

            return lastRecordModel;
        }
    }
}
