using TCC.Application.Models.Requests.Pagamento;
using TCC.Application.Models.Responses.Pagamento;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Entities;
using TCC.Domain.Enums;
using TCC.Domain.Interfaces;

namespace TCC.Application.Service.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPagamentoMockGateway _pagamentoMockGateway;

        public PagamentoService(IPagamentoRepository pagamentoRepository, IPagamentoMockGateway pagamentoMockGateway)
        {
            _pagamentoRepository = pagamentoRepository;
            _pagamentoMockGateway = pagamentoMockGateway;
        }

        public async Task<ConfirmarPagamentoResponse> SolicitarMockAsync(
            SolicitarPagamentoMockRequest request,
            int? usuarioLogadoId,
            IEnumerable<string> rolesUsuario)
        {
            try
            {
                var pedido = await _pagamentoRepository.GetPedidoByIdForPagamentoAsync(request.PedidoId);
                var validacao = ValidarFluxo(pedido, OrigemConfirmacao.MOCK_EXTERNO);
                if (validacao != null)
                    return validacao;

                var gatewayResult = await _pagamentoMockGateway.ProcessarAsync(new PagamentoMockGatewayRequest
                {
                    PedidoId = request.PedidoId,
                    MetodoPagamento = request.MetodoPagamento,
                    Aprovado = request.Aprovado,
                    SimularFalhaComunicacao = request.SimularFalhaComunicacao,
                    PayloadRetorno = request.PayloadRetorno
                });

                return await ProcessarConfirmacaoAsync(
                    pedido!,
                    request.MetodoPagamento,
                    OrigemConfirmacao.MOCK_EXTERNO,
                    usuarioLogadoId,
                    gatewayResult.Aprovado,
                    gatewayResult.FalhaComunicacao,
                    gatewayResult.PayloadRetorno);
            }
            catch (Exception ex)
            {
                return new ConfirmarPagamentoResponse
                {
                    Success = false,
                    Error = $"Erro ao confirmar pagamento: {ex.Message}"
                };
            }
        }

        public async Task<ConfirmarPagamentoResponse> ConfirmacaoLocalAsync(
            ConfirmacaoPagamentoLocalRequest request,
            int? usuarioLogadoId,
            IEnumerable<string> rolesUsuario)
        {
            try
            {
                var pedido = await _pagamentoRepository.GetPedidoByIdForPagamentoAsync(request.PedidoId);
                var validacao = ValidarFluxo(pedido, OrigemConfirmacao.CONFIRMACAO_LOCAL);
                if (validacao != null)
                    return validacao;

                if (!UsuarioPodeConfirmarLocal(rolesUsuario))
                {
                    return new ConfirmarPagamentoResponse
                    {
                        Success = false,
                        Error = "Confirmacao local permitida apenas para ATENDENTE ou GERENTE"
                    };
                }

                var payload = string.IsNullOrWhiteSpace(request.PayloadRetorno)
                    ? (request.Aprovado ? "{\"status\":\"APROVADO\"}" : "{\"status\":\"RECUSADO\"}")
                    : request.PayloadRetorno;

                return await ProcessarConfirmacaoAsync(
                    pedido!,
                    request.MetodoPagamento,
                    OrigemConfirmacao.CONFIRMACAO_LOCAL,
                    usuarioLogadoId,
                    request.Aprovado,
                    false,
                    payload);
            }
            catch (Exception ex)
            {
                return new ConfirmarPagamentoResponse
                {
                    Success = false,
                    Error = $"Erro ao confirmar pagamento local: {ex.Message}"
                };
            }
        }

        public async Task<ConfirmarPagamentoResponse> WebhookMockAsync(WebhookPagamentoMockRequest request)
        {
            try
            {
                var pedido = await _pagamentoRepository.GetPedidoByIdForPagamentoAsync(request.PedidoId);
                var validacao = ValidarFluxo(pedido, OrigemConfirmacao.MOCK_EXTERNO);
                if (validacao != null)
                    return validacao;

                var payload = string.IsNullOrWhiteSpace(request.PayloadRetorno)
                    ? (request.Aprovado ? "{\"status\":\"APROVADO\"}" : "{\"status\":\"RECUSADO\"}")
                    : request.PayloadRetorno;

                return await ProcessarConfirmacaoAsync(
                    pedido!,
                    request.MetodoPagamento,
                    OrigemConfirmacao.MOCK_EXTERNO,
                    null,
                    request.Aprovado,
                    false,
                    payload);
            }
            catch (Exception ex)
            {
                return new ConfirmarPagamentoResponse
                {
                    Success = false,
                    Error = $"Erro ao processar webhook de pagamento: {ex.Message}"
                };
            }
        }

        private async Task<ConfirmarPagamentoResponse> ProcessarConfirmacaoAsync(
            Pedido pedido,
            string metodoPagamento,
            OrigemConfirmacao origemConfirmacao,
            int? usuarioLogadoId,
            bool aprovado,
            bool falhaComunicacao,
            string payloadRetorno)
        {
            var pagamento = pedido.Pagamento ?? new Pagamento
            {
                PedidoId = pedido.Id,
                ValorTotalCobrado = pedido.ValorTotal,
                StatusAtual = StatusPagamento.PENDENTE
            };

            if (pedido.Pagamento == null)
            {
                pedido.Pagamento = pagamento;
            }

            var statusTentativa = falhaComunicacao
                ? StatusPagamento.PENDENTE
                : (aprovado ? StatusPagamento.APROVADO : StatusPagamento.RECUSADO);

            var sequenciaTentativa = (pagamento.Tentativas?.Count ?? 0) + 1;
            var nowUtc = DateTime.UtcNow;

            var tentativa = new PagamentoTentativa
            {
                Pagamento = pagamento,
                SequenciaTentativa = sequenciaTentativa,
                OrigemConfirmacao = origemConfirmacao,
                StatusTentativa = statusTentativa,
                ValorCobrado = pedido.ValorTotal,
                DataSolicitacao = nowUtc,
                DataRetorno = falhaComunicacao ? null : nowUtc,
                PayloadRetorno = payloadRetorno,
                UsuarioConfirmacaoId = origemConfirmacao == OrigemConfirmacao.CONFIRMACAO_LOCAL ? usuarioLogadoId : null,
                MetodoPagamento = metodoPagamento,
                Observacao = falhaComunicacao ? "Falha de comunicacao com o mock externo" : null,
                DataCriacao = nowUtc,
                DataAtualizacao = nowUtc
            };

            pagamento.DataAtualizacao = nowUtc;
            pedido.DataAtualizacao = nowUtc;

            if (!falhaComunicacao)
            {
                pagamento.StatusAtual = statusTentativa;
                pagamento.MetodoPagamentoFinal = metodoPagamento;
                pagamento.OrigemConfirmacaoFinal = origemConfirmacao;

                pedido.StatusPedido = statusTentativa == StatusPagamento.APROVADO
                    ? StatusPedido.PAGO
                    : StatusPedido.RECUSADO;
            }

            await _pagamentoRepository.RegistrarConfirmacaoAsync(tentativa);

            return new ConfirmarPagamentoResponse
            {
                Success = true,
                PedidoId = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                StatusPedido = pedido.StatusPedido,
                StatusPagamento = pagamento.StatusAtual,
                OrigemConfirmacao = origemConfirmacao,
                SequenciaTentativa = sequenciaTentativa,
                FalhaComunicacao = falhaComunicacao,
                DataProcessamento = nowUtc
            };
        }

        private static ConfirmarPagamentoResponse? ValidarFluxo(Pedido? pedido, OrigemConfirmacao origemEsperada)
        {
            if (pedido == null)
            {
                return new ConfirmarPagamentoResponse
                {
                    Success = false,
                    Error = "Pedido nao encontrado"
                };
            }

            if (pedido.StatusPedido != StatusPedido.AGUARDANDO_PAGAMENTO)
            {
                return new ConfirmarPagamentoResponse
                {
                    Success = false,
                    Error = "Somente pedidos em AGUARDANDO_PAGAMENTO podem confirmar pagamento"
                };
            }

            var origemPedido = GetOrigemByCanal(pedido.CanalPedido);
            if (origemPedido != origemEsperada)
            {
                return new ConfirmarPagamentoResponse
                {
                    Success = false,
                    Error = "Fluxo de confirmacao incompativel com o canal do pedido"
                };
            }

            return null;
        }

        private static OrigemConfirmacao GetOrigemByCanal(CanalAtendimento canal)
        {
            if (canal == CanalAtendimento.APP || canal == CanalAtendimento.WEB)
                return OrigemConfirmacao.MOCK_EXTERNO;

            return OrigemConfirmacao.CONFIRMACAO_LOCAL;
        }

        private static bool UsuarioPodeConfirmarLocal(IEnumerable<string> roles)
        {
            return roles.Any(r => r == "ATENDENTE" || r == "GERENTE");
        }
    }
}
