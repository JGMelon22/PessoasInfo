using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PessoasInfo.Controllers;

public class UsersController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _user;

    public UsersController(AppDbContext context, UserManager<AppUser> user)
    {
        _context = context;
        _user = user;
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
            var role = roleList.FirstOrDefault(x => x.RoleId == user.Id);
            if (role == null)
                user.Role = "N/A";

            else
                user.Role = roles.FirstOrDefault(x => x.Id == role.RoleId).Name;
        }

        return await Task.Run(() => View(userList));
    }

    /// <summary>
    /// Busca pelo usuário e seu cargo
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Em seguida mostra seu cargo através de uma lista de SelectListItem</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(string userId)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            return NotFound();

        var userRole = await _context.UserRoles.ToListAsync();
        var roles = await _context.Roles.ToListAsync();
        var role = userRole.FirstOrDefault(x => x.UserId == user.Id);

        if (role != null)
            user.RoleId = roles.FirstOrDefault(x => x.Id == role.RoleId).Id;

        user.RoleList = _context.Roles.Select(x => new SelectListItem()
        {
            Text = x.Name,
            Value = x.Id
        });

        return await Task.Run(() => View(user));
    }
}