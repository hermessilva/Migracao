using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MigracaoTabelas.Target.EntityConfiguration;

public class TelaAcaoConfiguration : IEntityTypeConfiguration<TelaAcao>
{
    public void Configure(EntityTypeBuilder<TelaAcao> builder)
    {
        builder.ToTable("tela_acao");

        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.TelaId, x.AcaoId });

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.TelaId)
            .HasColumnName("tela_id")
            .HasComment("Chave estrangeira da tabela tela")
            .IsRequired();

        builder.Property(x => x.AcaoId)
            .HasColumnName("acao_id")
            .HasComment("Chave estrangeira da tabela acao")
            .IsRequired();

        builder.HasIndex(x => x.TelaId)
            .HasDatabaseName("tela_acao_index_2");

        builder.HasIndex(x => x.AcaoId)
            .HasDatabaseName("tela_acao_index_3");

        builder.HasOne(x => x.Tela)
            .WithMany(t => t.TelasAcoes)
            .HasForeignKey(x => x.TelaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Acao)
            .WithMany(a => a.TelasAcoes)
            .HasForeignKey(x => x.AcaoId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}