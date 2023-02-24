using System.ComponentModel.DataAnnotations;

namespace EthBlockIndexer.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        public int Value { get; set; }

        public string Pair { get; set; }
    }
}
