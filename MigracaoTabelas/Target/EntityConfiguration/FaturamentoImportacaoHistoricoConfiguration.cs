using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace MigracaoTabelas.Target.EntityConfiguration;

public class FaturamentoImportacaoHistoricoConfiguration : BaseEntityConfiguration<FaturamentoImportacaoHistorico>
{
    public override void Configure(EntityTypeBuilder<FaturamentoImportacaoHistorico> builder)
    {
        builder.ToTable("faturamento_importacao_historico", t =>
            t.HasComment("Histórico de importações de arquivos de retorno das seguradoras para faturamento"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro de importação")
            .IsRequired();

        builder.Property(x => x.SeguradoraId)
            .HasColumnName("seguradora_id")
            .HasComment("Chave estrangeira referenciando a seguradora do arquivo importado")
            .IsRequired();

        builder.Property(x => x.Lote)
            .HasColumnName("lote")
            .HasColumnType(VarChar(50))
            .HasComment("Número do lote/fatura identificado no arquivo importado")
            .IsRequired();

        builder.Property(x => x.NomeArquivo)
            .HasColumnName("nome_arquivo")
            .HasColumnType(VarChar(255))
            .HasComment("Nome do arquivo importado pelo usuário")
            .IsRequired();

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .HasComment("Chave estrangeira referenciando o usuário que realizou a importação")
            .IsRequired();

        ConfigureEnum(
                builder.Property(x => x.TipoPagamento)
                    .HasColumnName("tipo_pagamento")
                    .HasConversion(v => v.AsString(), v => EnumHelper.FromString<TipoPagamentoSeguro>(v))
                    .HasComment("Tipo de pagamento: À Vista, Parcelado ou Único"),
                "À Vista", "Parcelado", "Único"
            )
            .IsRequired();

        ConfigureEnum(
                builder.Property(x => x.Origem)
                    .HasColumnName("origem")
                    .HasConversion(v => v.AsString(), v => EnumHelper.FromString<TipoFaturamentoParcelaOrigem>(v))
                    .HasComment("Origem da importação: Manual ou Automatico"),
                "Manual", "Automatico"
            )
            .IsRequired();

        ConfigureEnum(
                builder.Property(x => x.StatusImportacao)
                    .HasColumnName("status_importacao")
                    .HasConversion(v => v.AsString(), v => EnumHelper.FromString<StatusImportacaoFaturamento>(v))
                    .HasComment("Status da importação: Processado, Erro ou Processado Parcialmente"),
                "Processado", "Erro", "Processado Parcialmente"
            )
            .IsRequired();

        builder.Property(x => x.TotalRegistrosArquivo)
            .HasColumnName("total_registros_arquivo")
            .HasColumnType(Int())
            .HasComment("Total de registros/linhas encontrados no arquivo")
            .IsRequired();

        builder.Property(x => x.QuantidadeSegurosAceitos)
            .HasColumnName("quantidade_seguros_aceitos")
            .HasColumnType(Int())
            .HasComment("Quantidade de seguros com status aceito no arquivo")
            .IsRequired();

        builder.Property(x => x.QuantidadeSegurosNaoAceitos)
            .HasColumnName("quantidade_seguros_nao_aceitos")
            .HasColumnType(Int())
            .HasComment("Quantidade de seguros com status recusado ou pendente no arquivo")
            .IsRequired();

        builder.Property(x => x.ValorTotalSegurosAceitos)
            .HasColumnName("valor_total_seguros_aceitos")
            .HasColumnType(Decimal(18, 2))
            .HasComment("Valor total dos prêmios dos seguros aceitos")
            .IsRequired();

        builder.Property(x => x.ValorTotalSegurosNaoAceitos)
            .HasColumnName("valor_total_seguros_nao_aceitos")
            .HasColumnType(Decimal(18, 2))
            .HasComment("Valor total dos prêmios dos seguros não aceitos")
            .IsRequired();

        builder.Property(x => x.ValorTotalArquivo)
            .HasColumnName("valor_total_arquivo")
            .HasColumnType(Decimal(18, 2))
            .HasComment("Valor total geral de todos os registros do arquivo")
            .IsRequired();

        builder.Property(x => x.ValorTotalFaturar)
            .HasColumnName("valor_total_faturar")
            .HasColumnType(Decimal(18, 2))
            .HasComment("Valor total a ser faturado (aceitos)")
            .IsRequired();

        builder.Property(x => x.Observacao)
            .HasColumnName("observacao")
            .HasColumnType(MediumText())
            .HasComment("Observações adicionais ou mensagem de erro da importação")
            .IsRequired(false);

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasColumnType(DateTime())
            .HasComment("Data e hora de criação do registro de importação")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguradora)
            .WithMany()
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índices
        builder.HasIndex(x => x.SeguradoraId)
            .HasDatabaseName("idx_fat_imp_hist_seguradora_id");

        builder.HasIndex(x => x.Lote)
            .HasDatabaseName("idx_fat_imp_hist_lote");

        builder.HasIndex(x => x.UsuarioId)
            .HasDatabaseName("idx_fat_imp_hist_usuario_id");

        builder.HasIndex(x => x.StatusImportacao)
            .HasDatabaseName("idx_fat_imp_hist_status_importacao");

        builder.HasIndex(x => x.CriadoEm)
            .HasDatabaseName("idx_fat_imp_hist_criado_em");
    }
}
