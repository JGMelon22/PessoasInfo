namespace PessoasInfo.Controllers;

public class TelefonesController : Controller
{
    private readonly ITelefoneRepository _telefoneRepository;

    public TelefonesController(ITelefoneRepository telefoneRepository)
    {
        _telefoneRepository = telefoneRepository;
    }

    // GET Telefones
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var telefones = await _telefoneRepository.GetTelefones();
        return telefones != null
            ? await Task.Run(() => View(telefones))
            : NoContent();
    }

    // Informações telefônicas
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id != null || id <= 0)
            return NotFound();

        var telefone = await _telefoneRepository.GetTelefone(id);

        return await Task.Run(() => View(telefone));
    }
}