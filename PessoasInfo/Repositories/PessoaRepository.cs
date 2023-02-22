using Dapper;
using PessoasInfo.ViewModels.Pessoa;

namespace PessoasInfo.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dbConnection;

    public PessoaRepository(IDbConnection dbConnection, AppDbContext context)
    {
        _dbConnection = dbConnection;
        _context = context;
    }

    public async Task<PessoaCreateViewModel> AddPessoa(PessoaCreateViewModel pessoaCreateViewModel)
    {
        var addPessoaQuery = @"INSERT INTO Pessoas(Nome)
                                VALUES(@Nome);";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(addPessoaQuery, new PessoaCreateViewModel
        {
            Nome = pessoaCreateViewModel.Nome
        });

        _dbConnection.Close();

        return pessoaCreateViewModel;
    }

    public async Task<IEnumerable<PessoaIndexViewModel>> GetPessoas()
    {
        var getPessoasQuery = @"SELECT TOP(5) PERCENT PessoaId,
                                                       Nome
                                            FROM Pessoas
                                            ORDER BY PessoaId;";

        _dbConnection.Open();

        var result = await _dbConnection.QueryAsync<PessoaIndexViewModel>(getPessoasQuery);

        _dbConnection.Close();

        return result.ToList();
    }

    public async Task<PessoaIndexViewModel> GetPessoa(int id)
    {
        if (id == null && id <= 0)
            throw new Exception("PessoaId inválido ou não encontrado!");

        var pessoa = await _context.Pessoas
            .Where(x => x.PessoaId == id)
            .Select(y => new PessoaIndexViewModel
            {
                PessoaId = y.PessoaId,
                Nome = y.Nome
            })
            .FirstOrDefaultAsync();

        if (pessoa == null)
            throw new Exception("PessoaId não encontrado!");

        return pessoa;
    }

    public async Task<PessoaEditViewModel> UpdatePessoa(int id, PessoaEditViewModel pessoaEditViewModel)
    {
        if (id == null || id <= 0)
            throw new Exception("PessoaId inválido ou não encontrado!");

        var updatePessoaQuery = @"UPDATE Pessoas
                                  SET Nome = @Nome
                                  WHERE PessoaId = @PessoaId;";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(updatePessoaQuery, new
        {
            pessoaEditViewModel.Nome, pessoaEditViewModel.PessoaId
        });

        _dbConnection.Close();

        return pessoaEditViewModel;
    }

    public async Task DeletePessoa(int id)
    {
        if (id == null || id <= 0)
            throw new Exception("PessoaId inválido ou não encontrado!");

        var deletePessoaQuery = @"DELETE 
                                  FROM Detalhes
                                  WHERE PessoaId = @PessoaId;

                                  DELETE 
                                  FROM Telefones
                                  WHERE PessoaId = @PessoaId;

                                  DELETE 
                                  FROM Pessoas
                                  WHERE PessoaId = @PessoaId;";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(deletePessoaQuery, new
        {
            PessoaId = id
        });

        _dbConnection.Close();
    }
}