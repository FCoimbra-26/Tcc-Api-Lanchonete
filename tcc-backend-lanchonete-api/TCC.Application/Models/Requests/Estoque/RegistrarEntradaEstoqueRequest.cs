using System.ComponentModel.DataAnnotations;

namespace TCC.Application.Models.Requests.Estoque
{
    public class RegistrarEntradaEstoqueRequest
    {
        [Range(1, int.MaxValue)]
        public int UnidadeId { get; set; }

        [Range(1, int.MaxValue)]
        public int ProdutoId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantidade { get; set; }

        public string? Observacao { get; set; }
    }
}