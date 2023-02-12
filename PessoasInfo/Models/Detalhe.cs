using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [Required] [Range(1, int.MaxValue)] public int IdPessoa { get; set; }
}