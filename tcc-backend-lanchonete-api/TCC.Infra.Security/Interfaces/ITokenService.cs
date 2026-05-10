using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Domain.Entities;

namespace TCC.Infra.Security.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(Usuario usuario);
    }
}
