using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PessoasInfo.Data;
using PessoasInfo.Interfaces;
using PessoasInfo.ViewModels.Telefone;

namespace PessoasInfo.Repositories;

public class TelefoneRepository : ITelefoneRepository
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dbConnection;

    public TelefoneRepository(IDbConnection dbConnection, AppDbContext context)
    {
        _dbConnection = dbConnection;
        _context = context;
    }

    public async Task<TelefoneCreateViewModel> AddTelefone(TelefoneCreateViewModel telefoneCreateViewModel)
    {
        var addTelefone = @"INSERT INTO Telefones(TelefoneTexto, IdPessoa)
                            VALUES = @TelefoneTexto, @IdPessoa);";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(addTelefone, new TelefoneCreateViewModel
        {
            TelefoneTexto = telefoneCreateViewModel.TelefoneTexto,
            IdPessoa = telefoneCreateViewModel.IdPessoa
        });

        _dbConnection.Close();

        return telefoneCreateViewModel;
    }

    public async Task<IEnumerable<TelefoneIndexlViewModel>> GetTelefones()
    {
        var getTelefonesQuery = @"SELECT IdTelefone,
                                         TelefoneTexto,
                                         IdPessoa,
                                         Ativo
                                  FROM Telefones;";

        _dbConnection.Open();

        var result = await _dbConnection.QueryAsync<TelefoneIndexlViewModel>(getTelefonesQuery);

        _dbConnection.Close();

        return result.ToList();
    }

    public async Task<TelefoneIndexlViewModel> GetTelefone(int id)
    {
        if (id == null || id <= 0)
            throw new Exception("IdTelefone inválido ou não encontrado!");

        var telefone = await _context.Telefones
            .Where(x => x.IdTelefone == id)
            .Select(y => new TelefoneIndexlViewModel
            {
                TelefoneTexto = y.TelefoneTexto
            })
            .FirstOrDefaultAsync();

        if (telefone == null)
            throw new Exception("IdTelefone não encontrado!");

        return telefone;
    }

    public async Task<TelefoneEditViewModel> UpdateTelefone(int id, TelefoneEditViewModel telefoneEditViewModel)
    {
        var updateTelefoneQuery = @"UPDATE Telefones
                                    SET TelefoneTexto = @TelefoneTexto
                                    WHERE IdTelefone = @IdTelefone";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(updateTelefoneQuery, new TelefoneEditViewModel
        {
            TelefoneTexto = telefoneEditViewModel.TelefoneTexto,
            IdTelefone = telefoneEditViewModel.IdTelefone
        });

        _dbConnection.Close();

        return telefoneEditViewModel;
    }

    public async Task DeleteTelefone(int id)
    {
        if (id == null || id <= 0)
            throw new Exception("IdTelefone inválido ou não encontrado!");

        var telefoneDeleteQuery = @"DELETE
                                    FROM Telefones
                                    WHERE IdTelefone = @IdTelefone;";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(telefoneDeleteQuery, new
        {
            IdTelefone = id
        });

        _dbConnection.Close();
    }
}