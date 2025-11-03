using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class TelaAcaoConfiguration : IEntityTypeConfiguration<TelaAcao>
{
    public void Configure(EntityTypeBuilder<TelaAcao> builder)
    {
        builder.ToTable("tela_acao");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.TelaId)
            .HasColumnName("tela_id")
            .HasComment("Chave estrangeira da tabela tela");

        builder.Property(x => x.AcaoId)
            .HasColumnName("acao_id")
            .HasComment("Chave estrangeira da tabela ação");

        builder.HasIndex(x => x.TelaId)
            .HasDatabaseName("idx_tela_acao_tela_id");

        builder.HasIndex(x => x.AcaoId)
            .HasDatabaseName("idx_tela_acao_acao_id");

        builder.HasOne<Tela>()
            .WithMany()
            .HasForeignKey(x => x.TelaId);

        builder.HasOne<Acao>()
            .WithMany()
            .HasForeignKey(x => x.AcaoId);
    }
}