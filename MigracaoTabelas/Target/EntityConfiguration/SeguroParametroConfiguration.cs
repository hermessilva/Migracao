using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

using Seguros.Helpers;


namespace MigracaoTabelas.Target.EntityConfiguration;

public sealed class SeguroParametroConfiguration : BaseEntityConfiguration<SeguroParametro>
{
    public override void Configure(EntityTypeBuilder<SeguroParametro> pBuilder)
    {
        pBuilder.ToTable("seguro_parametro", pT => pT.HasComment("Parâmetros de contratação do seguro utilizados para cálculos de parcelas, prêmios e cancelamentos"));

        pBuilder.HasKey(pX => pX.Id);

        pBuilder.Property(pX => pX.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        ConfigureEnum(pBuilder.Property(pX => pX.TipoCapital)
            .HasColumnName("tipo_capital"), "Fixo", "Variável")
            .HasConversion(
                pV => pV.AsString(),
                pV => EnumHelper.FromString<TipoCapitalApolice>(pV))
            .HasComment("Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)")
            .IsRequired();

        pBuilder.Property(pX => pX.Periodicidade30Dias)
            .HasColumnName("periodicidade_30dias")
            .HasColumnType(TinyIntBoolean())
            .HasDefaultValue(false)
            .HasComment("Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)")
            .IsRequired();

        pBuilder.Property(pX => pX.Coeficiente)
            .HasColumnName("coeficiente")
            .HasColumnType(Decimal(8, 7))
            .HasComment("Coeficiente multiplicador utilizado para cálculo do prêmio e estornos")
            .IsRequired();

        pBuilder.Property(pX => pX.PorcentualIof)
            .HasColumnName("porcentual_iof")
            .HasColumnType(Decimal(5, 4))
            .HasComment("Porcentual de IOF cobrado no seguro")
            .IsRequired();

        pBuilder.Property(pX => pX.PorcentagemComissaoCorretora)
            .HasColumnName("porcentagem_comissao_corretora")
            .HasColumnType(Decimal(5, 4))
            .HasComment("Percentual de comissão destinado à corretora (ex: 0.1500 = 15%)")
            .IsRequired();

        pBuilder.Property(pX => pX.PorcentagemComissaoCooperativa)
            .HasColumnName("porcentagem_comissao_cooperativa")
            .HasColumnType(Decimal(5, 4))
            .HasComment("Percentual de comissão destinado à cooperativa (ex: 0.0500 = 5%)")
            .IsRequired();

        // Coberturas - Morte
        pBuilder.Property(pX => pX.PorcentagemCoberturaMorte)
            .HasColumnName("porcentagem_cobertura_morte")
            .HasColumnType(Decimal(5, 4))
            .HasDefaultValue(0m)
            .HasComment("Percentual de cobertura por morte (ex: 1.0000 = 100%)");

        pBuilder.Property(pX => pX.CapitalMorte)
            .HasColumnName("capital_morte")
            .HasColumnType(Decimal(18, 2))
            .HasDefaultValue(0m)
            .HasComment("Valor do capital segurado por morte");

        pBuilder.Property(pX => pX.PremioMorte)
            .HasColumnName("premio_morte")
            .HasColumnType(Decimal(18, 2))
            .HasDefaultValue(0m)
            .HasComment("Valor do prêmio referente à cobertura por morte");

        // Coberturas - Invalidez
        pBuilder.Property(pX => pX.PorcentagemCoberturaInvalidez)
            .HasColumnName("porcentagem_cobertura_invalidez")
            .HasColumnType(Decimal(5, 4))
            .HasDefaultValue(0m)
            .HasComment("Percentual de cobertura por invalidez permanente total por acidente - IPTA (ex: 1.0000 = 100%)");

        pBuilder.Property(pX => pX.CapitalInvalidez)
            .HasColumnName("capital_invalidez")
            .HasColumnType(Decimal(18, 2))
            .HasDefaultValue(0m)
            .HasComment("Valor do capital segurado por invalidez permanente total por acidente - IPTA");

        pBuilder.Property(pX => pX.PremioInvalidez)
            .HasColumnName("premio_invalidez")
            .HasColumnType(Decimal(18, 2))
            .HasDefaultValue(0m)
            .HasComment("Valor do prêmio referente à cobertura por invalidez - IPTA");
    }
}
