using TCC.Domain.Enums;

namespace TCC.Domain.Entities
{
    public class UnidadeCanal : Entity
    {
        public int UnidadeId { get; set; }
        public CanalAtendimento Canal { get; set; }
        public bool Ativo { get; set; }

        public virtual Unidade Unidade { get; set; }

        public UnidadeCanal()
        {
            Ativo = true;
        }
    }
}
