using System.Threading.Tasks;
using static UsingDataToDbService.Factory;

namespace UsingDataToDbService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            KindDb kindDb = KindDb.Mongo;

            await Factory.CreateConfiguration(kindDb);
        }
    }
}
