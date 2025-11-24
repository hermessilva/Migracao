using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuario");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        builder.Property(e => e.Login)
            .HasColumnName("login")
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

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("enum('Ativo','Inativo')")
            .HasConversion(
                v => v.AsString(),
                v => EnumEx.FromString<StatusUsuario>(v))
            .HasComment("Indica o status do usuário")
            .IsRequired();


        builder.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasComment("Data/hora de criação do registro")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Propriedades de chave estrangeira
        builder.Property(e => e.PerfilId)
           .HasColumnName("perfil_id")
           .HasComment("Chave estrangeira para a Perfil ")
           .IsRequired();

        builder.Property(e => e.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira para a Agência")
            .IsRequired();

        builder.Property(e => e.PontoAtendimentoId)
            .HasColumnName("ponto_atendimento_id")
            .HasComment("Chave estrangeira para Ponto de Atendimento")
            .IsRequired();

        // Relacionamentos
        builder.HasOne(e => e.Perfils)
            .WithMany(p => p.Usuarios)
            .IsRequired()
            .HasForeignKey(e => e.PerfilId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Agencias)
            .WithMany(a => a.Usuarios)
            .IsRequired()
            .HasForeignKey(e => e.AgenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PontosAtendimentos)
            .WithMany(p => p.Usuarios)
            .IsRequired()
            .HasForeignKey(e => e.PontoAtendimentoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}