using TCC.Domain.Entities;

namespace TCC.Domain.Interfaces
{
    public interface IPagamentoRepository
    {
        Task<Pedido?> GetPedidoByIdForPagamentoAsync(int pedidoId);
        Task RegistrarConfirmacaoAsync(PagamentoTentativa tentativa);
    }
}
