using TCC.Domain.Enums;

namespace TCC.Domain.Entities
{
    public class Unidade : Entity
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

        public int? EnderecoId { get; set; }
        public virtual Endereco? Endereco { get; set; }

        public virtual ICollection<UnidadeCanal> CanaisAtendimento { get; set; } = new List<UnidadeCanal>();

        public Unidade()
        {
            Ativo = true;
        }
    }
}
