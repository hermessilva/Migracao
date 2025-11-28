using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

using Seguros.Helpers;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class ParcelaConfiguration : IEntityTypeConfiguration<Parcela>
{
    public void Configure(EntityTypeBuilder<Parcela> builder)
    {
        builder.ToTable("parcela", t => t.HasComment("Parcelas financeiras de prêmio vinculadas a um contrato de seguro"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.SeguroId)
            .HasColumnName("seguro_id")
            .HasComment("Chave estrangeira referenciando a tabela seguro")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("enum('Em Aberto','Pago','Cancelada')")
            .HasConversion(v =>
                v.AsString(),
                v => EnumHelper.FromString<StatusParcela>(v)
            )
            .HasComment("Status atual da parcela conforme enum status_seguro")
            .IsRequired();

        builder.Property(x => x.NumeroParcela)
            .HasColumnName("numero_parcela")
            .HasColumnType("smallint")
            .HasComment("Número sequencial da parcela dentro do seguro (1, 2, 3...)")
            .IsRequired();

        builder.Property(x => x.ValorParcela)
            .HasColumnName("valor_parcela")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor nominal atual da parcela a ser cobrado")
            .IsRequired();

        builder.Property(x => x.ValorOriginal)
            .HasColumnName("valor_original")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor original da parcela calculado na contratação")
            .IsRequired();

        builder.Property(x => x.ValorPago)
            .HasColumnName("valor_pago")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor total efetivamente pago na parcela")
            .IsRequired();

        builder.Property(x => x.Vencimento)
            .HasColumnName("vencimento")
            .HasColumnType("date")
            .HasComment("Data de vencimento da parcela")
            .IsRequired();

        builder.Property(x => x.Liquidacao)
            .HasColumnName("liquidacao")
            .HasColumnType("datetime")
            .HasComment("Data e hora de liquidação/quitação da parcela");

        builder.Property(x => x.DataUltimoPagamento)
            .HasColumnName("data_ultimo_pagamento")
            .HasColumnType("datetime")
            .HasComment("Data e hora do último pagamento parcial ou total registrado");

        // Relacionamentos
        builder.HasOne(x => x.Seguros)
            .WithMany(x => x.Parcelas)
            .HasForeignKey(x => x.SeguroId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}