using System.ComponentModel.DataAnnotations;

namespace PessoasInfo.ViewModels.Pessoa;

public class PessoaDetailViewModel
{
    [Key] public int IdPessoa { get; set; }

    [Display(Name = "Nome")] public string Nome { get; set; } = string.Empty!;
}