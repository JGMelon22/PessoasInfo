using PessoasInfo.ViewModels.Report;

namespace PessoasInfo.Interfaces;

public interface IReportService
{
    Task<List<ReportViewModel>> GetRelatorios(); // Listar os relatorios gerados
    Task GerarRelatorio();
    Task<byte[]> DownloadRelatorio(string reportName);
    Task DeleteRelatorio(string reportName);
}