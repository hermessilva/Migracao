using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class CooperadoAgenciaContaConfiguration : IEntityTypeConfiguration<CooperadoAgenciaConta>
{
    public void Configure(EntityTypeBuilder<CooperadoAgenciaConta> builder)
    {
        builder.ToTable("cooperado_agencia_conta");

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
        builder.HasOne(x => x.Cooperado)
            .WithMany(x => x.CooperadoAgenciaContas)
            .HasForeignKey(x => x.CooperadoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Agencia)
            .WithMany(x => x.CooperadoAgenciaContas)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índices
        builder.HasIndex(x => new { x.CooperadoId, x.AgenciaId, x.ContaCorrente })
            .IsUnique();

        builder.HasIndex(x => x.CooperadoId);
        builder.HasIndex(x => x.AgenciaId);
        builder.HasIndex(x => x.ContaCorrente);
    }
}