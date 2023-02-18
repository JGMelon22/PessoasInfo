using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [Required] [Range(1, int.MaxValue)] 
    public int IdPessoa { get; set; }

    [Column("Ativo", TypeName = "BIT")] 
    public bool Ativo { get; set; }
}