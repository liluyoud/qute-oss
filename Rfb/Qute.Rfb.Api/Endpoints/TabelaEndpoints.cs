using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using Qute.Shared.Extensions;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Shared.Entities;

namespace Qute.Rfb.Api.Endpoints;

public static class TabelaEndpoints
{
    public static IEndpointRouteBuilder MapTables(this IEndpointRouteBuilder app)
    {
        var tables = app.MapGroup("tabelas").WithTags("Tabelas");

        tables.MapGet("cnaes", async (RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-cnaes", async () =>
            {
                return await context.Cnaes.ToListAsync();
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getCnaes")
        .WithSummary("Todos os CNAES")
        .WithDescription("Retorna todos os CNAES")
        .Produces<List<Cnae>?>(200);

        tables.MapGet("cnaes/{id}", async (int id, RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-cnaes-{id}", async () =>
            {
                return await context.Cnaes.FindAsync(id);
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getCnaePorId")
        .WithSummary("CNAE por Id")
        .WithDescription("Retorna um CNAE")
        .Produces<Cnae?>(200);

        tables.MapGet("motivos", async (RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-motivos", async () =>
            {
                return await context.Motivos.ToListAsync();
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getMotivos")
        .WithSummary("Todos os Motivos")
        .WithDescription("Retorna todos os Motivos")
        .Produces<List<Motivo>?>(200);

        tables.MapGet("motivos/{id}", async (int id, RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-motivos-{id}", async () =>
            {
                return await context.Motivos.FindAsync(id);
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getMotivoPorId")
        .WithSummary("Motivo por Id")
        .WithDescription("Retorna um Motivo")
        .Produces<Motivo?>(200);

        tables.MapGet("municipios", async (RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-municipios", async () =>
            {
                return await context.Municipios.ToListAsync();
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getMunicipios")
        .WithSummary("Todos os Municípios")
        .WithDescription("Retorna todos os Municípios")
        .Produces<List<Municipio>?>(200);

        tables.MapGet("municipios/{id}", async (int id, RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-municipios-{id}", async () =>
            {
                return await context.Municipios.FindAsync(id);
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getMunicipioPorId")
        .WithSummary("Município por Id")
        .WithDescription("Retorna um Município")
        .Produces<Municipio?>(200);


        tables.MapGet("naturezas", async (RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-naturezas", async () =>
            {
                return await context.NaturezasJuridicas.ToListAsync();
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getNaturezas")
        .WithSummary("Todos as naturezas")
        .WithDescription("Retorna todas as Naturezas Jurídicas")
        .Produces<List<NaturezaJuridica>?>(200);

        tables.MapGet("naturezas/{id}", async (int id, RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-naturezas-{id}", async () =>
            {
                return await context.NaturezasJuridicas.FindAsync(id);
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getNaturezaPorId")
        .WithSummary("Natureza por Id")
        .WithDescription("Retorna uma Natureza Jurídica")
        .Produces<NaturezaJuridica?>(200);

        tables.MapGet("paises", async (RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-paises", async () =>
            {
                return await context.Paises.ToListAsync();
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getPaises")
        .WithSummary("Todos as países")
        .WithDescription("Retorna todos os países")
        .Produces<List<Pais>?>(200);

        tables.MapGet("paises/{id}", async (int id, RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-paises-{id}", async () =>
            {
                return await context.Paises.FindAsync(id);
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getPaisPorId")
        .WithSummary("País por Id")
        .WithDescription("Retorna um país")
        .Produces<Pais?>(200);

        tables.MapGet("qualificacoes", async (RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-qualifacoes", async () =>
            {
                return await context.Qualificacoes.ToListAsync();
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getQualificacoes")
        .WithSummary("Todos as qualificações")
        .WithDescription("Retorna todas qualificações de sócios")
        .Produces<List<Qualificacao>?>(200);

        tables.MapGet("qualificacoes/{id}", async (int id, RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-qualifacoes-{id}", async () =>
            {
                return await context.Qualificacoes.FindAsync(id);
            }, TimeSpan.FromDays(30));
            return lista;
        })
        .WithName("getQualificacaoPorId")
        .WithSummary("Qualificações por Id")
        .WithDescription("Retorna uma qualificação de sócio")
        .Produces<Qualificacao?>(200);

        return app;
    }
}
