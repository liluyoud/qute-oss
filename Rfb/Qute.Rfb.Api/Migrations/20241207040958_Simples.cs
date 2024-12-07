using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qute.Rfb.Api.Migrations
{
    /// <inheritdoc />
    public partial class Simples : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rfb_cnaes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_cnaes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rfb_empresas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    natureza_juridica_id = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    qualificacao_id = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    capital_social = table.Column<decimal>(type: "numeric", nullable: true),
                    porte = table.Column<int>(type: "integer", nullable: true),
                    ente_federativo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_empresas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rfb_motivos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_motivos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rfb_municipios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_municipios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rfb_naturezas_juridicas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_naturezas_juridicas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rfb_paises",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_paises", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rfb_qualificacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_qualificacoes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rfb_simples",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    simples_opcao = table.Column<bool>(type: "boolean", nullable: true),
                    simples_inclusao = table.Column<DateOnly>(type: "date", nullable: true),
                    simples_exclusao = table.Column<DateOnly>(type: "date", nullable: true),
                    mei_opcao = table.Column<bool>(type: "boolean", nullable: true),
                    mei_inclusao = table.Column<DateOnly>(type: "date", nullable: true),
                    mei_exclusao = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_simples", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rfb_cnaes");

            migrationBuilder.DropTable(
                name: "rfb_empresas");

            migrationBuilder.DropTable(
                name: "rfb_motivos");

            migrationBuilder.DropTable(
                name: "rfb_municipios");

            migrationBuilder.DropTable(
                name: "rfb_naturezas_juridicas");

            migrationBuilder.DropTable(
                name: "rfb_paises");

            migrationBuilder.DropTable(
                name: "rfb_qualificacoes");

            migrationBuilder.DropTable(
                name: "rfb_simples");
        }
    }
}
