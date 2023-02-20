using System.ComponentModel.DataAnnotations.Schema;

namespace PessoasInfo.Models;

[Table("Pessoas")]
[Index(nameof(IdPessoa))]
public class Pessoa
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdPessoa")]
    public int IdPessoa { get; set; }

    [Column("Nome", TypeName = "VARCHAR(100)")]
    [Required]
    public string Nome { get; set; } = string.Empty!;

    public List<Detalhe>? Detalhes { get; set; }
    public List<Telefone>? Telefones { get; set; }
}