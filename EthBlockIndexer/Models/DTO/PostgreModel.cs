using System.ComponentModel.DataAnnotations;

namespace DataToDb.Models.DTO
{
    public class PostgreModel
    {
        [Key]
        public string id { get; set; }

        public string header { get; set; }

        public string transactions { get; set; }
    }
}
