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

        builder.Property(x => x.AgenciaSeguradoraId)
            .HasColumnName("agencia_seguradora_id")
            .HasComment("Chave estrangeira da tabela agencia_seguradora")
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
            .HasMaxLength(50)
            .HasComment("Valor modalidade unico");

        builder.Property(x => x.ModalidadeAVista)
            .HasColumnName("modalidade_avista")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor modalidade avista");

        builder.Property(x => x.ModalidadeParcelado)
            .HasColumnName("modalidade_parcelado")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor modalidade parcelado");

        // Relacionamentos
        builder.HasOne(x => x.AgenciasSeguradoras)
            .WithMany(x => x.ApolicesGruposSeguradoras)
            .HasForeignKey(x => x.AgenciaSeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}