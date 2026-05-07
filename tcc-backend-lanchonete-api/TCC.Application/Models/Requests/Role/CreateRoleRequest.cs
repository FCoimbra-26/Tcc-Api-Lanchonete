using System.ComponentModel.DataAnnotations;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Requests.Role
{
    public class AssignRoleToUserRequest
    {
        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "A role é obrigatória")]
        public UsuarioRole Role { get; set; }
    }
}
