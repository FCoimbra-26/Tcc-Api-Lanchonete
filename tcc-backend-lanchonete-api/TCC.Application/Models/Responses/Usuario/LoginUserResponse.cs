using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Application._Commom;
using TCC.Domain.Enums;

namespace TCC.Application.Models.Responses.Usuario
{
    public class LoginUserResponse : ResponseBase
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public UsuarioRole Tipo { get; set; }
    }
}
