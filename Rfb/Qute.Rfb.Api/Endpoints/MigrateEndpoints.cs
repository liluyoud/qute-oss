using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Api.Helpers;
using Qute.Rfb.Shared.Enums;

namespace Qute.Rfb.Api.Endpoints;

public static class MigrateEndpoints
{
    public static IEndpointRouteBuilder MapRfbMigration(this IEndpointRouteBuilder app)
    {
        var migration = app.MapGroup("migrate").WithTags("Migração");

        migration.MapGet("download", (IHostEnvironment env, ILogger<Program> logger) =>
        {
            Task task = MigrationHelper.DownloadRfbFiles(env, logger, RfbFileType.Todos);
            return Results.Ok($"Files downloaded");
        })
        .WithName("download")
        .WithSummary("Download All RFB Files")
        .WithDescription("Download All RFB Files (Basico, Empresas, Estabelecimentos, Socios).")
        .Produces(200);

        migration.MapGet("download/basico", (IHostEnvironment env, ILogger<Program> logger) =>
        {
            Task task = MigrationHelper.DownloadRfbFiles(env, logger, RfbFileType.Basico);
            return Results.Ok($"Files downloaded");
        })
        .WithName("downloadBasico")
        .WithSummary("Download Basico Files")
        .WithDescription("Download Basico RFB Files")
        .Produces(200);

        migration.MapGet("download/empresas", (IHostEnvironment env, ILogger<Program> logger) =>
        {
            Task task = MigrationHelper.DownloadRfbFiles(env, logger, RfbFileType.Empresas);
            return Results.Ok($"Files downloaded");
        })
        .WithName("downloadEmpresas")
        .WithSummary("Download Empresas Files")
        .WithDescription("Download Empresas RFB Files")
        .Produces(200);

        migration.MapGet("download/estabelecimentos", (IHostEnvironment env, ILogger<Program> logger) =>
        {
            Task task = MigrationHelper.DownloadRfbFiles(env, logger, RfbFileType.Estabelecimentos);
            return Results.Ok($"Files downloaded");
        })
        .WithName("downloadEstabelecimentos")
        .WithSummary("Download Estabelecimentos Files")
        .WithDescription("Download Estabelecimentos RFB Files")
        .Produces(200);

        migration.MapGet("download/socios", (IHostEnvironment env, ILogger<Program> logger) =>
        {
            Task task = MigrationHelper.DownloadRfbFiles(env, logger, RfbFileType.Socios);
            return Results.Ok($"Files downloaded");
        })
        .WithName("downloadSocios")
        .WithSummary("Download Socios Files")
        .WithDescription("Download Socios RFB Files")
        .Produces(200);

        migration.MapGet("extract", (IHostEnvironment env, ILogger<Program> logger) =>
        {
            MigrationHelper.ExtractRfbFiles(env, logger);
            return Results.Ok($"Files extracted");
        })
        .WithName("extract")
        .WithSummary("Extract RFB Files")
        .WithDescription("Extract RFB Files.")
        .Produces(200);

        migration.MapGet("move", (IHostEnvironment env, ILogger<Program> logger) =>
        {
            MigrationHelper.MoveFilesToRootFolder(env, logger);
            return Results.Ok($"Files moved");
        })
        .WithName("move")
        .WithSummary("Move RFB Files")
        .WithDescription("Move and join RFB Files.")
        .Produces(200);

        migration.MapGet("cnaes", async (RfbContext context, IHostEnvironment env, ILogger<Program> logger) =>
        {
            await context.MigrateCnaes(env, logger);
            return Results.Ok($"Cnaes migrated");
        })
        .WithName("cnaes")
        .WithSummary("Migrate Cnaes")
        .WithDescription("Migrate Cnae File to Database.")
        .Produces(200);

        migration.MapGet("motivos", async (RfbContext context, IHostEnvironment env, ILogger<Program> logger) =>
        {
            await context.MigrateMotivos(env, logger);
            return Results.Ok($"Motivos migrated");
        })
        .WithName("motivos")
        .WithSummary("Migrate Motivos")
        .WithDescription("Migrate Motivos File to Database.")
        .Produces(200);

        migration.MapGet("municipios", async (RfbContext context, IHostEnvironment env, ILogger<Program> logger) =>
        {
            await context.MigrateMunicipios(env, logger);
            return Results.Ok($"Municípios migrated");
        })
        .WithName("municipios")
        .WithSummary("Migrate Municípios")
        .WithDescription("Migrate Municipios File to Database.")
        .Produces(200);

        migration.MapGet("naturezasjuridicas", async (RfbContext context, IHostEnvironment env, ILogger<Program> logger) =>
        {
            await context.MigrateNaturezasJuridicas(env, logger);
            return Results.Ok($"Municípios migrated");
        })
        .WithName("naturezasJuridicas")
        .WithSummary("Migrate Naturezas Juridicas")
        .WithDescription("Migrate Naturezas Juridicas File to Database.")
        .Produces(200);

        migration.MapGet("paises", async (RfbContext context, IHostEnvironment env, ILogger<Program> logger) =>
        {
            await context.MigratePaises(env, logger);
            return Results.Ok($"Países migrated");
        })
        .WithName("paises")
        .WithSummary("Migrate Países")
        .WithDescription("Migrate Países File to Database.")
        .Produces(200);

        migration.MapGet("qualificacoes", async (RfbContext context, IHostEnvironment env, ILogger<Program> logger) =>
        {
            await context.MigrateQualificacoes(env, logger);
            return Results.Ok($"Qualificação dos Sócios migrated");
        })
        .WithName("qualificcoes")
        .WithSummary("Migrate Qualificações")
        .WithDescription("Migrate Qualificação dos Sócios File to Database.")
        .Produces(200);

        migration.MapGet("empresas", async (RfbContext context, IHostEnvironment env, ILogger<Program> logger) =>
        {
            await ContextHelper.MigrateEmpresas(context, env, logger);
            return Results.Ok($"Empresas migrated");
        })
        .WithName("empresas")
        .WithSummary("Migrate Empresas")
        .WithDescription("Migrate Empresas File to Database.")
        .Produces(200);

        return app;
    }
}
