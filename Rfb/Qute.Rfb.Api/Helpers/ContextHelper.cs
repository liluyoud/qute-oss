using Microsoft.EntityFrameworkCore;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Shared.Entities;
using System.Text;

namespace Qute.Rfb.Api.Helpers;

public static class ContextHelper
{
    public async static Task MigrateCnaes(this RfbContext context, IHostEnvironment env, ILogger logger)
    {
        var cnaes = await context.Cnaes.ToListAsync();
        if (cnaes.Count == 0)
        {
            var csvDir = Path.Combine(env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*CNAE*").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Cnae>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Cnae
                            {
                                Id = values[0].Replace("\"", ""),
                                Descricao = values[1].Replace("\"", "")
                            };
                            list.Add(item);
                        }
                    }
                }
                await context.Cnaes.AddRangeAsync(list);
                await context.SaveChangesAsync();
            }

        }
    }

    public async static Task MigrateMotivos(this RfbContext context, IHostEnvironment env, ILogger logger)
    {
        var motivos = await context.Motivos.ToListAsync();
        if (motivos.Count == 0)
        {
            var csvDir = Path.Combine(env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.MOTICSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Motivo>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Motivo
                            {
                                Id = values[0].Replace("\"", ""),
                                Descricao = values[1].Replace("\"", "")
                            };
                            list.Add(item);
                        }
                    }
                }
                await context.Motivos.AddRangeAsync(list);
                await context.SaveChangesAsync();
            }

        }
    }
}
