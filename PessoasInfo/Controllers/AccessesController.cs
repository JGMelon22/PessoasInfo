namespace PessoasInfo.Controllers;

public class AccessesController : Controller
{
    // Autorização via cookie - Basta estar logado
    [Authorize]
    public async Task<IActionResult> Index()
    {
        return await Task.Run(View);
    }

    // Por política - Verifica se o usuário é admin ou não
    [Authorize(Policy = "OnlyAdminChecker")]
    public IActionResult OnlyAdminChecker()
    {
        return View();
    }

    // Autenticação por claim
    [Authorize(Policy = "TestClaims")]
    public IActionResult CheckClaims()
    {
        return View();
    }

    // Apenas usuário não logado
    [AllowAnonymous]
    public IActionResult AnonymousUser()
    {
        return View();
    }
}