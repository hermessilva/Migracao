using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;



namespace MigracaoTabelas.Target.EntityConfiguration;

/// <summary>
/// Configuração do Entity Framework para a entidade FaturamentoParcela
/// </summary>
public class FaturamentoParcelaConfiguration : BaseEntityConfiguration<FaturamentoParcela>
{
    public override void Configure(EntityTypeBuilder<FaturamentoParcela> builder)
    {
        builder.ToTable("faturamento_parcela", t => 
            t.HasComment("Registros de faturamento e cancelamento de parcelas do seguro prestamista"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro de faturamento")
            .IsRequired();

        builder.Property(x => x.ParcelaId)
            .HasColumnName("parcela_id")
            .HasComment("Chave estrangeira referenciando a tabela parcela")
            .IsRequired();

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .HasComment("Chave estrangeira referenciando a tabela usuario que realizou a operação")
            .IsRequired();

        builder.Property(x => x.Lote)
            .HasColumnName("lote")
            .HasColumnType(VarChar(50))
            .HasComment("Número do lote/fatura informado pelo usuário no momento do faturamento")
            .IsRequired();

        ConfigureEnum(
                builder.Property(x => x.Status)
                    .HasColumnName("status")
                    .HasConversion(v =>
                        v.AsString(),
                        v => EnumHelper.FromString<TipoFaturamentoParcelaStatus>(v)
                    )
                    .HasComment("Status do faturamento: Faturado, Cancelado ou Em Processamento"),
                "Faturado", "Cancelado", "EmProcessamento"
            )
            .IsRequired();

        ConfigureEnum(
                builder.Property(x => x.Origem)
                    .HasColumnName("origem")
                    .HasConversion(v =>
                        v.AsString(),
                        v => EnumHelper.FromString<TipoFaturamentoParcelaOrigem>(v)
                    )
                    .HasComment("Origem do faturamento: Manual ou Automatico"),
                "Manual", "Automatico"
            )
            .IsRequired();

        builder.Property(x => x.DataPagamento)
            .HasColumnName("data_pagamento")
            .HasColumnType(Date())
            .HasComment("Data de pagamento informada no momento do faturamento")
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasColumnType(DateTime())
            .HasComment("Data e hora de criação do registro")
            .IsRequired();

        // Relacionamentos
        // 1:1 opcional - Uma parcela pode ter no máximo um faturamento
        // A propriedade Parcela.FaturamentoParcela é nullable, tornando o relacionamento opcional
        builder.HasOne(x => x.Parcela)
            .WithOne(p => p.FaturamentoParcela)
            .HasForeignKey<FaturamentoParcela>(x => x.ParcelaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índices para consultas frequentes
        builder.HasIndex(x => x.ParcelaId)
            .HasDatabaseName("idx_faturamento_parcela_parcela_id");

        builder.HasIndex(x => x.Lote)
            .HasDatabaseName("idx_faturamento_parcela_lote");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("idx_faturamento_parcela_status");

        builder.HasIndex(x => x.CriadoEm)
            .HasDatabaseName("idx_faturamento_parcela_criado_em");
    }
}
