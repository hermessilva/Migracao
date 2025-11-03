using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class PropostaSeguradoraConfiguration : IEntityTypeConfiguration<PropostaSeguradora>
{
    public void Configure(EntityTypeBuilder<PropostaSeguradora> builder)
    {
        builder.ToTable("proposta_seguradora");

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

        builder.Property(x => x.DescricaoSequencial)
            .HasColumnName("descricao_sequencial")
            .HasComment("Descrição sequencial da proposta")
            .IsRequired();

        builder.Property(x => x.NumeroSequencial)
            .HasColumnName("numero_sequencial")
            .HasComment("Número sequencial da proposta")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Seguradora)
            .WithMany()
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}