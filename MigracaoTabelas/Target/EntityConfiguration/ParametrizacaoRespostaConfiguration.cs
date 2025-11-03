using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class ParametrizacaoRespostaConfiguration : IEntityTypeConfiguration<ParametrizacaoResposta>
{
    public void Configure(EntityTypeBuilder<ParametrizacaoResposta> builder)
    {
        builder.ToTable("parametrizacao_resposta");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.ParametrizacaoId)
            .HasColumnName("parametrizacao_id")
            .HasComment("Chave estrangeira da tabela parametrizacao")
            .IsRequired();

        builder.Property(x => x.Resposta)
            .HasColumnName("resposta")
            .HasMaxLength(255)
            .HasComment("Valor de resposta do parâmetro")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Parametrizacao)
            .WithMany(x => x.ParametrizacaoRespostas)
            .HasForeignKey(x => x.ParametrizacaoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}