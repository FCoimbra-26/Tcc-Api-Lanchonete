using TCC.Application.Models.Requests.Role;
using TCC.Application.Models.Responses.Role;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Enums;
using TCC.Domain.Interfaces;

namespace TCC.Application.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUsuarioRoleRepository _usuarioRoleRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public RoleService(
            IUsuarioRoleRepository usuarioRoleRepository,
            IUsuarioRepository usuarioRepository)
        {
            _usuarioRoleRepository = usuarioRoleRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<RoleListResponse> GetAllRolesAsync()
        {
            try
            {
                var roles = Enum.GetValues<UsuarioRole>();

                return new RoleListResponse
                {
                    Success = true,
                    Roles = roles.Select(r => new RoleItemResponse
                    {
                        Role = r,
                        Descricao = GetRoleDescricao(r)
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return new RoleListResponse
                {
                    Success = false,
                    Error = $"Erro ao listar roles: {ex.Message}"
                };
            }
        }

        public async Task<UserRolesResponse> GetUserRolesAsync(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                {
                    return new UserRolesResponse
                    {
                        Success = false,
                        Error = "Usuário não encontrado"
                    };
                }

                var activeRoles = await _usuarioRoleRepository.GetActiveRolesByUsuarioIdAsync(usuarioId);

                return new UserRolesResponse
                {
                    Success = true,
                    UsuarioId = usuarioId,
                    UsuarioEmail = usuario.Email,
                    Roles = activeRoles.Select(r => new RoleItemResponse
                    {
                        Role = r,
                        Descricao = GetRoleDescricao(r)
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return new UserRolesResponse
                {
                    Success = false,
                    Error = $"Erro ao buscar roles do usuário: {ex.Message}"
                };
            }
        }

        public async Task<RoleResponse> AssignRoleToUserAsync(AssignRoleToUserRequest request)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(request.UsuarioId);
                if (usuario == null)
                {
                    return new RoleResponse
                    {
                        Success = false,
                        Error = "Usuário não encontrado"
                    };
                }

                var roleAssigned = await _usuarioRoleRepository.AssignRoleAsync(request.UsuarioId, request.Role);

                return new RoleResponse
                {
                    Success = true,
                    Id = roleAssigned.Id,
                    UsuarioId = roleAssigned.UsuarioId,
                    Role = roleAssigned.Role,
                    RoleDescricao = GetRoleDescricao(roleAssigned.Role),
                    Ativo = roleAssigned.Ativo,
                    DataAtribuicao = roleAssigned.DataCriacao
                };
            }
            catch (Exception ex)
            {
                return new RoleResponse
                {
                    Success = false,
                    Error = $"Erro ao associar role ao usuário: {ex.Message}"
                };
            }
        }

        public async Task<RoleResponse> RemoveRoleFromUserAsync(int usuarioId, UsuarioRole role)
        {
            try
            {
                var removed = await _usuarioRoleRepository.RemoveRoleAsync(usuarioId, role);
                if (!removed)
                {
                    return new RoleResponse
                    {
                        Success = false,
                        Error = "Role não encontrada para este usuário"
                    };
                }

                return new RoleResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new RoleResponse
                {
                    Success = false,
                    Error = $"Erro ao remover role do usuário: {ex.Message}"
                };
            }
        }

        public async Task<RoleResponse> ActivateUserRoleAsync(int usuarioId, UsuarioRole role)
        {
            try
            {
                var activated = await _usuarioRoleRepository.ActivateRoleAsync(usuarioId, role);
                if (!activated)
                {
                    return new RoleResponse
                    {
                        Success = false,
                        Error = "Role não encontrada para este usuário"
                    };
                }

                return new RoleResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new RoleResponse
                {
                    Success = false,
                    Error = $"Erro ao ativar role: {ex.Message}"
                };
            }
        }

        public async Task<RoleResponse> DeactivateUserRoleAsync(int usuarioId, UsuarioRole role)
        {
            try
            {
                var deactivated = await _usuarioRoleRepository.DeactivateRoleAsync(usuarioId, role);
                if (!deactivated)
                {
                    return new RoleResponse
                    {
                        Success = false,
                        Error = "Role não encontrada para este usuário"
                    };
                }

                return new RoleResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new RoleResponse
                {
                    Success = false,
                    Error = $"Erro ao desativar role: {ex.Message}"
                };
            }
        }

        private string GetRoleDescricao(UsuarioRole role)
        {
            return role switch
            {
                UsuarioRole.CLIENTE => "Cliente do estabelecimento",
                UsuarioRole.ATENDENTE => "Atendente responsável pelo atendimento",
                UsuarioRole.COZINHA => "Equipe da cozinha responsável pelo preparo",
                UsuarioRole.GERENTE => "Gerente da unidade",
                UsuarioRole.ADMIN => "Administrador do sistema",
                _ => "Role desconhecida"
            };
        }
    }
}
