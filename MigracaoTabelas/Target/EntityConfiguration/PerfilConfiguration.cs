using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;
public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
{
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
        builder.ToTable("perfil");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela");

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasMaxLength(255)
            .IsRequired()
            .HasComment("Nome do perfil");

        builder.Property(x => x.Slug)
            .HasColumnName("slug")
            .HasMaxLength(255)
            .IsRequired()
            .HasComment("Nome amigavel do perfil");

        builder.HasIndex(x => x.Nome)
            .IsUnique();
    }
}