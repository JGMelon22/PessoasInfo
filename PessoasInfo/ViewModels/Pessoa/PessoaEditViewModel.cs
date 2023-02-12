namespace PessoasInfo.ViewModels.Pessoa;

public class PessoaEditViewModel
{
    public int IdPessoa { get; set; }

    [Display(Name = "Nome")]
    [Required(ErrorMessage = "O campo nome deve ser informado!")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo nome deve conter entre 3 a 100 caracteres!")]
    public string Nome { get; set; } = string.Empty!;
}