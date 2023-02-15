using PessoasInfo.Interfaces;

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
}