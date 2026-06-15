using TCC.Application.Models.Requests.Pedido;
using TCC.Application.Models.Responses.Pedido;
using TCC.Domain.Enums;

namespace TCC.Application.Service.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponse> CreateAsync(CreatePedidoRequest request, int? usuarioLogadoId = null);
        Task<PedidoResponse> UpdateStatusAsync(int pedidoId, UpdatePedidoStatusRequest request, int? usuarioLogadoId = null);
        Task<PedidoResponse> CancelAsync(int pedidoId, CancelPedidoRequest request, int? usuarioLogadoId = null, IEnumerable<string>? rolesUsuario = null);
        Task<PedidoListResponse> GetAllAsync(CanalAtendimento? canalPedido = null);
    }
}
