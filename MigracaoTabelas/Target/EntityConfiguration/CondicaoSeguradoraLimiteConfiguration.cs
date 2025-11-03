using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class CondicaoSeguradoraLimiteConfiguration : IEntityTypeConfiguration<CondicaoSeguradoraLimite>
{
    public void Configure(EntityTypeBuilder<CondicaoSeguradoraLimite> builder)
    {
        builder.ToTable("limite");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.CondicaoSeguradoraId)
            .HasColumnName("condicao_seguradora_id")
            .HasComment("Chave estrangeira da tabela condicao_seguradora")
            .IsRequired();

        builder.Property(x => x.IdadeInicial)
            .HasColumnName("idade_inicial")
            .HasColumnType("smallint")
            .HasComment("Idade mínima para aplicação do limite")
            .IsRequired();

        builder.Property(x => x.IdadeFinal)
            .HasColumnName("idade_final")
            .HasColumnType("smallint")
            .HasComment("Idade máxima para aplicação do limite")
            .IsRequired();

        builder.Property(x => x.ValorMaximo)
            .HasColumnName("valor")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor máximo permitido para o limite")
            .IsRequired();

        builder.Property(x => x.Coeficiente)
            .HasColumnName("coeficiente")
            .HasColumnType("decimal(5,4)")
            .HasComment("Coeficiente aplicado ao cálculo do limite")
            .IsRequired();

        builder.Property(x => x.LimiteDps)
            .HasColumnName("limite_dps")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor limite para exigência de DPS")
            .IsRequired();

        builder.Property(x => x.DescricaoRegra)
            .HasColumnName("descricao_regra")
            .HasMaxLength(255)
            .HasComment("Descrição das regras de aplicação do limite")
            .IsRequired();

        builder.HasOne(x => x.CondicaoSeguradora)
            .WithMany()
            .HasForeignKey(x => x.CondicaoSeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}