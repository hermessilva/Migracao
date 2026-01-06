using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MigracaoTabelas.Target.EntityConfiguration;

public class AcaoConfiguration : BaseEntityConfiguration<Acao>
{
    public override void Configure(EntityTypeBuilder<Acao> builder)
    {
        builder.ToTable("acao", t => t.HasComment("Catálogo de ações que podem ser executadas nas telas"));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela");

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .IsRequired()
            .HasComment("Descrição da ação (ex.: Visualizar, Editar, Excluir)");

        builder.HasIndex(x => x.Descricao)
            .IsUnique()
            .HasDatabaseName("ux_acao_descricao");

    }
}