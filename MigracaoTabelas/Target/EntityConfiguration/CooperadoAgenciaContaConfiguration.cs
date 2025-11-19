using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class CooperadoAgenciaContaConfiguration : IEntityTypeConfiguration<CooperadoAgenciaConta>
{
    public void Configure(EntityTypeBuilder<CooperadoAgenciaConta> builder)
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
            .HasColumnType("char(9)")
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
            .HasDatabaseName("cooperado_agencia_conta_index_7");

        builder.HasIndex(x => x.CooperadoId)
            .HasDatabaseName("cooperado_agencia_conta_index_8");

        builder.HasIndex(x => x.AgenciaId)
            .HasDatabaseName("cooperado_agencia_conta_index_9");

        builder.HasIndex(x => x.ContaCorrente)
            .HasDatabaseName("cooperado_agencia_conta_index_10");
    }
}