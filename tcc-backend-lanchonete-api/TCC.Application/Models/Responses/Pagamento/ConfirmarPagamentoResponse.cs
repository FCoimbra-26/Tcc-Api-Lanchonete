using TCC.Application._Commom;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Responses.Pagamento
{
    public class ConfirmarPagamentoResponse : ResponseBase
    {
        public int PedidoId { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public StatusPedido StatusPedido { get; set; }
        public StatusPagamento StatusPagamento { get; set; }
        public OrigemConfirmacao? OrigemConfirmacao { get; set; }
        public int SequenciaTentativa { get; set; }
        public bool FalhaComunicacao { get; set; }
        public DateTime DataProcessamento { get; set; }
    }
}
