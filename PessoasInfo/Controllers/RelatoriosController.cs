namespace PessoasInfo.Controllers;

public class RelatoriosController : Controller
{
    private readonly IReportService _reportService;

    public RelatoriosController(IReportService reportService)
    {
        _reportService = reportService;
    }

    // Lista relatorios
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var relatorios = await _reportService.GetRelatorios();
            return await Task.Run(() => View(relatorios));
        }
        catch (Exception)
        {
            return RedirectToAction("Error");
        }
    }

    // Baixar relatorio
    [HttpGet]
    public async Task<IActionResult> DownloadReport(string reportName)
    {
        var bytes = await _reportService.DownloadRelatorio(reportName);
        return File(bytes, "application/octet-stream", reportName);
    }

    // Deleta relatorio
    [HttpGet]
    public async Task<IActionResult> DeleteReport(string reportName)
    {
        await _reportService.DeleteRelatorio(reportName);
        return RedirectToAction("Index", "Relatorios");
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}