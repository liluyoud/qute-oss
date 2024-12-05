using Qute.Rfb.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_empresas")]
public class Empresa
{
    [Key]
    [Column("id")]
    [StringLength(8)]
    public required string Id { get; set; }

    [Column("nome")]
    [StringLength(255)]
    public required string Nome { get; set; }

    [Column("natureza_juridica_id")]
    [StringLength(4)]
    [ForeignKey(nameof(NaturezaJuridica))]
    public string? NaturezaJuridicaId { get; set; }
    public NaturezaJuridica? NaturezaJuridica { get; set; }

    [Column("qualificacao_id")]
    [StringLength(2)]
    [ForeignKey(nameof(Qualificacao))]
    public string? QualificacaoId { get; set; }
    public Qualificacao? Qualificacao { get; set; }

    [Column("capital_social")]
    public decimal? CapitalSocial { get; set; }

    [Column("porte")]
    public PorteEmpresa? Porte { get; set; }

    [Column("ente_federativo")]
    [StringLength(8)]
    public string? EnteFederativo { get; set; }
}
