using System.Security.Claims;
using PessoasInfo.Services;
using PessoasInfo.ViewModels.UserClaim;

namespace PessoasInfo.Controllers;

public class UsersController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UsersController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userList = await _context.AppUsers.ToListAsync();
        var roleList = await _context.UserRoles.ToListAsync();
        var roles = await _context.Roles.ToListAsync();

        // Caso usuário não tenha cargo, coloca um placeholder "N/A"
        foreach (var user in userList)
        {
            var role = roleList.FirstOrDefault(x => x.UserId == user.Id);
            if (role == null)
                user.Role = "N/A";

            else
                user.Role = roles.FirstOrDefault(x => x.Id == role.RoleId).Name;
        }

        return await Task.Run(() => View(userList));
    }

    /// <summary>
    ///     Busca pelo usuário e seu cargo
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Em seguida mostra seu cargo através de uma lista de SelectListItem</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(string userId)
    {
        if (userId != null)
        {
            var editUserService = new EditUserService(_context, _userManager);

            var user = await editUserService.EditUserGet(userId);

            return await Task.Run(() => View(user));
        }

        return await Task.Run(View);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AppUser appUser)
    {
        if (ModelState.IsValid)
        {
            var editUserService = new EditUserService(_context, _userManager);
            await editUserService.EditUserPost(appUser);

            return await Task.Run(() => RedirectToAction(nameof(Index)));
        }

        return await Task.Run(() => View(appUser));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string userId)
    {
        if (ModelState.IsValid)
        {
            var editUserService = new EditUserService(_context, _userManager);
            await editUserService.DeleteUser(userId);

            return await Task.Run(() => RedirectToAction(nameof(Index)));
        }

        return await Task.Run(() => RedirectToAction("Error", "Home"));
    }

    // User Claims
    [HttpGet]
    public async Task<IActionResult> ManageUserClaims(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound();

        // Busca por permissões associadas ao usuário em questão
        var existingUserClaims = await _userManager.GetClaimsAsync(user);

        var model = new UserClaimsViewModel
        {
            UserId = userId
        };

        foreach (var claim in ClaimStore.ClaimList)
        {
            var userClaim = new UserClaim
            {
                ClaimType = claim.Type
            };

            // Verifica se existe claims associados a um usuário
            if (existingUserClaims.Any(x => x.Type == claim.Type)) userClaim.IsSelected = true;

            model.Claims.Add(userClaim);
        }

        return await Task.Run(() => View(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel userClaimsViewModel)
    {
        var user = await _userManager.FindByIdAsync(userClaimsViewModel.UserId); // Busca pelo usuário

        if (user == null)
            return NotFound();

        var claims = await _userManager.GetClaimsAsync(user); // Claims do usuário
        var result = await _userManager.RemoveClaimsAsync(user, claims); // Remove claim 

        if (!result.Succeeded)
            return View(userClaimsViewModel);

        result = await _userManager.AddClaimsAsync(user,
            userClaimsViewModel.Claims // Adiciona o claim selecionado na view
                .Where(x => x.IsSelected)
                .Select(y => new Claim(y.ClaimType, y.IsSelected.ToString())));

        if (result.Succeeded)
            return await Task.Run(() => View(userClaimsViewModel));

        return RedirectToAction(nameof(Index));
    }
}