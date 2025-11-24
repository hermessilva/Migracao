using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class SeguradoraConfiguration : IEntityTypeConfiguration<Seguradora>
{
    public void Configure(EntityTypeBuilder<Seguradora> builder)
    {
        builder.ToTable("seguradora");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasMaxLength(255)
            .HasComment("Nome fantasia da seguradora")
            .IsRequired();

        builder.Property(x => x.Cnpj)
            .HasColumnName("cnpj")
            .HasColumnType("char(14)")
            .HasComment("CNPJ da seguradora")
            .IsRequired();

        builder.Property(x => x.RazaoSocial)
            .HasColumnName("razao_social")
            .HasMaxLength(255)
            .HasComment("Razão social da seguradora")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("enum('Ativo','Inativo')")
            .HasConversion(
                v => v.AsString(),
                v => EnumEx.FromString<StatusSeguradora>(v))
            .HasComment("Status da seguradora")
            .IsRequired();

        builder.HasIndex(x => x.Cnpj)
            .IsUnique();
    }
}