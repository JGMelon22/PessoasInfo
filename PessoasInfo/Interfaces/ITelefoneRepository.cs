using PessoasInfo.ViewModels.Telefone;

namespace PessoasInfo.Interfaces;

public interface ITelefoneRepository
{
    Task<TelefoneCreateViewModel> AddTelefone(TelefoneCreateViewModel telefoneCreateViewModel);
    Task<IEnumerable<TelefoneDetailViewModel>> GetTelefones();
    Task<TelefoneDetailViewModel> GetTelefone(int id);
    Task<TelefoneEditViewModel> UpdateTelefone(int id, TelefoneEditViewModel telefoneEditViewModel);
    Task DeleteTelefone(int id);
}