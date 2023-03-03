namespace PessoasInfo.Authorizations;

public class ClaimTypeRequirement : IAuthorizationRequirement
{
    public ClaimTypeRequirement(string userClaimType)
    {
        userClaimType = UserClaimType;
    }

    public string UserClaimType { get; set; }
}