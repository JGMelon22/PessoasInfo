namespace PessoasInfo.ViewModels.UserClaim;

public class UserClaimsViewModel : Models.UserClaim
{
    // Estrutura de dados para nos ajudar a gerenciar claims facilmente
    public UserClaimsViewModel()
    {
        Claims = new List<Models.UserClaim>();
    }

    public string UserId { get; set; }
    public List<Models.UserClaim> Claims { get; set; }
}