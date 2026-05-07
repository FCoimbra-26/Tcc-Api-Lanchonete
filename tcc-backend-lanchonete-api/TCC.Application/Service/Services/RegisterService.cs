using TCC.Application.Models.Requests.Usuario;
using TCC.Application.Models.Responses.Usuario;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Entities;
using TCC.Domain.Enums;
using TCC.Domain.Interfaces;
using TCC.Infra.Security.Interfaces;

namespace TCC.Application.Service.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioRoleRepository _usuarioRoleRepository;
        private readonly ITokenService _tokenService;

        public RegisterService(
            IUsuarioRepository usuarioRepository, 
            IUsuarioRoleRepository usuarioRoleRepository,
            ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioRoleRepository = usuarioRoleRepository;
            _tokenService = tokenService;
        }

        public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request)
        {
            try
            {
                if (await _usuarioRepository.ExistsByEmailAsync(request.Email))
                {
                    return new RegisterUserResponse
                    {
                        Success = false,
                        Error = "Email já cadastrado"
                    };
                }

                if (!string.IsNullOrWhiteSpace(request.Cpf) && 
                    await _usuarioRepository.ExistsByCpfAsync(request.Cpf))
                {
                    return new RegisterUserResponse
                    {
                        Success = false,
                        Error = "CPF já cadastrado"
                    };
                }

                var pessoa = new Pessoa
                {
                    Nome = request.Nome,
                    Sobrenome = request.Sobrenome,
                    Cpf = request.Cpf,
                    Telefone = request.Telefone,
                    DataNascimento = request.DataNascimento ?? DateTime.MinValue
                };

                if (request.Endereco != null)
                {
                    pessoa.Endereco = new Endereco
                    {
                        Logradouro = request.Endereco.Logradouro,
                        Numero = request.Endereco.Numero,
                        Complemento = request.Endereco.Complemento,
                        Bairro = request.Endereco.Bairro,
                        Cidade = request.Endereco.Cidade,
                        Uf = request.Endereco.Uf.ToUpper(),
                        Cep = request.Endereco.Cep
                    };
                }

                var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

                var usuario = new Usuario
                {
                    Email = request.Email,
                    EmailNormalizado = request.Email.ToUpper(),
                    SenhaHash = senhaHash,
                    Pessoa = pessoa,
                    Ativo = true
                };

                var usuarioCriado = await _usuarioRepository.CreateAsync(usuario);

                await _usuarioRoleRepository.AssignRoleAsync(usuarioCriado.Id, UsuarioRole.CLIENTE);

                var token = _tokenService.GenerateToken(usuarioCriado);

                return new RegisterUserResponse
                {
                    Success = true,
                    Id = usuarioCriado.Id,
                    Nome = $"{usuarioCriado.Pessoa.Nome} {usuarioCriado.Pessoa.Sobrenome}",
                    Email = usuarioCriado.Email,
                    Role = UsuarioRole.CLIENTE,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                return new RegisterUserResponse
                {
                    Success = false,
                    Error = $"Erro ao cadastrar usuário: {ex.Message}"
                };
            }
        }
    }
}
