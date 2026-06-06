namespace TCC.Application.Service.Interfaces
{
    public interface IPagamentoMockGateway
    {
        Task<PagamentoMockGatewayResult> ProcessarAsync(PagamentoMockGatewayRequest request);
    }

    public class PagamentoMockGatewayRequest
    {
        public int PedidoId { get; set; }
        public string MetodoPagamento { get; set; } = string.Empty;
        public bool Aprovado { get; set; }
        public bool SimularFalhaComunicacao { get; set; }
        public string? PayloadRetorno { get; set; }
    }

    public class PagamentoMockGatewayResult
    {
        public bool FalhaComunicacao { get; set; }
        public bool Aprovado { get; set; }
        public string PayloadRetorno { get; set; } = string.Empty;
    }
}
