using TCC.Domain.Entities;
using TCC.Domain.Interfaces;
using TCC.Infra.Data.Context;

namespace TCC.Infra.Data.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuditLog> CreateAsync(AuditLog auditLog)
        {
            var nowUtc = DateTime.UtcNow;
            auditLog.DataCriacao = nowUtc;
            auditLog.DataAtualizacao = nowUtc;

            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();

            return auditLog;
        }
    }
}