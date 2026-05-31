using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
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
    }
}
