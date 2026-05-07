using TCC.Application.Models.Requests.Usuario;
using TCC.Application.Models.Responses.Usuario;

namespace TCC.Application.Service.Interfaces
{
    public interface IRegisterService
    {
        Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request);
    }
}
