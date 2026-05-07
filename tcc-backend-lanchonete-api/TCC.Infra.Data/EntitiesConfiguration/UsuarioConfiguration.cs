using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.EmailNormalizado)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.SenhaHash)
                .IsRequired();

            builder.Property(e => e.Role)
                .IsRequired();

            builder.Property(e => e.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasIndex(e => e.EmailNormalizado)
                .IsUnique()
                .HasDatabaseName("IX_Usuarios_EmailNormalizado");

            builder.HasOne(e => e.Pessoa)
                .WithMany()
                .HasForeignKey(e => e.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
