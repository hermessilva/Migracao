using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;


namespace MigracaoTabelas.Target.EntityConfiguration;

public class ContaCorrenteSeguradoraConfiguration : BaseEntityConfiguration<ContaCorrenteSeguradora>
{
    public override void Configure(EntityTypeBuilder<ContaCorrenteSeguradora> builder)
    {
        builder.ToTable("conta_corrente_seguradora", t => t.HasComment("Junção entre parâmetros de seguradora e contas correntes"));

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

        builder.Property(x => x.ContaCorrentePrestamista)
            .HasColumnName("conta_corrente_prestamista")
            .HasMaxLength(50)
            .HasComment("Numero da conta corrente de seguro prestamista")
            .IsRequired();

        builder.Property(x => x.DescricaoContaCorrentePrestamista)
            .HasColumnName("descricao_conta_corrente_prestamista")
            .HasMaxLength(255)
            .HasComment("Descrição da conta corrente de seguro prestamista")
            .IsRequired();

        builder.Property(x => x.ContaCancelamentoPrestamista)
            .HasColumnName("conta_cancelamento_prestamista")
            .HasMaxLength(50)
            .HasComment("Numero da conta de cancelamento de seguro prestamista")
            .IsRequired();

        builder.Property(x => x.DescricaoContaCancelamentoPrestamista)
            .HasColumnName("descricao_conta_cancelamento_prestamista")
            .HasMaxLength(255)
            .HasComment("Descrição da conta de cancelamento de seguro prestamista")
            .IsRequired();

        builder.Property(x => x.ContaEstornoPrestamista)
            .HasColumnName("conta_estorno_prestamista")
            .HasMaxLength(50)
            .HasComment("Numero da conta de estorno de seguro prestamista")
            .IsRequired();

        builder.Property(x => x.DescricaoContaEstornoPrestamista)
            .HasColumnName("descricao_conta_estorno_prestamista")
            .HasMaxLength(255)
            .HasComment("Descrição da conta de estorno de seguro prestamista")
            .IsRequired();

        builder.Property(x => x.ContaCancelamentoPrestamista)
            .HasColumnName("conta_cancelamento_prestamista")
            .HasMaxLength(50)
            .HasComment("Numero da conta de cancelamento de seguro prestamista")
            .IsRequired();

        builder.Property(x => x.DescricaoContaCancelamentoPrestamista)
            .HasColumnName("descricao_conta_cancelamento_prestamista")
            .HasMaxLength(255)
            .HasComment("Descricao da conta de cancelamento de seguro prestamista")
            .IsRequired();

        builder.Property(x => x.ContaEstornoPrestamista)
            .HasColumnName("conta_estorno_prestamista")
            .HasMaxLength(50)
            .HasComment("Numero da conta de estorno de seguro prestamista")
            .IsRequired();

        builder.Property(x => x.DescricaoContaEstornoPrestamista)
            .HasColumnName("descricao_conta_estorno_prestamista")
            .HasMaxLength(255)
            .HasComment("Descricao da conta de estorno de seguro prestamista")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.ContasCorrentes)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índice único
        builder.HasIndex(x => x.SeguradoraId)
            .IsUnique()
            .HasDatabaseName("conta_corrente_seguradora_index_4");
    }
}