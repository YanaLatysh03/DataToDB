using System.ComponentModel.DataAnnotations;

namespace EthBlockIndexer.Domain
{
    public class BlockHeader
    {
        [Key]
        public int number { get; set; }
        public uint timestamp { get; set; }
        public string hash { get; set; }
        public string parentHash { get; set; }
        public string receiptsRoot { get; set; }
        public string transactionsRoot { get; set; }
        public string miner { get; set; }
        public uint gasUsed { get; set; }
        public uint gasLimit { get; set; }
        public string difficulty { get; set; }
        public string extraData { get; set; }
        public string nonce { get; set; }
        public string sha3Uncles { get; set; }
        public string mixHash { get; set; }
        public string stateRoot { get; set; }
        public string totalDifficulty { get; set; }
        public uint size { get; set; }
    }
}
