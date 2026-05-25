using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.NumeroPedido)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.UnidadeId)
                .IsRequired();

            builder.Property(e => e.CanalPedido)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.StatusPedido)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.ValorTotal)
                .IsRequired()
                .HasPrecision(10, 2)
                .HasDefaultValue(0);

            builder.Property(e => e.Observacao)
                .HasMaxLength(500);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasIndex(e => e.NumeroPedido)
                .IsUnique()
                .HasDatabaseName("IX_Pedidos_NumeroPedido");

            builder.HasOne(e => e.Unidade)
                .WithMany()
                .HasForeignKey(e => e.UnidadeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            builder.HasMany(e => e.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Pagamento)
                .WithOne(p => p.Pedido)
                .HasForeignKey<Pagamento>(p => p.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Historicos)
                .WithOne(h => h.Pedido)
                .HasForeignKey(h => h.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
