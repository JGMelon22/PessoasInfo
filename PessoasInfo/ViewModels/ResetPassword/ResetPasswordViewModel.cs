namespace PessoasInfo.ViewModels.ResetPassword;

public class ResetPasswordViewModel
{
    [Required] [Display(Name = "Email")] public string Email { get; set; } = string.Empty!;

    [Required] [Display(Name = "Senha")] public string Password { get; set; } = string.Empty!;

    [Display(Name = "Confirme sua Senha")]
    [Compare("Password", ErrorMessage = "As senhas não batem!")]
    public string ConfirmPassword { get; set; } = string.Empty!;

    public string Code { get; set; } = string.Empty!; // 
}