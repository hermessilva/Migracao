using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(e => e.UsuarioLogin)
            .HasColumnName("usuario")
            .HasComment("Login de acesso do usuário")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Nome)
            .HasColumnName("nome")
            .HasComment("Nome completo do usuário")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasComment("E-mail do usuário")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Ativo)
            .HasColumnName("ativo")
            .HasColumnType("tinyint(1)")
            .HasComment("ENUM('Ativo', 'Inativo') — Indica o status do perfil")
            .IsRequired();

        builder.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasComment("Data/hora de criação do registro")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
    }
}