using System;
using TCC.Application.Models.Requests.Usuario;
using TCC.Application.Models.Responses.Usuario;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Interfaces;
using TCC.Infra.Security.Interfaces;

namespace TCC.Application.Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly ITokenService _tokenService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioRoleRepository _usuarioRoleRepository;
        private readonly IAuditService _auditService;

        public LoginService(
            ITokenService tokenService, 
            IUsuarioRepository usuarioRepository,
            IUsuarioRoleRepository usuarioRoleRepository,
            IAuditService auditService)
        {
            _tokenService = tokenService;
            _usuarioRepository = usuarioRepository;
            _usuarioRoleRepository = usuarioRoleRepository;
            _auditService = auditService;
        }

        public async Task<LoginUserResponse> logar(LoginUserRequest userLogin)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(userLogin.Email);

            if (usuario == null)
            {
                await _auditService.RegistrarAsync("LOGIN", "USUARIO", false, null, null, null, "Email nao encontrado");
                return new LoginUserResponse
                {
                    Success = false,
                    Error = "Email ou senha inválidos"
                };
            }

            if (!usuario.Ativo)
            {
                await _auditService.RegistrarAsync("LOGIN", "USUARIO", false, usuario.Id, null, usuario.Id, "Usuario inativo");
                return new LoginUserResponse
                {
                    Success = false,
                    Error = "Usuário inativo"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(userLogin.Senha, usuario.SenhaHash))
            {
                await _auditService.RegistrarAsync("LOGIN", "USUARIO", false, usuario.Id, null, usuario.Id, "Senha invalida");
                return new LoginUserResponse
                {
                    Success = false,
                    Error = "Email ou senha inválidos"
                };
            }

            var roles = await _usuarioRoleRepository.GetActiveRolesByUsuarioIdAsync(usuario.Id);
            var primaryRole = roles.FirstOrDefault();

            var token = _tokenService.GenerateToken(usuario);

            await _auditService.RegistrarAsync("LOGIN", "USUARIO", true, usuario.Id, null, usuario.Id, "Login realizado com sucesso");

            return new LoginUserResponse
            {
                Success = true,
                Id = usuario.Id,
                Tipo = primaryRole,
                Token = token
            };
        }
    }
}
