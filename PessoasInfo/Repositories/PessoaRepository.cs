using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PessoasInfo.Data;
using PessoasInfo.Interfaces;
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
        var getPessoasQuery = @"SELECT TOP 20 IdPessoa,
                                              Nome
                                FROM Pessoas;";

        _dbConnection.Open();

        var result = await _dbConnection.QueryAsync<PessoaIndexViewModel>(getPessoasQuery);

        _dbConnection.Close();

        return result.ToList();
    }

    public async Task<PessoaIndexViewModel> GetPessoa(int id)
    {
        if (id == null && id <= 0)
            throw new Exception("IdPessoa inválido ou não encontrado!");

        var pessoa = await _context.Pessoas
            .Where(x => x.IdPessoa == id)
            .Select(y => new PessoaIndexViewModel
            {
                IdPessoa = y.IdPessoa,
                Nome = y.Nome
            })
            .FirstOrDefaultAsync();

        if (pessoa == null)
            throw new Exception("IdPessoa não encontrado!");

        return pessoa;
    }

    public async Task<PessoaEditViewModel> UpdatePessoa(int id, PessoaEditViewModel pessoaEditViewModel)
    {
        if (id == null || id <= 0)
            throw new Exception("IdPessoa inválido ou não encontrado!");

        var updatePessoaQuery = @"UPDATE Pessoas
                                  SET Nome = @Nome
                                  WHERE IdPessoa = @IdPessoa;";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(updatePessoaQuery, new PessoaEditViewModel
        {
            Nome = pessoaEditViewModel.Nome,
            IdPessoa = pessoaEditViewModel.IdPessoa
        });

        _dbConnection.Close();

        return pessoaEditViewModel;
    }

    public async Task DeletePessoa(int id)
    {
        if (id == null || id <= 0)
            throw new Exception("IdPessoa inválido ou não encontrado!");

        var deletePessoaQuery = @"DELETE 
                                  FROM Detalhes
                                  WHERE IdPessoa = @IdPessoa;

                                  DELETE 
                                  FROM Telefones
                                  WHERE IdPessoa = @IdPessoa;

                                  DELETE 
                                  FROM Pessoas
                                  WHERE IdPessoa = @IdPessoa;";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(deletePessoaQuery, new
        {
            IdPessoa = id
        });

        _dbConnection.Close();
    }
}