using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PessoasInfo.Data;
using PessoasInfo.Interfaces;
using PessoasInfo.ViewModels.Pessoa;

namespace PessoasInfo.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly AppDbContext _context;

    public PessoaRepository(IDbConnection dbConnection, AppDbContext context)
    {
        _dbConnection = dbConnection;
        _context = context;
    }

    public async Task<PessoaCreateViewModel> AddPessoa(PessoaCreateViewModel pessoaCreateViewModel)
    {
        var addPessoaQuery = @"INSERT INTO Pessoas(Nome)
                                VALUE(@Nome);";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(addPessoaQuery, new
        {
            pessoaCreateViewModel.Nome
        });

        _dbConnection.Close();

        return pessoaCreateViewModel;
    }

    public async Task<IEnumerable<PessoaDetailViewModel>> GetPessoas()
    {
        var getPessoasQuery = @"SELECT IdPessoa,
                                       Nome
                                FROM Pessoas;";

        _dbConnection.Open();

        var result = await _dbConnection.QueryAsync<PessoaDetailViewModel>(getPessoasQuery);

        _dbConnection.Close();

        return result.ToList();
    }

    public async Task<PessoaDetailViewModel> GetPessoa(int id)
    {
        if (id == null && id <= 0)
            throw new Exception("IdPessoa inválido!");

        var pessoa = await _context.Pessoas
            .Where(x => x.IdPessoa == id)
            .Select(y => new PessoaDetailViewModel()
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
        var updatePessoaQuery = @"UPDATE Pessoas
                                  SET Nome = @Nome
                                  WHERE IdPessoa = @Id;";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(updatePessoaQuery, new
        {
            pessoaEditViewModel.Nome,
            pessoaEditViewModel.IdPessoa
        });

        _dbConnection.Close();

        return pessoaEditViewModel;
    }

    public async Task DeletePessoa(int id)
    {
        if (id == null || id <= 0)
            throw new Exception("IdPessoa inválido");

        var pessoaToDelete = await _context.Pessoas
            .Where(x => x.IdPessoa == id)
            .FirstOrDefaultAsync();

        if (pessoaToDelete == null)
            throw new Exception("IdPessoa não encontrado!");

        _context.Remove(pessoaToDelete);
        await _context.SaveChangesAsync();
    }
}