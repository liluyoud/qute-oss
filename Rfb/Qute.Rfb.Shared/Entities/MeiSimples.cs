using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_simples")]
public class MeiSimples
{
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    [Column("simples_opcao")]
    public bool? SimplesOpcao { get; set; }

    [Column("simples_inclusao")]
    public DateOnly? SimplesInclusao { get; set; }

    [Column("simples_exclusao")]
    public DateOnly? SimplesExclusao { get; set; }

    [Column("mei_opcao")]
    public bool? MeiOpcao { get; set; }

    [Column("mei_inclusao")]
    public DateOnly? MeiInclusao { get; set; }

    [Column("mei_exclusao")]
    public DateOnly? MeiExclusao { get; set; }
}
