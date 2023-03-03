using Microsoft.AspNetCore.Authorization;

namespace PessoasInfo.Controllers;

public class AccessesController : Controller
{
    // Autorização via cookie - Basta estar logado
    [Authorize]
    public async Task<IActionResult> Index()
    {
        return await Task.Run(View);
    }

    // Por cargo
    [Authorize(Roles = "Basico, Comum")]
    public IActionResult BasicAndNormalAccess()
    {
        return View();
    }

    // Por política - Verifica se o usuário é admin ou não
    [Authorize(Policy = "OnlyAdminChecker")]
    public async Task<IActionResult> OnlyAdminChecker()
    {
        return await Task.Run(View);
    }

    // Autenticação por claim
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> CheckClaims()
    {
        return await Task.Run(View);
    }
}