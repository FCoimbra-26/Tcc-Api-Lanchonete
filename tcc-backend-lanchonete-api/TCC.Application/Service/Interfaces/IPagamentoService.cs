using TCC.Application.Models.Requests.Pagamento;
using TCC.Application.Models.Responses.Pagamento;

namespace TCC.Application.Service.Interfaces
{
    public interface IPagamentoService
    {
        Task<ConfirmarPagamentoResponse> SolicitarMockAsync(
            SolicitarPagamentoMockRequest request,
            int? usuarioLogadoId,
            IEnumerable<string> rolesUsuario);

        Task<ConfirmarPagamentoResponse> ConfirmacaoLocalAsync(
            ConfirmacaoPagamentoLocalRequest request,
            int? usuarioLogadoId,
            IEnumerable<string> rolesUsuario);

        Task<ConfirmarPagamentoResponse> WebhookMockAsync(
            WebhookPagamentoMockRequest request);
    }
}
