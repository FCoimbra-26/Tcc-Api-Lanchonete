using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class EstoqueItemConfiguration : IEntityTypeConfiguration<EstoqueItem>
    {
        public void Configure(EntityTypeBuilder<EstoqueItem> builder)
        {
            builder.ToTable("EstoqueItens");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.UnidadeId)
                .IsRequired();

            builder.Property(e => e.ProdutoId)
                .IsRequired();

            builder.Property(e => e.QuantidadeDisponivel)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(e => e.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.EstoqueMinimo);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasIndex(e => new { e.UnidadeId, e.ProdutoId })
                .IsUnique()
                .HasDatabaseName("IX_EstoqueItens_UnidadeId_ProdutoId");

            builder.HasOne(e => e.Unidade)
                .WithMany()
                .HasForeignKey(e => e.UnidadeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Produto)
                .WithMany(p => p.EstoqueItens)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Movimentacoes)
                .WithOne(m => m.EstoqueItem)
                .HasForeignKey(m => m.EstoqueItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
