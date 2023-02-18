using Microsoft.EntityFrameworkCore;
using PessoasInfo.ViewModels.Detalhe;
using PessoasInfo.ViewModels.Pessoa;
using PessoasInfo.ViewModels.Telefone;
using ReflectionIT.Mvc.Paging;

namespace PessoasInfo.Services;

public class PagingService : IPagingService
{
    private readonly AppDbContext _context;

    public PagingService(AppDbContext context)
    {
        _context = context;
    }

    // Paginação Pessoas
    public async Task<PagingList<PessoaIndexViewModel>> PagingPessoas(int pageIndex = 1)
    {
        var result = _context.Pessoas
            .AsNoTracking()
            .Select(x => new PessoaIndexViewModel
            {
                IdPessoa = x.IdPessoa,
                Nome = x.Nome
            })
            .OrderBy(x => x.IdPessoa);

        var model = await PagingList.CreateAsync(result, 50, pageIndex);
        model.Action = "PagedIndex";
        return model;
    }

    // Paginação Detalhes
    public async Task<PagingList<DetalheIndexViewModel>> PagingDetalhes(int pageIndex = 1)
    {
        var result = _context.Detalhes
            .AsNoTracking()
            .Select(x => new DetalheIndexViewModel
            {
                IdDetalhe = x.IdDetalhe,
                DetalheTexto = x.DetalheTexto
            })
            .OrderBy(x => x.IdDetalhe);

        var model = await PagingList.CreateAsync(result, 50, pageIndex);
        model.Action = "PagedIndex";
        return model;
    }

    // Paginação Telefones
    public async Task<PagingList<TelefoneIndexlViewModel>> PagingTelefones(int pageIndex = 1)
    {
        var result = _context.Telefones
            .AsNoTracking()
            .Select(x => new TelefoneIndexlViewModel
            {
                IdTelefone = x.IdTelefone,
                TelefoneTexto = x.TelefoneTexto,
                Ativo = x.Ativo
            })
            .OrderBy(x => x.IdTelefone);

        var model = await PagingList.CreateAsync(result, 50, pageIndex);
        model.Action = "PagedIndex";
        return model;
    }

    public async Task<PagingList<PessoaTelefoneDetalheDetailViewModel>> PagingPessoasInnerJoinEF(int pageIndex = 1)
    {
        var result = (from p in _context.Pessoas
                join d in _context.Detalhes on p.IdPessoa equals d.IdPessoa
                join t in _context.Telefones on p.IdPessoa equals t.IdTelefone
                select new
                {
                    p.IdPessoa,
                    p.Nome,
                    t.TelefoneTexto,
                    t.Ativo,
                    d.DetalheTexto
                }).AsNoTracking()
            .Select(x => new PessoaTelefoneDetalheDetailViewModel
            {
                IdPessoa = x.IdPessoa,
                Nome = x.Nome,
                TelefoneTexto = x.TelefoneTexto,
                Ativo = x.Ativo,
                DetalheTexto = x.DetalheTexto
            })
            .OrderBy(x => x.IdPessoa);

        var model = await PagingList.CreateAsync(result, 50, pageIndex);
        model.Action = "AllDetails";
        return model;
    }
}