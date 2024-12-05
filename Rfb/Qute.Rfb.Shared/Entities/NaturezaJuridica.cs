using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_naturezas_juridicas")]
public class NaturezaJuridica
{
    [Key]
    [Column("id")]
    [StringLength(4)]
    public required string Id { get; set; }

    [Column("descricao")]
    [StringLength(255)]
    public required string Descricao { get; set; }
}
