using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PessoasInfo.ViewModels.Detalhe;

namespace PessoasInfo.Repositories;

public class DetalheRepository : IDetalheRepository
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dbConnection;

    public DetalheRepository(IDbConnection dbConnection, AppDbContext context)
    {
        _dbConnection = dbConnection;
        _context = context;
    }

    public async Task<DetalheCreateViewModel> AddDetalhe(DetalheCreateViewModel detalheCreateViewModel)
    {
        var addDetalheQuery = @"INSERT INTO Detalhes(DetalheTexto, IdPessoa)
                                VALUES(@DetalheTexto, @IdPessoa);";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(addDetalheQuery, new DetalheCreateViewModel
        {
            DetalheTexto = detalheCreateViewModel.DetalheTexto,
            IdPessoa = detalheCreateViewModel.IdPessoa
        });

        _dbConnection.Close();

        return detalheCreateViewModel;
    }

    public async Task<IEnumerable<DetalheIndexViewModel>> GetDetalhes()
    {
        var getDetalhesQuery = @"SELECT TOP 100 IdDetalhe, 
                                                DetalheTexto,
                                                IdPessoa
                                 FROM Detalhes;";

        _dbConnection.Open();

        var result = await _dbConnection.QueryAsync<DetalheIndexViewModel>(getDetalhesQuery);

        _dbConnection.Close();

        return result.ToList();
    }

    public async Task<DetalheIndexViewModel> GetDetalhe(int id)
    {
        if (id == null || id <= 0)
            throw new Exception("IdDetalhe inválido ou não encontrado!");

        var detalhe = await _context.Detalhes
            .Where(x => x.IdDetalhe == id)
            .Select(y => new DetalheIndexViewModel
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
                                   WHERE IdDetalhe = @IdDetalhe";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(updateDetalheQuery, new DetalheEditViewModel
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
                                   WHERE IdDetalhe = @IdDetalhe;";

        _dbConnection.Open();

        await _dbConnection.ExecuteAsync(deleteDetalheQuery, new
        {
            IdDetalhe = id
        });

        _dbConnection.Close();
    }
}