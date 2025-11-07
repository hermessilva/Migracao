using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class GestaoDocumentoConfiguration : IEntityTypeConfiguration<GestaoDocumento>
{
    public void Configure(EntityTypeBuilder<GestaoDocumento> builder)
    {
        builder.ToTable("gestao_documento", t => t.HasComment("Gestão de documentos"));

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

        builder.Property(x => x.NomeDocumento)
            .HasColumnName("nome_documento")
            .HasMaxLength(255)
            .HasComment("Nome/título do documento")
            .IsRequired();

        builder.Property(x => x.Versao)
            .HasColumnName("versao")
            .HasColumnType("smallint")
            .HasComment("Versão do documento")
            .IsRequired();

        builder.Property(x => x.Label)
            .HasColumnName("label")
            .HasMaxLength(255)
            .HasComment("Nome amigável do parâmetro")
            .IsRequired();

        builder.Property(x => x.Campo)
            .HasColumnName("campo")
            .HasMaxLength(255)
            .HasComment("Identificador do parâmetro")
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasMaxLength(255)
            .HasComment("Valor de resposta do parâmetro")
            .IsRequired();

        builder.Property(x => x.Ordem)
            .HasColumnName("ordem")
            .HasColumnType("int")
            .HasComment("Ordem de exibição")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasComment("Data/hora de criação")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Relacionamentos
        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.GestoesDocumentos)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}