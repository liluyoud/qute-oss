using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_qualificacoes")]
public class Qualificacao
{
    [Key]
    [Column("id")]
    [StringLength(2)]
    public required string Id { get; set; }

    [Column("descricao")]
    [StringLength(255)]
    public required string Descricao { get; set; }
}
