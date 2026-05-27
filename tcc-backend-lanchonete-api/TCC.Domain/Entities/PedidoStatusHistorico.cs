using TCC.Domain.Enums;

namespace TCC.Domain.Entities
{
    public class PedidoStatusHistorico : Entity
    {
        public int PedidoId { get; set; }
        public StatusPedido? StatusAnterior { get; set; }
        public StatusPedido StatusNovo { get; set; }
        public int? UsuarioResponsavelId { get; set; }
        public string? Observacao { get; set; }

        public virtual Pedido Pedido { get; set; }
        public virtual Usuario? UsuarioResponsavel { get; set; }
    }
}
