using TCC.Application._Commom;

namespace TCC.Application.Models.Responses.Cardapio
{
    public class CardapioResponse : ResponseBase
    {
        public int UnidadeId { get; set; }
        public string UnidadeNome { get; set; }
        public string UnidadeCodigo { get; set; }
        public List<CardapioItemResponse> Itens { get; set; } = new();
    }

    public class CardapioItemResponse
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        public string? ImagemUrl { get; set; }
        public decimal PrecoBase { get; set; }
        public decimal? PrecoPraticado { get; set; }
        public decimal PrecoFinal => PrecoPraticado ?? PrecoBase;
        public bool ProdutoAtivo { get; set; }
        public int? QuantidadeEstoque { get; set; }
        public bool Disponivel { get; set; }
    }
}
