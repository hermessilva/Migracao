using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;



namespace MigracaoTabelas.Target.EntityConfiguration;

public class BaixaComissaoConfiguration : BaseEntityConfiguration<BaixaComissao>
{
    public override void Configure(EntityTypeBuilder<BaixaComissao> builder)
    {
        builder.ToTable("baixa_comissao", t =>
            t.HasComment("Registros de baixa de comissão de faturamento de seguro prestamista, agrupados por seguradora, lote e competência"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id").ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro de baixa de comissão").IsRequired();

        builder.Property(x => x.SeguradoraId)
            .HasColumnName("seguradora_id")
            .HasComment("Chave estrangeira referenciando a tabela seguradora").IsRequired();

        builder.Property(x => x.Lote)
            .HasColumnName("lote").HasColumnType(VarChar(50))
            .HasComment("Número do lote/fatura oriundo do faturamento").IsRequired();

        builder.Property(x => x.Competencia)
            .HasColumnName("competencia").HasColumnType(Date())
            .HasComment("Mês e ano correspondente ao movimento do faturamento (vencimento da parcela)").IsRequired();

        ConfigureEnum(
                builder.Property(x => x.TipoPagamento).HasColumnName("tipo_pagamento")
                    .HasConversion(v => v.AsString(), v => EnumHelper.FromString<TipoPagamentoSeguro>(v))
                    .HasComment("Tipo de pagamento do seguro: À Vista, Parcelado ou Único"),
                "À Vista", "Parcelado", "Único"
            ).IsRequired();

        builder.Property(x => x.DataEmissao)
            .HasColumnName("data_emissao").HasColumnType(Date())
            .HasComment("Data de expectativa de pagamento de comissão indicada no faturamento").IsRequired();

        builder.Property(x => x.ValorComissao)
            .HasColumnName("valor_comissao").HasColumnType(Decimal(14, 2))
            .HasComment("Valor total de comissão provisionada para o lote/fatura").IsRequired();

        builder.Property(x => x.QuantidadeCooperados)
            .HasColumnName("quantidade_cooperados").HasColumnType(Int())
            .HasComment("Número total de movimentos faturados no lote").IsRequired();

        builder.Property(x => x.DataRecebimento)
            .HasColumnName("data_recebimento").HasColumnType(Date())
            .HasComment("Data em que a comissão foi efetivamente recebida/baixada");

        ConfigureEnum(
                builder.Property(x => x.Situacao).HasColumnName("situacao")
                    .HasConversion(v => v.AsString(), v => EnumHelper.FromString<SituacaoBaixaComissao>(v))
                    .HasComment("Situação da comissão: Pendente ou Recebida"),
                "Pendente", "Recebida"
            ).IsRequired();

        builder.Property(x => x.UsuarioBaixaId)
            .HasColumnName("usuario_baixa_id")
            .HasComment("Chave estrangeira referenciando o usuário que realizou a baixa");

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em").HasColumnType(DateTime())
            .HasComment("Data e hora de criação do registro").IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguradora).WithMany()
            .HasForeignKey(x => x.SeguradoraId).OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.UsuarioBaixa).WithMany()
            .HasForeignKey(x => x.UsuarioBaixaId).OnDelete(DeleteBehavior.NoAction);

        // Índices
        builder.HasIndex(x => x.SeguradoraId).HasDatabaseName("idx_baixa_comissao_seguradora_id");
        builder.HasIndex(x => x.Lote).HasDatabaseName("idx_baixa_comissao_lote");
        builder.HasIndex(x => x.Competencia).HasDatabaseName("idx_baixa_comissao_competencia");
        builder.HasIndex(x => x.Situacao).HasDatabaseName("idx_baixa_comissao_situacao");
        builder.HasIndex(x => new { x.SeguradoraId, x.Lote, x.Competencia, x.TipoPagamento })
            .IsUnique()
            .HasDatabaseName("idx_baixa_comissao_seguradora_lote_competencia_tipo_pagamento");
    }
}
