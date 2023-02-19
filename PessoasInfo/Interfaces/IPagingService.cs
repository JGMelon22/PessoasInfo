using PessoasInfo.ViewModels.Detalhe;
using PessoasInfo.ViewModels.Pessoa;
using PessoasInfo.ViewModels.Telefone;
using ReflectionIT.Mvc.Paging;

namespace PessoasInfo.Interfaces;

public interface IPagingService
{
    Task<PagingList<PessoaIndexViewModel>> PagingPessoas(string searchString, string sortOder, int pageIndex = 1);
    Task<PagingList<DetalheIndexViewModel>> PagingDetalhes(string searchString, string sortOrder, int pageIndex = 1);
    Task<PagingList<TelefoneIndexlViewModel>> PagingTelefones(int pageIndex = 1);
    Task<PagingList<PessoaTelefoneDetalheDetailViewModel>> PagingPessoasInnerJoinEF(int pageIndex = 1);
}