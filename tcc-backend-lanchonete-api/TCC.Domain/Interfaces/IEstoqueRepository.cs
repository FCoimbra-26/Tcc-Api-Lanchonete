using TCC.Domain.Entities;

namespace TCC.Domain.Interfaces
{
    public interface IEstoqueRepository
    {
        Task<EstoqueItem?> GetByUnidadeAndProdutoAsync(int unidadeId, int produtoId);
        Task<IEnumerable<EstoqueItem>> GetByUnidadeIdAsync(int unidadeId);
    }
}
