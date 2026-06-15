using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
using TCC.Domain.Enums;
using TCC.Domain.Interfaces;
using TCC.Infra.Data.Context;

namespace TCC.Infra.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido?> GetByIdAsync(int pedidoId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(i => i.CardapioItem)
                        .ThenInclude(c => c.Produto)
                .Include(p => p.Historicos)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync(CanalAtendimento? canalPedido = null)
        {
            var query = _context.Pedidos
                .Include(p => p.Itens)
                .AsQueryable();

            if (canalPedido.HasValue)
            {
                query = query.Where(p => p.CanalPedido == canalPedido.Value);
            }

            return await query
                .OrderByDescending(p => p.DataCriacao)
                .ToListAsync();
        }

        public async Task<Pedido> CreateWithStockAsync(
            Pedido pedido,
            IEnumerable<EstoqueItem> estoqueItensAtualizados,
            IEnumerable<EstoqueMovimentacao> movimentacoesEstoque)
        {
            var utcNow = DateTime.UtcNow;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                pedido.DataCriacao = utcNow;
                pedido.DataAtualizacao = utcNow;

                foreach (var item in pedido.Itens)
                {
                    item.DataCriacao = utcNow;
                    item.DataAtualizacao = utcNow;
                }

                await _context.Pedidos.AddAsync(pedido);

                _context.EstoqueItens.UpdateRange(estoqueItensAtualizados);

                foreach (var movimentacao in movimentacoesEstoque)
                {
                    movimentacao.Pedido = pedido;
                    movimentacao.DataCriacao = utcNow;
                    movimentacao.DataAtualizacao = utcNow;
                }

                await _context.EstoqueMovimentacoes.AddRangeAsync(movimentacoesEstoque);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var pedidoCriado = await _context.Pedidos
                    .Include(p => p.Itens)
                        .ThenInclude(i => i.CardapioItem)
                            .ThenInclude(c => c.Produto)
                    .FirstAsync(p => p.Id == pedido.Id);

                return pedidoCriado;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Pedido> UpdateStatusAsync(Pedido pedido, PedidoStatusHistorico historico)
        {
            var utcNow = DateTime.UtcNow;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                pedido.DataAtualizacao = utcNow;
                historico.DataCriacao = utcNow;
                historico.DataAtualizacao = utcNow;

                _context.Pedidos.Update(pedido);
                await _context.PedidoStatusHistoricos.AddAsync(historico);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await _context.Pedidos
                    .Include(p => p.Itens)
                        .ThenInclude(i => i.CardapioItem)
                            .ThenInclude(c => c.Produto)
                    .Include(p => p.Historicos)
                    .FirstAsync(p => p.Id == pedido.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Pedido> CancelWithStockReversalAsync(
            Pedido pedido,
            PedidoStatusHistorico historico,
            IEnumerable<EstoqueItem> estoqueItensAtualizados,
            IEnumerable<EstoqueMovimentacao> movimentacoesEstoque)
        {
            var utcNow = DateTime.UtcNow;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                pedido.DataAtualizacao = utcNow;
                historico.DataCriacao = utcNow;
                historico.DataAtualizacao = utcNow;

                _context.Pedidos.Update(pedido);
                _context.EstoqueItens.UpdateRange(estoqueItensAtualizados);
                await _context.PedidoStatusHistoricos.AddAsync(historico);

                foreach (var movimentacao in movimentacoesEstoque)
                {
                    movimentacao.PedidoId = pedido.Id;
                    movimentacao.DataCriacao = utcNow;
                    movimentacao.DataAtualizacao = utcNow;
                }

                await _context.EstoqueMovimentacoes.AddRangeAsync(movimentacoesEstoque);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await _context.Pedidos
                    .Include(p => p.Itens)
                        .ThenInclude(i => i.CardapioItem)
                            .ThenInclude(c => c.Produto)
                    .Include(p => p.Historicos)
                    .FirstAsync(p => p.Id == pedido.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
