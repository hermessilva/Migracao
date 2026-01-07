using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

using Seguros.Helpers;


namespace MigracaoTabelas.Target.EntityConfiguration;

public sealed class GestaoDocumentoConfiguration : BaseEntityConfiguration<GestaoDocumento>
{
    public override void Configure(EntityTypeBuilder<GestaoDocumento> builder)
    {
        builder.ToTable("gestao_documento", t => t.HasComment("Armazena os documentos de gestão por seguradora"));

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

        ConfigureEnum(builder.Property(x => x.Tipo)
            .HasColumnName("tipo"), "Termo de Adesão", "DPS")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<TipoGestaoDocumento>(v))
            .HasComment("Tipo do documento/modelo")
            .IsRequired();

        builder.Property(x => x.Validade)
            .HasColumnName("validade")
            .HasColumnType(Date())
            .HasComment("Data inicial de validade")
            .IsRequired();

        ConfigureEnum(builder.Property(x => x.Status)
            .HasColumnName("status"), "Ativo", "Inativo")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<StatusGestaoDocumento>(v))
            .HasComment("Indica se um documento está disponível para uso")
            .IsRequired();

        builder.Property(x => x.Modelo)
            .HasColumnName("modelo")
            .HasColumnType(MediumBlob())
            .HasComment("Modelo que será usado para gerar o documento")
            .IsRequired();

        builder.Property(x => x.Extensao)
            .HasColumnName("extensao")
            .HasMaxLength(15)
            .HasComment("Extensão do documento")
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasComment("Data e hora de criação do registro")
            .HasDefaultValueSql(CurrentTimestamp());

        builder.HasOne(x => x.Seguradora)
            .WithMany(x => x.GestoesDocumentos)
            .HasForeignKey(x => x.SeguradoraId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}