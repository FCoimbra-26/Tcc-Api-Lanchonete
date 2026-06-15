using TCC.Domain.Entities;
using TCC.Domain.Enums;

namespace TCC.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetByIdAsync(int pedidoId);
        Task<IEnumerable<Pedido>> GetAllAsync(CanalAtendimento? canalPedido = null);

        Task<Pedido> CreateWithStockAsync(
            Pedido pedido,
            IEnumerable<EstoqueItem> estoqueItensAtualizados,
            IEnumerable<EstoqueMovimentacao> movimentacoesEstoque);

        Task<Pedido> UpdateStatusAsync(Pedido pedido, PedidoStatusHistorico historico);

        Task<Pedido> CancelWithStockReversalAsync(
            Pedido pedido,
            PedidoStatusHistorico historico,
            IEnumerable<EstoqueItem> estoqueItensAtualizados,
            IEnumerable<EstoqueMovimentacao> movimentacoesEstoque);
    }
}
