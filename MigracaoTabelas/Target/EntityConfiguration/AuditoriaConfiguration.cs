using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> builder)
    {
        builder.ToTable("auditoria", t => t.HasComment("Tabela de auditoria para rastreamento de todas as operações realizadas no sistema"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .HasComment("Chave estrangeira referenciando a tabela usuario que realizou a ação")
            .IsRequired();

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira referenciando a tabela agencia onde a ação foi realizada")
            .IsRequired();

        builder.Property(x => x.Modulo)
            .HasColumnName("modulo")
            .HasMaxLength(255)
            .HasComment("Nome do módulo ou área do sistema onde a operação foi realizada")
            .IsRequired();

        builder.Property(x => x.Operacao)
            .HasColumnName("operacao")
            .HasColumnType("enum('Insert','Delete','Update')")
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<OperacaoAuditoria>(v)
            )
            .HasComment("Tipo da operação realizada: Insert (inserção), Delete (exclusão) ou Update (atualização)")
            .IsRequired();

        builder.Property(x => x.Antes)
            .HasColumnName("antes")
            .HasMaxLength(255)
            .HasComment("Dados do registro antes da alteração em formato JSON ou serializado")
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasComment("Data e hora em que a ação foi registrada")
            .HasColumnType("datetime");

        builder.HasOne(x => x.Usuarios)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Agencias)
            .WithMany(a => a.Auditorias)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}