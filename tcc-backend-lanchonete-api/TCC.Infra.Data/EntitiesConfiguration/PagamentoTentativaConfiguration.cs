using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class PagamentoTentativaConfiguration : IEntityTypeConfiguration<PagamentoTentativa>
    {
        public void Configure(EntityTypeBuilder<PagamentoTentativa> builder)
        {
            builder.ToTable("PagamentoTentativas");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.PagamentoId)
                .IsRequired();

            builder.Property(e => e.SequenciaTentativa)
                .IsRequired();

            builder.Property(e => e.OrigemConfirmacao)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.StatusTentativa)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.ValorCobrado)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(e => e.DataSolicitacao)
                .IsRequired();

            builder.Property(e => e.DataRetorno);

            builder.Property(e => e.PayloadRetorno)
                .HasColumnType("text");

            builder.Property(e => e.MetodoPagamento)
                .HasMaxLength(100);

            builder.Property(e => e.Observacao)
                .HasMaxLength(500);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasOne(e => e.Pagamento)
                .WithMany(p => p.Tentativas)
                .HasForeignKey(e => e.PagamentoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.UsuarioConfirmacao)
                .WithMany()
                .HasForeignKey(e => e.UsuarioConfirmacaoId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
