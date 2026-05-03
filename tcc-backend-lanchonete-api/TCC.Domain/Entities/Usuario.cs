using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.Domain.Entities
{
    public  class Usuario
    {
        public string email { get; set; }
        public string senha { get; set; }

        public UsuarioRole role { get; set; }

        public Usuario() { }
    }
}
