using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PessoasInfo.Services;

public class EditUserService
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public EditUserService(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<AppUser> EditUserGet(string userId)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == userId);

        if (user != null)
        {
            var userRole = await _context.UserRoles.ToListAsync();
            var roles = await _context.Roles.ToListAsync();
            var role = userRole.FirstOrDefault(x => x.UserId == userId);

            if (role != null)
                user.RoleId = roles.FirstOrDefault(x => x.Id == role.RoleId).Id;

            // Adiciona items a lista SelectedListItem para ser mostrado
            user.RoleList = _context.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            });
        }

        return user;
    }

    public async Task<AppUser> EditUserPost(AppUser appUser)
    {
        if (appUser != null)
        {
            // Pesquisa pelo usuário no banco
            var userDbValue = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == appUser.Id);

            var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userDbValue.Id);

            // Obtem o último perfil/cargo do usuário 
            if (userRole != null)
            {
                var previousRoleName = await _context.Roles
                    .Where(x => x.Id == userRole.RoleId)
                    .Select(y => y.Name)
                    .FirstOrDefaultAsync();

                // Remove cargo atual...
                await _userManager.RemoveFromRoleAsync(userDbValue, previousRoleName);
            }

            // e muda para o novo escolhido
            await _userManager.AddToRoleAsync(userDbValue,
                _context.Roles.FirstOrDefault(x => x.Id == appUser.RoleId).Name);

            await _context.SaveChangesAsync();
        }

        // Mostra o cargo escolhido na SelectedListItem
        appUser.RoleList = _context.Roles.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id
        });

        return appUser;
    }

    public async Task<AppUser> DeleteUser(string userId)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == userId);

        if (user != null)
        {
            _context.AppUsers.Remove(user);
            await _context.SaveChangesAsync();
        }

        return user;
    }
}