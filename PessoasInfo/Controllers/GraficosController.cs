using PessoasInfo.Services;

namespace PessoasInfo.Controllers;

public class GraficosController : Controller
{
    private readonly AppDbContext _context;

    public GraficosController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> SimpleGraph()
    {
        var graphService = new GraphService(_context);
        var graph = await graphService.GenerateGraph();
        return await Task.Run(() => View(graph));
    }
}