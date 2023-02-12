using PessoasInfo.ViewModels.Detalhe;

namespace PessoasInfo.Interfaces;

public interface IDetalheRepository
{
    Task<DetalheCreateViewModel> AddDetalhe(DetalheCreateViewModel detalheCreateViewModel);
    Task<IEnumerable<DetalheIndexViewModel>> GetDetalhes();
    Task<DetalheIndexViewModel> GetDetalhe(int id);
    Task<DetalheEditViewModel> UpdateDetalhe(int id, DetalheEditViewModel detalheEditViewModel);
    Task DeleteDetalhe(int id);
}