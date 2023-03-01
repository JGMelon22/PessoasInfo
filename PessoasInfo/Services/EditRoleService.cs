using PessoasInfo.ViewModels.Role;

namespace PessoasInfo.Services;

public class EditRoleService
{
    private readonly AppDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EditRoleService(AppDbContext context, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public async Task Upsert(RoleViewModel roleViewModel)
    {
        // Cria
        if (string.IsNullOrEmpty(roleViewModel.Id))
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = roleViewModel.Name });
        }

        // Atualiza
        else
        {
            var roleDb = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleViewModel.Id);

            if (roleDb != null)
            {
                // Não é vazio, atualiza
                roleDb.Name = roleViewModel.Name;
                roleDb.NormalizedName = roleViewModel.Name.ToUpper();

                var result = await _roleManager.UpdateAsync(roleDb);
            }
        }
    }

    public async Task Delete(string id)
    {
        if (id != null)
        {
            var roleDb = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (roleDb != null)
            {
                // Cargos associados ao usuário
                var userForThisRole = await _context.UserRoles.Where(x => x.UserId == id).CountAsync();
                if (userForThisRole == 0)
                    await _roleManager.DeleteAsync(roleDb);
            }
        }
    }
}