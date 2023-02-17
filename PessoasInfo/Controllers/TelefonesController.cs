namespace PessoasInfo.Controllers;

public class TelefonesController : Controller
{
    private readonly ITelefoneRepository _telefoneRepository;

    public TelefonesController(ITelefoneRepository telefoneRepository)
    {
        _telefoneRepository = telefoneRepository;
    }
}