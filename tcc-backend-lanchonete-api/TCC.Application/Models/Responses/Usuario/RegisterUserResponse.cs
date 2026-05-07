using TCC.Application._Commom;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Responses.Usuario
{
    public class RegisterUserResponse : ResponseBase
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public UsuarioRole Role { get; set; }
        public string Token { get; set; }
    }
}
