using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class CooperadoConfiguration : IEntityTypeConfiguration<Cooperado>
{
    public void Configure(EntityTypeBuilder<Cooperado> builder)
    {
        builder.ToTable("cooperado");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(x => x.NumeroDocumento)
            .HasColumnName("numero_documento")
            .HasMaxLength(14)
            .HasComment("Documento do cooperado (CPF/CNPJ sem formatação)")
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasColumnName("tipo")
            .HasColumnType("enum('Física','Jurídica')")
            .HasConversion<string>()
            .HasComment("Tipo de pessoa")
            .IsRequired();

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasMaxLength(255)
            .HasComment("Nome/Razão social do cooperado")
            .IsRequired();

        builder.Property(x => x.NomeFantasia)
            .HasColumnName("nome_fantasia")
            .HasMaxLength(255)
            .HasComment("Nome fantasia (para PJ)");

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasComment("E-mail de contato")
            .HasMaxLength(255);
    }
}