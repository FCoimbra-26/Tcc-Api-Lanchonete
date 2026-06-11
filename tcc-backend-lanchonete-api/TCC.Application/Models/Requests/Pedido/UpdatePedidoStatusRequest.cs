using System.ComponentModel.DataAnnotations;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Requests.Pedido
{
    public class UpdatePedidoStatusRequest
    {
        [Required(ErrorMessage = "O novo status e obrigatorio")]
        public StatusPedido NovoStatus { get; set; }

        [MaxLength(500, ErrorMessage = "A observacao deve ter no maximo 500 caracteres")]
        public string? Observacao { get; set; }
    }
}