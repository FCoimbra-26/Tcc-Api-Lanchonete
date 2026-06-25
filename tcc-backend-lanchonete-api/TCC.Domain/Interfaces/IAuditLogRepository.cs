using TCC.Domain.Entities;

namespace TCC.Domain.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<AuditLog> CreateAsync(AuditLog auditLog);
    }
}