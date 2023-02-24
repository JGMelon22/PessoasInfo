using Microsoft.AspNetCore.Identity;
using PessoasInfo.ViewModels.ForgotPassword;
using PessoasInfo.ViewModels.Login;
using PessoasInfo.ViewModels.Register;

namespace PessoasInfo.Controllers;

public class AccountsController : Controller
{
    private readonly ISendGridEmailService _sendGridEmailService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        ISendGridEmailService sendGridEmailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _sendGridEmailService = sendGridEmailService;
    }

    [HttpGet]
    public async Task<IActionResult> Register(string? returnUrl)
    {
        var registerViewModel = new RegisterViewModel();
        registerViewModel.ReturnUrl = returnUrl;
        return await Task.Run(() => View(registerViewModel));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
    {
        registerViewModel.ReturnUrl = returnUrl;
        returnUrl = returnUrl ?? Url.Content("~/"); // Tartativa em caso o usuário não venha de uma página em específico

        if (!ModelState.IsValid)
            return BadRequest();

        // Registra o usuário
        var user = new AppUser { Email = registerViewModel.Email, UserName = registerViewModel.UserName };
        var result = await _userManager.CreateAsync(user, registerViewModel.Password);

        // Caso ocorra com exito, loga 
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError("Email",
            "Não foi possível criar o usuário. Senha incompatível com padrões de segurança.");

        return await Task.Run(() => View(registerViewModel));
    }

    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        var loginViewModel =
            new LoginViewModel(); // Precisamos instanciar a view model para evitar null exeception na url
        loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
        return await Task.Run(() => View(loginViewModel));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password,
            loginViewModel.RememberMe, true);

        if (result.IsLockedOut)
            return View("Lockout");

        if (result.Succeeded) return await Task.Run(() => RedirectToAction("Index", "Home"));

        ModelState.AddModelError(string.Empty, "Erro ao realizar log in. Usuário ou senha inválidos");
        return View(loginViewModel);
    }

    // Logout 
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return await Task.Run(() => RedirectToAction("Index", "Home"));
    }

    // Password Reset
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user == null)
                return RedirectToAction("ForgotPasswordConfirmation");

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Accounts", new { userId = user.Id, code },
                HttpContext.Request.Scheme);

            await _sendGridEmailService.SendEmailAsync(forgotPasswordViewModel.Email,
                "Email de confirmação para reset de senha",
                "Por favor, redefina sua senha clicando neste " +
                "<a href=\"" + callbackUrl + "\">link</a>");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        return View(forgotPasswordViewModel);
    }

    // Confirmar reset de senha
    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Lockout()
    {
        return View();
    }
}