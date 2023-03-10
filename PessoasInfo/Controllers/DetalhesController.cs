using PessoasInfo.ViewModels.Detalhe;

namespace PessoasInfo.Controllers;

public class DetalhesController : Controller
{
    private readonly IDetalheRepository _detalheRepository;
    private readonly IPagingService _pagingService;

    public DetalhesController(IDetalheRepository detalheRepository, IPagingService pagingService)
    {
        _detalheRepository = detalheRepository;
        _pagingService = pagingService;
    }

    // Get Detalhes
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var detalhes = await _detalheRepository.GetDetalhes();

        return detalhes != null
            ? await Task.Run(() => View(detalhes))
            : NoContent();
    }

    // Paged Detalhes
    [HttpGet]
    [Authorize(Roles = "Comum, Admin")]
    public async Task<IActionResult> PagedIndex(string searchString, string sortOrder, int pageIndex = 1)
    {
        ViewBag.DetalheSortParam = string.IsNullOrEmpty(sortOrder) ? "detalhe_desc" : "";
        ViewBag.IdSortParam = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
        ViewBag.CurrentFilter = searchString;

        var detalhes = await _pagingService.PagingDetalhes(searchString, sortOrder, pageIndex);
        return detalhes != null
            ? await Task.Run(() => View(detalhes))
            : NoContent();
    }

    // Informações do Detalhe 
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Details(int id)
    {
        if (id == null || id <= 0)
            return NotFound();

        var detalhe = await _detalheRepository.GetDetalhe(id);

        return await Task.Run(() => View(detalhe));
    }

    // Delete Detalhe
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Delete(int id)
    {
        var detalhe = await _detalheRepository.GetDetalhe(id);

        if (id == null || detalhe == null)
            return NotFound();

        return await Task.Run(() => View(detalhe));
    }

    // Adicionar novo detalhe
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Create()
    {
        return await Task.Run(View);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Create(DetalheCreateViewModel detalheCreateViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        try
        {
            await _detalheRepository.AddDetalhe(detalheCreateViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Detalhes");
        }

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var detalhe = await _detalheRepository.GetDetalhe(id);

        if (id == null || detalhe == null)
            return NotFound();

        await _detalheRepository.DeleteDetalhe(id);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Editar Detalhe
    [HttpGet]
    [Authorize(Policy = "TestClaims")]
    public async Task<IActionResult> Edit(int id)
    {
        if (id == null || id <= 0)
            return NotFound();

        var detalhe = await _detalheRepository.GetDetalhe(id);

        return await Task.Run(() => View(detalhe));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DetalheEditViewModel detalheEditViewModel)
    {
        if (id == null || id <= 0)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        await _detalheRepository.UpdateDetalhe(id, detalheEditViewModel);

        return await Task.Run(() => RedirectToAction(nameof(Index)));
    }

    // Error
    public async Task<IActionResult> Error()
    {
        return await Task.Run(View);
    }
}