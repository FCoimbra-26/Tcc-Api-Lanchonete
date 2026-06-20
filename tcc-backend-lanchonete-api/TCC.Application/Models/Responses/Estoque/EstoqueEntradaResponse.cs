using TCC.Application._Commom;

namespace TCC.Application.Models.Responses.Estoque
{
    public class EstoqueEntradaResponse : ResponseBase
    {
        public int EstoqueItemId { get; set; }
        public int UnidadeId { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public int QuantidadeAdicionada { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}