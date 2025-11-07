using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class PriorizacaoConfiguration : IEntityTypeConfiguration<Priorizacao>
{
    public void Configure(EntityTypeBuilder<Priorizacao> builder)
    {
        builder.ToTable("priorizacao");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira da tabela agencia")
            .IsRequired();

        builder.Property(x => x.SeguradoraId)
            .HasColumnName("seguradora_id")
            .HasComment("Chave estrangeira da tabela seguradora")
            .IsRequired();

        builder.Property(x => x.Ordem)
            .HasColumnName("ordem")
            .HasColumnType("tinyint")
            .HasComment("ordem de prioridade dentro da agência")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Agencias)
            .WithMany(x => x.Priorizacoes)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.Priorizacoes)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índices
        builder.HasIndex(x => x.AgenciaId);
        builder.HasIndex(x => x.SeguradoraId);

        // Índice único composto: uma seguradora não pode aparecer duas vezes na mesma agência
        builder.HasIndex(x => new { x.AgenciaId, x.SeguradoraId })
            .IsUnique();

        // Índice único composto: não pode haver duas seguradoras com a mesma ordem na mesma agência
        builder.HasIndex(x => new { x.AgenciaId, x.Ordem })
            .IsUnique();
    }
}

