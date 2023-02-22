using Microsoft.AspNetCore.Identity;
using PessoasInfo.ViewModels.Register;

namespace PessoasInfo.Controllers;

public class AccountsController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
}