using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class ProdutoGlobalConfiguration : IEntityTypeConfiguration<ProdutoGlobal>
    {
        public void Configure(EntityTypeBuilder<ProdutoGlobal> builder)
        {
            builder.ToTable("ProdutosGlobais");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.NomeProduto)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.PrecoBase)
                .IsRequired()
                .HasPrecision(10, 2);

            builder.Property(e => e.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.Descricao)
                .HasMaxLength(500);

            builder.Property(e => e.Categoria)
                .HasMaxLength(100);

            builder.Property(e => e.ImagemUrl)
                .HasMaxLength(500);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasMany(e => e.CardapioItens)
                .WithOne(c => c.Produto)
                .HasForeignKey(c => c.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.EstoqueItens)
                .WithOne(e => e.Produto)
                .HasForeignKey(e => e.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
