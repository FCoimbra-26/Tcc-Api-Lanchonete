using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
using TCC.Domain.Interfaces;
using TCC.Infra.Data.Context;

namespace TCC.Infra.Data.Repositories
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly ApplicationDbContext _context;

        public EstoqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EstoqueItem?> GetByUnidadeAndProdutoAsync(int unidadeId, int produtoId)
        {
            return await _context.EstoqueItens
                .FirstOrDefaultAsync(e => e.UnidadeId == unidadeId && e.ProdutoId == produtoId);
        }

        public async Task<IEnumerable<EstoqueItem>> GetByUnidadeIdAsync(int unidadeId)
        {
            return await _context.EstoqueItens
                .Where(e => e.UnidadeId == unidadeId)
                .ToListAsync();
        }
    }
}
