using System.ComponentModel.DataAnnotations;
using TCC.Application.Models.Requests.Usuario;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Requests.Unidade
{
    public class CreateUnidadeRequest
    {
        [Required(ErrorMessage = "O código da unidade é obrigatório")]
        [MaxLength(20, ErrorMessage = "O código deve ter no máximo 20 caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "O nome da unidade é obrigatório")]
        [MaxLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Ao menos um canal de atendimento é obrigatório")]
        [MinLength(1, ErrorMessage = "Ao menos um canal de atendimento deve ser informado")]
        public List<CanalAtendimento> CanaisAtendimento { get; set; } = new();

        public EnderecoRequest? Endereco { get; set; }
    }
}
