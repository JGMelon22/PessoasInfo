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
            throw new Exception("IdPessoa inválido ou não encontrado!");

        var pessoa = await _context.Pessoas
            .Where(x => x.IdPessoa == id)
            .Select(y => new PessoaDetailViewModel
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
                                  WHERE IdPessoa = @Id;";

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

        var pessoaToRemove = await _context.Pessoas
            .Where(x => x.IdPessoa == id)
            .FirstOrDefaultAsync();

        var telefoneToRemove = await _context.Telefones
            .Where(x => x.IdPessoa == id)
            .FirstOrDefaultAsync();

        var detalheToRemove = await _context.Detalhes
            .Where(x => x.IdPessoa == id)
            .FirstOrDefaultAsync();

        if (pessoaToRemove == null)
            throw new Exception("IdPessoa não encontrado!");

        if (telefoneToRemove == null)
            throw new Exception("IdTelefone não encontrado!");

        if (detalheToRemove == null)
            throw new Exception("IdDetalhe não encontrado!");

        _context.Remove(pessoaToRemove);
        _context.Remove(telefoneToRemove);
        _context.Remove(detalheToRemove);

        await _context.SaveChangesAsync();

        /*
        if (id == null || id <= 0)
            throw new Exception("IdPessoa inválido ou não encontrado!");

        var deletePessoaQuery = @"DELETE 
                                  FROM Detalhes
                                  WHERE IdPessoa = @Id;

                                  DELETE 
                                  FROM Telefones
                                  WHERE IdPessoa = @Id;

                                  DELETE 
                                  FROM Pessoas
                                  WHERE IdPessoa = @Id;";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(deletePessoaQuery, new PessoaEditViewModel()
        {
            IdPessoa = id
        });

        _dbConnection.Close();
    */
    }
}