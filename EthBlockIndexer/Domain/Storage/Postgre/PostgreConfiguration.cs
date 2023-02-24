using EthBlockIndexer.Domain.Storage.Postgre.Model;
using EthBlockIndexer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EthBlockIndexer.Domain.Storage.Postgre
{
    public class PostgreConfiguration : DbContext
    {
        public DbSet<PostgreModel> Blocks { get; set; }

        public DbSet<State> LastBlock { get; set; }

        public PostgreConfiguration()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", optional: false);

                IConfiguration config = builder.Build();

                var connectionString = config["PostreDb:ConnectionString"];

                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}
