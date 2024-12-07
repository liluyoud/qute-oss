using System;
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
                    natureza_juridica_id = table.Column<int>(type: "integer", nullable: true),
                    qualificacao_id = table.Column<int>(type: "integer", nullable: true),
                    capital_social = table.Column<decimal>(type: "numeric", nullable: true),
                    porte = table.Column<int>(type: "integer", nullable: true),
                    ente_federativo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfb_empresas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rfb_estabelecimentos",
                columns: table => new
                {
                    cnpj = table.Column<int>(type: "integer", nullable: false),
                    ordem = table.Column<short>(type: "smallint", nullable: false),
                    dv = table.Column<byte>(type: "smallint", nullable: false),
                    matriz_filial = table.Column<int>(type: "integer", nullable: true),
                    nome_fantasia = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    situacao_cadastral = table.Column<int>(type: "integer", nullable: true),
                    situacao_cadastral_data = table.Column<DateOnly>(type: "date", nullable: true),
                    motivo_id = table.Column<int>(type: "integer", nullable: true),
                    cidade_exterior = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    pais_id = table.Column<int>(type: "integer", nullable: true),
                    inicio_atividades = table.Column<DateOnly>(type: "date", nullable: true),
                    cnae_id = table.Column<int>(type: "integer", nullable: true),
                    cnaes_secundarios = table.Column<int[]>(type: "integer[]", nullable: true),
                    tipo_logradouro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    logradouro = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    complemento = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    bairro = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    municipio_id = table.Column<int>(type: "integer", nullable: true),
                    ddd1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    telefone1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ddd2 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    telefone2 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ddd_fax = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    telefone_fax = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    situacao_especial = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    situacao_especial_data = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
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

            migrationBuilder.CreateTable(
                name: "rfb_socios",
                columns: table => new
                {
                    cnpj = table.Column<int>(type: "integer", nullable: false),
                    tipo = table.Column<int>(type: "integer", nullable: true),
                    nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    cpf_cnpj = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    qualificacao_id = table.Column<int>(type: "integer", nullable: true),
                    data_entrada = table.Column<DateOnly>(type: "date", nullable: true),
                    pais_id = table.Column<int>(type: "integer", nullable: true),
                    representante_cpf_cnpj = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    representante_nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    representante_qualificacao_id = table.Column<int>(type: "integer", nullable: true),
                    faixa_etaria = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "ix_estabelecimentos_cnpj",
                table: "rfb_estabelecimentos",
                column: "cnpj");

            migrationBuilder.CreateIndex(
                name: "ix_socios_cnpj",
                table: "rfb_socios",
                column: "cnpj");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rfb_cnaes");

            migrationBuilder.DropTable(
                name: "rfb_empresas");

            migrationBuilder.DropTable(
                name: "rfb_estabelecimentos");

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

            migrationBuilder.DropTable(
                name: "rfb_socios");
        }
    }
}
