using Microsoft.Extensions.Caching.Distributed;
using Qute.Shared.Extensions;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Shared.Models;

namespace Qute.Rfb.Api.Endpoints;

public static class RfbEndpoints
{
    public static IEndpointRouteBuilder MapRfb(this IEndpointRouteBuilder app)
    {
        var tables = app.MapGroup("rfb").WithTags("Rfb");

        tables.MapGet("empresa/{id}", async (int id, RfbContext context, IDistributedCache cache) =>
        {
            var lista = await cache.GetOrCreateAsync($"qute-cnpj-{id}", async () =>
            {
                var empresa =await context.Empresas.FindAsync(id);
                if (empresa is not null)
                {
                    var qualificacao = await context.Qualificacoes.FindAsync(empresa.QualificacaoId);
                    var naturezaJuridica = await context.NaturezasJuridicas.FindAsync(empresa.NaturezaJuridicaId);
                    var opcao = await context.MeisSimples.FindAsync(empresa.Id);
                    var strOpcao = "Normal";
                    DateOnly? dataOpcao = null;
                    if (opcao?.MeiOpcao == true)
                    {
                        strOpcao = "MEI";
                        dataOpcao = opcao.MeiInclusao;

                    }
                    if (opcao?.SimplesOpcao == true)
                    {
                        strOpcao = "Simples";
                        dataOpcao = opcao.SimplesInclusao;
                    }
                    var empresaModel = new EmpresaModel
                    {
                        Cnpj = empresa.Id,
                        Nome = empresa.Nome,
                        NaturezaJuridica = naturezaJuridica?.Descricao,
                        Qualificacao = qualificacao?.Descricao,
                        CapitalSocial = empresa.CapitalSocial,
                        Porte = empresa.Porte?.GetDescription(),
                        EnteFederativo = empresa.EnteFederativo,
                        Opcao = strOpcao,
                        DataOpcao = dataOpcao
                    };
                    return empresaModel;
                }
                return null;
            }, TimeSpan.FromSeconds(5));
            return lista;
        })
        .WithName("getCnpj")
        .WithSummary("Empresa por Cnpj")
        .WithDescription("Retorna uma Empresa")
        .Produces<EmpresaModel?>(200);

        return app;
    }
}
