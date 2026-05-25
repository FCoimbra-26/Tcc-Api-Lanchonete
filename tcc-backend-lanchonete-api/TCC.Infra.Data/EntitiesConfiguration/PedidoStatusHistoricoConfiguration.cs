using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class PedidoStatusHistoricoConfiguration : IEntityTypeConfiguration<PedidoStatusHistorico>
    {
        public void Configure(EntityTypeBuilder<PedidoStatusHistorico> builder)
        {
            builder.ToTable("PedidoStatusHistoricos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.PedidoId)
                .IsRequired();

            builder.Property(e => e.StatusAnterior)
                .HasConversion<int?>();

            builder.Property(e => e.StatusNovo)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.Observacao)
                .HasMaxLength(500);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasOne(e => e.Pedido)
                .WithMany(p => p.Historicos)
                .HasForeignKey(e => e.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.UsuarioResponsavel)
                .WithMany()
                .HasForeignKey(e => e.UsuarioResponsavelId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
