using EthBlockIndexer;
using System.Threading.Tasks;
using static UsingDataToDbService.StorageFactory;

namespace UsingDataToDbService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            KindDb kindDb = KindDb.Postgre;

            var storage = CreateStorage(kindDb);

            var daemon = new Daemon(storage);
            await daemon.GetBlockData();
        }
    }
}
