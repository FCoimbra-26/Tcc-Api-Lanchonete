using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCC.Application.Models.Requests.Estoque;
using TCC.Application.Service.Interfaces;

namespace tcc_backend_lanchonete_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueService _estoqueService;

        public EstoqueController(IEstoqueService estoqueService)
        {
            _estoqueService = estoqueService;
        }

        [HttpPost("entrada")]
        [Authorize(Roles = "GERENTE,ADMIN")]
        public async Task<ActionResult<dynamic>> RegistrarEntradaAsync([FromBody] RegistrarEntradaEstoqueRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new { success = false, error = "Dados invalidos", errors = ModelState });
            }

            int? usuarioLogadoId = null;
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (int.TryParse(userIdClaim, out var parsedUserId))
            {
                usuarioLogadoId = parsedUserId;
            }

            var response = await _estoqueService.RegistrarEntradaAsync(request, usuarioLogadoId);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);

            return StatusCode((int)HttpStatusCode.BadRequest, response);
        }
    }
}