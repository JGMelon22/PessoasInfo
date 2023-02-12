using System.ComponentModel.DataAnnotations;

namespace PessoasInfo.ViewModels.Pessoa;

public class PessoaCreateViewModel
{
    [Required(ErrorMessage = "O campo Nome deve ser informado!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo Nome deve se conter entre 3 a 100 caracteres!")]
    public string Nome { get; set; } = string.Empty!;
}