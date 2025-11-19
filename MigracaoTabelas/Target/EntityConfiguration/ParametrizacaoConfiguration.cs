using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class ParametrizacaoConfiguration : IEntityTypeConfiguration<Parametrizacao>
{
    public void Configure(EntityTypeBuilder<Parametrizacao> builder)
    {
        builder.ToTable("parametrizacao", t => t.HasComment("Parametrizações de campos para preechimento"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .HasComment("Descrição do item")
            .IsRequired();


        builder.Navigation(p => p.ParametrizacoesRespostas).AutoInclude();
    }
}