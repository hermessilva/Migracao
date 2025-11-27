using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class AgenciaSeguradoraConfiguration : IEntityTypeConfiguration<AgenciaSeguradora>
{
    public void Configure(EntityTypeBuilder<AgenciaSeguradora> builder)
    {
        builder.ToTable("agencia_seguradora");

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

        builder.Property(x => x.Ordem)
            .HasColumnName("ordem")
            .HasComment("ordem de prioridade dentro da agência")
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