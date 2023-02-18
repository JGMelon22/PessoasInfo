namespace PessoasInfo.ViewModels.Pessoa;

public class PessoaTelefoneDetalheDetailViewModel
{
    [Key] public int IdPessoa { get; set; }
    public string Nome { get; set; } = string.Empty!;
    public int IdDetalhe { get; set; }
    public string? DetalheTexto { get; set; }
    public int IdTelefone { get; set; }
    public string TelefoneTexto { get; set; } = string.Empty!;
    public bool Ativo { get; set; }
}