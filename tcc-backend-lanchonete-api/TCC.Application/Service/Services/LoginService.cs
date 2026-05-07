using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Application.Models.Requests.Usuario;
using TCC.Application.Models.Responses.Usuario;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Entities;
using TCC.Domain.Enums;
using TCC.Infra.Security.Interfaces;

namespace TCC.Application.Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly ITokenService _tokenService;
        public LoginService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public Task<LoginUserResponse> logar(LoginUserRequest userLogin)
        {
            Usuario usuario = new Usuario
            {
                email = userLogin.Email,
                senha = userLogin.Senha,
                role = UsuarioRole.ADMIN
            };
            this._tokenService.GenerateToken(usuario);

            return Task.FromResult(new LoginUserResponse
            {
                Id = 1,
                Tipo = UsuarioRole.ADMIN,
                Token = this._tokenService.GenerateToken(usuario)

            });
        }
    }
}
