using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Domain.Entities
{
    public class Pessoa : Entity
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
