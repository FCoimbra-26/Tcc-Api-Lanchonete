using TCC.Domain.Enums;

namespace TCC.Domain.Entities
{
    public class PagamentoTentativa : Entity
    {
        public int PagamentoId { get; set; }
        public int SequenciaTentativa { get; set; }
        public OrigemConfirmacao OrigemConfirmacao { get; set; }
        public StatusPagamento StatusTentativa { get; set; }
        public decimal ValorCobrado { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataRetorno { get; set; }
        public string? PayloadRetorno { get; set; }
        public int? UsuarioConfirmacaoId { get; set; }
        public string? MetodoPagamento { get; set; }
        public string? Observacao { get; set; }

        public virtual Pagamento Pagamento { get; set; }
        public virtual Usuario? UsuarioConfirmacao { get; set; }
    }
}
