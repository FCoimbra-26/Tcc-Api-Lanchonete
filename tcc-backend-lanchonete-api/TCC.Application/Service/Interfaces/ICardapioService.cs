using TCC.Application.Models.Requests.Cardapio;
using TCC.Application.Models.Responses.Cardapio;

namespace TCC.Application.Service.Interfaces
{
    public interface ICardapioService
    {
        Task<CardapioResponse> GetCardapioByUnidadeAsync(int unidadeId, bool apenasDisponiveis = true);
        Task<CardapioResponse> GetCardapioByUnidadeCodigoAsync(string codigoUnidade, bool apenasDisponiveis = true);
        Task<CardapioResponse> AddProdutoToCardapioAsync(int unidadeId, AddProdutoCardapioRequest request);
        Task<CardapioResponse> UpdateProdutoCardapioAsync(int cardapioItemId, UpdateProdutoCardapioRequest request);
        Task<CardapioResponse> RemoveProdutoFromCardapioAsync(int cardapioItemId);
    }
}
