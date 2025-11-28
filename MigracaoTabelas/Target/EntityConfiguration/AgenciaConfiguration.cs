using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class AgenciaConfiguration : IEntityTypeConfiguration<Agencia>
{
    public void Configure(EntityTypeBuilder<Agencia> builder)
    {
        builder.ToTable("agencia", t => t.HasComment("Armazena informações cadastrais das agências da cooperativa"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.Codigo)
            .HasColumnName("codigo")
            .HasColumnType("char(4)")
            .HasComment("Código único da agência no formato de 4 caracteres")
            .IsRequired();

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasMaxLength(255)
            .HasComment("Nome completo da agência")
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .HasComment("Data e hora de criação do registro")
            .IsRequired();

        builder.HasIndex(x => x.Codigo)
            .IsUnique();

        builder.HasIndex(x => x.Nome)
            .IsUnique();
    }
}