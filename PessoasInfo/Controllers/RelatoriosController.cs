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
    [Authorize(Policy = "OnlyAdminChecker")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var relatorios = await _reportService.GetRelatorios();
            return await Task.Run(() => View(relatorios));
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Relatorios");
        }
    }

    // Baixar relatorio
    [HttpGet]
    [Authorize(Policy = "OnlyAdminChecker")]
    public async Task<IActionResult> DownloadReport(string reportName)
    {
        var bytes = await _reportService.DownloadRelatorio(reportName);
        return File(bytes, "application/octet-stream", reportName);
    }

    // Deleta relatorio
    [HttpGet]
    [Authorize(Policy = "OnlyAdminChecker")]
    public async Task<IActionResult> DeleteReport(string reportName)
    {
        await _reportService.DeleteRelatorio(reportName);
        return RedirectToAction("Index", "Relatorios");
    }

    // Gerar relatorio
    [Authorize(Policy = "OnlyAdminChecker")]
    public async Task<IActionResult> CreateReport()
    {
        await _reportService.GerarRelatorio();
        return RedirectToAction("Index", "Relatorios");
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}