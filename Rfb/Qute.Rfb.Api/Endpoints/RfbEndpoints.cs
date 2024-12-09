using Microsoft.Extensions.Caching.Distributed;
using Qute.Shared.Extensions;
using Qute.Rfb.Api.Contexts;
using Qute.Rfb.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Qute.Rfb.Shared.Contracts;

namespace Qute.Rfb.Api.Endpoints;

public static class RfbEndpoints
{
    public static IEndpointRouteBuilder MapRfb(this IEndpointRouteBuilder app)
    {
        var tables = app.MapGroup("rfb").WithTags("Rfb");

        tables.MapGet("empresa/{cnpj}", async (string cnpj, RfbContext context, IDistributedCache cache) =>
        {
            var id = cnpj.GetCnpjSimples();

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
                        Nome = empresa.Nome,
                        NaturezaJuridica = naturezaJuridica?.Descricao,
                        CapitalSocial = empresa.CapitalSocial,
                        Porte = empresa.Porte?.GetDescription(),
                        Opcao = strOpcao,
                        DataOpcao = dataOpcao
                    };

                    // preenche estabelecimentos
                    var estabelecimentos = await context.Estabelecimentos.Where(e => e.Cnpj == empresa.Id).ToListAsync();
                    foreach (var estabelecimento in estabelecimentos)
                    {
                        string? cnaeStr = string.Empty;
                        if (estabelecimento.CnaeId is not null)
                        {
                            var cnae = await context.Cnaes.FindAsync(estabelecimento.CnaeId);
                            cnaeStr = cnae?.Descricao;
                        }
                        string? municipioStr = string.Empty;
                        if (estabelecimento.MunicipioId is not null)
                        {
                            var municipio = await context.Municipios.FindAsync(estabelecimento.MunicipioId);
                            municipioStr = municipio?.Nome;
                        }
                        string? motivoStr = string.Empty;
                        if (estabelecimento.MotivoId is not null)
                        {
                            var motivo = await context.Motivos.FindAsync(estabelecimento.MotivoId);
                            motivoStr = motivo?.Descricao;
                        }
                        string? paisStr = string.Empty;
                        if (estabelecimento.PaisId is not null)
                        {
                            var pais = await context.Paises.FindAsync(estabelecimento.PaisId);
                            paisStr = pais?.Nome;
                        }
                        string telefone1str = estabelecimento.Telefone1 != null ? $"({estabelecimento.Ddd1}) {estabelecimento.Telefone1}" : "";
                        string telefone2str = estabelecimento.Telefone2 != null ? $"({estabelecimento.Ddd2}) {estabelecimento.Telefone2}" : "";
                        string telefoneFaxStr = estabelecimento.TelefoneFax1 != null ? $"({estabelecimento.DddFax}) {estabelecimento.TelefoneFax1}" : "";
                        string cnpj = estabelecimento.Cnpj.ToString("00000000");
                        cnpj = cnpj.Substring(0, 2) + "." + cnpj.Substring(2, 3) + "." + cnpj.Substring(5, 3);
                        cnpj += "/" + estabelecimento.Ordem.ToString("0000");
                        cnpj += "-" + estabelecimento.Dv.ToString("00");
                        var estabelecimentoModel = new EstabelecimentoModel
                        {
                            Cnpj = cnpj,
                            MatrizFilial = estabelecimento.MatrizFilial?.GetDescription(),
                            NomeFantasia = estabelecimento.NomeFantasia,
                            SituacaoCadastral = estabelecimento.SituacaoCadastral?.GetDescription(),
                            SituacaoCadastralData = estabelecimento.SituacaoCadastralData,
                            Motivo = motivoStr,
                            CidadeExterior = estabelecimento.CidadeExterior,
                            Pais = paisStr,
                            InicioAtividades = estabelecimento.InicioAtividades,
                            Cnae = estabelecimento.CnaeId + " - " + cnaeStr,
                            Endereco = estabelecimento.TipoLogradouro + " " + estabelecimento.Logradouro + ", " + estabelecimento.Numero,
                            Complemento = estabelecimento.Complemento,
                            Bairro = estabelecimento.Bairro,
                            Cep = estabelecimento.Cep,
                            Municipio = municipioStr + " - " + estabelecimento.Uf,
                            Telefone1 = telefone1str,
                            Telefone2 = telefone2str,
                            TelefoneFax = telefoneFaxStr,
                            Email = estabelecimento.Email,
                            SituacaoEspecial = estabelecimento.SituacaoEspecial,
                            SituacaoEspecialData = estabelecimento.SituacaoEspecialData
                        };

                        if (estabelecimento.CnaesSecundarios != null && estabelecimento.CnaesSecundarios.Count() > 0)
                        {
                            foreach (var cnaeSec in estabelecimento.CnaesSecundarios)
                            {
                                var cnae = await context.Cnaes.FindAsync(cnaeSec);
                                if (cnae is not null)
                                {
                                    estabelecimentoModel.CnaesSecundarios.Add(cnae.Id + " - " + cnae.Descricao);
                                }
                            }
                        }
                        empresaModel.Estabelecimentos.Add(estabelecimentoModel);
                    }

                    // preenche socios
                    var socios = await context.Socios.Where(e => e.Cnpj == empresa.Id).ToListAsync();
                    foreach (var socio in socios)
                    {
                        var qualificacaoSocio = await context.Qualificacoes.FindAsync(socio.QualificacaoId);
                        var qualificacaoRepresentante = await context.Qualificacoes.FindAsync(socio.RepresentanteQualificacaoId);
                        var paisSocio = await context.Paises.FindAsync(socio.PaisId);
                        var paisRepresentante = await context.Paises.FindAsync(socio.PaisId);
                        var socioModel = new SocioModel
                        {
                            Tipo = socio.Tipo?.GetDescription(),
                            Nome = socio.Nome,
                            CpfCnpj = socio.CpfCnpj,
                            Qualificacao = qualificacaoSocio?.Descricao,
                            DataEntrada = socio.DataEntrada,
                            Pais = paisSocio?.Nome,
                            RepresentanteCpfCnpj = socio.RepresentanteCpfCnpj,
                            RepresentanteNome = socio.RepresentanteNome,
                            RepresentanteQualificacao = qualificacaoRepresentante?.Descricao,
                            FaixaEtaria = socio.FaixaEtaria?.GetDescription()
                        };
                        empresaModel.Socios.Add(socioModel);
                    }

                    return empresaModel;
                }
                return null;
            }, TimeSpan.FromDays(1));
            return lista;
        })
        .WithName("getCnpj")
        .WithSummary("Empresa por Cnpj")
        .WithDescription("Retorna uma Empresa")
        .Produces<EmpresaModel?>(200);

        tables.MapPost("empresaai", async (AIRequest request, RfbContext context, IDistributedCache cache) =>
        {
            var (id, ordem, dv) = request.Cnpj.GetCnpj();

            var lista = await cache.GetOrCreateAsync($"qute-cnpj-{id}-{ordem}", async () =>
            {
                var empresa = await context.Empresas.FindAsync(id);
                var estabelecimento = await context.Estabelecimentos.FirstOrDefaultAsync(e => e.Cnpj == empresa.Id && e.Ordem == ordem);

                if (empresa is not null && estabelecimento is not null)
                {
                    var strId = id.ToString("00000000");
                    var strCnpj = strId.Substring(0, 2) + "." + strId.Substring(2, 3) + "." + strId.Substring(5, 3) +
                                    "/" + estabelecimento.Ordem.ToString("0000") +
                                    "-" + dv.ToString("00");

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
                    var cnae = await context.Cnaes.FindAsync(estabelecimento.CnaeId);
                    var municipio = await context.Municipios.FindAsync(estabelecimento.MunicipioId);
                    var motivo = await context.Motivos.FindAsync(estabelecimento.MotivoId);
                    var pais = await context.Paises.FindAsync(estabelecimento.PaisId);

                    string telefone1str = estabelecimento.Telefone1 != null ? $"({estabelecimento.Ddd1}) {estabelecimento.Telefone1}" : "";
                    string telefone2str = estabelecimento.Telefone2 != null ? $"({estabelecimento.Ddd2}) {estabelecimento.Telefone2}" : "";
                    string telefoneFaxStr = estabelecimento.TelefoneFax1 != null ? $"({estabelecimento.DddFax}) {estabelecimento.TelefoneFax1}" : "";

                    var aiModel = new AiResponse
                    {
                        Cnpj = strCnpj,
                        Nome = empresa.Nome,
                        NaturezaJuridica = naturezaJuridica?.Descricao,
                        CapitalSocial = empresa.CapitalSocial,
                        Porte = empresa.Porte?.GetDescription(),
                        Opcao = strOpcao,
                        DataOpcao = dataOpcao,
                        MatrizFilial = estabelecimento.MatrizFilial?.GetDescription(),
                        NomeFantasia = estabelecimento.NomeFantasia,
                        SituacaoCadastral = estabelecimento.SituacaoCadastral?.GetDescription(),
                        SituacaoCadastralData = estabelecimento.SituacaoCadastralData,
                        Motivo = motivo?.Descricao,
                        CidadeExterior = estabelecimento.CidadeExterior,
                        Pais = pais?.Nome,
                        InicioAtividades = estabelecimento.InicioAtividades,
                        Cnae = estabelecimento.CnaeId + " - " + cnae?.Descricao,
                        Endereco = estabelecimento.TipoLogradouro + " " + estabelecimento.Logradouro + ", " + estabelecimento.Numero,
                        Complemento = estabelecimento.Complemento,
                        Bairro = estabelecimento.Bairro,
                        Cep = estabelecimento.Cep,
                        Municipio = municipio?.Nome + " - " + estabelecimento.Uf,
                        Telefone1 = telefone1str,
                        Telefone2 = telefone2str,
                        TelefoneFax = telefoneFaxStr,
                        Email = estabelecimento.Email,
                        SituacaoEspecial = estabelecimento.SituacaoEspecial,
                        SituacaoEspecialData = estabelecimento.SituacaoEspecialData
                    };

                    // preenche cnaes secundarios
                    if (estabelecimento.CnaesSecundarios != null && estabelecimento.CnaesSecundarios.Count() > 0)
                    {
                        foreach (var cnaeSec in estabelecimento.CnaesSecundarios)
                        {
                            var cnaeai = await context.Cnaes.FindAsync(cnaeSec);
                            if (cnaeai is not null)
                            {
                                aiModel.CnaesSecundarios.Add(cnaeai.Id + " - " + cnaeai.Descricao);
                            }
                        }
                    }

                    // preenche socios
                    var socios = await context.Socios.Where(e => e.Cnpj == empresa.Id).ToListAsync();
                    foreach (var socio in socios)
                    {
                        var qualificacaoSocio = await context.Qualificacoes.FindAsync(socio.QualificacaoId);
                        var qualificacaoRepresentante = await context.Qualificacoes.FindAsync(socio.RepresentanteQualificacaoId);
                        var paisSocio = await context.Paises.FindAsync(socio.PaisId);
                        var paisRepresentante = await context.Paises.FindAsync(socio.PaisId);
                        var socioModel = new AISocioModel
                        {
                            Tipo = socio.Tipo?.GetDescription(),
                            Nome = socio.Nome,
                            CpfCnpj = socio.CpfCnpj,
                            Qualificacao = qualificacaoSocio?.Descricao,
                            DataEntrada = socio.DataEntrada,
                            Pais = paisSocio?.Nome,
                            RepresentanteCpfCnpj = socio.RepresentanteCpfCnpj,
                            RepresentanteNome = socio.RepresentanteNome,
                            RepresentanteQualificacao = qualificacaoRepresentante?.Descricao,
                            FaixaEtaria = socio.FaixaEtaria?.GetDescription()
                        };
                        aiModel.Socios.Add(socioModel);
                    }

                    return aiModel;
                }
                return null;
            }, TimeSpan.FromDays(1));
            return lista;
        })
        .WithName("getAiEmpresa")
        .WithSummary("Empresa por Cnpj completo")
        .WithDescription("Retorna uma Empresa")
        .Produces<EmpresaModel?>(200);

        return app;
    }
}
