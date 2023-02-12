using PessoasInfo.ViewModels.Detalhe;

namespace PessoasInfo.Interfaces;

public interface IDetalheRepository
{
    Task<DetalheCreateViewModel> AddDetalhe(DetalheCreateViewModel detalheCreateViewModel);
    Task<IEnumerable<DetalheDetailViewModel>> GetDetalhes();
    Task<DetalheDetailViewModel> GetDetalhe(int id);
    Task<DetalheEditViewModel> UpdateDetalhe(int id, DetalheEditViewModel detalheEditViewModel);
    Task DeleteDetalhe(int id);
}