using System;
using TCC.Domain.Enums;

namespace TCC.Domain.Entities
{
    public class Usuario : Entity
    {
        public string Email { get; set; }
        public string EmailNormalizado { get; set; }
        public string SenhaHash { get; set; }
        public UsuarioRole Role { get; set; }
        public bool Ativo { get; set; }

        public int PessoaId { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        public Usuario() 
        {
            Ativo = true;
        }
    }
}
