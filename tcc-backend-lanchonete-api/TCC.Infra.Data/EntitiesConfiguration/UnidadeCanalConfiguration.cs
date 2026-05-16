using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class UnidadeCanalConfiguration : IEntityTypeConfiguration<UnidadeCanal>
    {
        public void Configure(EntityTypeBuilder<UnidadeCanal> builder)
        {
            builder.ToTable("UnidadesCanais");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.UnidadeId)
                .IsRequired();

            builder.Property(e => e.Canal)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasIndex(e => new { e.UnidadeId, e.Canal })
                .IsUnique()
                .HasDatabaseName("IX_UnidadesCanais_UnidadeId_Canal");

            builder.HasOne(e => e.Unidade)
                .WithMany(u => u.CanaisAtendimento)
                .HasForeignKey(e => e.UnidadeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
