using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class PropostaSeguradoraConfiguration : IEntityTypeConfiguration<PropostaSeguradora>
{
    public void Configure(EntityTypeBuilder<PropostaSeguradora> builder)
    {
        builder.ToTable("proposta_seguradora", t => t.HasComment("Controle de numeração sequencial de propostas por seguradora"));

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

        builder.Property(x => x.DescricaoSequencial)
            .HasColumnName("descricao_sequencial")
            .HasMaxLength(255)
            .HasComment("Descrição ou prefixo do formato do número sequencial da proposta");

        builder.Property(x => x.NumeroSequencial)
            .HasColumnName("numero_sequencial")
            .HasMaxLength(255)
            .HasComment("Último número sequencial utilizado para geração de propostas");

        // Relacionamentos
        builder.HasOne(x => x.Seguradoras)
            .WithMany(s => s.PropostasSeguradoras)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}