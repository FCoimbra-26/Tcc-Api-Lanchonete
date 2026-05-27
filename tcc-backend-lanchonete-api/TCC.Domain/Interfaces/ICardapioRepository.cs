using TCC.Domain.Entities;

namespace TCC.Domain.Interfaces
{
    public interface ICardapioRepository
    {
        Task<IEnumerable<CardapioItem>> GetByUnidadeIdAsync(int unidadeId, bool apenasDisponiveis = true);
        Task<CardapioItem?> GetByIdAsync(int id);
        Task<CardapioItem?> GetByUnidadeAndProdutoAsync(int unidadeId, int produtoId);
        Task<CardapioItem> CreateAsync(CardapioItem cardapioItem);
        Task<CardapioItem> UpdateAsync(CardapioItem cardapioItem);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int unidadeId, int produtoId);
    }
}
