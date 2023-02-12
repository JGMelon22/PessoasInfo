using System.ComponentModel.DataAnnotations;

namespace PessoasInfo.ViewModels.Detalhe;

public class DetalheDetailViewModel
{
    [Key] public int IdDetalhe { get; set; }

    [StringLength(255, MinimumLength = 3, ErrorMessage = "O campo detalhe texto deve conter entre 3 a 255 caracteres!")]
    [Required(ErrorMessage = "O texto referente ao detalhe é um campo obrigatório")]
    public string DetalheTexto { get; set; }
}