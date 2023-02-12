using System.ComponentModel.DataAnnotations;

namespace PessoasInfo.ViewModels.Detalhe;

public class DetalheDetailViewModel
{
    [Key] public int IdDetalhe { get; set; }

    public string DetalheTexto { get; set; } = string.Empty!;
    
    public int IdPessoa { get; set; }
}