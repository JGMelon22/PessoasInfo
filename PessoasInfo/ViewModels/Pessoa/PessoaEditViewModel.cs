using System.ComponentModel.DataAnnotations;

namespace PessoasInfo.ViewModels.Pessoa;

public class PessoaEditViewModel
{
    [Key] public int IdPessoa { get; set; }

    [Required(ErrorMessage = "O campo nome deve ser informado!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo nome deve conter entre 3 a 100 caracteres!")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty!;
}