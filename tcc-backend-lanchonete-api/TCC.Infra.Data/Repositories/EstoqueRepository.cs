using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
using TCC.Domain.Enums;
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

        public async Task<EstoqueItem> RegistrarEntradaAsync(int unidadeId, int produtoId, int quantidade, int? usuarioResponsavelId = null, string? observacao = null)
        {
            var utcNow = DateTime.UtcNow;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var unidadeExiste = await _context.Unidades.AnyAsync(u => u.Id == unidadeId);
                if (!unidadeExiste)
                {
                    throw new InvalidOperationException("Unidade nao encontrada");
                }

                var produto = await _context.ProdutosGlobais.FirstOrDefaultAsync(p => p.Id == produtoId);
                if (produto == null)
                {
                    throw new InvalidOperationException("Produto nao encontrado");
                }

                var estoque = await _context.EstoqueItens
                    .Include(e => e.Produto)
                    .FirstOrDefaultAsync(e => e.UnidadeId == unidadeId && e.ProdutoId == produtoId);

                if (estoque == null)
                {
                    estoque = new EstoqueItem
                    {
                        UnidadeId = unidadeId,
                        ProdutoId = produtoId,
                        QuantidadeDisponivel = 0,
                        Ativo = true,
                        DataCriacao = utcNow,
                        DataAtualizacao = utcNow
                    };

                    await _context.EstoqueItens.AddAsync(estoque);
                }

                estoque.Ativo = true;
                estoque.QuantidadeDisponivel += quantidade;
                estoque.DataAtualizacao = utcNow;

                var movimentacao = new EstoqueMovimentacao
                {
                    EstoqueItem = estoque,
                    TipoMovimentacao = TipoMovimentacaoEstoque.ENTRADA,
                    Quantidade = quantidade,
                    UsuarioResponsavelId = usuarioResponsavelId,
                    Observacao = string.IsNullOrWhiteSpace(observacao)
                        ? $"Entrada de estoque do produto {produto.NomeProduto}"
                        : observacao,
                    DataCriacao = utcNow,
                    DataAtualizacao = utcNow
                };

                await _context.EstoqueMovimentacoes.AddAsync(movimentacao);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await _context.EstoqueItens
                    .Include(e => e.Produto)
                    .FirstAsync(e => e.Id == estoque.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
