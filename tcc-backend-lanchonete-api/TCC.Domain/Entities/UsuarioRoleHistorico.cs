using TCC.Domain.Enums;

namespace TCC.Domain.Entities
{
    public class UsuarioRoleHistorico : Entity
    {
        public int UsuarioId { get; set; }
        public UsuarioRole Role { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario Usuario { get; set; }

        public UsuarioRoleHistorico()
        {
            Ativo = true;
        }
    }
}
