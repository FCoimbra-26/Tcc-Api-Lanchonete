using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Acao)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Recurso)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Sucesso)
                .IsRequired();

            builder.Property(e => e.Detalhes)
                .HasMaxLength(2000);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            builder.HasOne(e => e.Unidade)
                .WithMany()
                .HasForeignKey(e => e.UnidadeId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            builder.HasIndex(e => e.DataCriacao)
                .HasDatabaseName("IX_AuditLogs_DataCriacao");

            builder.HasIndex(e => new { e.Recurso, e.Acao })
                .HasDatabaseName("IX_AuditLogs_Recurso_Acao");
        }
    }
}