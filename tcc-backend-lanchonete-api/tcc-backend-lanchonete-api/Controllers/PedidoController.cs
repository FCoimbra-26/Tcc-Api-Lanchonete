using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCC.Application.Models.Requests.Pedido;
using TCC.Application.Service.Interfaces;

namespace tcc_backend_lanchonete_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        [Authorize(Roles = "CLIENTE,ATENDENTE,GERENTE,ADMIN")]
        public async Task<ActionResult<dynamic>> CreateAsync([FromBody] CreatePedidoRequest request)
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

            var response = await _pedidoService.CreateAsync(request, usuarioLogadoId);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.Created, response);

            return StatusCode((int)HttpStatusCode.BadRequest, response);
        }
    }
}
