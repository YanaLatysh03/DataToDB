using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using System;
using Nethereum.JsonRpc.Client;
using System.Threading.Tasks;
using MongoDB.Driver;
using DataToMongoDB.Models;
using System.Collections.Generic;
using MongoDB.Driver.Linq;

namespace DataToMongoDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GetBlockData().Wait();
        }

        public static async Task GetBlockData()
        {
            try {
                string connectionString = "mongodb://localhost:27017";
                string databaseName = "blockData";
                string collectionName = "data";

                var client = new MongoClient(connectionString);
                var db = client.GetDatabase(databaseName);
                var collection = db.GetCollection<Block>(collectionName);

                var filter = collection.Find(e => e._id.Equals("1"));
                var count = filter.CountDocuments();

                int i;

                if (count == 0)
                {
                    i = 1;
                }
                else
                {
                    var lastDoc = collection.AsQueryable().OrderByDescending(c => c.header.number).First();
                    i = lastDoc.header.number + 1;
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
                    var transaction = new Transaction()
                    {
                        data = arrayTransData,
                    };


                    var oneData = new Block { header = block, transactions = transaction, _id =  string.Concat(data.Number.Value, "-", data.Number.HexValue)  };

                    await collection.InsertOneAsync(oneData);

                    var lastDocument = collection.AsQueryable().OrderByDescending(c => c.header.number).First();
                    i = lastDocument.header.number;

                    i++;
                }   
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }
}
