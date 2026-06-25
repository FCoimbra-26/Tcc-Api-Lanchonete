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
        private readonly IAuditService _auditService;

        public RoleService(
            IUsuarioRoleRepository usuarioRoleRepository,
            IUsuarioRepository usuarioRepository,
            IAuditService auditService)
        {
            _usuarioRoleRepository = usuarioRoleRepository;
            _usuarioRepository = usuarioRepository;
            _auditService = auditService;
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
                        Error = "Usu�rio n�o encontrado"
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
                    Error = $"Erro ao buscar roles do usu�rio: {ex.Message}"
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
                    await _auditService.RegistrarAsync("ROLE_ATRIBUIR", "USUARIO_ROLE", false, null, null, request.UsuarioId, "Usuario nao encontrado");
                    return new RoleResponse
                    {
                        Success = false,
                        Error = "Usu�rio n�o encontrado"
                    };
                }

                var roleAssigned = await _usuarioRoleRepository.AssignRoleAsync(request.UsuarioId, request.Role);

                await _auditService.RegistrarAsync("ROLE_ATRIBUIR", "USUARIO_ROLE", true, request.UsuarioId, null, roleAssigned.Id, $"Role atribuida: {request.Role}");

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
                await _auditService.RegistrarAsync("ROLE_ATRIBUIR", "USUARIO_ROLE", false, request.UsuarioId, null, null, ex.Message);
                return new RoleResponse
                {
                    Success = false,
                    Error = $"Erro ao associar role ao usu�rio: {ex.Message}"
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
                    await _auditService.RegistrarAsync("ROLE_REMOVER", "USUARIO_ROLE", false, usuarioId, null, null, $"Role nao encontrada: {role}");
                    return new RoleResponse
                    {
                        Success = false,
                        Error = "Role n�o encontrada para este usu�rio"
                    };
                }

                await _auditService.RegistrarAsync("ROLE_REMOVER", "USUARIO_ROLE", true, usuarioId, null, null, $"Role removida: {role}");
                return new RoleResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                await _auditService.RegistrarAsync("ROLE_REMOVER", "USUARIO_ROLE", false, usuarioId, null, null, ex.Message);
                return new RoleResponse
                {
                    Success = false,
                    Error = $"Erro ao remover role do usu�rio: {ex.Message}"
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
                    await _auditService.RegistrarAsync("ROLE_ATIVAR", "USUARIO_ROLE", false, usuarioId, null, null, $"Role nao encontrada: {role}");
                    return new RoleResponse
                    {
                        Success = false,
                        Error = "Role n�o encontrada para este usu�rio"
                    };
                }

                await _auditService.RegistrarAsync("ROLE_ATIVAR", "USUARIO_ROLE", true, usuarioId, null, null, $"Role ativada: {role}");
                return new RoleResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                await _auditService.RegistrarAsync("ROLE_ATIVAR", "USUARIO_ROLE", false, usuarioId, null, null, ex.Message);
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
                    await _auditService.RegistrarAsync("ROLE_DESATIVAR", "USUARIO_ROLE", false, usuarioId, null, null, $"Role nao encontrada: {role}");
                    return new RoleResponse
                    {
                        Success = false,
                        Error = "Role n�o encontrada para este usu�rio"
                    };
                }

                await _auditService.RegistrarAsync("ROLE_DESATIVAR", "USUARIO_ROLE", true, usuarioId, null, null, $"Role desativada: {role}");
                return new RoleResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                await _auditService.RegistrarAsync("ROLE_DESATIVAR", "USUARIO_ROLE", false, usuarioId, null, null, ex.Message);
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
                UsuarioRole.ATENDENTE => "Atendente respons�vel pelo atendimento",
                UsuarioRole.COZINHA => "Equipe da cozinha respons�vel pelo preparo",
                UsuarioRole.GERENTE => "Gerente da unidade",
                UsuarioRole.ADMIN => "Administrador do sistema",
                _ => "Role desconhecida"
            };
        }
    }
}
