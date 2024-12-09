namespace Qute.Rfb.Shared.Contracts;

public class AiResponse
{
    public string? Cnpj { get; set; }
    public required string Nome { get; set; }
    public string? NaturezaJuridica { get; set; }
    public decimal? CapitalSocial { get; set; }
    public string? Porte { get; set; }
    public string? Opcao { get; set; }
    public DateOnly? DataOpcao { get; set; }
    public string? MatrizFilial { get; set; }
    public string? NomeFantasia { get; set; }
    public string? SituacaoCadastral { get; set; }
    public DateOnly? SituacaoCadastralData { get; set; }
    public string? Motivo { get; set; }
    public string? CidadeExterior { get; set; }
    public string? Pais { get; set; }
    public DateOnly? InicioAtividades { get; set; }
    public string? Endereco { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cep { get; set; }
    public string? Municipio { get; set; }
    public string? Telefone1 { get; set; }
    public string? Telefone2 { get; set; }
    public string? TelefoneFax { get; set; }
    public string? Email { get; set; }
    public string? SituacaoEspecial { get; set; }
    public DateOnly? SituacaoEspecialData { get; set; }
    public string? Cnae { get; set; }
    public List<string> CnaesSecundarios { get; set; } = new();
    public List<AISocioModel> Socios { get; set; } = new();
}

public class AISocioModel
{
    public string? Tipo { get; set; }
    public string? Nome { get; set; }
    public string? CpfCnpj { get; set; }
    public string? Qualificacao { get; set; }
    public DateOnly? DataEntrada { get; set; }
    public string? Pais { get; set; }
    public string? RepresentanteCpfCnpj { get; set; }
    public string? RepresentanteNome { get; set; }
    public string? RepresentanteQualificacao { get; set; }
    public string? FaixaEtaria { get; set; }
}