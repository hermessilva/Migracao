using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Seguros.Helpers;


namespace MigracaoTabelas.Target.EntityConfiguration;

public class ParcelaConfiguration : BaseEntityConfiguration<Parcela>
{
    public override void Configure(EntityTypeBuilder<Parcela> builder)
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

        ConfigureEnum(
                builder.Property(x => x.Status)
                    .HasColumnName("status")
                    .HasConversion(v =>
                        v.AsString(),
                        v => EnumHelper.FromString<StatusParcela>(v)
                    )
                    .HasComment("Status atual da parcela conforme enum status_seguro"),
                "Pendente", "Em Aberto", "Pago", "Cancelada"
            )
            .IsRequired();

        builder.Property(x => x.NumeroParcela)
            .HasColumnName("numero_parcela")
            .HasColumnType(SmallInt())
            .HasComment("Número sequencial da parcela dentro do seguro (1, 2, 3...)")
            .IsRequired();

        builder.Property(x => x.ValorParcela)
            .HasColumnName("valor_parcela")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor nominal atual da parcela a ser cobrado")
            .IsRequired();

        builder.Property(x => x.ValorOriginal)
            .HasColumnName("valor_original")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor original da parcela calculado na contratação")
            .IsRequired();

        builder.Property(x => x.ValorPago)
            .HasColumnName("valor_pago")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor total efetivamente pago na parcela")
            .IsRequired();

        builder.Property(x => x.Vencimento)
            .HasColumnName("vencimento")
            .HasColumnType(Date())
            .HasComment("Data de vencimento da parcela")
            .IsRequired();

        builder.Property(x => x.Liquidacao)
            .HasColumnName("liquidacao")
            .HasColumnType(DateTime())
            .HasComment("Data e hora de liquidação/quitação da parcela");

        builder.Property(x => x.DataUltimoPagamento)
            .HasColumnName("data_ultimo_pagamento")
            .HasColumnType(DateTime())
            .HasComment("Data e hora do último pagamento parcial ou total registrado");

        builder.Property(x => x.ComissaoCorretora)
            .HasColumnName("comissao_corretora")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor da comissão do corretor sobre a parcela")
            .IsRequired();

        builder.Property(x => x.ComissaoCooperativa)
            .HasColumnName("comissao_cooperativa")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor da comissão da cooperativa sobre a parcela")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguros)
            .WithMany(x => x.Parcelas)
            .HasForeignKey(x => x.SeguroId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}