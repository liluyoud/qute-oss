using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qute.Rfb.Api.Migrations
{
    /// <inheritdoc />
    public partial class EmpresasNoFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rfb_empresas_rfb_naturezas_juridicas_natureza_juridica_id",
                table: "rfb_empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_rfb_empresas_rfb_qualificacoes_qualificacao_id",
                table: "rfb_empresas");

            migrationBuilder.DropIndex(
                name: "IX_rfb_empresas_natureza_juridica_id",
                table: "rfb_empresas");

            migrationBuilder.DropIndex(
                name: "IX_rfb_empresas_qualificacao_id",
                table: "rfb_empresas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_rfb_empresas_natureza_juridica_id",
                table: "rfb_empresas",
                column: "natureza_juridica_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfb_empresas_qualificacao_id",
                table: "rfb_empresas",
                column: "qualificacao_id");

            migrationBuilder.AddForeignKey(
                name: "FK_rfb_empresas_rfb_naturezas_juridicas_natureza_juridica_id",
                table: "rfb_empresas",
                column: "natureza_juridica_id",
                principalTable: "rfb_naturezas_juridicas",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_rfb_empresas_rfb_qualificacoes_qualificacao_id",
                table: "rfb_empresas",
                column: "qualificacao_id",
                principalTable: "rfb_qualificacoes",
                principalColumn: "id");
        }
    }
}
