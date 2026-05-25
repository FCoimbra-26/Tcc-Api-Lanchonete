using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCC.Application.Models.Requests.Cardapio;
using TCC.Application.Service.Interfaces;

namespace tcc_backend_lanchonete_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardapioController : ControllerBase
    {
        private readonly ICardapioService _cardapioService;

        public CardapioController(ICardapioService cardapioService)
        {
            _cardapioService = cardapioService;
        }

        /// <summary>
        /// Consulta cardápio por ID da unidade
        /// </summary>
        [HttpGet("unidade/{unidadeId}")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> GetCardapioByUnidadeAsync(int unidadeId, [FromQuery] bool apenasDisponiveis = true)
        {
            var response = await _cardapioService.GetCardapioByUnidadeAsync(unidadeId, apenasDisponiveis);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.NotFound, response);
        }

        /// <summary>
        /// Consulta cardápio por código da unidade
        /// </summary>
        [HttpGet("unidade/codigo/{codigoUnidade}")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> GetCardapioByCodigoAsync(string codigoUnidade, [FromQuery] bool apenasDisponiveis = true)
        {
            var response = await _cardapioService.GetCardapioByUnidadeCodigoAsync(codigoUnidade, apenasDisponiveis);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.NotFound, response);
        }

        /// <summary>
        /// Adiciona produto ao cardápio da unidade (ADMIN/GERENTE)
        /// </summary>
        [HttpPost("unidade/{unidadeId}/produto")]
        [Authorize(Roles = "ADMIN,GERENTE")]
        public async Task<ActionResult<dynamic>> AddProdutoAsync(int unidadeId, [FromBody] AddProdutoCardapioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new { success = false, error = "Dados inválidos", errors = ModelState });
            }

            var response = await _cardapioService.AddProdutoToCardapioAsync(unidadeId, request);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.Created, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        /// <summary>
        /// Atualiza item do cardápio (ADMIN/GERENTE)
        /// </summary>
        [HttpPut("{cardapioItemId}")]
        [Authorize(Roles = "ADMIN,GERENTE")]
        public async Task<ActionResult<dynamic>> UpdateProdutoAsync(int cardapioItemId, [FromBody] UpdateProdutoCardapioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,
                    new { success = false, error = "Dados inválidos", errors = ModelState });
            }

            var response = await _cardapioService.UpdateProdutoCardapioAsync(cardapioItemId, request);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        /// <summary>
        /// Remove produto do cardápio (ADMIN/GERENTE)
        /// </summary>
        [HttpDelete("{cardapioItemId}")]
        [Authorize(Roles = "ADMIN,GERENTE")]
        public async Task<ActionResult<dynamic>> RemoveProdutoAsync(int cardapioItemId)
        {
            var response = await _cardapioService.RemoveProdutoFromCardapioAsync(cardapioItemId);

            if (response.Success)
                return StatusCode((int)HttpStatusCode.OK, response);
            else
                return StatusCode((int)HttpStatusCode.BadRequest, response);
        }
    }
}
