namespace TCC.Domain.Entities
{
    public class EstoqueItem : Entity
    {
        public int UnidadeId { get; set; }
        public int ProdutoId { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public bool Ativo { get; set; }
        public int? EstoqueMinimo { get; set; }

        public virtual Unidade Unidade { get; set; }
        public virtual ProdutoGlobal Produto { get; set; }
        public virtual ICollection<EstoqueMovimentacao> Movimentacoes { get; set; } = new List<EstoqueMovimentacao>();

        public EstoqueItem()
        {
            Ativo = true;
            QuantidadeDisponivel = 0;
        }
    }
}
