using TCC.Application._Commom;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Responses.Pedido
{
    public class PedidoListResponse : ResponseBase
    {
        public List<PedidoListItemResponse> Pedidos { get; set; } = new();
    }

    public class PedidoListItemResponse
    {
        public int Id { get; set; }
        public string NumeroPedido { get; set; }
        public int UnidadeId { get; set; }
        public CanalAtendimento CanalPedido { get; set; }
        public StatusPedido StatusPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataCriacao { get; set; }
        public int TotalItens { get; set; }
    }
}