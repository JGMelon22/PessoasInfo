using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PessoasInfo.Models;

[Table("Detalhes")]
[Index(nameof(IdDetalhe))]
public class Detalhe
{
    [Key] public int IdDetalhe { get; set; }

    [Required] public string DetalheTexto { get; set; } = string.Empty!;

    [Required] [Range(1, int.MaxValue)] public int IdPessoa { get; set; }
}