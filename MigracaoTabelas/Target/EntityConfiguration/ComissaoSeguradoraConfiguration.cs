using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class ComissaoSeguradoraConfiguration : IEntityTypeConfiguration<ComissaoSeguradora>
{
    public void Configure(EntityTypeBuilder<ComissaoSeguradora> builder)
    {
        builder.ToTable("comissao_seguradora", t => t.HasComment("Junção entre parâmetros de seguradora e comissoes"));

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

        builder.Property(x => x.PorcentagemComissaoCorretora)
            .HasColumnName("porcentagem_comissao_corretora")
            .HasColumnType("decimal(5,4)")
            .HasComment("Percentual de comissão da corretora")
            .IsRequired();

        builder.Property(x => x.PorcentagemComissaoCooperativa)
            .HasColumnName("porcentagem_comissao_cooperativa")
            .HasColumnType("decimal(5,4)")
            .HasComment("Percentual de comissão da cooperativa")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.ComissoesSeguradoras)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}