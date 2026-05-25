namespace TCC.Domain.Entities
{
    public class CardapioItem : Entity
    {
        public int UnidadeId { get; set; }
        public int ProdutoId { get; set; }
        public decimal? PrecoPraticado { get; set; }

        public virtual Unidade Unidade { get; set; }
        public virtual ProdutoGlobal Produto { get; set; }
        public virtual ICollection<PedidoItem> PedidoItens { get; set; } = new List<PedidoItem>();

        // Propriedade calculada: produto ativo + estoque > 0
        public bool DisponivelNaUnidade => Produto?.Ativo == true;
    }
}
