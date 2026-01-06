using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;


namespace MigracaoTabelas.Target.EntityConfiguration;

public sealed class ComissaoSeguradoraConfiguration : BaseEntityConfiguration<ComissaoSeguradora>
{
    public override void Configure(EntityTypeBuilder<ComissaoSeguradora> pBuilder)
    {
        pBuilder.ToTable("comissao_seguradora", pT => pT.HasComment("Configurações de comissões por seguradora"));

        pBuilder.HasKey(pX => pX.Id);
        pBuilder.Property(pX => pX.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        pBuilder.Property(pX => pX.SeguradoraId)
            .HasColumnName("seguradora_id")
            .HasComment("Chave estrangeira da tabela seguradora")
            .IsRequired();

        pBuilder.Property(pX => pX.PorcentagemComissaoCorretora)
            .HasColumnName("porcentagem_comissao_corretora")
            .HasColumnType(Decimal(5, 4))
            .HasComment("Percentual de comissão da corretora")
            .IsRequired();

        pBuilder.Property(pX => pX.PorcentagemComissaoCooperativa)
            .HasColumnName("porcentagem_comissao_cooperativa")
            .HasColumnType(Decimal(5, 4))
            .HasComment("Percentual de comissão da cooperativa")
            .IsRequired();

        pBuilder.HasOne(pX => pX.Seguradoras)
            .WithMany(pX => pX.ComissoesSeguradoras)
            .HasForeignKey(pX => pX.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índice único
        pBuilder.HasIndex(pX => pX.SeguradoraId)
            .IsUnique()
            .HasDatabaseName("comissao_seguradora_index_5");
    }
}
