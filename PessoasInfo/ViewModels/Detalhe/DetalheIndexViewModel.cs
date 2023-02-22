namespace PessoasInfo.ViewModels.Detalhe;

public class DetalheIndexViewModel
{
    [Required] public int IdDetalhe { get; set; }

    [Display(Name = "Detalhe Texto")] public string DetalheTexto { get; set; } = string.Empty!;

    [Display(Name = "Id Chave Pessoa")] public int PessoaId { get; set; }
}