using ReflectionIT.Mvc.Paging;
using PessoasInfo.ViewModels.Pessoa;

namespace PessoasInfo.Interfaces;

public interface IPagingService
{
    Task<PagingList<PessoaIndexViewModel>> PagingPessoas(int pageIndex = 1);
}