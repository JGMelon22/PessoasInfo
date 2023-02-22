namespace PessoasInfo.ViewModels.Register;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty!;

    [Required]
    [Display(Name = "Nome de Usuário")]
    public string UserName { get; set; } = string.Empty!;

    [Required]
    [StringLength(100, ErrorMessage = "O campo senha deve conter no mínimo {1} caracteres!", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Password { get; set; } = string.Empty!;

    [DataType(DataType.Password)]
    [Display(Name = "Confirmação de senha")]
    [Compare("Password", ErrorMessage = "As senhas não são iguais!")]
    public string ConfirmPassword { get; set; } = string.Empty!;

    public string? ReturnUrl { get; set; }
}