using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using Qute.Shared.Extensions;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Shared.Entities;
using System.IO;

namespace Qute.Rfb.Api.Endpoints;

public static class MigrateEndpoints
{
    public static IEndpointRouteBuilder MapRfbMigration(this IEndpointRouteBuilder app)
    {
        var migration = app.MapGroup("migrate").WithTags("Migração");

        migration.MapGet("download", async (IHostEnvironment env) =>
        {
            string appDir = env.ContentRootPath;
            //if (env.IsDevelopment())
            //    appDir = Path.Combine();
            string downloadDir = Path.Combine(appDir, "downloads"); // Diretório no container
            string fileUrl = "https://arquivos.receitafederal.gov.br/cnpj/dados_abertos_cnpj/2024-11/Cnaes.zip";
            string fileName = "Cnaes.zip";
            string filePath = Path.Combine(downloadDir, fileName);

            // Cria o diretório
            if (!Directory.Exists(downloadDir))
                Directory.CreateDirectory(downloadDir);

            // Baixar o arquivo
            using var httpClient = new HttpClient();
            var fileBytes = await httpClient.GetByteArrayAsync(fileUrl);

            // Salvar no diretório de download
            await File.WriteAllBytesAsync(filePath, fileBytes);

            return Results.Ok($"Arquivo salvo em: {filePath}");
        })
        .WithName("download")
        .WithSummary("Download RFB Files")
        .WithDescription("Download RFB Files.")
        .Produces(200);

        return app;
    }
}
