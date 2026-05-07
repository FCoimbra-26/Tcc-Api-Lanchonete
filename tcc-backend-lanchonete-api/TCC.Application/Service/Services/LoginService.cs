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

        public LoginService(
            ITokenService tokenService, 
            IUsuarioRepository usuarioRepository,
            IUsuarioRoleRepository usuarioRoleRepository)
        {
            _tokenService = tokenService;
            _usuarioRepository = usuarioRepository;
            _usuarioRoleRepository = usuarioRoleRepository;
        }

        public async Task<LoginUserResponse> logar(LoginUserRequest userLogin)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(userLogin.Email);

            if (usuario == null)
            {
                return new LoginUserResponse
                {
                    Success = false,
                    Error = "Email ou senha inválidos"
                };
            }

            if (!usuario.Ativo)
            {
                return new LoginUserResponse
                {
                    Success = false,
                    Error = "Usuário inativo"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(userLogin.Senha, usuario.SenhaHash))
            {
                return new LoginUserResponse
                {
                    Success = false,
                    Error = "Email ou senha inválidos"
                };
            }

            var roles = await _usuarioRoleRepository.GetActiveRolesByUsuarioIdAsync(usuario.Id);
            var primaryRole = roles.FirstOrDefault();

            var token = _tokenService.GenerateToken(usuario);

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
