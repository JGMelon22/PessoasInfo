namespace PessoasInfo.ViewModels.Detalhe;

public class DetalheEditViewModel
{
    [Key] public int IdDetalhe { get; set; }

    [Display(Name = "Detalhe Texto")]
    [Required(ErrorMessage = "Detalhe Texto é um campo obrigatório!")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Detalhe Texto deve conter entre 3 a 255 caracteres!")]
    public string DetalheTexto { get; set; } = string.Empty!;

    [Display(Name = "Id Chave Pessoa")]
    [Required(ErrorMessage = "É necessário informar a chave extrangeira IdPessoa!")]
    [Range(1, int.MaxValue, ErrorMessage = "Certifique-se que a chave extrangeira IdPessoa esteja correta!")]
    public int IdPessoa { get; set; }
}