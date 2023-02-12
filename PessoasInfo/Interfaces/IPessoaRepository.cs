using PessoasInfo.ViewModels.Pessoa;

namespace PessoasInfo.Interfaces;

public interface IPessoaRepository
{
    Task<PessoaCreateViewModel> AddPessoa(PessoaCreateViewModel pessoaCreateViewModel);
    Task<IEnumerable<PessoaDetailViewModel>> GetPessoas();
    Task<PessoaDetailViewModel> GetPessoa(int id);
    Task<PessoaEditViewModel> UpdatePessoa(int id, PessoaEditViewModel pessoaEditViewModel);
    Task DeletePessoa(int id);
}