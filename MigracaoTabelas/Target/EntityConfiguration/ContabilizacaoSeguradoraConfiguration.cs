using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class ContabilizacaoSeguradoraConfiguration : IEntityTypeConfiguration<ContabilizacaoSeguradora>
{
    public void Configure(EntityTypeBuilder<ContabilizacaoSeguradora> builder)
    {
        builder.ToTable("contabilizacao_seguradora");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.SeguradoraId)
            .HasColumnName("seguradora_id")
            .HasComment("Chave estrangeira da tabela seguradora")
            .IsRequired();

        builder.Property(x => x.ContaContabilId)
            .HasColumnName("conta_contabil_id")
            .HasComment("Chave estrangeira da tabela conta_contabil")
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasColumnName("tipo")
            .HasColumnType("enum('Crédito','Débito','Crédito Comissão', 'Débito Comissão')")
            .HasConversion<string>()
            .HasComment("Tipo de conta")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguradora)
            .WithMany(x => x.ContabilizacaoSeguradoras)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ContaContabil)
            .WithMany(x => x.ContabilizacaoSeguradoras)
            .HasForeignKey(x => x.ContaContabilId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índices
        builder.HasIndex(x => new { x.SeguradoraId, x.ContaContabilId })
            .IsUnique();

        builder.HasIndex(x => x.SeguradoraId);
        builder.HasIndex(x => x.ContaContabilId);
    }
}