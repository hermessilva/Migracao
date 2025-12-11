using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class TelaConfiguration : BaseEntityConfiguration<Tela>
{
    public override void Configure(EntityTypeBuilder<Tela> builder)
    {
        builder.ToTable("tela", t => t.HasComment("Catálogo de telas (módulos/páginas) disponíveis no sistema"));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela");

        builder.Property(x => x.Slug)
            .HasColumnName("slug")
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Identificador amigável da tela para uso em URLs e código");

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .IsRequired()
            .HasComment("Nome ou descrição completa da tela");

        builder.HasIndex(x => x.Slug)
            .IsUnique();
    }
}