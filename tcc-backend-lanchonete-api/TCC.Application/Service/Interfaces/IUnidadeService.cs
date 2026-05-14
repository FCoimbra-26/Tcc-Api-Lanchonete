using TCC.Application.Models.Requests.Unidade;
using TCC.Application.Models.Responses.Unidade;
using TCC.Domain.Enums;

namespace TCC.Application.Service.Interfaces
{
    public interface IUnidadeService
    {
        Task<UnidadeResponse> CreateAsync(CreateUnidadeRequest request);
        Task<UnidadeResponse> UpdateAsync(int id, UpdateUnidadeRequest request);
        Task<UnidadeResponse> GetByIdAsync(int id);
        Task<UnidadeResponse> GetByCodigoAsync(string codigo);
        Task<UnidadeListResponse> GetAllAsync(bool? apenasAtivas = null);
        Task<UnidadeResponse> DeleteAsync(int id);
        Task<UnidadeResponse> ActivateAsync(int id);
        Task<UnidadeResponse> DeactivateAsync(int id);
        Task<UnidadeResponse> AddCanalAsync(int id, CanalAtendimento canal);
        Task<UnidadeResponse> RemoveCanalAsync(int id, CanalAtendimento canal);
    }
}
