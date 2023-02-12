using System.ComponentModel.DataAnnotations;

namespace PessoasInfo.ViewModels.Detalhe;

public class DetalheCreateViewModel
{
    [Required(ErrorMessage = "Detalhe Texto é um campo obrigatório!")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Detalhe Texto deve conter entre 3 a 255 caracteres!")]
    public string DetalheTexto { get; set; } = string.Empty!;

    [Required(ErrorMessage = "É necessário informar a chave extrangeira IdPessoa!")]
    [Range(1, Int32.MaxValue, ErrorMessage = "Certifique-se que a chave extrangeira IdPessoa esteja correta!")]
    public int IdPessoa { get; set; }
}