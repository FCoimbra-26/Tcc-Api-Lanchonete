using TCC.Application.Models.Requests.Cardapio;
using TCC.Application.Models.Responses.Cardapio;
using TCC.Application.Service.Interfaces;
using TCC.Domain.Entities;
using TCC.Domain.Interfaces;

namespace TCC.Application.Service.Services
{
    public class CardapioService : ICardapioService
    {
        private readonly ICardapioRepository _cardapioRepository;
        private readonly IUnidadeRepository _unidadeRepository;
        private readonly IEstoqueRepository _estoqueRepository;

        public CardapioService(
            ICardapioRepository cardapioRepository,
            IUnidadeRepository unidadeRepository,
            IEstoqueRepository estoqueRepository)
        {
            _cardapioRepository = cardapioRepository;
            _unidadeRepository = unidadeRepository;
            _estoqueRepository = estoqueRepository;
        }

        public async Task<CardapioResponse> GetCardapioByUnidadeAsync(int unidadeId, bool apenasDisponiveis = true)
        {
            try
            {
                var unidade = await _unidadeRepository.GetByIdAsync(unidadeId);
                if (unidade == null)
                {
                    return new CardapioResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                var itens = await _cardapioRepository.GetByUnidadeIdAsync(unidadeId, apenasDisponiveis);
                var estoques = await _estoqueRepository.GetByUnidadeIdAsync(unidadeId);
                var estoqueDict = estoques.ToDictionary(e => e.ProdutoId, e => e);

                var itensResponse = new List<CardapioItemResponse>();

                foreach (var item in itens)
                {
                    estoqueDict.TryGetValue(item.ProdutoId, out var estoque);

                    var disponivel = item.Produto.Ativo &&
                                   (estoque == null || (estoque.Ativo && estoque.QuantidadeDisponivel > 0));

                    itensResponse.Add(new CardapioItemResponse
                    {
                        Id = item.Id,
                        ProdutoId = item.ProdutoId,
                        NomeProduto = item.Produto.NomeProduto,
                        Descricao = item.Produto.Descricao,
                        Categoria = item.Produto.Categoria,
                        ImagemUrl = item.Produto.ImagemUrl,
                        PrecoBase = item.Produto.PrecoBase,
                        PrecoPraticado = item.PrecoPraticado,
                        ProdutoAtivo = item.Produto.Ativo,
                        QuantidadeEstoque = estoque?.QuantidadeDisponivel,
                        Disponivel = disponivel
                    });
                }

                return new CardapioResponse
                {
                    Success = true,
                    UnidadeId = unidade.Id,
                    UnidadeNome = unidade.Nome,
                    UnidadeCodigo = unidade.Codigo,
                    Itens = itensResponse
                };
            }
            catch (Exception ex)
            {
                return new CardapioResponse
                {
                    Success = false,
                    Error = $"Erro ao consultar cardápio: {ex.Message}"
                };
            }
        }

        public async Task<CardapioResponse> GetCardapioByUnidadeCodigoAsync(string codigoUnidade, bool apenasDisponiveis = true)
        {
            try
            {
                var unidade = await _unidadeRepository.GetByCodigoAsync(codigoUnidade);
                if (unidade == null)
                {
                    return new CardapioResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                return await GetCardapioByUnidadeAsync(unidade.Id, apenasDisponiveis);
            }
            catch (Exception ex)
            {
                return new CardapioResponse
                {
                    Success = false,
                    Error = $"Erro ao consultar cardápio: {ex.Message}"
                };
            }
        }

        public async Task<CardapioResponse> AddProdutoToCardapioAsync(int unidadeId, AddProdutoCardapioRequest request)
        {
            try
            {
                var unidade = await _unidadeRepository.GetByIdAsync(unidadeId);
                if (unidade == null)
                {
                    return new CardapioResponse
                    {
                        Success = false,
                        Error = "Unidade não encontrada"
                    };
                }

                if (await _cardapioRepository.ExistsAsync(unidadeId, request.ProdutoId))
                {
                    return new CardapioResponse
                    {
                        Success = false,
                        Error = "Produto já está no cardápio desta unidade"
                    };
                }

                var cardapioItem = new CardapioItem
                {
                    UnidadeId = unidadeId,
                    ProdutoId = request.ProdutoId,
                    PrecoPraticado = request.PrecoPraticado
                };

                await _cardapioRepository.CreateAsync(cardapioItem);

                return await GetCardapioByUnidadeAsync(unidadeId, false);
            }
            catch (Exception ex)
            {
                return new CardapioResponse
                {
                    Success = false,
                    Error = $"Erro ao adicionar produto ao cardápio: {ex.Message}"
                };
            }
        }

        public async Task<CardapioResponse> UpdateProdutoCardapioAsync(int cardapioItemId, UpdateProdutoCardapioRequest request)
        {
            try
            {
                var cardapioItem = await _cardapioRepository.GetByIdAsync(cardapioItemId);
                if (cardapioItem == null)
                {
                    return new CardapioResponse
                    {
                        Success = false,
                        Error = "Item de cardápio não encontrado"
                    };
                }

                cardapioItem.PrecoPraticado = request.PrecoPraticado;

                await _cardapioRepository.UpdateAsync(cardapioItem);

                return await GetCardapioByUnidadeAsync(cardapioItem.UnidadeId, false);
            }
            catch (Exception ex)
            {
                return new CardapioResponse
                {
                    Success = false,
                    Error = $"Erro ao atualizar item do cardápio: {ex.Message}"
                };
            }
        }

        public async Task<CardapioResponse> RemoveProdutoFromCardapioAsync(int cardapioItemId)
        {
            try
            {
                var cardapioItem = await _cardapioRepository.GetByIdAsync(cardapioItemId);
                if (cardapioItem == null)
                {
                    return new CardapioResponse
                    {
                        Success = false,
                        Error = "Item de cardápio não encontrado"
                    };
                }

                var unidadeId = cardapioItem.UnidadeId;

                await _cardapioRepository.DeleteAsync(cardapioItemId);

                return await GetCardapioByUnidadeAsync(unidadeId, false);
            }
            catch (Exception ex)
            {
                return new CardapioResponse
                {
                    Success = false,
                    Error = $"Erro ao remover produto do cardápio: {ex.Message}"
                };
            }
        }
    }
}
