using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PessoasInfo.Models;

[Table("Telefones")]
[Index(nameof(IdTelefone))]
public class Telefone
{
    [Key] public int IdTelefone { get; set; }
    [Required] [MaxLength(13)] public string TelefoneTexto { get; set; } = string.Empty!;

    [Required] [Range(1, int.MaxValue)] public int IdPessoa { get; set; }

    public bool Ativo { get; set; }
}