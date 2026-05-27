using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class EstoqueMovimentacaoConfiguration : IEntityTypeConfiguration<EstoqueMovimentacao>
    {
        public void Configure(EntityTypeBuilder<EstoqueMovimentacao> builder)
        {
            builder.ToTable("EstoqueMovimentacoes");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.EstoqueItemId)
                .IsRequired();

            builder.Property(e => e.TipoMovimentacao)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.Quantidade)
                .IsRequired();

            builder.Property(e => e.Observacao)
                .HasMaxLength(500);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasOne(e => e.EstoqueItem)
                .WithMany(ei => ei.Movimentacoes)
                .HasForeignKey(e => e.EstoqueItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Pedido)
                .WithMany(p => p.EstoqueMovimentacoes)
                .HasForeignKey(e => e.PedidoId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(e => e.UsuarioResponsavel)
                .WithMany()
                .HasForeignKey(e => e.UsuarioResponsavelId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
