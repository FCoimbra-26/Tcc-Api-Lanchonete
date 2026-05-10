using TCC.Domain.Entities;
using TCC.Domain.Enums;

namespace TCC.Domain.Interfaces
{
    public interface IUsuarioRoleRepository
    {
        Task<IEnumerable<UsuarioRoleHistorico>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<UsuarioRole>> GetActiveRolesByUsuarioIdAsync(int usuarioId);
        Task<UsuarioRoleHistorico> AssignRoleAsync(int usuarioId, UsuarioRole role);
        Task<bool> RemoveRoleAsync(int usuarioId, UsuarioRole role);
        Task<bool> HasRoleAsync(int usuarioId, UsuarioRole role);
        Task<bool> ActivateRoleAsync(int usuarioId, UsuarioRole role);
        Task<bool> DeactivateRoleAsync(int usuarioId, UsuarioRole role);
    }
}
