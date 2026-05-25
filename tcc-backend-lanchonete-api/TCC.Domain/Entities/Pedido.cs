using TCC.Domain.Enums;

namespace TCC.Domain.Entities
{
    public class Pedido : Entity
    {
        public string NumeroPedido { get; set; }
        public int UnidadeId { get; set; }
        public CanalAtendimento CanalPedido { get; set; }
        public StatusPedido StatusPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public int? ClienteId { get; set; }
        public string? Observacao { get; set; }

        public virtual Unidade Unidade { get; set; }
        public virtual Usuario? Cliente { get; set; }
        public virtual ICollection<PedidoItem> Itens { get; set; } = new List<PedidoItem>();
        public virtual Pagamento? Pagamento { get; set; }
        public virtual ICollection<PedidoStatusHistorico> Historicos { get; set; } = new List<PedidoStatusHistorico>();
        public virtual ICollection<EstoqueMovimentacao> EstoqueMovimentacoes { get; set; } = new List<EstoqueMovimentacao>();

        public Pedido()
        {
            StatusPedido = StatusPedido.AGUARDANDO_PAGAMENTO;
            ValorTotal = 0;
        }
    }
}
