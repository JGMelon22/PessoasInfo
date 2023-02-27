using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using PessoasInfo.ViewModels.ForgotPassword;
using PessoasInfo.ViewModels.Login;
using PessoasInfo.ViewModels.Register;
using PessoasInfo.ViewModels.ResetPassword;

namespace PessoasInfo.Controllers;

public class AccountsController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ISendGridEmailService _sendGridEmailService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        ISendGridEmailService sendGridEmailService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _sendGridEmailService = sendGridEmailService;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> Register(string? returnUrl)
    {
        // Verifica se o role existe ou não
        if (!await _roleManager.RoleExistsAsync("Comum"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Comum"));
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Basico"));
        }

        // Dropdown list
        var listItems = new List<SelectListItem>();
        listItems.Add(new SelectListItem
        {
            Value = "Comum",
            Text = "Comum"
        });

        listItems.Add(new SelectListItem
        {
            Value = "Basico",
            Text = "Basico"
        });

        listItems.Add(new SelectListItem
        {
            Value = "Admin",
            Text = "Admin"
        });

        var registerViewModel = new RegisterViewModel();

        // Atribui os valores (cargos) a dropdown list
        registerViewModel.RoleList = listItems;

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
            // Verifica se exiwste atributo role e qual foi selecionado
            if (registerViewModel.RoleSelected != null && registerViewModel.RoleSelected.Length > 0 &&
                registerViewModel.RoleSelected == "Comum")
                await _userManager.AddToRoleAsync(user, "Comum");

            else if (registerViewModel.RoleSelected != null && registerViewModel.RoleSelected.Length > 0 &&
                     registerViewModel.RoleSelected == "Basico")
                await _userManager.AddToRoleAsync(user, "Basico");

            else
                await _userManager.AddToRoleAsync(user, "Admin");

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

    // Reset Password
    [HttpGet]
    public IActionResult ResetPassword(string code = null)
    {
        return code == null ? View("Error") : View();
    }


    // Confirmar reset de senha
    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Usuário não encontrado!");
                return View();
            }

            var result =
                await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code,
                    resetPasswordViewModel.Password);

            if (result.Succeeded) return RedirectToAction("ResetPasswordConfirmation");
        }

        return View(resetPasswordViewModel);
    }

    // Reset Password Confirmation
    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Lockout()
    {
        return View();
    }
}