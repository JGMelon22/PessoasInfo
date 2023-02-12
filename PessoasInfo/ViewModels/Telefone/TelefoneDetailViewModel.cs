namespace PessoasInfo.ViewModels.Telefone;

public class TelefoneDetailViewModel
{
    [Key] public int IdTelefone { get; set; }

    [Display(Name = "Num. Telefone")] public string TelefoneTexto { get; set; } = string.Empty!;

    [Display(Name = "Id Chave Pessoa")] public int IdPessoa { get; set; }
}