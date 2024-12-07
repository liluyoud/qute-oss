using Qute.Rfb.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qute.Rfb.Shared.Entities;

[Table("rfb_estabelecimentos")]
public class Estabelecimento
{
    [Column("cnpj")]
    public required int Cnpj { get; set; }

    [Column("ordem")]
    public required short Ordem { get; set; }

    [Column("dv")]
    public required byte Dv { get; set; }

    [Column("matriz_filial")]
    public MatrizFilial? MatrizFilial { get; set; }

    [Column("nome_fantasia")]
    [StringLength(255)]
    public string? NomeFantasia { get; set; }

    [Column("situacao_cadastral")]
    public SituacaoCadastral? SituacaoCadastral { get; set; }

    [Column("situacao_cadastral_data")]
    public DateOnly? SituacaoCadastralData { get; set; }

    [Column("motivo_id")]
    public int? MotivoId { get; set; }

    [Column("cidade_exterior")]
    [StringLength(255)]
    public string? CidadeExterior { get; set; }

    [Column("pais_id")]
    public int? PaisId { get; set; }

    [Column("inicio_atividades")]
    public DateOnly? InicioAtividades { get; set; }

    [Column("cnae_id")]
    public int? CnaeId { get; set; }

    [Column("cnaes_secundarios")]
    public int[]? CnaesSecundarios { get; set; }

    [Column("tipo_logradouro")]
    [StringLength(100)]
    public string? TipoLogradouro { get; set; }

    [Column("logradouro")]
    [StringLength(255)]
    public string? Logradouro { get; set; }

    [Column("numero")]
    [StringLength(50)]
    public string? Numero { get; set; }

    [Column("complemento")]
    [StringLength(255)]
    public string? Complemento { get; set; }

    [Column("bairro")]
    [StringLength(255)]
    public string? Bairro { get; set; }

    [Column("cep")]
    [StringLength(8)]
    public string? Cep { get; set; }

    [Column("uf")]
    [StringLength(2)]
    public string? Uf { get; set; }

    [Column("municipio_id")]
    public int? MunicipioId { get; set; }

    [Column("ddd1")]
    [StringLength(10)]
    public string? Ddd1 { get; set; }

    [Column("telefone1")]
    [StringLength(50)]
    public string? Telefone1 { get; set; }

    [Column("ddd2")]
    [StringLength(10)]
    public string? Ddd2 { get; set; }

    [Column("telefone2")]
    [StringLength(50)]
    public string? Telefone2 { get; set; }

    [Column("ddd_fax")]
    [StringLength(10)]
    public string? DddFax { get; set; }

    [Column("telefone_fax")]
    [StringLength(50)]
    public string? TelefoneFax1 { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("situacao_especial")]
    [StringLength(255)]
    public string? SituacaoEspecial { get; set; }

    [Column("situacao_especial_data")]
    public DateOnly? SituacaoEspecialData { get; set; }


}
