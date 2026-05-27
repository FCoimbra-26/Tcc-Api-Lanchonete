using System.ComponentModel.DataAnnotations;

namespace TCC.Application.Models.Requests.Cardapio
{
    public class AddProdutoCardapioRequest
    {
        [Required(ErrorMessage = "O ID do produto é obrigatório")]
        public int ProdutoId { get; set; }

        public decimal? PrecoPraticado { get; set; }
    }
}
