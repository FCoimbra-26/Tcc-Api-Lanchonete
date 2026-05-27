namespace TCC.Domain.Entities
{
    public class ProdutoGlobal : Entity
    {
        public string NomeProduto { get; set; }
        public decimal PrecoBase { get; set; }
        public bool Ativo { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        public string? ImagemUrl { get; set; }

        public virtual ICollection<CardapioItem> CardapioItens { get; set; } = new List<CardapioItem>();
        public virtual ICollection<EstoqueItem> EstoqueItens { get; set; } = new List<EstoqueItem>();

        public ProdutoGlobal()
        {
            Ativo = true;
        }
    }
}
