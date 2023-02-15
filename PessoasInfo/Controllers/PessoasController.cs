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
            : NoContent();
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

    // Editar pessoa
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (id == null || id <= 0)
            return NotFound();

        var pessoa = await _pessoaRepository.GetPessoa(id);

        return await Task.Run(() => View(pessoa));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PessoaEditViewModel pessoaEditViewModel)
    {
        if (id != pessoaEditViewModel.IdPessoa)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        await _pessoaRepository.UpdatePessoa(id, pessoaEditViewModel);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Delete Pessoa
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var pessoa = await _pessoaRepository.GetPessoa(id);

        if (id == null || pessoa == null)
            return NotFound();

        return await Task.Run(() => View(pessoa));
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pessoa = await _pessoaRepository.GetPessoa(id);

        if (id == null || pessoa == null)
            return NotFound();

        await _pessoaRepository.DeletePessoa(id);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }
}