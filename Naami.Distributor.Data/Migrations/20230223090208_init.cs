using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Naami.Distributor.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoinTypes",
                columns: table => new
                {
                    ObjectType = table.Column<string>(type: "text", nullable: false),
                    PackageId = table.Column<string>(type: "text", nullable: false),
                    Module = table.Column<string>(type: "text", nullable: false),
                    Struct = table.Column<string>(type: "text", nullable: false),
                    Decimals = table.Column<byte>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinTypes", x => x.ObjectType);
                });

            migrationBuilder.CreateTable(
                name: "Distributions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ShareType = table.Column<string>(type: "text", nullable: false),
                    CoinType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    InitialAmount = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventSourcingSnapshots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShareTypeTxDigest = table.Column<string>(type: "text", nullable: true),
                    ShareTypeEventSeq = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    DistributionTxDigest = table.Column<string>(type: "text", nullable: true),
                    DistributionEventSeq = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    CoinTypeTxDigest = table.Column<string>(type: "text", nullable: true),
                    CoinTypeEventSeq = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSourcingSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareTypes",
                columns: table => new
                {
                    ObjectType = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    TotalSupply = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RegistryObjectId = table.Column<string>(type: "text", nullable: false),
                    MetadataObjectId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareTypes", x => x.ObjectType);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinTypes");

            migrationBuilder.DropTable(
                name: "Distributions");

            migrationBuilder.DropTable(
                name: "EventSourcingSnapshots");

            migrationBuilder.DropTable(
                name: "ShareTypes");
        }
    }
}
