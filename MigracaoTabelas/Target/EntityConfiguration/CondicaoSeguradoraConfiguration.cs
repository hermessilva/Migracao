using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class CondicaoSeguradoraConfiguration : IEntityTypeConfiguration<CondicaoSeguradora>
{
    public void Configure(EntityTypeBuilder<CondicaoSeguradora> builder)
    {
        builder.ToTable("condicao_seguradora");

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

        builder.Property(x => x.MaxMesesContrato)
            .HasColumnName("max_meses_contrato")
            .HasColumnType("smallint")
            .HasComment("Quantidade máxima de meses permitidos para o contrato")
            .IsRequired();

        builder.Property(x => x.MaxIdade)
            .HasColumnName("max_idade")
            .HasColumnType("smallint")
            .HasComment("Idade máxima do proponente")
            .IsRequired();

        builder.Property(x => x.PorcentagemCoberturaMorte)
            .HasColumnName("porcentagem_cobertura_morte")
            .HasComment("Percentual de cobertura para morte")
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(x => x.PorcentagemCoberturaInvalidez)
            .HasColumnName("porcentagem_cobertura_invalidez")
            .HasColumnType("decimal(5,4)")
            .HasComment("Percentual de cobertura para invalidez")
            .IsRequired();

        builder.Property(x => x.PorcentagemCoberturaPerdaRenda)
            .HasColumnName("porcentagem_cobertura_perda_renda")
            .HasColumnType("decimal(5,4)")
            .HasComment("Percentual de cobertura para perda de renda")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.CondicoesSeguradora)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}