using TCC.Application.Models.Requests.Role;
using TCC.Application.Models.Responses.Role;
using TCC.Domain.Enums;

namespace TCC.Application.Service.Interfaces
{
    public interface IRoleService
    {
        Task<RoleListResponse> GetAllRolesAsync();
        Task<UserRolesResponse> GetUserRolesAsync(int usuarioId);
        Task<RoleResponse> AssignRoleToUserAsync(AssignRoleToUserRequest request);
        Task<RoleResponse> RemoveRoleFromUserAsync(int usuarioId, UsuarioRole role);
        Task<RoleResponse> ActivateUserRoleAsync(int usuarioId, UsuarioRole role);
        Task<RoleResponse> DeactivateUserRoleAsync(int usuarioId, UsuarioRole role);
    }
}
