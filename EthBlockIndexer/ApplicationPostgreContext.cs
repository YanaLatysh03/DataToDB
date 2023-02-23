using DataToDb.Models;
using DataToDb.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataToDb
{
    public class ApplicationPostgreContext : DbContext
    {
        public DbSet<PostgreModel> Blocks { get; set; }

        public DbSet<ModelForLastRecordTable> LastBlock { get; set; }

        public ApplicationPostgreContext()
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
