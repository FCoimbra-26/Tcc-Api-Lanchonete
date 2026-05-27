namespace TCC.Domain.Entities
{
    public class PedidoItem : Entity
    {
        public int PedidoId { get; set; }
        public int CardapioItemId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public string? ObservacaoItem { get; set; }

        public virtual Pedido Pedido { get; set; }
        public virtual CardapioItem CardapioItem { get; set; }
    }
}
