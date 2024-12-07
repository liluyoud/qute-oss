using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_paises")]
public class Pais
{
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    [Column("nome")]
    [StringLength(255)]
    public required string Nome { get; set; }
}
