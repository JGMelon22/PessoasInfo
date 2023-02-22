using System.ComponentModel.DataAnnotations.Schema;

namespace PessoasInfo.Models;

[Table("Pessoas")]
[Index(nameof(PessoaId))]
public class Pessoa
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("PessoaId")]
    public int PessoaId { get; set; }

    [Column("Nome", TypeName = "VARCHAR(100)")]
    [Required]
    public string Nome { get; set; } = string.Empty!;

    public List<Detalhe>? Detalhes { get; set; }
    public List<Telefone>? Telefones { get; set; }
}