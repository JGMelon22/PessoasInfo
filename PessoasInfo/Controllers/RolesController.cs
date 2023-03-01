using PessoasInfo.Services;
using PessoasInfo.ViewModels.Role;

namespace PessoasInfo.Controllers;

public class RolesController : Controller
{
    private readonly AppDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;


    public RolesController(AppDbContext context, UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var roles = await _context.Roles.ToListAsync();
        return await Task.Run(() => View(roles));
    }

    // Upsert
    [HttpGet]
    public async Task<IActionResult> Upsert(string id)
    {
        if (string.IsNullOrEmpty(id)) return View();

        var user = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
        return await Task.Run(() => View(user));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(RoleViewModel roleViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        // Cargo já existe
        if (await _roleManager.RoleExistsAsync(roleViewModel.Name))
            return await Task.Run(() => RedirectToAction("Index"));

        var roleService = new EditRoleService(_context, _roleManager);

        await roleService.Upsert(roleViewModel);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Deletar cargo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var editRoleService = new EditRoleService(_context, _roleManager);

        if (id == null)
            return await Task.Run(() => RedirectToAction(nameof(Index)));

        if (ModelState.IsValid)
            await editRoleService.Delete(id);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }
}