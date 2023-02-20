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
    public async Task<PagingList<PessoaIndexViewModel>> PagingPessoas(string searchString, string sortOrder,
        int pageIndex = 1)
    {
        var result = _context.Pessoas
            .AsNoTracking()
            .Select(x => new PessoaIndexViewModel
            {
                IdPessoa = x.IdPessoa,
                Nome = x.Nome
            })
            .OrderBy(x => x.IdPessoa);

        // Verifica searchString
        // ReflectionIT.Mvc.Paging não aceita parametro IOrderedQueryable, precisa de um cast 
        if (!string.IsNullOrEmpty(searchString))
            result = (IOrderedQueryable<PessoaIndexViewModel>)result.Where(x => x.Nome.Contains(searchString));

        // Sorting
        switch (sortOrder)
        {
            case "nome_desc":
                result = result.OrderByDescending(x => x.Nome);
                break;
            case "id_desc":
                result = result.OrderByDescending(x => x.IdPessoa);
                break;

            default:
                result = result.OrderBy(x => x.IdPessoa);
                break;
        }

        var model = await PagingList.CreateAsync(result, 50, pageIndex);
        model.Action = "PagedIndex";
        return model;
    }

    // Paginação Detalhes
    public async Task<PagingList<DetalheIndexViewModel>> PagingDetalhes(string searchString, string sortOrder,
        int pageIndex = 1)
    {
        var result = _context.Detalhes
            .AsNoTracking()
            .Select(x => new DetalheIndexViewModel
            {
                IdDetalhe = x.IdDetalhe,
                DetalheTexto = x.DetalheTexto
            })
            .OrderBy(x => x.IdDetalhe);

        // Verfica searchString
        // ReflectionIT.Mvc.Paging não aceita parametro IOrderedQueryable, precisa de um cast
        if (!string.IsNullOrEmpty(searchString))
            result = (IOrderedQueryable<DetalheIndexViewModel>)result.Where(x => x.DetalheTexto.Contains(searchString));

        // Sorting
        switch (sortOrder)
        {
            case "detalhe_desc":
                result = result.OrderByDescending(x => x.DetalheTexto);
                break;

            case "id_desc":
                result = result.OrderByDescending(x => x.IdDetalhe);
                break;

            default:
                result = result.OrderBy(x => x.IdDetalhe);
                break;
        }

        var model = await PagingList.CreateAsync(result, 50, pageIndex);
        model.Action = "PagedIndex";
        return model;
    }

    // Paginação Telefones
    public async Task<PagingList<TelefoneIndexlViewModel>> PagingTelefones(string searchString, string sortOrder,
        int pageIndex = 1)
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

        // Verifica searchString
        // ReflectionIT.Mvc.Paging não aceita parametro IOrderedQueryable, precisa de um cast
        if (!string.IsNullOrEmpty(searchString))
            result = (IOrderedQueryable<TelefoneIndexlViewModel>)result.Where(x =>
                x.TelefoneTexto.Contains(searchString));

        // Sorting
        switch (sortOrder)
        {
            case "telefone_desc":
                result = result.OrderByDescending(x => x.TelefoneTexto);
                break;

            case "ativo_desc":
                result = result.OrderByDescending(x => x.Ativo);
                break;

            case "id_desc":
                result = result.OrderByDescending(x => x.IdTelefone);
                break;

            default:
                result = result.OrderBy(x => x.IdTelefone);
                break;
        }


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