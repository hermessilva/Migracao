using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MigracaoTabelas.Target.EntityConfiguration;

public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> builder)
    {
        builder.ToTable("auditoria");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .HasComment("Chave estrangeira da tabela usuario")
            .IsRequired();

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira da tabela agencia")
            .IsRequired();

        builder.Property(x => x.Modulo)
            .HasColumnName("modulo")
            .HasMaxLength(255)
            .HasComment("Módulo/área do sistema afetado")
            .IsRequired();

        builder.Property(x => x.Operacao)
            .HasColumnName("operacao")
            .HasColumnType("enum('Insert','Delete','Update')")
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<OperacaoAuditoria>(v)
            )
            .HasComment("Tipo da operação")
            .IsRequired();

        builder.Property(x => x.Antes)
            .HasColumnName("antes")
            .HasComment("Payload de dados antes da operação")
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasComment("Data/hora da ação registrada")
            .HasColumnType("datetime");

        builder.HasOne(x => x.Agencias)
            .WithMany(a => a.Auditorias)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Usuarios)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}