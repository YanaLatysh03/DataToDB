

namespace DataToMongoDB.Models
{
    public class Block
    {
        public string number { get; set; }
        public BlockHeader header { get; set; }
        public Transaction transactions { get; set; }
    }
}
