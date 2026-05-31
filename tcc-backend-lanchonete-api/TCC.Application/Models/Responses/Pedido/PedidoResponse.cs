using TCC.Application._Commom;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Responses.Pedido
{
    public class PedidoResponse : ResponseBase
    {
        public int Id { get; set; }
        public string NumeroPedido { get; set; }
        public int UnidadeId { get; set; }
        public CanalAtendimento CanalPedido { get; set; }
        public StatusPedido StatusPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<PedidoItemResponse> Itens { get; set; } = new();
    }

    public class PedidoItemResponse
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
