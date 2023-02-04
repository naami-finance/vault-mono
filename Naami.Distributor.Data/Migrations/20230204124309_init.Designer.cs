﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Naami.Distributor.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Naami.Distributor.Data.Migrations
{
    [DbContext(typeof(VaultContext))]
    [Migration("20230204124309_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Naami.Distributor.Data.ShareType", b =>
                {
                    b.Property<string>("ObjectType")
                        .HasColumnType("text");

                    b.Property<decimal?>("EventSeq")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("MetadataObjectId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal?>("NextEventSeq")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("NextTxDigest")
                        .HasColumnType("text");

                    b.Property<string>("RegistryObjectId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TotalSupply")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("TxDigest")
                        .HasColumnType("text");

                    b.HasKey("ObjectType");

                    b.ToTable("ShareTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
