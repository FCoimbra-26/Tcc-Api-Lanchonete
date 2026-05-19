using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCC.Application.Models.Requests.Role;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Enums;

namespace tcc_backend_lanchonete_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN,GERENTE")] 
    public class UsuarioRoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public UsuarioRoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("assign")]
        public async Task<ActionResult<dynamic>> AssignRoleAsync([FromBody] AssignRoleToUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new { success = false, error = "Dados inválidos", errors = ModelState });
            }

            var response = await _roleService.AssignRoleToUserAsync(request);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.Created, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpDelete("remove")]
        public async Task<ActionResult<dynamic>> RemoveRoleAsync([FromQuery] int usuarioId, [FromQuery] UsuarioRole role)
        {
            var response = await _roleService.RemoveRoleFromUserAsync(usuarioId, role);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.NoContent);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpGet("user/{usuarioId}")]
        [Authorize] 
        public async Task<ActionResult<dynamic>> GetUserRolesAsync(int usuarioId)
        {
            var response = await _roleService.GetUserRolesAsync(usuarioId);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
        }

        [HttpPatch("activate")]
        public async Task<ActionResult<dynamic>> ActivateAsync([FromQuery] int usuarioId, [FromQuery] UsuarioRole role)
        {
            var response = await _roleService.ActivateUserRoleAsync(usuarioId, role);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpPatch("deactivate")]
        public async Task<ActionResult<dynamic>> DeactivateAsync([FromQuery] int usuarioId, [FromQuery] UsuarioRole role)
        {
            var response = await _roleService.DeactivateUserRoleAsync(usuarioId, role);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }
    }
}
