using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class TelaAcaoPerfilConfiguration : BaseEntityConfiguration<TelaAcaoPerfil>
{
    public override void Configure(EntityTypeBuilder<TelaAcaoPerfil> builder)
    {
        builder.ToTable("tela_acao_perfil");

        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.TelaId, x.AcaoId, x.PerfilId });

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

        builder.Property(x => x.PerfilId)
            .HasColumnName("perfil_id")
            .HasComment("Chave estrangeira da tabela perfil")
            .IsRequired();

        builder.HasIndex(x => x.TelaId)
            .HasDatabaseName("tela_acao_perfil_index_4");

        builder.HasIndex(x => x.AcaoId)
            .HasDatabaseName("tela_acao_perfil_index_5");

        builder.HasIndex(x => x.PerfilId)
            .HasDatabaseName("tela_acao_perfil_index_6");

        builder.HasOne(x => x.Telas)
            .WithMany(t => t.TelasAcoesPerfis)
            .HasForeignKey(x => x.TelaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Acoes)
            .WithMany(a => a.TelasAcoesPerfis)
            .HasForeignKey(x => x.AcaoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Perfils)
            .WithMany(p => p.TelasAcoesPerfis)
            .HasForeignKey(x => x.PerfilId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}