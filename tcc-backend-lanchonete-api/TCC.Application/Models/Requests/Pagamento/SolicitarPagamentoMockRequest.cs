using System.ComponentModel.DataAnnotations;

namespace TCC.Application.Models.Requests.Pagamento
{
    public class SolicitarPagamentoMockRequest
    {
        [Required(ErrorMessage = "O pedido e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Pedido invalido")]
        public int PedidoId { get; set; }

        [Required(ErrorMessage = "O metodo de pagamento e obrigatorio")]
        [MaxLength(100, ErrorMessage = "Metodo de pagamento deve ter no maximo 100 caracteres")]
        public string MetodoPagamento { get; set; } = string.Empty;

        public bool Aprovado { get; set; } = true;

        public bool SimularFalhaComunicacao { get; set; } = false;

        public string? PayloadRetorno { get; set; }
    }
}
