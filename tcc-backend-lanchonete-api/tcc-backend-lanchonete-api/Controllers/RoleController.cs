using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCC.Application.Service.Interfaces;

namespace tcc_backend_lanchonete_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<dynamic>> GetAllRolesAsync()
        {
            var response = await _roleService.GetAllRolesAsync();

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
        }
    }
}
