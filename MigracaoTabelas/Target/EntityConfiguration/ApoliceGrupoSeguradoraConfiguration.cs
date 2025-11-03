using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class ApoliceGrupoSeguradoraConfiguration : IEntityTypeConfiguration<ApoliceGrupoSeguradora>
{
    public void Configure(EntityTypeBuilder<ApoliceGrupoSeguradora> builder)
    {
        builder.ToTable("apolice_grupo_seguradora");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira da tabela agencia")
            .IsRequired();

        builder.Property(x => x.SeguradoraId)
            .HasColumnName("seguradora_id")
            .HasComment("Chave estrangeira da tabela seguradora")
            .IsRequired();

        builder.Property(x => x.Apolice)
            .HasColumnName("apolice")
            .HasMaxLength(255)
            .HasComment("Código da apólice");

        builder.Property(x => x.Grupo)
            .HasColumnName("grupo")
            .HasMaxLength(255)
            .HasComment("Código do grupo da apólice");

        builder.Property(x => x.SubGrupo)
            .HasColumnName("subgrupo")
            .HasMaxLength(255)
            .HasComment("Código do subgrupo da apólice");

        builder.Property(x => x.TipoCapital)
            .HasColumnName("tipo_capital")
            .HasColumnType("enum('Fixo','Variável')")
            .HasConversion<string>()
            .HasComment("Tipo de capital")
            .IsRequired();

        builder.Property(x => x.ModalidadeUnico)
            .HasColumnName("modalidade_unico")
            .HasColumnType("varchar(50)")
            .HasComment("Valor da modalidade único");

        builder.Property(x => x.ModalidadeAvista)
            .HasColumnName("modalidade_avista")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor da modalidade à vista");

        builder.Property(x => x.ModalidadeParcelado)
            .HasColumnName("modalidade_parcelado")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor da modalidade parcelado");

        builder.Property(x => x.Ordem)
            .HasColumnName("ordem")
            .HasComment("Ordem de priorização da seguradora");

        // Relacionamentos
        builder.HasOne(x => x.Agencia)
            .WithMany(x => x.ApolicesGrupoSeguradora)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Seguradora)
            .WithMany(x => x.ApolicesGrupoSeguradora)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}