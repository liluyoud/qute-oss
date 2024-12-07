using Qute.Rfb.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Qute.Rfb.Shared.Models;

public class EmpresaModel
{
    public int Cnpj { get; set; }

    public required string Nome { get; set; }
    public string? NaturezaJuridica { get; set; }
    public string? Qualificacao { get; set; }
    public decimal? CapitalSocial { get; set; }
    public string? Porte { get; set; }
    public string? Opcao { get; set; }
    public DateOnly? DataOpcao { get; set; }
}

public class EstabelecimentoModel
{
    public int Cnpj { get; set; }
    public short Ordem { get; set; }
    public byte Dv { get; set; }
    public string? MatrizFilial { get; set; }
    public string? NomeFantasia { get; set; }
    public string? SituacaoCadastral { get; set; }
    public DateOnly? SituacaoCadastralData { get; set; }
    public string? Motivo { get; set; }
    public string? CidadeExterior { get; set; }
    public string? Pais { get; set; }
    public DateOnly? InicioAtividades { get; set; }
    public int? CnaeId { get; set; }
    public string? Cnae { get; set; }

    public string? TipoLogradouro { get; set; }
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cep { get; set; }
    public string? Uf { get; set; }
    public string? MunicipioId { get; set; }
    public string? Telefone1 { get; set; }
    public string? Telefone2 { get; set; }
    public string? TelefoneFax { get; set; }
    public string? Email { get; set; }
    public string? SituacaoEspecial { get; set; }
    public DateOnly? SituacaoEspecialData { get; set; }

}