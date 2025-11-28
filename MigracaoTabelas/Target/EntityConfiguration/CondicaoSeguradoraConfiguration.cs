using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class CondicaoSeguradoraConfiguration : IEntityTypeConfiguration<CondicaoSeguradora>
{
    public void Configure(EntityTypeBuilder<CondicaoSeguradora> builder)
    {
        builder.ToTable("condicao_seguradora", t => t.HasComment("Parâmetros de condições operacionais e financeiras aplicados por seguradora"));

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

        builder.Property(x => x.MaxMesesContrato)
            .HasColumnName("max_meses_contrato")
            .HasColumnType("smallint")
            .HasComment("Quantidade máxima de meses permitidos para vigência do contrato")
            .IsRequired();

        builder.Property(x => x.MaxIdade)
            .HasColumnName("max_idade")
            .HasColumnType("smallint")
            .HasComment("Idade máxima permitida do proponente para contratação do seguro")
            .IsRequired();

        builder.Property(x => x.PorcentagemCoberturaMorte)
            .HasColumnName("porcentagem_cobertura_morte")
            .HasColumnType("decimal(5,4)")
            .HasComment("Percentual de cobertura para sinistro por morte (ex: 1.0000 = 100%)")
            .IsRequired();

        builder.Property(x => x.PorcentagemCoberturaInvalidez)
            .HasColumnName("porcentagem_cobertura_invalidez")
            .HasColumnType("decimal(5,4)")
            .HasComment("Percentual de cobertura para sinistro por invalidez (ex: 0.5000 = 50%)")
            .IsRequired();

        builder.Property(x => x.PorcentagemCoberturaPerdaRenda)
            .HasColumnName("porcentagem_cobertura_perda_renda")
            .HasColumnType("decimal(5,4)")
            .HasComment("Percentual de cobertura para sinistro por perda de renda (ex: 0.3000 = 30%)")
            .IsRequired();

        builder.Property(x => x.Periodicidade30Dias)
            .HasColumnName("periodicidade_30dias")
            .HasColumnType("tinyint(1)")
            .HasDefaultValue(false)
            .HasComment("Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.CondicoesSeguradora)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índice único
        builder.HasIndex(x => x.SeguradoraId)
            .IsUnique()
            .HasDatabaseName("condicao_seguradora_index_1");
    }
}