using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MigracaoTabelas.Target.EntityConfiguration;

public sealed class SeguroHistoricoPagamentoConfiguration : BaseEntityConfiguration<SeguroHistoricoPagamento>
{
    public override void Configure(EntityTypeBuilder<SeguroHistoricoPagamento> builder)
    {
        builder.ToTable("seguro_historico_pagamento", t =>
            t.HasComment("Histórico de pagamentos e estornos vinculados às parcelas de seguros"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.ParcelaId)
            .HasColumnName("parcela_id")
            .HasComment("Chave estrangeira referenciando a tabela parcela associada ao pagamento")
            .IsRequired();

        builder.Property(x => x.DataPagamento)
            .HasColumnName("data_pagamento")
            .HasColumnType(DateTime())
            .HasComment("Data e hora do pagamento registrado")
            .IsRequired();

        builder.Property(x => x.ValorPago)
            .HasColumnName("valor_pago")
            .HasColumnType(Decimal(15, 2))
            .HasComment("Valor efetivamente pago na transação")
            .IsRequired();

        ConfigureEnum(
                builder.Property(x => x.Operacao)
                    .HasColumnName("operacao")
                    .HasConversion(
                        v => v.AsString(),
                        v => EnumHelper.FromString<OperacaoSeguroHistoricoPagamento>(v)
                    )
                    .HasComment("Tipo de operação do pagamento: Recebimento ou Estorno"),
                "Recebimento", "Estorno"
            )
            .IsRequired();

        builder.HasOne(x => x.Parcela)
            .WithMany(x => x.SeguroHistoricoPagamentos)
            .HasForeignKey(x => x.ParcelaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
