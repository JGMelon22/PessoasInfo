using System.ComponentModel.DataAnnotations.Schema;

namespace PessoasInfo.Models;

[Table("Telefones")]
[Index(nameof(IdTelefone))]
public class Telefone
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("IdTelefone")]
    public int IdTelefone { get; set; }


    [Column("TelefoneTexto", TypeName = "VARCHAR(13)")]
    [Required]
    [MaxLength(13)]
    public string TelefoneTexto { get; set; } = string.Empty!;

    [ForeignKey("PessoaId")] public int PessoaId { get; set; }

    public Pessoa Pessoas { get; set; }

    [Column("Ativo", TypeName = "BIT")] public bool Ativo { get; set; }
}