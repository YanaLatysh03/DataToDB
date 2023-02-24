using System.ComponentModel.DataAnnotations;

namespace EthBlockIndexer.Domain.Storage.Postgre.Model
{
    public class PostgreModel
    {
        [Key]
        public string id { get; set; }

        public string header { get; set; }

        public string transactions { get; set; }
    }
}
