using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("Pagamentos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.PedidoId)
                .IsRequired();

            builder.Property(e => e.StatusAtual)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.ValorTotalCobrado)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(e => e.MetodoPagamentoFinal)
                .HasMaxLength(100);

            builder.Property(e => e.OrigemConfirmacaoFinal)
                .HasConversion<int?>();

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasIndex(e => e.PedidoId)
                .IsUnique()
                .HasDatabaseName("IX_Pagamentos_PedidoId");

            builder.HasOne(e => e.Pedido)
                .WithOne(p => p.Pagamento)
                .HasForeignKey<Pagamento>(e => e.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Tentativas)
                .WithOne(t => t.Pagamento)
                .HasForeignKey(t => t.PagamentoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
