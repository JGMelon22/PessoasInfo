namespace PessoasInfo.ViewModels.Telefone;

public class TelefoneEditViewModel
{
    [Required] public int IdTelefone { get; set; }

    [Display(Name = "Num. Telefone")]
    [Phone]
    [Required(ErrorMessage = "Telefone Texto é uma campo obrigatório!")]
    public string TelefoneTexto { get; set; } = string.Empty!;

    [Display(Name = "Ativo?")] public bool Ativo { get; set; }

    [Display(Name = "Id Chave Pessoa")]
    [Required(ErrorMessage = "É necessário informar a chave extrangeira IdPessoa!")]
    [Range(1, int.MaxValue, ErrorMessage = "Certifique-se que a chave extrangeira IdPessoa esteja correta!")]
    public int IdPessoa { get; set; }
}