namespace TCC.Domain.Entities
{
    public class AuditLog : Entity
    {
        public string Acao { get; set; } = string.Empty;
        public string Recurso { get; set; } = string.Empty;
        public int? EntidadeId { get; set; }
        public int? UsuarioId { get; set; }
        public int? UnidadeId { get; set; }
        public bool Sucesso { get; set; }
        public string? Detalhes { get; set; }

        public virtual Usuario? Usuario { get; set; }
        public virtual Unidade? Unidade { get; set; }
    }
}