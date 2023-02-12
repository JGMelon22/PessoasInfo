namespace PessoasInfo.ViewModels.Pessoa;

public class PessoaDetailViewModel
{
    [Display(Name = "Id")] [Key] public int IdPessoa { get; set; }

    [Display(Name = "Nome")] public string Nome { get; set; } = string.Empty!;
}