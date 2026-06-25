namespace TCC.Application.Service.Interfaces
{
    public interface IAuditService
    {
        Task RegistrarAsync(
            string acao,
            string recurso,
            bool sucesso,
            int? usuarioId = null,
            int? unidadeId = null,
            int? entidadeId = null,
            string? detalhes = null);
    }
}