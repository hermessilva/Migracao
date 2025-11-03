using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class ParcelaConfiguration : IEntityTypeConfiguration<Parcela>
{
    public void Configure(EntityTypeBuilder<Parcela> builder)
    {
        builder.ToTable("parcela");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.SeguroId)
            .HasColumnName("seguro_id")
            .HasComment("Chave estrangeira da tabela seguro")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("tinyint")
            .HasComment("Status da parcela (1 - Pendente, 2 - Pago, 3 - Cancelado)")
            .IsRequired();

        builder.Property(x => x.NumeroParcela)
            .HasColumnName("numero_parcela")
            .HasColumnType("smallint")
            .HasComment("Número sequencial da parcela")
            .IsRequired();

        builder.Property(x => x.ValorParcela)
            .HasColumnName("valor_parcela")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor original da parcela")
            .IsRequired();

        builder.Property(x => x.ValorPago)
            .HasColumnName("valor_pago")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor efetivamente pago da parcela")
            .IsRequired();

        builder.Property(x => x.Vencimento)
            .HasColumnName("vencimento")
            .HasColumnType("date")
            .HasComment("Data de vencimento da parcela")
            .IsRequired();

        builder.Property(x => x.Liquidacao)
            .HasColumnName("liquidacao")
            .HasColumnType("datetime")
            .HasComment("Data e hora da liquidação da parcela");

        builder.Property(x => x.DataUltimoPagamento)
            .HasColumnName("data_ultimo_pagamento")
            .HasColumnType("datetime")
            .HasComment("Data e hora do último pagamento realizado");

        // Relacionamentos
        builder.HasOne(x => x.Seguro)
            .WithMany(x => x.Parcelas)
            .HasForeignKey(x => x.SeguroId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}