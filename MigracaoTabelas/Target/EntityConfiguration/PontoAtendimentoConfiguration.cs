using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class PontoAtendimentoConfiguration : IEntityTypeConfiguration<PontoAtendimento>
{
    public void Configure(EntityTypeBuilder<PontoAtendimento> builder)
    {
        builder.ToTable("ponto_atendimento");

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

        builder.Property(x => x.Codigo)
            .HasColumnName("codigo")
            .HasColumnType("char(3)")
            .HasComment("Código único do ponto de atendimento")
            .IsRequired();

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasMaxLength(255)
            .HasComment("Nome do ponto de atendimento")
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .HasComment("Data/hora da criação do registro");

        // Relacionamentos
        builder.HasOne(x => x.Agencia)
            .WithMany(x => x.PontosAtendimento)
            .HasForeignKey(x => x.AgenciaId)
            .OnDelete(DeleteBehavior.NoAction);

        // Índices
        builder.HasIndex(x => x.Nome)
            .IsUnique();

        builder.HasIndex(x => new { x.AgenciaId, x.Codigo })
            .IsUnique();

        builder.HasIndex(x => x.AgenciaId);
    }
}