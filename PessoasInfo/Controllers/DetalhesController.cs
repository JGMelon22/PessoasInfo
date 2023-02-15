using PessoasInfo.ViewModels.Detalhe;

namespace PessoasInfo.Controllers;

public class DetalhesController : Controller
{
    private readonly IDetalheRepository _detalheRepository;

    public DetalhesController(IDetalheRepository detalheRepository)
    {
        _detalheRepository = detalheRepository;
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

    // Informações do Detalhe 
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id == null || id <= 0)
            return NotFound();

        var detalhe = await _detalheRepository.GetDetalhe(id);

        return await Task.Run(() => View(detalhe));
    }

    // Delete Detalhe
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var detalhe = await _detalheRepository.GetDetalhe(id);

        if (id == null || detalhe == null)
            return NotFound();

        return await Task.Run(() => View(detalhe));
    }

    // Adicionar novo detalhe
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return await Task.Run(View);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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