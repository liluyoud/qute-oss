using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qute.Rfb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rfb_cnaes",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_cnaes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rfb_cnaes");
        }
    }
}
