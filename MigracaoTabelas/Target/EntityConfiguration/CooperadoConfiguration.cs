using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MigracaoTabelas.Target;

using Seguros.Helpers;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class CooperadoConfiguration : IEntityTypeConfiguration<Cooperado>
{
    public void Configure(EntityTypeBuilder<Cooperado> builder)
    {
        builder.ToTable("cooperado", t => t.HasComment("Cadastro de cooperados (clientes) da cooperativa que podem contratar seguros"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(x => x.NumeroDocumento)
            .HasColumnName("numero_documento")
            .HasMaxLength(14)
            .HasComment("Documento de identificação do cooperado (CPF com 11 dígitos ou CNPJ com 14 dígitos, sem formatação)")
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasColumnName("tipo")
            .HasColumnType("enum('Física','Jurídica')")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<TipoPessoaCooperado>(v)
            )
            .HasComment("Tipo de pessoa: Física (CPF) ou Jurídica (CNPJ)")
            .IsRequired();

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasMaxLength(255)
            .HasComment("Nome completo (pessoa física) ou razão social (pessoa jurídica) do cooperado")
            .IsRequired();

        builder.Property(x => x.NomeFantasia)
            .HasColumnName("nome_fantasia")
            .HasMaxLength(255)
            .HasComment("Nome fantasia do cooperado (aplicável apenas para pessoa jurídica)");

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .HasComment("Endereço de e-mail para contato e comunicações com o cooperado");
    }
}