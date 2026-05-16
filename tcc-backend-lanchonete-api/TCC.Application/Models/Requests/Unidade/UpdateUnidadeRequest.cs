using System.ComponentModel.DataAnnotations;
using TCC.Application.Models.Requests.Usuario;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Requests.Unidade
{
    public class UpdateUnidadeRequest
    {
        [Required(ErrorMessage = "O nome da unidade é obrigatório")]
        [MaxLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
        public string Nome { get; set; }

        public EnderecoRequest? Endereco { get; set; }
    }
}
