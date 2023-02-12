using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PessoasInfo.Data;
using PessoasInfo.Interfaces;
using PessoasInfo.ViewModels.Detalhe;
using PessoasInfo.ViewModels.Pessoa;

namespace PessoasInfo.Repositories;

public class DetalheRepository : IDetalheRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly AppDbContext _context;

    public DetalheRepository(IDbConnection dbConnection, AppDbContext context)
    {
        _dbConnection = dbConnection;
        _context = context;
    }

    public async Task<DetalheCreateViewModel> AddDetalhe(DetalheCreateViewModel detalheCreateViewModel)
    {
        var addDetalheQuery = @"INSERT INTO Detalhes(DetalheTexto, IdPessoa)
                           @VALUES(@DetalheTexto, @IdPessoa);";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(addDetalheQuery, new DetalheCreateViewModel()
        {
            DetalheTexto = detalheCreateViewModel.DetalheTexto,
            IdPessoa = detalheCreateViewModel.IdPessoa
        });

        _dbConnection.Close();

        return detalheCreateViewModel;
    }

    public async Task<IEnumerable<DetalheDetailViewModel>> GetDetalhes()
    {
        var getDetalhesQuery = @"SELECT IdDetalhe, 
                                   DetalheTexto,
                                   IdPessoa
                            FROM Detalhes;";

        _dbConnection.Open();

        var result = await _dbConnection.QueryAsync<DetalheDetailViewModel>(getDetalhesQuery);

        _dbConnection.Close();

        return result.ToList();
    }

    public async Task<DetalheDetailViewModel> GetDetalhe(int id)
    {
        if (id == null || id <= 0)
            throw new Exception("IdDetalhe inválido ou não encontrado!");

        var detalhe = await _context.Detalhes
            .Where(x => x.IdDetalhe == id)
            .Select(y => new DetalheDetailViewModel()
            {
                IdDetalhe = y.IdDetalhe,
                DetalheTexto = y.DetalheTexto,
                IdPessoa = y.IdPessoa
            })
            .FirstOrDefaultAsync();

        if (detalhe == null)
            throw new Exception("IdDetalhe não encontrado!");

        return detalhe;
    }

    public async Task<DetalheEditViewModel> UpdateDetalhe(int id, DetalheEditViewModel detalheEditViewModel)
    {
        if (id == null || id <= 0)
            throw new Exception("IdDetalhe inválido ou não encontrado!");

        var updateDetalheQuery = @"UPDATE Detalhes
                                   SET DetalheTexto = @DetalheTexto
                                   WHERE IdDetalhe = @Id";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(updateDetalheQuery, new DetalheEditViewModel()
        {
            DetalheTexto = detalheEditViewModel.DetalheTexto,
            IdDetalhe = detalheEditViewModel.IdDetalhe
        });

        _dbConnection.Close();

        return detalheEditViewModel;
    }

    public async Task DeleteDetalhe(int id)
    {
        if (id == null || id <= 0)
            throw new Exception("IdDetalhe inválido ou não encontrado!");

        var deleteDetalheQuery = @"DELETE 
                                   FROM Detalhes
                                   WHERE IdDatalhe = @Id;";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(deleteDetalheQuery, new DetalheDetailViewModel()
        {
            IdDetalhe = id
        });

        _dbConnection.Close();
    }
}