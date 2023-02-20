namespace PessoasInfo.Interfaces;

public interface IReportRepository
{
    Task<List<Pessoa>> GetPessoasInnerJoin();
}