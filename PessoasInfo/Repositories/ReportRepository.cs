using Dapper;

namespace PessoasInfo.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly IDbConnection _dbConnection;

    public ReportRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<List<Pessoa>> GetPessoasInnerJoin()
    {
        var getPessoasInnerJoinQuery = @"SELECT *
                                         FROM Pessoas p
                                             INNER JOIN Telefones T 
                                             ON p.IdPessoa = T.IdPessoa
                                         INNER JOIN Detalhes D 
                                         ON p.IdPessoa = D.IdPessoa;";

        _dbConnection.Open();

        var lookup = new Dictionary<int, Pessoa>();

        var result = await _dbConnection.QueryAsync<Pessoa, Telefone, Detalhe, Pessoa>(getPessoasInnerJoinQuery,
            (pessoaFunc, telefoneFunc, detalheFunc) =>
            {
                Pessoa pessoa1;

                if (!lookup.TryGetValue(pessoaFunc.IdPessoa, out pessoa1))
                {
                    pessoa1 = pessoaFunc;
                    pessoa1.Telefones = new List<Telefone>();
                    pessoa1.Detalhes = new List<Detalhe>();
                    lookup.Add(pessoa1.IdPessoa, pessoa1);
                }

                pessoa1.Telefones.Add(telefoneFunc);
                pessoa1.Detalhes.Add(detalheFunc);

                return pessoa1;
            },
            splitOn: "IdTelefone, IdDetalhe"
        );

        return result.Distinct().ToList();
    }
}