using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class SeguroConfiguration : IEntityTypeConfiguration<Seguro>
{
    public void Configure(EntityTypeBuilder<Seguro> builder)
    {
        builder.ToTable("seguro", t => t.HasComment("Contratos de seguros e seus metadados financeiros e relacionamentos"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.AgenciaSeguradoraId)
            .HasColumnName("agencia_seguradora_id")
            .HasComment("Chave estrangeira na tabela agencia_seguradora")
            .IsRequired();

        builder.Property(x => x.CooperadoAgenciaContaId)
            .HasColumnName("cooperado_agencia_conta_id")
            .HasComment("Chave estrangeira na tabela cooperado_agencia_conta")
            .IsRequired();

        builder.Property(x => x.PontoAtendimentoId)
            .HasColumnName("ponto_atendimento_id")
            .HasComment("Chave estrangeira na tabela ponto de atendimento")
            .IsRequired();

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .HasComment("Chave estrangeira na tabela usuario");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("enum('Ativo','Em análise pela Seguradora','Pendente de Documentação','Expiração da Vigência do Seguro','Cancelado pelo Cooperado','Cancelado pela Cooperativa','Sinistro','Recusado pela Seguradora','Cancelamento por Prejuízo','Liquidação Antecipada','Cancelado por Renegociação','Cancelado por Aditivo')")
            .HasConversion(
                v => v.AsString(),
                v => EnumEx.FromString<StatusSeguro>(v)
            )
            .HasComment("Status do seguro")
            .IsRequired();

        builder.Property(x => x.Contrato)
            .HasColumnName("contrato")
            .HasMaxLength(10)
            .HasComment("Número do contrato do seguro")
            .IsRequired();

        builder.Property(x => x.InicioVigencia)
            .HasColumnName("inicio_vigencia")
            .HasColumnType("date")
            .HasComment("Início de vigência do seguro");

        builder.Property(x => x.FimVigencia)
            .HasColumnName("fim_vigencia")
            .HasColumnType("date")
            .HasComment("Fim de vigência do seguro");

        builder.Property(x => x.CodigoGrupo)
            .HasColumnName("codigo_grupo")
            .HasColumnType("int")
            .HasComment("Identificador do grupo (código) do contrato")
            .IsRequired();

        builder.Property(x => x.QuantidadeParcelas)
            .HasColumnName("quantidade_parcelas")
            .HasColumnType("smallint")
            .HasComment("Quantidade total de parcelas do seguro")
            .IsRequired();

        builder.Property(x => x.Vencimento)
            .HasColumnName("vencimento")
            .HasColumnType("date")
            .HasComment("Data de vencimento (padrão do contrato ou próxima parcela)");

        builder.Property(x => x.CapitalSegurado)
            .HasColumnName("capital_segurado")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor do capital segurado")
            .IsRequired();

        builder.Property(x => x.PremioTotal)
            .HasColumnName("premio_total")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor do prêmio total do seguro")
            .IsRequired();

        builder.Property(x => x.TipoPagamento)
            .HasColumnName("tipo_pagamento")
            .HasColumnType("enum('À Vista','Parcelado','Único')")
            .HasConversion(
                v => v.AsString(),
                v => EnumEx.FromString<TipoPagamentoSeguro>(v)
            )
            .HasComment("Identificador do tipo de pagamento (1=à vista, 2=parcelado, 3=Única)")
            .IsRequired();

        builder.Property(x => x.EstornoProporcional)
            .HasColumnName("estorno_proporcional")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor de estorno proporcional quando aplicável")
            .IsRequired();

        builder.Property(x => x.ValorBase)
            .HasColumnName("valor_base")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor Base Segurado");

        builder.Property(x => x.Dps)
            .HasColumnName("dps")
            .HasColumnType("tinyint(1)")
            .HasComment("Informação de Exigência de DPS");

        builder.Property(x => x.ValorIof)
            .HasColumnName("valor_iof")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor de IOF");

        // Relacionamentos
        builder.HasOne(x => x.AgenciasSeguradoras)
            .WithMany(x => x.Seguros)
            .HasForeignKey(x => x.AgenciaSeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.CooperadosAgenciasContas)
            .WithMany(x => x.Seguros)
            .HasForeignKey(x => x.CooperadoAgenciaContaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.PontosAtendimentos)
            .WithMany(x => x.Seguros)
            .HasForeignKey(x => x.PontoAtendimentoId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Usuarios)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}