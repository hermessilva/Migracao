using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class CooperadoAgenciaContaConfiguration : BaseEntityConfiguration<CooperadoAgenciaConta>
{
    public override void Configure(EntityTypeBuilder<CooperadoAgenciaConta> builder)
    {
        builder.ToTable("cooperado_agencia_conta", t => t.HasComment("Junção entre cooperados, agencias e contas"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.CooperadoId)
            .HasColumnName("cooperado_id")
            .HasComment("Chave estrangeira da tabela cooperado")
            .IsRequired();

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira da tabela agencia")
            .IsRequired();

        builder.Property(x => x.ContaCorrente)
            .HasColumnName("conta_corrente")
            .HasColumnType(Char(9))
            .HasComment("Código da conta corrente")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Cooperados)
            .WithMany(x => x.CooperadosAgenciasContas)
            .HasForeignKey(x => x.CooperadoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Agencias)
            .WithMany(x => x.CooperadosAgenciasContas)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índices
        builder.HasIndex(x => new { x.CooperadoId, x.AgenciaId, x.ContaCorrente })
            .IsUnique()
            .HasDatabaseName("idx_cooperado_id_agencia_id_conta_corrente");

        builder.HasIndex(x => x.ContaCorrente)
            .HasDatabaseName("idx_conta_corrente");
    }
}