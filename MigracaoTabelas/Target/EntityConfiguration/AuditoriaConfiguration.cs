using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;




namespace MigracaoTabelas.Target.EntityConfiguration;

public class AuditoriaConfiguration : BaseEntityConfiguration<Auditoria>
{
    public override void Configure(EntityTypeBuilder<Auditoria> builder)
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
            .IsRequired(false);

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira referenciando a tabela agencia onde a ação foi realizada")
            .IsRequired(false);

        builder.Property(x => x.Chave)
            .HasColumnName("chave")
            .HasComment("Chave primária da tabela auditada")
            .IsRequired(false);

        builder.Property(x => x.Tabela)
            .HasColumnName("tabela")
            .HasMaxLength(255)
            .HasComment("Nome da tabela auditada")
            .IsRequired();

        builder.Property(x => x.Modulo)
            .HasColumnName("modulo")
            .HasMaxLength(255)
            .HasComment("Nome do módulo ou área do sistema onde a operação foi realizada")
            .IsRequired(false);

        builder.Property(x => x.Rota)
            .HasColumnName("rota")
            .HasMaxLength(255)
            .HasComment("Rota onde a operação foi realizada")
            .IsRequired(false);

        ConfigureEnum(builder.Property(x => x.Operacao)
            .HasColumnName("operacao"), "Inserção", "Atualização", "Deleção", "Login", "Refresh Token")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<OperacaoAuditoria>(v)
            )
            .HasComment("Tipo da operação realizada: Insert (inserção), Delete (exclusão) ou Update (atualização)")
            .IsRequired();

        builder.Property(x => x.DadosAnteriores)
            .HasColumnName("dados_anteriores")
            .HasMaxLength(8000)
            .HasComment("Dados do registro antes da alteração em formato JSON ou serializado")
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasComment("Data e hora em que a ação foi registrada")
            .HasColumnType(DateTime());
    }
}