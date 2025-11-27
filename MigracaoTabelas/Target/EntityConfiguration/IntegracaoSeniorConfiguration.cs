using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace MigracaoTabelas.Target.EntityConfiguration;

public class IntegracaoSeniorConfiguration : IEntityTypeConfiguration<IntegracaoSenior>
{
    public void Configure(EntityTypeBuilder<IntegracaoSenior> builder)
    {
        builder.ToTable("integracao_senior");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira da tabela agencia")
            .IsRequired();

        builder.Property(x => x.ContaContabilCredito)
            .HasColumnName("conta_contabil_credito")
            .HasColumnType("varchar(255)")
            .HasComment("Conta contábil de crédito")
            .IsRequired();

        builder.Property(x => x.ContaContabilDebito)
            .HasColumnName("conta_contabil_debito")
            .HasColumnType("varchar(255)")
            .HasComment("Conta contábil de débito")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("enum('Enviado', 'Falha')")
            .HasConversion<string>()
            .HasComment("Status integração")
            .IsRequired();

        builder.Property(x => x.DataMovimentacao)
            .HasColumnName("data_movimentacao")
            .HasColumnType("datetime")
            .HasComment("Data e hora da movimentação")
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor da movimentação")
            .IsRequired();

        builder.Property(x => x.NumeroLancamento)
            .HasColumnName("numero_lancamento")
            .HasColumnType("int")
            .HasComment("Identificador do lançamento associado")
            .IsRequired();

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .HasComment("Descrição da integração")
            .IsRequired();

        builder.Property(x => x.TipoLancamentoContabil)
            .HasColumnName("tipo_lancamento_contabil")
            .HasColumnType("enum('Seguro Prestamista Contratado', 'Comissão Seguro Prestamista Contratado', 'Cancelamento Seguro Prestamista Parcelado', 'Cancelamento Seguro Prestamista Parcelado Comissão', 'Cancelamento Seguro Prestamista À Vista Proporcional Comissão', 'Pagamento Seguro Prestamista', 'Recebimento Comissão Seguro Prestamista')")
            .HasConversion(v =>
                v.AsString(),
                v => EnumHelper.FromString<TipoLancamentoContabilIntegracaoSenior>(v)
            )
            .HasComment("Tipo de lançamento contábil")
            .IsRequired();

        builder.Property(x => x.CodigoPa)
            .HasColumnName("codigo_pa")
            .HasColumnType("char(3)")
            .HasComment("Código único do ponto de atendimento")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Agencia)
            .WithMany(x => x.IntegracoesSeniores)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}