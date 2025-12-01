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

        builder.Property(x => x.CreditoPremioContratacao)
            .HasColumnName("credito_premio_contratacao")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil premio de credito")
            .IsRequired();

        builder.Property(x => x.DescricaoCreditoPremioContratacao)
            .HasColumnName("descricao_credito_premio_contratacao")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil premio de credito")
            .IsRequired();

        builder.Property(x => x.DebitoPremioContratacao)
            .HasColumnName("debito_premio_contratacao")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil premio de debito")
            .IsRequired();

        builder.Property(x => x.DescricaoDebitoPremioContratacao)
            .HasColumnName("descricao_debito_premio_contratacao")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil premio de debito")
            .IsRequired();

        builder.Property(x => x.CreditoComissaoContratacao)
            .HasColumnName("credito_comissao_contratacao")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil comissao de credito ")
            .IsRequired();

        builder.Property(x => x.DescricaoCreditoComissaoContratacao)
            .HasColumnName("descricao_credito_comissao_contratacao")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil comissao de credito")
            .IsRequired();

        builder.Property(x => x.DebitoComissaoContratacao)
            .HasColumnName("debito_comissao_contratacao")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil comissao de debito")
            .IsRequired();

        builder.Property(x => x.DescricaoDebitoComissaoContratacao)
            .HasColumnName("descricao_debito_comissao_contratacao")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil comissao de debito")
            .IsRequired();

        builder.Property(x => x.CreditoCancelamentoComissaoParcTot)
            .HasColumnName("credito_cancelamento_comissao_parc_tot")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil cancelamento parcial total de credito")
            .IsRequired();

        builder.Property(x => x.DescricaoCreditoCancelamentoComissaoParcTot)
            .HasColumnName("descricao_credito_cancelamento_comissao_parc_tot")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil cancelamento parcial total de credito")
            .IsRequired();

        builder.Property(x => x.DebitoCancelamentoComissaoParcTot)
            .HasColumnName("debito_cancelamento_comissao_parc_tot")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil cancelamento parcial total de debito")
            .IsRequired();

        builder.Property(x => x.DescricaoDebitoCancelamentoComissaoParcTot)
            .HasColumnName("descricao_debito_cancelamento_comissao_parc_tot")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil cancelamento parcial total de debito")
            .IsRequired();

        builder.Property(x => x.CreditoCancelamentoComissaoAVista)
            .HasColumnName("credito_cancelamento_comissao_avista")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil cancelamento comissao a vista credito")
            .IsRequired();

        builder.Property(x => x.DescricaoCreditoCancelamentoComissaoAVista)
            .HasColumnName("descricao_credito_cancelamento_comissao_avista")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil cancelamento comissao a vista credito")
            .IsRequired();

        builder.Property(x => x.DebitoCancelamentoComissaoAVista)
            .HasColumnName("debito_cancelamento_comissao_avista")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil cancelamento comissao a vista debito")
            .IsRequired();

        builder.Property(x => x.DescricaoDebitoCancelamentoComissaoAVista)
            .HasColumnName("descricao_debito_cancelamento_comissao_avista")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil cancelamento comissao a vista debito")
            .IsRequired();

        builder.Property(x => x.CreditoValorPago)
            .HasColumnName("credito_valor_pago")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil valor pago credito")
            .IsRequired();

        builder.Property(x => x.DescricaoCreditoValorPago)
            .HasColumnName("descricao_credito_valor_pago")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil valor pago credito")
            .IsRequired();

        builder.Property(x => x.DebitoValorPago)
            .HasColumnName("debito_valor_pago")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil valor pago debito")
            .IsRequired();

        builder.Property(x => x.DescricaoDebitoValorPago)
            .HasColumnName("descricao_debito_valor_pago")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil valor pago debito")
            .IsRequired();

        builder.Property(x => x.CreditoComissaoValorPago)
            .HasColumnName("credito_comissao_valor_pago")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil comissao valor pago credito")
            .IsRequired();

        builder.Property(x => x.DescricaoComissaoCreditoValorPago)
            .HasColumnName("descricao_comissao_credito_valor_pago")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil comissao valor pago credito")
            .IsRequired();

        builder.Property(x => x.DebitoComissaoValorPago)
            .HasColumnName("debito_comissao_valor_pago")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil comissao valor pago debito")
            .IsRequired();

        builder.Property(x => x.DescricaoComissaoDebitoValorPago)
            .HasColumnName("descricao_comissao_debito_valor_pago")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil comissao valor pago debito")
            .IsRequired();

        builder.Property(x => x.DebitoPremioParcela)
            .HasColumnName("debito_premio_parcela")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil debito premio parcela")
            .IsRequired();

        builder.Property(x => x.DescricaoDebitoPremioParcela)
            .HasColumnName("descricao_debito_premio_parcela")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil debito premio parcela")
            .IsRequired();
        
        builder.Property(x => x.CreditoPremioParcela)
            .HasColumnName("credito_premio_parcela")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil credito premio parcela")
            .IsRequired();

        builder.Property(x => x.DescricaoCreditoPremioParcela)
            .HasColumnName("descricao_credito_premio_parcela")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil credito premio parcela")
            .IsRequired();

        builder.Property(x => x.DebitoComissaoParcela)
            .HasColumnName("debito_comissao_parcela")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil debito comissao parcela")
            .IsRequired();

        builder.Property(x => x.DescricaoDebitoComissaoParcela)
            .HasColumnName("descricao_debito_comissao_parcela")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil debito comissao parcela")
            .IsRequired();

        builder.Property(x => x.CreditoComissaoParcela)
            .HasColumnName("credito_comissao_parcela")
            .HasMaxLength(50)
            .HasComment("Código da conta contábil credito comissao parcela")
            .IsRequired();

        builder.Property(x => x.DescricaoCreditoComissaoParcela)
            .HasColumnName("descricao_credito_comissao_parcela")
            .HasMaxLength(255)
            .HasComment("Descrição da conta contábil credito comissao parcela")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.ContabilizacoesSeguradoras)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índice único
        builder.HasIndex(x => x.SeguradoraId)
            .IsUnique()
            .HasDatabaseName("contabilizacao_seguradora_index_3");
    }
}