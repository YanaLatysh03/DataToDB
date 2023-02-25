using EthBlockIndexer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using static ConsoleApp.StorageFactory;

namespace ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: false);

            IConfiguration config = builder.Build();

            var typeDb = config["TypeDb"];

            var loggerFactory = LoggerFactory.Create(
            builder => builder
                        .AddConsole()
            );

            var storage = CreateStorage(typeDb);

            var daemon = new EthBlockProcessor(storage, loggerFactory);
            await daemon.RunProcessAddingDatatoDB();
        }
    }
}
