using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace MigracaoTabelas.Target.EntityConfiguration;

public class SeguroConfiguration : BaseEntityConfiguration<Seguro>
{
    public override void Configure(EntityTypeBuilder<Seguro> builder)
    {
        builder.ToTable("seguro", t => t.HasComment("Contratos de seguros e seus metadados financeiros e relacionamentos"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.CooperadoAgenciaContaId)
            .HasColumnName("cooperado_agencia_conta_id")
            .HasComment("Chave estrangeira referenciando a tabela cooperado_agencia_conta")
            .IsRequired();
        builder.Property(x => x.ApoliceGrupoSeguradoraId)
            .HasColumnName("apolice_grupo_seguradora_id")
            .HasComment("Chave estrangeira referenciando a tabela apolice_grupo_seguradora indicando a apolice contratada")
            .IsRequired();

        builder.Property(x => x.PontoAtendimentoId)
            .HasColumnName("ponto_atendimento_id")
            .HasComment("Chave estrangeira referenciando a tabela ponto_atendimento onde o seguro foi contratado")
            .IsRequired();

        builder.Property(x => x.SeguroParametroId)
            .HasColumnName("seguro_parametro_id")
            .HasComment("Chave estrangeira referenciando a tabela seguro_parametro com os parâmetros de cálculo")
            .IsRequired();

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .HasComment("Chave estrangeira referenciando a tabela usuario responsável pela contratação");

        ConfigureEnum(builder.Property(x => x.Status)
            .HasColumnName("status"), "Pendente", "Ativo", "Recusado", "Expirado", "Cancelado")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<StatusSeguro>(v)
            )
            .HasComment("Status do seguro")
            .IsRequired();

        ConfigureEnum(builder.Property(x => x.Motivo)
            .HasColumnName("motivo"), "Em analise na seguradora", "Aguardando faturamento", "Aguardando documentação", "Pagamento à vista",
                                      "Pagamento parcelado", "Inadimplente", "Regular", "Recusado pela seguradora",
                                      "Expiração da vigência do seguro", "Aditivo", "Cancelamento por prejuízo", "Renegociação",
                                      "Sinistro", "Solicitado pela cooperativa", "Solicitado pelo cooperado", "Liquidação antecipada")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<MotivoSeguro>(v)
            )
            .HasComment("Motivo do seguro")
            .IsRequired();

        builder.Property(x => x.Contrato)
            .HasColumnName("contrato")
            .HasMaxLength(10)
            .HasComment("Número do contrato de seguro")
            .IsRequired();

        builder.Property(x => x.InicioVigencia)
            .HasColumnName("inicio_vigencia")
            .HasColumnType(Date())
            .HasComment("Data de início da vigência do seguro");

        builder.Property(x => x.FimVigencia)
            .HasColumnName("fim_vigencia")
            .HasColumnType(Date())
            .HasComment("Data de término da vigência do seguro");

        builder.Property(x => x.CodigoGrupo)
            .HasColumnName("codigo_grupo")
            .HasColumnType(Int())
            .HasComment("Código identificador do grupo/produto do seguro")
            .IsRequired();

        builder.Property(x => x.QuantidadeParcelas)
            .HasColumnName("quantidade_parcelas")
            .HasColumnType(SmallInt())
            .HasComment("Quantidade total de parcelas do seguro")
            .IsRequired();

        builder.Property(x => x.Vencimento)
            .HasColumnName("vencimento")
            .HasColumnType(Date())
            .HasComment("Data de vencimento base do contrato ou da próxima parcela");

        builder.Property(x => x.CapitalSegurado)
            .HasColumnName("capital_segurado")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor total do capital segurado (valor coberto em caso de sinistro)")
            .IsRequired();

        builder.Property(x => x.PremioTotal)
            .HasColumnName("premio_total")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor total do prêmio do seguro a ser pago")
            .IsRequired();

        ConfigureEnum(builder.Property(x => x.TipoPagamento)
            .HasColumnName("tipo_pagamento"), "À Vista", "Parcelado", "Único")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<TipoPagamentoSeguro>(v)
            )
            .HasComment("Modalidade de pagamento: À Vista, Parcelado ou Único")
            .IsRequired();

        builder.Property(x => x.EstornoProporcional)
            .HasColumnName("estorno_proporcional")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor de estorno proporcional em caso de cancelamento")
            .IsRequired();

        builder.Property(x => x.ValorBase)
            .HasColumnName("valor_base")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor base utilizado para cálculo do seguro (saldo devedor ou valor financiado)");

        builder.Property(x => x.DeclaracaoPessoalSaude)
            .HasColumnName("declaracao_pessoal_saude")
            .HasColumnType(TinyIntBoolean())
            .HasComment("Indica se foi exigida Declaração Pessoal de Saúde (true/false)");

        builder.Property(x => x.ValorIof)
            .HasColumnName("valor_iof")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor do IOF (Imposto sobre Operações Financeiras) incidente sobre o prêmio");

        builder.Property(x => x.NumeroContratoEmprestimo)
            .HasColumnName("numero_contrato_emprestimo")
            .HasMaxLength(20)
            .HasComment("Número do contrato de crédito do empréstimo");

        builder.Property(x => x.ContratoSequencia)
            .HasColumnName("contrato_sequencia")
            .HasMaxLength(2)
            .HasDefaultValue("00")
            .HasComment("Numero sequêncial do contrato");

        // Relacionamentos
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

        builder.HasOne(x => x.ApolicesGruposSeguradoras)
            .WithMany(x => x.Seguros)
            .HasForeignKey(x => x.ApoliceGrupoSeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.SeguroParametro)
            .WithOne(x => x.Seguro)
            .HasForeignKey<Seguro>(x => x.SeguroParametroId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índices
        builder.HasIndex(x => x.SeguroParametroId)
            .IsUnique()
            .HasDatabaseName("idx_seguro_parametro_id");

    }
}