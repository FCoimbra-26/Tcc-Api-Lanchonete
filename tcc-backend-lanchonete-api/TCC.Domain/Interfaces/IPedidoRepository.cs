using TCC.Domain.Entities;

namespace TCC.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido> CreateWithStockAsync(
            Pedido pedido,
            IEnumerable<EstoqueItem> estoqueItensAtualizados,
            IEnumerable<EstoqueMovimentacao> movimentacoesEstoque);
    }
}
