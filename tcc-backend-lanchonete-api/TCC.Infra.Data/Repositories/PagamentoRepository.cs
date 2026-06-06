using Microsoft.EntityFrameworkCore;
using TCC.Domain.Entities;
using TCC.Domain.Interfaces;
using TCC.Infra.Data.Context;

namespace TCC.Infra.Data.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly ApplicationDbContext _context;

        public PagamentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido?> GetPedidoByIdForPagamentoAsync(int pedidoId)
        {
            return await _context.Pedidos
                .Include(p => p.Pagamento)
                    .ThenInclude(pg => pg!.Tentativas)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }

        public async Task RegistrarConfirmacaoAsync(PagamentoTentativa tentativa)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.PagamentoTentativas.AddAsync(tentativa);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
