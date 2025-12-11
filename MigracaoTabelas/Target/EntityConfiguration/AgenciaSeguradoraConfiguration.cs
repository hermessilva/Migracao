using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class AgenciaSeguradoraConfiguration : BaseEntityConfiguration<AgenciaSeguradora>
{
    public override void Configure(EntityTypeBuilder<AgenciaSeguradora> builder)
    {
        builder.ToTable("agencia_seguradora", t => t.HasComment("Tabela de vínculo que relaciona agências com seguradoras autorizadas e define prioridade"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira referenciando a tabela agencia")
            .IsRequired();

        builder.Property(x => x.SeguradoraId)
            .HasColumnName("seguradora_id")
            .HasComment("Chave estrangeira referenciando a tabela seguradora")
            .IsRequired();

        builder.Property(x => x.Ordem)
            .HasColumnName("ordem")
            .HasColumnType(TinyInt())
            .HasComment("Ordem de prioridade da seguradora dentro da agência (menor = maior prioridade)")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Agencias)
            .WithMany(x => x.AgenciasSeguradoras)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.AgenciasSeguradoras)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}