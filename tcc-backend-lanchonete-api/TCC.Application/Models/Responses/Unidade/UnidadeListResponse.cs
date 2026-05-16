using TCC.Application._Commom;

namespace TCC.Application.Models.Responses.Unidade
{
    public class UnidadeListResponse : ResponseBase
    {
        public List<UnidadeItemResponse> Unidades { get; set; } = new();
    }

    public class UnidadeItemResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public string? Cidade { get; set; }
        public int TotalCanais { get; set; }
    }
}
