using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCC.Application.Models.Requests.Usuario;
using TCC.Application.Service.Interfaces;

namespace tcc_backend_lanchonete_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> RegisterAsync([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, 
                    new { success = false, error = "Dados inválidos", errors = ModelState });
            }

            var response = await _registerService.RegisterAsync(request);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.Created, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }
    }
}
