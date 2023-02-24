namespace PessoasInfo.ViewModels.ForgotPassword;

public class ForgotPasswordViewModel
{
    [Required] [Display(Name = "Email")] public string Email { get; set; } = string.Empty!;
}