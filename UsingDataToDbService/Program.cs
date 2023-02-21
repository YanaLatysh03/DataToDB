using DataToDb.Service;
using System;
using System.Threading.Tasks;
using static DataToDb.Service.DataToDbService;

namespace UsingDataToDbService
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var dataToDb = new DataToDbService();

            await dataToDb.GetBlockData(KindDb.Mongo);

            Console.WriteLine("Hello World!");
        }
    }
}
