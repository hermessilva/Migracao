using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class LancamentoEfetivarConfiguration : IEntityTypeConfiguration<LancamentoEfetivar>
{
    public void Configure(EntityTypeBuilder<LancamentoEfetivar> builder)
    {
        builder.ToTable("lancamento_efetivar");

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
            .HasComment("Número da conta corrente do cooperado")
            .IsRequired();

        builder.Property(x => x.DataMovimentacao)
            .HasColumnName("data_movimentacao")
            .HasColumnType("datetime")
            .HasComment("Data e hora da movimentação");

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .HasComment("Descrição do lançamento a efetivar")
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor do lançamento")
            .IsRequired();

        builder.Property(x => x.DataLancamento)
            .HasColumnName("data_lancamento")
            .HasColumnType("date")
            .HasComment("Data prevista para o lançamento");

        // Relacionamentos
        builder.HasOne(x => x.Agencia)
            .WithMany(x => x.LancamentosEfetivar)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Cooperado)
            .WithMany(x => x.LancamentosEfetivar)
            .HasForeignKey(x => x.CooperadoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}