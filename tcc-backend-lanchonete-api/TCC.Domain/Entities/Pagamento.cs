using TCC.Domain.Enums;

namespace TCC.Domain.Entities
{
    public class Pagamento : Entity
    {
        public int PedidoId { get; set; }
        public StatusPagamento StatusAtual { get; set; }
        public decimal ValorTotalCobrado { get; set; }
        public string? MetodoPagamentoFinal { get; set; }
        public OrigemConfirmacao? OrigemConfirmacaoFinal { get; set; }

        public virtual Pedido Pedido { get; set; }
        public virtual ICollection<PagamentoTentativa> Tentativas { get; set; } = new List<PagamentoTentativa>();

        public Pagamento()
        {
            StatusAtual = StatusPagamento.PENDENTE;
        }
    }
}
