using PessoasInfo.ViewModels.Telefone;

namespace PessoasInfo.Controllers;

public class TelefonesController : Controller
{
    private readonly IPagingService _pagingService;
    private readonly ITelefoneRepository _telefoneRepository;

    public TelefonesController(ITelefoneRepository telefoneRepository, IPagingService pagingService)
    {
        _telefoneRepository = telefoneRepository;
        _pagingService = pagingService;
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

    // Paged Telefones
    [HttpGet]
    [Authorize(Roles = "Comum, Admin")]
    public async Task<IActionResult> PagedIndex(string searchString, string sortOrder, int pageIndex = 1)
    {
        ViewBag.TelefoneSortParam = string.IsNullOrEmpty(sortOrder) ? "telefone_desc" : "";
        ViewBag.AtivoSortParam = string.IsNullOrEmpty(sortOrder) ? "ativo_desc" : "";
        ViewBag.IdSortParam = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
        ViewBag.CurrentFilter = searchString;

        var telefones = await _pagingService.PagingTelefones(searchString, sortOrder, pageIndex);
        return telefones != null
            ? await Task.Run(() => View(telefones))
            : NoContent();
    }

    // Informações telefônicas
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Details(int id)
    {
        if (id == null || id <= 0)
            return NotFound();

        var telefone = await _telefoneRepository.GetTelefone(id);

        return await Task.Run(() => View(telefone));
    }

    // Adicionar novo telefone
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Create()
    {
        return await Task.Run(View);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TelefoneCreateViewModel telefoneCreateViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        try
        {
            await _telefoneRepository.AddTelefone(telefoneCreateViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Telefones");
        }

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Editar Telefone
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Edit(int id)
    {
        if (id == null || id <= 0)
            return NotFound();

        var telefone = await _telefoneRepository.GetTelefone(id);

        return await Task.Run(() => View(telefone));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TelefoneEditViewModel telefoneEditViewModel)
    {
        if (id != telefoneEditViewModel.IdTelefone)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        await _telefoneRepository.UpdateTelefone(id, telefoneEditViewModel);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Deletar Telefone
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Delete(int id)
    {
        var telefone = await _telefoneRepository.GetTelefone(id);

        if (id == null || telefone == null)
            return NotFound();

        return await Task.Run(() => View(telefone));
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var telefone = await _telefoneRepository.GetTelefone(id);

        if (id == null || telefone == null)
            return NotFound();

        await _telefoneRepository.DeleteTelefone(id);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Error Telefone
    [HttpGet]
    public async Task<IActionResult> Error()
    {
        return await Task.Run(View);
    }
}