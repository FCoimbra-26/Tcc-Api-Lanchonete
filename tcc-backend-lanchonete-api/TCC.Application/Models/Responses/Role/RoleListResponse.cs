using TCC.Application._Commom;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Responses.Role
{
    public class RoleListResponse : ResponseBase
    {
        public List<RoleItemResponse> Roles { get; set; } = new();
    }

    public class RoleItemResponse
    {
        public UsuarioRole Role { get; set; }
        public string Descricao { get; set; }
    }

    public class UserRolesResponse : ResponseBase
    {
        public int UsuarioId { get; set; }
        public string UsuarioEmail { get; set; }
        public List<RoleItemResponse> Roles { get; set; } = new();
    }
}
