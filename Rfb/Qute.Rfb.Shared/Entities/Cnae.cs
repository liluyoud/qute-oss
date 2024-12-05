using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_cnaes")]
public class Cnae
{
    [Key]
    [Column("id")]
    [StringLength(7)]
    public required string Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public required string Name { get; set; }
}
