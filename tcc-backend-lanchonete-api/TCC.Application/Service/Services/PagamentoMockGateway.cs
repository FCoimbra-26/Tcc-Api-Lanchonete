using TCC.Application.Service.Interfaces;

namespace TCC.Application.Service.Services
{
    public class PagamentoMockGateway : IPagamentoMockGateway
    {
        public Task<PagamentoMockGatewayResult> ProcessarAsync(PagamentoMockGatewayRequest request)
        {
            var falha = request.SimularFalhaComunicacao;

            if (falha)
            {
                return Task.FromResult(new PagamentoMockGatewayResult
                {
                    FalhaComunicacao = true,
                    Aprovado = false,
                    PayloadRetorno = string.IsNullOrWhiteSpace(request.PayloadRetorno)
                        ? "{\"status\":\"FALHA_COMUNICACAO\"}"
                        : request.PayloadRetorno
                });
            }

            return Task.FromResult(new PagamentoMockGatewayResult
            {
                FalhaComunicacao = false,
                Aprovado = request.Aprovado,
                PayloadRetorno = string.IsNullOrWhiteSpace(request.PayloadRetorno)
                    ? (request.Aprovado ? "{\"status\":\"APROVADO\"}" : "{\"status\":\"RECUSADO\"}")
                    : request.PayloadRetorno
            });
        }
    }
}
