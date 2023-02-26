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
    [Migration("20230223090208_init")]
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

            modelBuilder.Entity("Naami.Distributor.Data.CoinType", b =>
                {
                    b.Property<string>("ObjectType")
                        .HasColumnType("text");

                    b.Property<byte>("Decimals")
                        .HasColumnType("smallint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IconUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Module")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PackageId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Struct")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ObjectType");

                    b.ToTable("CoinTypes");
                });

            modelBuilder.Entity("Naami.Distributor.Data.Distribution", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CoinType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("CreatedAt")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("InitialAmount")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("RemainingAmount")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("ShareType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Distributions");
                });

            modelBuilder.Entity("Naami.Distributor.Data.EventSourcingSnapshot", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal?>("CoinTypeEventSeq")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("CoinTypeTxDigest")
                        .HasColumnType("text");

                    b.Property<decimal?>("DistributionEventSeq")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("DistributionTxDigest")
                        .HasColumnType("text");

                    b.Property<decimal?>("ShareTypeEventSeq")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("ShareTypeTxDigest")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EventSourcingSnapshots");
                });

            modelBuilder.Entity("Naami.Distributor.Data.ShareType", b =>
                {
                    b.Property<string>("ObjectType")
                        .HasColumnType("text");

                    b.Property<string>("MetadataObjectId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RegistryObjectId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TotalSupply")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("ObjectType");

                    b.ToTable("ShareTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
