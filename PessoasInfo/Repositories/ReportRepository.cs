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
        var getPessoasInnerJoinQuery = @"SELECT p.PessoaId,
                                                p.Nome,
                                                t.IdTelefone,
                                                t.TelefoneTexto,
                                                t.PessoaId,
                                                t.Ativo,
                                                d.IdDetalhe,
                                                d.DetalheTexto,
                                                d.PessoaId
                                         FROM Pessoas p
                                             INNER JOIN Telefones T
                                         ON p.PessoaId = T.PessoaId
                                             INNER JOIN Detalhes D
                                         ON p.PessoaId = D.PessoaId;";

        _dbConnection.Open();

        // var lookup = new Dictionary<int, Pessoa>();

        var result = await _dbConnection.QueryAsync<Pessoa, Telefone, Detalhe, Pessoa>(getPessoasInnerJoinQuery,
            (pessoa, telefone, detalhe) =>
            {
                // Pessoa pessoa1;

                if (pessoa.Telefones == null) pessoa.Telefones = new List<Telefone>();

                if (pessoa.Detalhes == null) pessoa.Detalhes = new List<Detalhe>();

                pessoa.Telefones.Add(telefone);
                pessoa.Detalhes.Add(detalhe);

                return pessoa;
            },
            splitOn: "IdTelefone, IdDetalhe"
        );

        return result.Distinct().ToList();
    }
}