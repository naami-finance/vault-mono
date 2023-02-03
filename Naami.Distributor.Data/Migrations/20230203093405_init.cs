using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "ShareTypes",
                columns: table => new
                {
                    ObjectType = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    TotalSupply = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RegistryObjectId = table.Column<string>(type: "text", nullable: false),
                    MetadataObjectId = table.Column<string>(type: "text", nullable: false),
                    TxDigest = table.Column<string>(type: "text", nullable: true),
                    EventSeq = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
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
                name: "ShareTypes");
        }
    }
}
