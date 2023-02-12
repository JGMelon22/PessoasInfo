using PessoasInfo.ViewModels.Telefone;

namespace PessoasInfo.Interfaces;

public interface ITelefoneRepository
{
    Task<TelefoneCreateViewModel> AddTelefone(TelefoneCreateViewModel telefoneCreateViewModel);
    Task<IEnumerable<TelefoneIndexlViewModel>> GetTelefones();
    Task<TelefoneIndexlViewModel> GetTelefone(int id);
    Task<TelefoneEditViewModel> UpdateTelefone(int id, TelefoneEditViewModel telefoneEditViewModel);
    Task DeleteTelefone(int id);
}