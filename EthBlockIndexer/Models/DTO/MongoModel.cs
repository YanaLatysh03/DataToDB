namespace DataToDb.Models.DTO
{
    public class MongoModel
    {
        public string id { get; set; }

        public BlockHeader header { get; set; }

        public Transaction transactions { get; set; }
    }
}
