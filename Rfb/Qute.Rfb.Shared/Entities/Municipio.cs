using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_municipios")]
public class Municipio
{
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    [Column("nome")]
    [StringLength(255)]
    public required string Nome { get; set; }
}
