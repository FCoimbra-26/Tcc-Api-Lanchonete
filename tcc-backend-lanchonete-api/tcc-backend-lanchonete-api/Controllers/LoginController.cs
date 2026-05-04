using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCC.Application.Models.Requests.Usuario;
using TCC.Application.Service.Interfaces;
using TCC.Application.Service.Services;

namespace tcc_backend_lanchonete_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService) { 
                _loginService = loginService;
        }
        [HttpPost]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody] LoginUserRequest request)
        {
            var response = await this._loginService.logar(request);
            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.Unauthorized);
        }
    }
}
