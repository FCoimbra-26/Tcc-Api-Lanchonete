using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class CardapioItemConfiguration : IEntityTypeConfiguration<CardapioItem>
    {
        public void Configure(EntityTypeBuilder<CardapioItem> builder)
        {
            builder.ToTable("CardapioItens");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.UnidadeId)
                .IsRequired();

            builder.Property(e => e.ProdutoId)
                .IsRequired();

            builder.Property(e => e.PrecoPraticado)
                .HasPrecision(10, 2);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasIndex(e => new { e.UnidadeId, e.ProdutoId })
                .IsUnique()
                .HasDatabaseName("IX_CardapioItens_UnidadeId_ProdutoId");

            builder.HasOne(e => e.Unidade)
                .WithMany()
                .HasForeignKey(e => e.UnidadeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Produto)
                .WithMany(p => p.CardapioItens)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.PedidoItens)
                .WithOne(p => p.CardapioItem)
                .HasForeignKey(p => p.CardapioItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(e => e.DisponivelNaUnidade);
        }
    }
}
