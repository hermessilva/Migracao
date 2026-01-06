using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MigracaoTabelas.Target.EntityConfiguration;

public class ParametrizacaoConfiguration : BaseEntityConfiguration<Parametrizacao>
{
    public override void Configure(EntityTypeBuilder<Parametrizacao> builder)
    {
        builder.ToTable("parametrizacao", t => t.HasComment("Catálogo de campos de parametrização do sistema para configurações dinâmicas"));

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

        builder.Property(x => x.Identificador)
            .HasColumnName("identificador")
            .HasMaxLength(50)
            .HasComment("Nome do parâmetro que será usando no código")
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasMaxLength(255)
            .HasComment("Valor atribuido ao parametro")
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasColumnName("tipo")
            .HasMaxLength(50)
            .HasComment("Tipo de dados do campo valor")
            .IsRequired();
    }
}