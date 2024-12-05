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
            Task task = MigrationHelper.DownloadRfbFiles(env, logger, RfbFileType.Basico);
            
            return Results.Ok($"Files downloaded");
        })
        .WithName("download")
        .WithSummary("Download RFB Files")
        .WithDescription("Download RFB Files.")
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

        return app;
    }
}
