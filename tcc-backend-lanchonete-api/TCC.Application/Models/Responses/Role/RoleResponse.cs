using TCC.Application._Commom;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Responses.Role
{
    public class RoleResponse : ResponseBase
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioRole Role { get; set; }
        public string RoleDescricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAtribuicao { get; set; }
    }
}
