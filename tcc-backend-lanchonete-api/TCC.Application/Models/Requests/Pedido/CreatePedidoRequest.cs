using System.ComponentModel.DataAnnotations;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Requests.Pedido
{
    public class CreatePedidoRequest
    {
        [Required(ErrorMessage = "A unidade e obrigatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Unidade invalida")]
        public int UnidadeId { get; set; }

        [Required(ErrorMessage = "O canal do pedido e obrigatorio")]
        public CanalAtendimento CanalPedido { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Cliente invalido")]
        public int? ClienteId { get; set; }

        [MaxLength(500, ErrorMessage = "A observacao deve ter no maximo 500 caracteres")]
        public string? Observacao { get; set; }

        [Required(ErrorMessage = "Os itens do pedido sao obrigatorios")]
        [MinLength(1, ErrorMessage = "O pedido deve conter ao menos um item")]
        public List<CreatePedidoItemRequest> Itens { get; set; } = new();
    }

    public class CreatePedidoItemRequest
    {
        [Required(ErrorMessage = "O produto e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Produto invalido")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "A quantidade e obrigatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }

        [MaxLength(500, ErrorMessage = "A observacao do item deve ter no maximo 500 caracteres")]
        public string? ObservacaoItem { get; set; }
    }
}
