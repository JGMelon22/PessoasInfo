using System.ComponentModel.DataAnnotations;

namespace PessoasInfo.ViewModels.Pessoa;

public class PessoaCreateViewModel
{
    [Required(ErrorMessage = "O campo nome deve ser informado!")]
    public string Nome { get; set; } = string.Empty!;
}