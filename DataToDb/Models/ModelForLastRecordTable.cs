using System.ComponentModel.DataAnnotations;

namespace DataToDb.Models
{
    public class ModelForLastRecordTable
    {
        [Key]
        public int Id { get; set; }

        public int Value { get; set; }

        public string Pair { get; set; }
    }
}
