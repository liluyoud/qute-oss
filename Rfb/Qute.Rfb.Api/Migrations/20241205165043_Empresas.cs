using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qute.Rfb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Empresas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rfb_empresas",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    natureza_juridica_id = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    qualificacao_id = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    capital_social = table.Column<decimal>(type: "numeric", nullable: true),
                    porte = table.Column<int>(type: "integer", nullable: true),
                    ente_federativo = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_empresas", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfb_empresas_rfb_naturezas_juridicas_natureza_juridica_id",
                        column: x => x.natureza_juridica_id,
                        principalTable: "rfb_naturezas_juridicas",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_rfb_empresas_rfb_qualificacoes_qualificacao_id",
                        column: x => x.qualificacao_id,
                        principalTable: "rfb_qualificacoes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_rfb_empresas_natureza_juridica_id",
                table: "rfb_empresas",
                column: "natureza_juridica_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfb_empresas_qualificacao_id",
                table: "rfb_empresas",
                column: "qualificacao_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rfb_empresas");
        }
    }
}
