using PessoasInfo.Interfaces;
using PessoasInfo.ViewModels.Pessoa;

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

    // Detalhe de uma pessoa 
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id == null || id <= 0)
            return NotFound();

        var pessoa = await _pessoaRepository.GetPessoa(id);

        return await Task.Run(() => View(pessoa));
    }

    // Adicionar nova pessoa
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return await Task.Run(View);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PessoaCreateViewModel pessoaCreateViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _pessoaRepository.AddPessoa(pessoaCreateViewModel);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Delete Pessoa
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var pessoa = await _pessoaRepository.GetPessoa(id);

        if (id == null || pessoa == null)
            return await Task.Run(NotFound);

        var pessoaToRemove = await _pessoaRepository.GetPessoa(id);

        return await Task.Run(() => View(pessoaToRemove));
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pessoa = await _pessoaRepository.GetPessoa(id);

        if (id == null || pessoa == null)
            return await Task.Run(NotFound);

        await _pessoaRepository.DeletePessoa(id);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }
}