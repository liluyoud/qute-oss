using Qute.Rfb.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_socios")]
public class Socio
{
    [Column("cnpj")]
    public required int Cnpj { get; set; }

    [Column("tipo")]
    public TipoSocio? Tipo { get; set; }

    [Column("nome")]
    [StringLength(255)]
    public string? Nome { get; set; }

    [Column("cpf_cnpj")]
    [StringLength(18)]
    public string? CpfCnpj { get; set; }

    [Column("qualificacao_id")]
    public int? QualificacaoId { get; set; }

    [Column("data_entrada")]
    public DateOnly? DataEntrada { get; set; }

    [Column("pais_id")]
    public int? PaisId { get; set; }

    [Column("representante_cpf_cnpj")]
    [StringLength(18)]
    public string? RepresentanteCpfCnpj { get; set; }

    [Column("representante_nome")]
    [StringLength(255)]
    public string? RepresentanteNome { get; set; }

    [Column("representante_qualificacao_id")]
    public int? RepresentanteQualificacaoId { get; set; }

    [Column("faixa_etaria")]
    public FaixaEtaria? FaixaEtaria { get; set; }

}
