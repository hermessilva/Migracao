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

        builder.Property(x => x.ContaCredito)
            .HasColumnName("conta_credito")
            .HasColumnType("tinyint(1)")
            .HasComment("Indica se é uma conta de crédito")
            .IsRequired();

        builder.Property(x => x.ContaDebito)
            .HasColumnName("conta_debito")
            .HasColumnType("tinyint(1)")
            .HasComment("Indica se é uma conta de débito")
            .IsRequired();

        builder.Property(x => x.ContaCreditoComissao)
            .HasColumnName("conta_credito_comissao")
            .HasColumnType("tinyint(1)")
            .HasComment("Indica se é uma conta de crédito de comissão")
            .IsRequired();

        builder.Property(x => x.ContaDebitoComissao)
            .HasColumnName("conta_debito_comissao")
            .HasColumnType("tinyint(1)")
            .HasComment("Indica se é uma conta de débito de comissão")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("enum('Enviado', 'Erro')")
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

        builder.Property(x => x.Lancamento)
            .HasColumnName("lancamento")
            .HasColumnType("int")
            .HasComment("Número do lançamento no sistema Senior")
            .IsRequired();

        builder.Property(x => x.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(255)
            .HasComment("Descrição da integração")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Agencia)
            .WithMany(x => x.IntegracoesSenior)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}