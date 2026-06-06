using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TCC.Application.Models.Requests.Pagamento;
using TCC.Application.Service.Interfaces;

namespace tcc_backend_lanchonete_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoController(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        [HttpPost("solicitar-mock")]
        [Authorize(Roles = "CLIENTE,ATENDENTE,GERENTE,ADMIN")]
        public async Task<ActionResult<dynamic>> SolicitarMockAsync([FromBody] SolicitarPagamentoMockRequest request)
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

            var rolesUsuario = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var response = await _pagamentoService.SolicitarMockAsync(request, usuarioLogadoId, rolesUsuario);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);

            return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpPost("confirmacao-local")]
        [Authorize(Roles = "ATENDENTE,GERENTE")]
        public async Task<ActionResult<dynamic>> ConfirmacaoLocalAsync([FromBody] ConfirmacaoPagamentoLocalRequest request)
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

            var rolesUsuario = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var response = await _pagamentoService.ConfirmacaoLocalAsync(request, usuarioLogadoId, rolesUsuario);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);

            return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        [HttpPost("webhook-mock")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> WebhookMockAsync([FromBody] WebhookPagamentoMockRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new { success = false, error = "Dados invalidos", errors = ModelState });
            }

            var response = await _pagamentoService.WebhookMockAsync(request);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);

            return StatusCode((int)HttpStatusCode.BadRequest, response);
        }
    }
}
