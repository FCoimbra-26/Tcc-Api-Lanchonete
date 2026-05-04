using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Application.Models.Requests.Usuario;
using TCC.Application.Models.Responses.Usuario;

namespace TCC.Application.Service.Interfaces
{
    public interface ILoginService
    {
        public Task<LoginUserResponse> logar(LoginUserRequest userLogin);
    }
}
