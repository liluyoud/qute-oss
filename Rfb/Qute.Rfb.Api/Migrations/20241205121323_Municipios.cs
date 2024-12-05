using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qute.Rfb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Municipios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rfb_municipios",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_municipios", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rfb_municipios");
        }
    }
}
