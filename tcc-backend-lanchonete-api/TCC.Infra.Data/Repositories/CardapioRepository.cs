using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
using TCC.Domain.Interfaces;
using TCC.Infra.Data.Context;

namespace TCC.Infra.Data.Repositories
{
    public class CardapioRepository : ICardapioRepository
    {
        private readonly ApplicationDbContext _context;

        public CardapioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CardapioItem>> GetByUnidadeIdAsync(int unidadeId, bool apenasDisponiveis = true)
        {
            var query = _context.CardapioItens
                .Include(c => c.Produto)
                .Include(c => c.Unidade)
                .Where(c => c.UnidadeId == unidadeId);

            if (apenasDisponiveis)
            {
                query = query.Where(c => c.Produto.Ativo);

                var queryComEstoque = from cardapio in query
                                     join estoque in _context.EstoqueItens
                                         on new { cardapio.UnidadeId, cardapio.ProdutoId }
                                         equals new { estoque.UnidadeId, estoque.ProdutoId }
                                         into estoqueGroup
                                     from estoque in estoqueGroup.DefaultIfEmpty()
                                     where estoque == null || (estoque.Ativo && estoque.QuantidadeDisponivel > 0)
                                     select cardapio;

                return await queryComEstoque
                    .OrderBy(c => c.Produto.Categoria)
                    .ThenBy(c => c.Produto.NomeProduto)
                    .ToListAsync();
            }

            return await query
                .OrderBy(c => c.Produto.Categoria)
                .ThenBy(c => c.Produto.NomeProduto)
                .ToListAsync();
        }

        public async Task<CardapioItem?> GetByIdAsync(int id)
        {
            return await _context.CardapioItens
                .Include(c => c.Produto)
                .Include(c => c.Unidade)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CardapioItem?> GetByUnidadeAndProdutoAsync(int unidadeId, int produtoId)
        {
            return await _context.CardapioItens
                .Include(c => c.Produto)
                .Include(c => c.Unidade)
                .FirstOrDefaultAsync(c => c.UnidadeId == unidadeId && c.ProdutoId == produtoId);
        }

        public async Task<CardapioItem> CreateAsync(CardapioItem cardapioItem)
        {
            cardapioItem.DataCriacao = DateTime.UtcNow;
            cardapioItem.DataAtualizacao = DateTime.UtcNow;

            await _context.CardapioItens.AddAsync(cardapioItem);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(cardapioItem.Id) ?? cardapioItem;
        }

        public async Task<CardapioItem> UpdateAsync(CardapioItem cardapioItem)
        {
            cardapioItem.DataAtualizacao = DateTime.UtcNow;

            _context.CardapioItens.Update(cardapioItem);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(cardapioItem.Id) ?? cardapioItem;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cardapioItem = await _context.CardapioItens.FindAsync(id);
            if (cardapioItem == null)
                return false;

            _context.CardapioItens.Remove(cardapioItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int unidadeId, int produtoId)
        {
            return await _context.CardapioItens
                .AnyAsync(c => c.UnidadeId == unidadeId && c.ProdutoId == produtoId);
        }
    }
}
