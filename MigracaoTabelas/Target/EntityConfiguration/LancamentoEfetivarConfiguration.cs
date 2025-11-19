using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class LancamentoEfetivarConfiguration : IEntityTypeConfiguration<LancamentoEfetivar>
{
    public void Configure(EntityTypeBuilder<LancamentoEfetivar> builder)
    {
        builder.ToTable("lancamento_efetivar", t => t.HasComment("Lançamentos financeiros que serão/são efetivados"));

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

        builder.Property(x => x.CooperadoId)
            .HasColumnName("cooperado_id")
            .HasComment("Chave estrangeira da tabela cooperado")
            .IsRequired();

        builder.Property(x => x.ContaCorrente)
            .HasColumnName("conta_corrente")
            .HasMaxLength(255)
            .HasComment("Conta corrente associada ao lançamento")
            .IsRequired();

        builder.Property(x => x.DataMovimentacao)
            .HasColumnName("data_movimentacao")
            .HasColumnType("datetime")
            .HasComment("Data de movimentação financeira");

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .HasComment("Descrição do lançamento")
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor do lançamento")
            .IsRequired();

        builder.Property(x => x.DataLancamento)
            .HasColumnName("data_lancamento")
            .HasColumnType("date")
            .HasComment("Data de lançamento/registro no sistema");

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