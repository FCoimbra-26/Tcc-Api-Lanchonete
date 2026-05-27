using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class PedidoItemConfiguration : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItens");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.PedidoId)
                .IsRequired();

            builder.Property(e => e.CardapioItemId)
                .IsRequired();

            builder.Property(e => e.Quantidade)
                .IsRequired();

            builder.Property(e => e.ValorUnitario)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(e => e.Subtotal)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(e => e.ObservacaoItem)
                .HasMaxLength(500);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasOne(e => e.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(e => e.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.CardapioItem)
                .WithMany(c => c.PedidoItens)
                .HasForeignKey(e => e.CardapioItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
