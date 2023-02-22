using System.ComponentModel.DataAnnotations.Schema;

namespace PessoasInfo.Models;

[Table("Detalhes")]
[Index(nameof(IdDetalhe))]
public class Detalhe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdDetalhe")]
    public int IdDetalhe { get; set; }

    [Required]
    [Column("DetalheTexto", TypeName = "VARCHAR(255)")]
    [StringLength(255, MinimumLength = 3)]
    public string DetalheTexto { get; set; } = string.Empty!;

    [ForeignKey("PessoaId")] public int PessoaId { get; set; }

    public Pessoa Pessoas { get; set; }
}