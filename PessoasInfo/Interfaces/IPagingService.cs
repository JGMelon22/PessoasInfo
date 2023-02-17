using PessoasInfo.ViewModels.Detalhe;
using PessoasInfo.ViewModels.Pessoa;
using ReflectionIT.Mvc.Paging;

namespace PessoasInfo.Interfaces;

public interface IPagingService
{
    Task<PagingList<PessoaIndexViewModel>> PagingPessoas(int pageIndex = 1);
    Task<PagingList<DetalheIndexViewModel>> PagingDetalhes(int pageIndex);
}