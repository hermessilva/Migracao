using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;



namespace MigracaoTabelas.Target.EntityConfiguration;

/// <summary>
/// Configuração de mapeamento da entidade ArmazenamentoDocumento para o Entity Framework Core.
/// Suporta MySQL (produção) e SQLite (testes).
/// </summary>
public sealed class ArmazenamentoDocumentoConfiguration : BaseEntityConfiguration<ArmazenamentoDocumento>
{
    public override void Configure(EntityTypeBuilder<ArmazenamentoDocumento> builder)
    {
        builder.ToTable("armazenamento_documento", t => t.HasComment(
            "Armazena referências de documentos em provedores de nuvem ou on-premises vinculados a seguros"));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.SeguroId)
            .HasColumnName("seguro_id")
            .HasComment("Chave estrangeira referenciando a tabela seguro")
            .IsRequired();

        builder.Property(x => x.NomeOriginal)
            .HasColumnName("nome_original")
            .HasColumnType(VarChar(255))
            .HasMaxLength(255)
            .HasComment("Nome original do arquivo no momento do upload")
            .IsRequired();

        builder.Property(x => x.ExtensaoArquivo)
            .HasColumnName("extensao_arquivo")
            .HasColumnType(VarChar(15))
            .HasMaxLength(15)
            .HasComment("Extensão do arquivo (ex: pdf, png, jpg)")
            .IsRequired();

        ConfigureEnum(builder.Property(x => x.Tipo)
                .HasColumnName("tipo"),
            "Termo de Adesão com DPS",
            "Termo de Adesão sem DPS",
            "Documentação Complementar")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<TipoDocumentoArmazenado>(v))
            .HasComment("Tipo/finalidade do documento armazenado")
            .IsRequired();

        builder.Property(x => x.Finalidade)
            .HasColumnName("finalidade")
            .HasColumnType(VarChar(30))
            .HasMaxLength(30)
            .HasComment("Descrição da finalidade do documento quando Tipo for Documentação Complementar");

        ConfigureEnum(builder.Property(x => x.Local)
                .HasColumnName("local"),
            "Amazon Web Services S3",
            "Microsoft Azure Blob Storage",
            "Google Cloud Platform Storage",
            "Oracle Cloud Infrastructure Object Storage",
            "Armazenamento On-Premises")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<LocalArmazenamentoDocumento>(v))
            .HasComment("Local de armazenamento (AWS, Azure, GCP, OCI, OnPrem)")
            .IsRequired();

        builder.Property(x => x.UnidadeArmazenamento)
            .HasColumnName("unidade_armazenamento")
            .HasColumnType(VarChar(100))
            .HasMaxLength(100)
            .HasComment("Nome do Bucket (S3/GCP/MinIO) ou Container (Azure/OCI)")
            .IsRequired();

        builder.Property(x => x.CaminhoArquivo)
            .HasColumnName("caminho_arquivo")
            .HasColumnType(VarChar(450))
            .HasMaxLength(450)
            .HasComment("Caminho completo do objeto (Key/Path) no provedor de armazenamento")
            .IsRequired();

        builder.Property(x => x.CodigoRegiao)
            .HasColumnName("codigo_regiao")
            .HasColumnType(VarChar(50))
            .HasMaxLength(50)
            .HasComment("Código da região do provedor (ex: us-east-1, eastus, southamerica-east1)");

        builder.Property(x => x.EndpointAlias)
            .HasColumnName("endpoint_alias")
            .HasColumnType(VarChar(255))
            .HasMaxLength(255)
            .HasComment("URL base customizada para CDN, proxy ou endpoint personalizado");

        builder.Property(x => x.TipoMime)
            .HasColumnName("tipo_mime")
            .HasColumnType(VarChar(100))
            .HasMaxLength(100)
            .HasComment("Tipo MIME do arquivo (ex: application/pdf, image/png)")
            .IsRequired();

        builder.Property(x => x.TamanhoBytes)
            .HasColumnName("tamanho_bytes")
            .HasComment("Tamanho do arquivo em bytes")
            .IsRequired();

        builder.Property(x => x.HashControle)
            .HasColumnName("hash_controle")
            .HasColumnType(VarChar(64))
            .HasMaxLength(64)
            .HasComment("Hash SHA-256 do arquivo para verificação de integridade e auditoria");

        ConfigureEnum(builder.Property(x => x.Status)
                .HasColumnName("status"),
            "Ativo",
            "Inativo",
            "Pendente de Processamento",
            "Erro no Upload",
            "Excluído")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<StatusArmazenamentoDocumento>(v))
            .HasComment("Status atual do documento armazenado")
            .HasDefaultValue(StatusArmazenamentoDocumento.Ativo)
            .IsRequired();

        builder.Property(x => x.CriadoPor)
            .HasColumnName("criado_por")
            .HasColumnType(VarChar(100))
            .HasMaxLength(100)
            .HasComment("Identificador do usuário que criou o registro")
            .IsRequired();

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasColumnType(DateTime())
            .HasComment("Data e hora de criação do registro")
            .HasDefaultValueSql(CurrentTimestamp())
            .IsRequired();

        builder.HasOne(x => x.Seguro)
            .WithMany(x => x.ArmazenamentosDocumentos)
            .HasForeignKey(x => x.SeguroId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SeguroId)
            .HasDatabaseName("ix_armazenamento_documento_seguro_id");

        builder.HasIndex(x => x.HashControle)
            .HasDatabaseName("ix_armazenamento_documento_hash_controle");

        builder.HasIndex(x => new { x.Local, x.UnidadeArmazenamento, x.CaminhoArquivo })
            .HasDatabaseName("ix_armazenamento_documento_local_caminho")
            .IsUnique();

        builder.HasIndex(x => x.Tipo)
            .HasDatabaseName("ix_armazenamento_documento_tipo");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("ix_armazenamento_documento_status");
    }
}
