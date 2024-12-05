using Qute.Rfb.Api.Helpers;
using Qute.Rfb.Shared.Enums;

namespace Qute.Rfb.Api.Endpoints;

public static class MigrateEndpoints
{
    public static IEndpointRouteBuilder MapRfbMigration(this IEndpointRouteBuilder app)
    {
        var migration = app.MapGroup("migrate").WithTags("Migração");

        migration.MapGet("download", async (IHostEnvironment env, ILogger<Program> logger) =>
        {
            await MigrationHelper.DownloadRfbFiles(env, logger, RfbFileType.Basico);

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
       .WithSummary("Unzip RFB Files")
       .WithDescription("Unzip RFB Files.")
       .Produces(200);

        return app;
    }
}
