using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class TelaConfiguration : IEntityTypeConfiguration<Tela>
{
    public void Configure(EntityTypeBuilder<Tela> builder)
    {
        builder.ToTable("tela");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela");

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .IsRequired()
            .HasComment("Descrição/nome da tela");
    }
}