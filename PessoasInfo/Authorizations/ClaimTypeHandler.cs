namespace PessoasInfo.Authorizations;

public class ClaimTypeHandler : AuthorizationHandler<ClaimTypeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ClaimTypeRequirement requirement)
    {
        if (context.User.HasClaim(x => x.Type == "Criar")) // Julga se o usuário tem autorização para criar dados
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}