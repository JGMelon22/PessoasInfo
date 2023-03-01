using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PessoasInfo.Controllers;

public class UsersController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UsersController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        userManager = userManager;
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
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return NotFound();

        var userRole = _context.UserRoles.ToList();
        var roles = await _context.Roles.ToListAsync();
        var role = userRole.FirstOrDefault(x => x.UserId == user.Id);


        if (role != null)
            user.RoleId = roles.FirstOrDefault(x => x.Id == role.RoleId).Id;

        user.RoleList = _context.Roles.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id
        });

        return await Task.Run(() => View(user));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AppUser appUser)
    {
        if (ModelState.IsValid)
        {
            // Busca pelo usuário na base de dados
            var userDbValue = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == appUser.Id);

            if (userDbValue == null) return NotFound();

            // Tabela de ligação para buscar o cargo do usuário em questão
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userDbValue.Id);

            // Se não tiver um cargo atual, tenta buscar pelo seu cargo anterior

            if (userRole != null)
            {
                var previousRoleName = await _context.Roles.Where(x => x.Id == userRole.RoleId).Select(y => y.Name)
                    .FirstOrDefaultAsync();
                await _userManager.RemoveFromRoleAsync(userDbValue, previousRoleName);
            }

            // Adiciona novo cargo e salva
            await _userManager.AddToRoleAsync(userDbValue,
                _context.Roles.FirstOrDefault(x => x.Id == appUser.RoleId).Name);
            await _context.SaveChangesAsync();

            return await Task.Run(() => RedirectToAction(nameof(Index)));
        }

        // Lista de cargos
        appUser.RoleList = _context.Roles.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id
        });

        return await Task.Run(() => View(appUser));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string userId)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            return NotFound();

        _context.AppUsers.Remove(user);
        await _context.SaveChangesAsync();

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }
}