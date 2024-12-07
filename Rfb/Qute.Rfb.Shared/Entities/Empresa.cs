using Qute.Rfb.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_empresas")]
public class Empresa
{
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    [Column("nome")]
    [StringLength(255)]
    public required string Nome { get; set; }

    [Column("natureza_juridica_id")]
    public int? NaturezaJuridicaId { get; set; }

    [Column("qualificacao_id")]
    public int? QualificacaoId { get; set; }

    [Column("capital_social")]
    public decimal? CapitalSocial { get; set; }

    [Column("porte")]
    public PorteEmpresa? Porte { get; set; }

    [Column("ente_federativo")]
    [StringLength(255)]
    public string? EnteFederativo { get; set; }
}
