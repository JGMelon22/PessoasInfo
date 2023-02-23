namespace PessoasInfo.ViewModels.Login;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Nome de Usuário")]
    public string UserName { get; set; } = string.Empty!;

    [Required]
    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty!;

    public string? ReturnUrl { get; set; }

    [Display(Name = "Lembrar de mim")] public bool RememberMe { get; set; }
}