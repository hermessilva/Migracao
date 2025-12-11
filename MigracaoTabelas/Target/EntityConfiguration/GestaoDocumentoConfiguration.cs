using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class GestaoDocumentoConfiguration : BaseEntityConfiguration<GestaoDocumento>
{
    public override void Configure(EntityTypeBuilder<GestaoDocumento> builder)
    {
        builder.ToTable("gestao_documento", t => t.HasComment("Gestão de templates e campos de documentos por seguradora para geração automática"));

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

        builder.Property(x => x.NomeDocumento)
            .HasColumnName("nome_documento")
            .HasMaxLength(255)
            .HasComment("Nome ou título do documento a ser gerado")
            .IsRequired();

        builder.Property(x => x.Versao)
            .HasColumnName("versao")
            .HasColumnType(SmallInt())
            .HasComment("Número da versão do documento para controle de alterações")
            .IsRequired();

        builder.Property(x => x.Label)
            .HasColumnName("label")
            .HasMaxLength(255)
            .HasComment("Rótulo amigável do campo para exibição ao usuário")
            .IsRequired();

        builder.Property(x => x.Campo)
            .HasColumnName("campo")
            .HasMaxLength(255)
            .HasComment("Identificador técnico do campo no documento")
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnName("valor")
            .HasMaxLength(255)
            .HasComment("Valor padrão ou resposta configurada para o campo")
            .IsRequired();

        builder.Property(x => x.Ordem)
            .HasColumnName("ordem")
            .HasColumnType(Int())
            .HasComment("Ordem de exibição do campo no documento")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasComment("Data e hora de criação do registro")
            .HasDefaultValueSql(CurrentTimestamp());

        // Relacionamentos
        builder.HasOne(x => x.Seguradoras)
            .WithMany(x => x.GestoesDocumentos)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}