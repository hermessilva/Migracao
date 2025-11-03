using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class TelaAcaoPerfilConfiguration : IEntityTypeConfiguration<TelaAcaoPerfil>
{
    public void Configure(EntityTypeBuilder<TelaAcaoPerfil> builder)
    {
        builder.ToTable("tela_acao_perfil");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela");

        builder.Property(x => x.TelaId)
            .HasColumnName("tela_id")
            .HasComment("Chave estrangeira da tabela tela");

        builder.Property(x => x.AcaoId)
            .HasColumnName("acao_id")
            .HasComment("Chave estrangeira da tabela ação");

        builder.Property(x => x.PerfilId)
            .HasColumnName("perfil_id")
            .HasComment("Chave estrangeira da tabela perfil");

        builder.HasIndex(x => x.TelaId)
            .HasDatabaseName("idx_tela_acao_perfil_tela_id");

        builder.HasIndex(x => x.AcaoId)
            .HasDatabaseName("idx_tela_acao_perfil_acao_id");

        builder.HasIndex(x => x.PerfilId)
            .HasDatabaseName("idx_tela_acao_perfil_perfil_id");

        builder.HasOne<Tela>()
            .WithMany()
            .HasForeignKey(x => x.TelaId);

        builder.HasOne<Acao>()
            .WithMany()
            .HasForeignKey(x => x.AcaoId);

        builder.HasOne<Perfil>()
            .WithMany()
            .HasForeignKey(x => x.PerfilId);
    }
}