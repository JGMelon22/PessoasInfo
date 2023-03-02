using System.Security.Claims;
using PessoasInfo.ViewModels.UserClaim;

namespace PessoasInfo.Services;

public class ManageUserClaimsService
{
    private readonly UserManager<AppUser> _userManager;

    public ManageUserClaimsService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task ManageClaims(UserClaimsViewModel userClaimsViewModel)
    {
        var user = await _userManager.FindByIdAsync(userClaimsViewModel.UserId); // Busca pelo usuário

        if (user != null)
        {
            var userClaim = await _userManager.GetClaimsAsync(user); // Claims do usuário
            var result = await _userManager.RemoveClaimsAsync(user, userClaim); // Remove claim 

            if (result.Succeeded)
                result = await _userManager.AddClaimsAsync(user,
                    userClaimsViewModel.Claims // Adiciona o claim selecionado na view
                        .Where(x => x.IsSelected)
                        .Select(y => new Claim(y.ClaimType, y.IsSelected.ToString())));
        }
    }
}