using System.ComponentModel.DataAnnotations;

namespace EthBlockIndexer.Domain
{
    public class TransactionData
    {
        [Key]
        public string from { get; set; }
        public string to { get; set; }
        public string hash { get; set; }
        public string value { get; set; }
        public string gas { get; set; }
        public string gasPrice { get; set; }
        public string input { get; set; }
    }
}
