﻿// <auto-generated />
using DataToDb;
using EthBlockIndexer.Storage.Postgre;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DataToDb.Migrations
{
    [DbContext(typeof(PostgreConfiguration))]
    [Migration("20230220182915_CreateDatabase")]
    partial class CreateDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("DataToDb.Models.DTO.PostgreModel", b =>
                {
                    b.Property<string>("_id")
                        .HasColumnType("text");

                    b.Property<string>("header")
                        .HasColumnType("text");

                    b.Property<string>("transactions")
                        .HasColumnType("text");

                    b.HasKey("_id");

                    b.ToTable("Blocks");
                });
#pragma warning restore 612, 618
        }
    }
}
