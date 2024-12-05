using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Shared.Entities;
using Qute.Rfb.Shared.Enums;
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

    public async static Task MigrateMunicipios(this RfbContext context, IHostEnvironment env, ILogger logger)
    {
        var municipios = await context.Municipios.ToListAsync();
        if (municipios.Count == 0)
        {
            var csvDir = Path.Combine(env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.MUNICCSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Municipio>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Municipio
                            {
                                Id = values[0].Replace("\"", ""),
                                Nome = values[1].Replace("\"", "")
                            };
                            list.Add(item);
                        }
                    }
                }
                await context.Municipios.AddRangeAsync(list);
                await context.SaveChangesAsync();
            }
        }
    }

    public async static Task MigrateNaturezasJuridicas(this RfbContext context, IHostEnvironment env, ILogger logger)
    {
        var naturezasJuridicas = await context.NaturezasJuridicas.ToListAsync();
        if (naturezasJuridicas.Count == 0)
        {
            var csvDir = Path.Combine(env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.NATJUCSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<NaturezaJuridica>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new NaturezaJuridica
                            {
                                Id = values[0].Replace("\"", ""),
                                Descricao = values[1].Replace("\"", "")
                            };
                            list.Add(item);
                        }
                    }
                }
                await context.NaturezasJuridicas.AddRangeAsync(list);
                await context.SaveChangesAsync();
            }
        }
    }

    public async static Task MigratePaises(this RfbContext context, IHostEnvironment env, ILogger logger)
    {
        var paises = await context.Paises.ToListAsync();
        if (paises.Count == 0)
        {
            var csvDir = Path.Combine(env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.PAISCSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Pais>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Pais
                            {
                                Id = values[0].Replace("\"", ""),
                                Nome = values[1].Replace("\"", "")
                            };
                            list.Add(item);
                        }
                    }
                }
                await context.Paises.AddRangeAsync(list);
                await context.SaveChangesAsync();
            }
        }
    }

    public async static Task MigrateQualificacoes(this RfbContext context, IHostEnvironment env, ILogger logger)
    {
        var qualificacoes = await context.Qualificacoes.ToListAsync();
        if (qualificacoes.Count == 0)
        {
            var csvDir = Path.Combine(env.ContentRootPath, "downloads", "csv");
            var file = Directory.GetFiles(csvDir, "*.QUALSCSV").FirstOrDefault();

            if (file != null)
            {
                var list = new List<Qualificacao>();
                using (var reader = new StreamReader(file, Encoding.Latin1))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 2)
                        {
                            var item = new Qualificacao
                            {
                                Id = values[0].Replace("\"", ""),
                                Descricao = values[1].Replace("\"", "")
                            };
                            list.Add(item);
                        }
                    }
                }
                await context.Qualificacoes.AddRangeAsync(list);
                await context.SaveChangesAsync();
            }
        }
    }

    public async static Task MigrateEmpresas(RfbContext context, IHostEnvironment env, ILogger logger)
    {
        var empresas = context.Empresas.Count();
        if (empresas == 0)
        {
            var csvDir = Path.Combine(env.ContentRootPath, "downloads", "csv");
            var files = Directory.GetFiles(csvDir, "*.EMPRECSV");
            var list = new List<Empresa>();
            foreach (var file in files) {
                using (var reader = new StreamReader(file, Encoding.Latin1)) {
                    var i = 0;
                    while (!reader.EndOfStream && ++i < 10)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(';');
                        if (values != null && values.Length == 7)
                        {
                            var item = new Empresa
                            {
                                Id = values[0].GetString(),
                                Nome = values[1].GetString(),
                                NaturezaJuridicaId = values[2].GetStringValue(),
                                QualificacaoId = values[3].GetStringValue(),
                                CapitalSocial = values[4].GetDecimalValue(),
                                Porte = values[5].GetPorteEmpresaValue(),
                                EnteFederativo = values[6].GetStringValue()
                            };
                            list.Add(item);
                        }
                    }
                }
            }

            await context.Empresas.AddRangeAsync(list);
            await context.SaveChangesAsync();
        }
    }
}
