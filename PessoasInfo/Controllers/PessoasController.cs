using PessoasInfo.ViewModels.Pessoa;

namespace PessoasInfo.Controllers;

public class PessoasController : Controller
{
    private readonly IPagingService _pagingService;
    private readonly IPessoaRepository _pessoaRepository;

    public PessoasController(IPessoaRepository pessoaRepository, IPagingService pagingService)
    {
        _pessoaRepository = pessoaRepository;
        _pagingService = pagingService;
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

    // Paged Pessoas
    [HttpGet]
    [Authorize(Roles = "Comum, Admin")]
    public async Task<IActionResult> PagedIndex(string searchString, string sortOrder, int pageIndex = 1)
    {
        ViewBag.NomeSortParam = string.IsNullOrEmpty(sortOrder) ? "nome_desc" : "";
        ViewBag.IdSortParam = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
        ViewBag.CurrentFilter = searchString;

        var pessoas = await _pagingService.PagingPessoas(searchString, sortOrder, pageIndex);
        return pessoas != null
            ? await Task.Run(() => View(pessoas))
            : NoContent();
    }

    // Paged Pessoas Cruzadas
    public async Task<IActionResult> AllDetails(int pageIndex = 1)
    {
        var pessoasCruzadas = await _pagingService.PagingPessoasInnerJoinEF(pageIndex);
        return pessoasCruzadas != null
            ? await Task.Run(() => View(pessoasCruzadas))
            : NoContent();
    }

    // Detalhe de uma pessoa 
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Details(int id)
    {
        if (id == null || id <= 0)
            return NotFound();

        var pessoa = await _pessoaRepository.GetPessoa(id);

        return await Task.Run(() => View(pessoa));
    }

    // Adicionar nova pessoa
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
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
    [Authorize(Policy = "TestClaims")]
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
        if (id != pessoaEditViewModel.PessoaId)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        await _pessoaRepository.UpdatePessoa(id, pessoaEditViewModel);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Delete Pessoa
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
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