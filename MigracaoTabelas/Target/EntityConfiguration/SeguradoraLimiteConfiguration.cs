using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class SeguradoraLimiteConfiguration : IEntityTypeConfiguration<SeguradoraLimite>
{
    public void Configure(EntityTypeBuilder<SeguradoraLimite> builder)
    {
        builder.ToTable("seguradora_limite");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.SeguradoraId)
            .HasColumnName("seguradora_id")
            .HasComment("Chave estrangeira da tabela seguradora")
            .IsRequired();

        builder.Property(x => x.IdadeInicial)
            .HasColumnName("idade_inicial")
            .HasColumnType("smallint")
            .HasComment("Idade inicial da faixa")
            .IsRequired();

        builder.Property(x => x.IdadeFinal)
            .HasColumnName("idade_final")
            .HasColumnType("smallint")
            .HasComment("Idade final da faixa")
            .IsRequired();

        builder.Property(x => x.ValorMaximo)
            .HasColumnName("valor_maximo")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor associado à faixa")
            .IsRequired();

        builder.Property(x => x.Coeficiente)
            .HasColumnName("coeficiente")
            .HasColumnType("decimal(5,4)")
            .HasComment("Coeficiente aplicado para cálculos")
            .IsRequired();

        builder.Property(x => x.LimiteDps)
            .HasColumnName("limite_dps")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor limite de exigibilidade para DPS")
            .IsRequired();

        builder.Property(x => x.DescricaoRegra)
            .HasColumnName("descricao_regra")
            .HasMaxLength(255)
            .HasComment("Descrição detalhada da regra para o limite DPS")
            .IsRequired();

        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.SeguradorasLimites)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}