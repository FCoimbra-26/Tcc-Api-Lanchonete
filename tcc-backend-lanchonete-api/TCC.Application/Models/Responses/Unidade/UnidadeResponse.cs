using TCC.Application._Commom;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Responses.Unidade
{
    public class UnidadeResponse : ResponseBase
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public EnderecoResponse? Endereco { get; set; }
        public List<CanalAtendimento> CanaisAtendimento { get; set; } = new();
        public DateTime DataCriacao { get; set; }
    }

    public class EnderecoResponse
    {
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
    }
}
