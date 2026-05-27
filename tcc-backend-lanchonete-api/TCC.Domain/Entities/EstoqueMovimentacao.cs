using TCC.Domain.Enums;

namespace TCC.Domain.Entities
{
    public class EstoqueMovimentacao : Entity
    {
        public int EstoqueItemId { get; set; }
        public TipoMovimentacaoEstoque TipoMovimentacao { get; set; }
        public int Quantidade { get; set; }
        public int? PedidoId { get; set; }
        public int? UsuarioResponsavelId { get; set; }
        public string? Observacao { get; set; }

        public virtual EstoqueItem EstoqueItem { get; set; }
        public virtual Pedido? Pedido { get; set; }
        public virtual Usuario? UsuarioResponsavel { get; set; }
    }
}
