using PessoasInfo.Interfaces;

namespace PessoasInfo.Controllers;

public class PessoasController : Controller
{
    private readonly IPessoaRepository _pessoaRepository;

    public PessoasController(IPessoaRepository pessoaRepository)
    {
        _pessoaRepository = pessoaRepository;
    }

    // Get Pessoas
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var pessoas = await _pessoaRepository.GetPessoas();
        return pessoas != null
            ? await Task.Run(() => View(pessoas))
            : await Task.Run(NoContent);
    }
}