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
        // Se já existir, retorna
        if (await _roleManager.RoleExistsAsync(roleViewModel.Name))
            return RedirectToAction("Index");

        // Ademais, cria um novo
        if (string.IsNullOrEmpty(roleViewModel.Id))
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = roleViewModel.Name });
        }

        // Atualiza
        else
        {
            var roleDb = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleViewModel.Id);
            if (roleDb == null)
                return RedirectToAction(nameof(Index));

            roleDb.Name = roleViewModel.Name;
            roleDb.NormalizedName = roleDb.Name.ToUpper();

            var result = await _roleManager.UpdateAsync(roleDb);
        }

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Deletar cargo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var roleDb = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

        if (roleDb == null)
            return RedirectToAction(nameof(Index));

        // Cargos associados ao usuário
        var userRolesForThisRole = await _context.UserRoles.Where(x => x.RoleId == id).CountAsync();

        if (userRolesForThisRole > 0)
            return await Task.Run(() => RedirectToAction(nameof(Index)));

        await _roleManager.DeleteAsync(roleDb);
        return RedirectToAction(nameof(Index));
    }
}