using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace MigracaoTabelas.Target.EntityConfiguration;

public class IntegracaoSeniorConfiguration : BaseEntityConfiguration<IntegracaoSenior>
{
    public override void Configure(EntityTypeBuilder<IntegracaoSenior> builder)
    {
        builder.ToTable("integracao_senior", t => t.HasComment("Fila de controle de integrações contábeis com o sistema Sênior (ERP)"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira referenciando a tabela agencia")
            .IsRequired();

        builder.Property(x => x.ContaContabilCredito)
            .HasColumnName("conta_contabil_credito")
            .HasColumnType(VarChar(255))
            .HasComment("Código da conta contábil de crédito para o lançamento")
            .IsRequired();

        builder.Property(x => x.ContaContabilDebito)
            .HasColumnName("conta_contabil_debito")
            .HasColumnType(VarChar(255))
            .HasComment("Código da conta contábil de débito para o lançamento")
            .IsRequired();

        ConfigureEnum(builder.Property(x => x.Status)
            .HasColumnName("status"), "Enviado", "Falha")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<StatusEnvioIntegracaoSenior>(v)
            )
            .HasComment("Status da integração: Enviado (sucesso) ou Falha (erro no envio)")
            .IsRequired();

        builder.Property(x => x.DataMovimentacao)
            .HasColumnName("data_movimentacao")
            .HasColumnType(DateTime())
            .HasComment("Data e hora da movimentação a ser integrada")
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor monetário do lançamento a ser integrado")
            .IsRequired();

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .HasComment("Descrição detalhada do lançamento para identificação")
            .IsRequired();

        ConfigureEnum(builder.Property(x => x.TipoLancamentoContabil)
            .HasColumnName("tipo_lancamento_contabil"), "Seguro Prestamista Contratado", "Comissão Seguro Prestamista Contratado",
                                                       "Cancelamento Seguro Prestamista Parcelado Comissão", "Cancelamento Seguro Prestamista À Vista Proporcional Comissão",
                                                       "Pagamento Seguro Prestamista", "Recebimento Comissão Seguro Prestamista",
                                                       "Recebimento Premio Seguro Prestamista Parcelado", "Recebimento Comissão Seguro Prestamista Parcelado")
            .HasConversion(v =>
                v.AsString(),
                v => EnumHelper.FromString<TipoLancamentoContabilIntegracaoSenior>(v)
            )
            .HasComment("Tipo do lançamento contábil conforme enum tipo_lancamento")
            .IsRequired();

        builder.Property(x => x.CodigoPa)
            .HasColumnName("codigo_pa")
            .HasColumnType(Char(3))
            .HasComment("Código do ponto de atendimento de origem do lançamento")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Agencia)
            .WithMany(x => x.IntegracoesSeniores)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}