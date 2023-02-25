using System.ComponentModel.DataAnnotations;

namespace EthBlockIndexer.Domain
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        public int Value { get; set; }

        public string Pair { get; set; }
    }
}
