using System.ComponentModel.DataAnnotations;

namespace TCC.Application.Models.Requests.Usuario
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; }

        public string? Cpf { get; set; }

        public string? Telefone { get; set; }

        public DateTime? DataNascimento { get; set; }

        public EnderecoRequest? Endereco { get; set; }
    }

    public class EnderecoRequest
    {
        [Required(ErrorMessage = "O logradouro é obrigatório")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O número é obrigatório")]
        public string Numero { get; set; }

        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "A UF é obrigatória")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "A UF deve ter 2 caracteres")]
        public string Uf { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        public string Cep { get; set; }
    }
}
