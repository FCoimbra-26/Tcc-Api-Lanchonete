using TCC.Application.Models.Requests.Pedido;
using TCC.Application.Models.Responses.Pedido;

namespace TCC.Application.Service.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponse> CreateAsync(CreatePedidoRequest request, int? usuarioLogadoId = null);
    }
}
