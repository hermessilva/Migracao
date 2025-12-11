using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace MigracaoTabelas.Target.EntityConfiguration;

public class ApoliceGrupoSeguradoraConfiguration : BaseEntityConfiguration<ApoliceGrupoSeguradora>
{
    public override void Configure(EntityTypeBuilder<ApoliceGrupoSeguradora> builder)
    {
        builder.ToTable("apolice_grupo_seguradora", t => t.HasComment("Configurações de apólices e grupos por vínculo agência-seguradora"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.AgenciaSeguradoraId)
            .HasColumnName("agencia_seguradora_id")
            .HasComment("Chave estrangeira referenciando a tabela agencia_seguradora")
            .IsRequired();

        builder.Property(x => x.Apolice)
            .HasColumnName("apolice")
            .HasMaxLength(255)
            .HasComment("Número ou código da apólice contratada");

        builder.Property(x => x.Grupo)
            .HasColumnName("grupo")
            .HasMaxLength(255)
            .HasComment("Código do grupo dentro da apólice");

        builder.Property(x => x.SubGrupo)
            .HasColumnName("subgrupo")
            .HasMaxLength(255)
            .HasComment("Código do subgrupo dentro do grupo da apólice");

        ConfigureEnum(builder.Property(x => x.TipoCapital)
            .HasColumnName("tipo_capital"), "Fixo", "Variável")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<TipoCapitalApoliceGrupoSeguradora>(v)
            )
            .HasComment("Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)")
            .IsRequired();

        builder.Property(x => x.ModalidadeUnico)
            .HasColumnName("modalidade_unico")
            .HasMaxLength(50)
            .HasComment("Identificador ou código da modalidade de pagamento único");

        builder.Property(x => x.ModalidadeAVista)
            .HasColumnName("modalidade_avista")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor ou taxa para modalidade de pagamento à vista");

        builder.Property(x => x.ModalidadeParcelado)
            .HasColumnName("modalidade_parcelado")
            .HasColumnType(Decimal(10, 2))
            .HasComment("Valor ou taxa para modalidade de pagamento parcelado");

        // Relacionamentos
        builder.HasOne(x => x.AgenciasSeguradoras)
            .WithMany(x => x.ApolicesGruposSeguradoras)
            .HasForeignKey(x => x.AgenciaSeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}