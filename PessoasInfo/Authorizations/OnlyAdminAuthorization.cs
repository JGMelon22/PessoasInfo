using Microsoft.AspNetCore.Authorization;

namespace PessoasInfo.Authorizations;

public class OnlyAdminAuthorization : AuthorizationHandler<OnlyAdminAuthorization>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OnlyAdminAuthorization requirement)
    {
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}