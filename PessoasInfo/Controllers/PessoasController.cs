using DocumentFormat.OpenXml.Drawing;
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