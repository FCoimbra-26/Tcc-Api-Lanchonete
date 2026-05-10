using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCC.Domain.Entities;

namespace TCC.Infra.Data.EntitiesConfiguration
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoas");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Sobrenome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Cpf)
                .HasMaxLength(11);

            builder.Property(e => e.Telefone)
                .HasMaxLength(20);

            builder.Property(e => e.DataNascimento)
                .IsRequired();

            builder.Property(e => e.DataCriacao)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired();

            builder.HasOne(e => e.Endereco)
                .WithMany()
                .HasForeignKey("EnderecoId")
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            builder.HasIndex(e => e.Cpf)
                .HasDatabaseName("IX_Pessoas_Cpf");
        }
    }
}
