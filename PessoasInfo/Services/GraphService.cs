using PessoasInfo.ViewModels.Grafico;

namespace PessoasInfo.Services;

public class GraphService
{
    private readonly AppDbContext _context;

    public GraphService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GraficoViewModel>> GenerateGraph()
    {
        var lstModel = new List<GraficoViewModel>();
        lstModel.Add(new GraficoViewModel
        {
            DimensionOne = "Pessoas",
            Quantity = await _context.Pessoas.CountAsync()
        });

        lstModel.Add(new GraficoViewModel
        {
            DimensionOne = "Telefones",
            Quantity = await _context.Telefones.CountAsync()
        });

        lstModel.Add(new GraficoViewModel
        {
            DimensionOne = "Detalhes",
            Quantity = await _context.Detalhes.CountAsync()
        });

        return lstModel;
    }
}