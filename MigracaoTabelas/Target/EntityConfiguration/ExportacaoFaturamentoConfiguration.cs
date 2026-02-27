using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

/// <summary>
/// Configuração EF Core para a entidade ExportacaoFaturamento
/// </summary>
public class ExportacaoFaturamentoConfiguration : BaseEntityConfiguration<ExportacaoFaturamento>
{
    public override void Configure(EntityTypeBuilder<ExportacaoFaturamento> builder)
    {
        builder.ToTable("faturamento_exportacao");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.SeguradoraId)
            .HasColumnName("seguradora_id")
            .IsRequired();

        builder.Property(e => e.UsuarioNome)
            .HasColumnName("usuario_nome")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.UsuarioEmail)
            .HasColumnName("usuario_email")
            .HasMaxLength(255)
            .IsRequired();

        ConfigureEnum(builder.Property(e => e.TipoPagamento)
            .HasColumnName("tipo_pagamento")
            .HasComment("Tipo de pagamento do seguro: À Vista ou Parcelado")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<TipoPagamentoSeguro>(v))
            .IsRequired(), "À Vista", "Parcelado");

        builder.Property(e => e.PeriodoInicio)
            .HasColumnName("periodo_inicio")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(e => e.PeriodoFim)
            .HasColumnName("periodo_fim")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(e => e.QuantidadeRegistros)
            .HasColumnName("quantidade_registros")
            .IsRequired();

        ConfigureEnum(builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasComment("Status da exportação: Sucesso ou Erro")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<StatusExportacao>(v))
            .IsRequired(), "Sucesso", "Erro");

        builder.Property(e => e.NomeArquivo)
            .HasColumnName("nome_arquivo")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.ArquivoHash)
            .HasColumnName("arquivo_hash")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(e => e.MensagemErro)
            .HasColumnName("mensagem_erro")
            .HasColumnType("text");

        builder.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasColumnType("datetime")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(e => e.Seguradora)
            .WithMany()
            .HasForeignKey(e => e.SeguradoraId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(e => e.SeguradoraId)
            .HasDatabaseName("idx_faturamento_exportacao_seguradora_id");

        builder.HasIndex(e => e.UsuarioEmail)
            .HasDatabaseName("idx_faturamento_exportacao_usuario_email");

        builder.HasIndex(e => e.ArquivoHash)
            .HasDatabaseName("idx_faturamento_exportacao_arquivo_hash");

        builder.HasIndex(e => e.CriadoEm)
            .HasDatabaseName("idx_faturamento_exportacao_criado_em");

        builder.HasIndex(e => new { e.SeguradoraId, e.TipoPagamento, e.PeriodoInicio, e.PeriodoFim })
            .HasDatabaseName("idx_faturamento_exportacao_filtros");
    }
}
