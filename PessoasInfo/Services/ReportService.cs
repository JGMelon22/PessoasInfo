using ClosedXML.Excel;
using PessoasInfo.ViewModels.Report;

namespace PessoasInfo.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<List<ReportViewModel>> GetRelatorios()
    {
        // Obtem os arquivos do diretório (relatórios já gerados)
        var filePath = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Relatorios"));

        var relatoriosGerados = new List<ReportViewModel>(); // Monta uma lista com os relatorios encontrados 

        foreach (var item in filePath) // Adiciona a lista o nome dos relatórios
            relatoriosGerados.Add(new ReportViewModel
            {
                ReportName = Path.GetFileName(item)
            });

        return await Task.FromResult(relatoriosGerados.ToList());
    }

    public async Task GerarRelatorio()
    {
        var dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[5] // Monta o cabeçalho do relatório
        {
            new("Id"),
            new("Nome"),
            new("Telefone"),
            new("Ativo?"),
            new("Detalhe")
        });

        // Invoca a query responsáver por obter os dados cruzados
        var registros = await _reportRepository.GetPessoasInnerJoin();

        // Popula as linhas do relatório c/ os registros obtidos do banco
        foreach (var registro in registros)
            dt.Rows.Add(registro.IdPessoa, registro.Nome,
                registro.Telefones.Select(x => x.TelefoneTexto).FirstOrDefault(),
                registro.Telefones.Select(x => x.Ativo).FirstOrDefault(),
                registro.Detalhes.Select(x => x.DetalheTexto).FirstOrDefault());

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Relatorios");

        // Verifica se o caminho existe, ademais, cria
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // Monta o relatório em si usando o closedXML
        using var wb = new XLWorkbook();
        wb.Worksheets.Add(dt, "PessoasCruzadas");
        wb.SaveAs(folderPath + Path.DirectorySeparatorChar + "RelatorioPessoasCruzadas.xlsx");
    }

    public async Task<byte[]> DownloadRelatorio(string reportName)
    {
        // Busca arquivo no caminho
        var path = Path.Combine(Path.Combine(Environment.CurrentDirectory, "Relatorios" + Path.DirectorySeparatorChar) +
                                reportName);
        // Le o arquivo 
        var bytes = await File.ReadAllBytesAsync(path);
        return await Task.FromResult(bytes);
    }

    public Task DeleteRelatorio(string reportName)
    {
        // Busca pelo caminho do arquivo
        var path = Path.Combine(Path.Combine(Environment.CurrentDirectory, "Relatorios" +
                                                                           Path.DirectorySeparatorChar) + reportName);
        // Se encontrar, deleta o arquivo
        if (File.Exists(path))
            File.Delete(path);

        return Task.CompletedTask;
    }
}