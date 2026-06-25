using TCC.Application.Service.Interfaces;
using TCC.Domain.Entities;
using TCC.Domain.Interfaces;

namespace TCC.Application.Service.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task RegistrarAsync(
            string acao,
            string recurso,
            bool sucesso,
            int? usuarioId = null,
            int? unidadeId = null,
            int? entidadeId = null,
            string? detalhes = null)
        {
            var auditLog = new AuditLog
            {
                Acao = acao,
                Recurso = recurso,
                Sucesso = sucesso,
                UsuarioId = usuarioId,
                UnidadeId = unidadeId,
                EntidadeId = entidadeId,
                Detalhes = detalhes
            };

            await _auditLogRepository.CreateAsync(auditLog);
        }
    }
}