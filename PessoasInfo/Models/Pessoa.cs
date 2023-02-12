using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PessoasInfo.Models;

[Table("Pessoas")]
[Index(nameof(IdPessoa))]
public class Pessoa
{
    [Key] public int IdPessoa { get; set; }

    [Required] public string Nome { get; set; } = string.Empty!;

    public List<Detalhe>? Detalhes { get; set; }
    public List<Telefone>? Telefones { get; set; }
}