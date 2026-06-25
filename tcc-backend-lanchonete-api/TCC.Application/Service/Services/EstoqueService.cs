using TCC.Application.Models.Requests.Estoque;
using TCC.Application.Models.Responses.Estoque;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Interfaces;

namespace TCC.Application.Service.Services
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IAuditService _auditService;

        public EstoqueService(IEstoqueRepository estoqueRepository, IAuditService auditService)
        {
            _estoqueRepository = estoqueRepository;
            _auditService = auditService;
        }

        public async Task<EstoqueEntradaResponse> RegistrarEntradaAsync(RegistrarEntradaEstoqueRequest request, int? usuarioLogadoId = null)
        {
            try
            {
                if (request.Quantidade <= 0)
                {
                    return new EstoqueEntradaResponse
                    {
                        Success = false,
                        Error = "A quantidade de entrada deve ser maior que zero"
                    };
                }

                var estoque = await _estoqueRepository.RegistrarEntradaAsync(
                    request.UnidadeId,
                    request.ProdutoId,
                    request.Quantidade,
                    usuarioLogadoId,
                    request.Observacao);

                await _auditService.RegistrarAsync(
                    "ESTOQUE_ENTRADA",
                    "ESTOQUE",
                    true,
                    usuarioLogadoId,
                    request.UnidadeId,
                    estoque.Id,
                    $"Entrada de {request.Quantidade} unidade(s) para o produto {request.ProdutoId}");

                return new EstoqueEntradaResponse
                {
                    Success = true,
                    EstoqueItemId = estoque.Id,
                    UnidadeId = estoque.UnidadeId,
                    ProdutoId = estoque.ProdutoId,
                    NomeProduto = estoque.Produto.NomeProduto,
                    QuantidadeAdicionada = request.Quantidade,
                    QuantidadeDisponivel = estoque.QuantidadeDisponivel,
                    Ativo = estoque.Ativo,
                    DataAtualizacao = estoque.DataAtualizacao
                };
            }
            catch (Exception ex)
            {
                await _auditService.RegistrarAsync("ESTOQUE_ENTRADA", "ESTOQUE", false, usuarioLogadoId, request.UnidadeId, null, ex.Message);
                return new EstoqueEntradaResponse
                {
                    Success = false,
                    Error = $"Erro ao registrar entrada de estoque: {ex.Message}"
                };
            }
        }
    }
}