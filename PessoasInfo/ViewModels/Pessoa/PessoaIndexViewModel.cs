namespace PessoasInfo.ViewModels.Pessoa;

public class PessoaIndexViewModel
{
    [Display(Name = "Id")] [Key] public int PessoaId { get; set; }

    [Display(Name = "Nome")] public string Nome { get; set; } = string.Empty!;
}