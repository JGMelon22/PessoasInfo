namespace PessoasInfo.ViewModels.Telefone;

public class TelefoneCreateViewModel
{
    [Display(Name = "Num. Telefone")]
    [Phone]
    [Required(ErrorMessage = "Telefone Texto é uma campo obrigatório!")]

    public string TelefoneTexto { get; set; } = string.Empty!;

    [Display(Name = "Id Chave Pessoa")]
    [Required(ErrorMessage = "É necessário informar a chave extrangeira PessoaId!")]
    [Range(1, int.MaxValue, ErrorMessage = "Certifique-se que a chave extrangeira PessoaId esteja correta!")]
    public int PessoaId { get; set; }

    [Display(Name = "Ativo?")] public bool Ativo { get; set; }
}