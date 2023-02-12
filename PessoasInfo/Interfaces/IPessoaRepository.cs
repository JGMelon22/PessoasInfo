using PessoasInfo.ViewModels.Pessoa;

namespace PessoasInfo.Interfaces;

public interface IPessoaRepository
{
    Task<PessoaCreateViewModel> AddPessoa(PessoaCreateViewModel pessoaCreateViewModel);
    Task<IEnumerable<PessoaDetailViewModel>> GetPessoas();
    Task<PessoaDetailViewModel> GetPessoa(int id);
    Task<PessoaEditViewModel> UpdatePessoa(int id, PessoaDetailViewModel pessoaDetailViewModel);
    Task DeletePessoa(int id);
}