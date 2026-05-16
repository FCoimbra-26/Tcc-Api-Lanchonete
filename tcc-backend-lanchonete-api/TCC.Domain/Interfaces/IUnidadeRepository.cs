using TCC.Domain.Entities;
using TCC.Domain.Enums;

namespace TCC.Domain.Interfaces
{
    public interface IUnidadeRepository
    {
        Task<Unidade?> GetByIdAsync(int id);
        Task<Unidade?> GetByCodigoAsync(string codigo);
        Task<IEnumerable<Unidade>> GetAllAsync(bool? apenasAtivas = null);
        Task<Unidade> CreateAsync(Unidade unidade);
        Task<Unidade> UpdateAsync(Unidade unidade);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByCodigoAsync(string codigo);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<bool> AddCanalAsync(int unidadeId, CanalAtendimento canal);
        Task<bool> RemoveCanalAsync(int unidadeId, CanalAtendimento canal);
        Task<IEnumerable<CanalAtendimento>> GetCanaisAtivosAsync(int unidadeId);
    }
}
