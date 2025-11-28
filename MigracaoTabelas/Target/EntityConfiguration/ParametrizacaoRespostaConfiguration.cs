using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class ParametrizacaoRespostaConfiguration : IEntityTypeConfiguration<ParametrizacaoResposta>
{
    public void Configure(EntityTypeBuilder<ParametrizacaoResposta> builder)
    {
        builder.ToTable("parametrizacao_resposta", t => t.HasComment("Opções de resposta disponíveis para cada campo de parametrização"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.ParametrizacaoId)
            .HasColumnName("parametrizacao_id")
            .HasComment("Chave estrangeira referenciando a tabela parametrizacao")
            .IsRequired();

        builder.Property(x => x.Resposta)
            .HasColumnName("resposta")
            .HasMaxLength(255)
            .HasComment("Valor de resposta ou opção disponível para o campo de parametrização")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(x => x.Parametrizacoes)
            .WithMany(x => x.ParametrizacoesRespostas)
            .HasForeignKey(x => x.ParametrizacaoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}