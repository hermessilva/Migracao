using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class SeguradoraLimiteConfiguration : BaseEntityConfiguration<SeguradoraLimite>
{
    public override void Configure(EntityTypeBuilder<SeguradoraLimite> builder)
    {
        builder.ToTable("seguradora_limite", t => t.HasComment("Define faixas etárias, coeficientes e limites de DPS por seguradora para cálculo de prêmios"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.SeguradoraId)
            .HasColumnName("seguradora_id")
            .HasComment("Chave estrangeira referenciando a tabela seguradora")
            .IsRequired();

        builder.Property(x => x.IdadeInicial)
            .HasColumnName("idade_inicial")
            .HasColumnType(SmallInt())
            .HasComment("Idade inicial da faixa etária para aplicação da regra")
            .IsRequired();

        builder.Property(x => x.IdadeFinal)
            .HasColumnName("idade_final")
            .HasColumnType(SmallInt())
            .HasComment("Idade final da faixa etária para aplicação da regra")
            .IsRequired();

        builder.Property(x => x.ValorMaximo)
            .HasColumnName("valor_maximo")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor máximo de capital segurado permitido para a faixa")
            .IsRequired();

        builder.Property(x => x.Coeficiente)
            .HasColumnName("coeficiente")
            .HasColumnType(Decimal(8, 7))
            .HasComment("Coeficiente multiplicador para cálculo do prêmio")
            .IsRequired();

        builder.Property(x => x.LimiteDps)
            .HasColumnName("limite_dps")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor limite de capital segurado que exige Declaração Pessoal de Saúde (DPS)")
            .IsRequired();

        builder.Property(x => x.DescricaoRegra)
            .HasColumnName("descricao_regra")
            .HasMaxLength(255)
            .HasComment("Descrição textual detalhada da regra aplicada para o limite e DPS")
            .IsRequired();

        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.SeguradorasLimites)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}