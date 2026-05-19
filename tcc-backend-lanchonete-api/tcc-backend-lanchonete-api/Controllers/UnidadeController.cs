using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCC.Application.Models.Requests.Unidade;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Enums;

namespace tcc_backend_lanchonete_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadeController : ControllerBase
    {
        private readonly IUnidadeService _unidadeService;

        public UnidadeController(IUnidadeService unidadeService)
        {
            _unidadeService = unidadeService;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,GERENTE")]
        public async Task<ActionResult<dynamic>> CreateAsync([FromBody] CreateUnidadeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new { success = false, error = "Dados inválidos", errors = ModelState });
            }

            var response = await _unidadeService.CreateAsync(request);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.Created, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,GERENTE")]
        public async Task<ActionResult<dynamic>> UpdateAsync(int id, [FromBody] UpdateUnidadeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new { success = false, error = "Dados inválidos", errors = ModelState });
            }

            var response = await _unidadeService.UpdateAsync(id, request);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> GetByIdAsync(int id)
        {
            var response = await _unidadeService.GetByIdAsync(id);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.NotFound, response);
        }

        [HttpGet("codigo/{codigo}")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> GetByCodigoAsync(string codigo)
        {
            var response = await _unidadeService.GetByCodigoAsync(codigo);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.NotFound, response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> GetAllAsync([FromQuery] bool? apenasAtivas = null)
        {
            var response = await _unidadeService.GetAllAsync(apenasAtivas);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<dynamic>> DeleteAsync(int id)
        {
            var response = await _unidadeService.DeleteAsync(id);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.NoContent);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "ADMIN,GERENTE")]
        public async Task<ActionResult<dynamic>> ActivateAsync(int id)
        {
            var response = await _unidadeService.ActivateAsync(id);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "ADMIN,GERENTE")]
        public async Task<ActionResult<dynamic>> DeactivateAsync(int id)
        {
            var response = await _unidadeService.DeactivateAsync(id);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpPost("{id}/canais")]
        [Authorize(Roles = "ADMIN,GERENTE")]
        public async Task<ActionResult<dynamic>> AddCanalAsync(int id, [FromBody] CanalAtendimento canal)
        {
            var response = await _unidadeService.AddCanalAsync(id, canal);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpDelete("{id}/canais/{canal}")]
        [Authorize(Roles = "ADMIN,GERENTE")]
        public async Task<ActionResult<dynamic>> RemoveCanalAsync(int id, CanalAtendimento canal)
        {
            var response = await _unidadeService.RemoveCanalAsync(id, canal);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }
    }
}
