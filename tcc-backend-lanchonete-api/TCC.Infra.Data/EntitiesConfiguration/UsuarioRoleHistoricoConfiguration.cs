using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class UsuarioRoleHistoricoConfiguration : IEntityTypeConfiguration<UsuarioRoleHistorico>
    {
        public void Configure(EntityTypeBuilder<UsuarioRoleHistorico> builder)
        {
            builder.ToTable("UsuariosRoles");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.UsuarioId)
                .IsRequired();

            builder.Property(e => e.Role)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(e => e.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasIndex(e => new { e.UsuarioId, e.Role })
                .HasDatabaseName("IX_UsuariosRoles_UsuarioId_Role");

            builder.HasOne(e => e.Usuario)
                .WithMany(u => u.Roles)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
