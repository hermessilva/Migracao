using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class LancamentoEfetivarConfiguration : IEntityTypeConfiguration<LancamentoEfetivar>
{
    public void Configure(EntityTypeBuilder<LancamentoEfetivar> builder)
    {
        builder.ToTable("lancamento_efetivar", t => t.HasComment("Fila de lançamentos financeiros pendentes de efetivação nas contas dos cooperados"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira referenciando a tabela agencia")
            .IsRequired();

        builder.Property(x => x.CooperadoId)
            .HasColumnName("cooperado_id")
            .HasComment("Chave estrangeira referenciando a tabela cooperado")
            .IsRequired();

        builder.Property(x => x.ContaCorrente)
            .HasColumnName("conta_corrente")
            .HasMaxLength(255)
            .HasComment("Número da conta corrente do cooperado para débito/crédito do lançamento")
            .IsRequired();

        builder.Property(x => x.DataMovimentacao)
            .HasColumnName("data_movimentacao")
            .HasColumnType("datetime")
            .HasComment("Data e hora da movimentação financeira no sistema de origem");

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .HasComment("Descrição detalhada do lançamento a ser efetivado")
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor monetário do lançamento a ser efetivado")
            .IsRequired();

        builder.Property(x => x.DataLancamento)
            .HasColumnName("data_lancamento")
            .HasColumnType("date")
            .HasComment("Data programada para efetivação do lançamento no sistema");

        // Relacionamentos
        builder.HasOne(x => x.Agencias)
            .WithMany(x => x.LancamentosEfetivar)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Cooperados)
            .WithMany(x => x.LancamentosEfetivar)
            .HasForeignKey(x => x.CooperadoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}