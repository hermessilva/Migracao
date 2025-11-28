using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuario", t => t.HasComment("Tabela de usuários do sistema com suas credenciais e vínculos organizacionais"));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasComment("Identificador único do registro na tabela")
            .IsRequired();

        builder.Property(e => e.AgenciaId)
            .HasColumnName("agencia_id")
            .HasComment("Chave estrangeira referenciando a tabela agencia")
            .IsRequired();

        builder.Property(e => e.PontoAtendimentoId)
            .HasColumnName("ponto_atendimento_id")
            .HasComment("Chave estrangeira referenciando a tabela ponto_atendimento")
            .IsRequired();

        builder.Property(e => e.PerfilId)
           .HasColumnName("perfil_id")
           .HasComment("Chave estrangeira opcional referenciando a tabela perfil para o perfil principal do usuário");

        builder.Property(e => e.Login)
            .HasColumnName("login")
            .HasComment("Login de acesso do usuário ao sistema")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Nome)
            .HasColumnName("nome")
            .HasComment("Nome completo do usuário")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasComment("Endereço de e-mail do usuário para contato e notificações")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("enum('Ativo','Inativo')")
            .HasConversion(
                v => v.AsString(),
                v => EnumHelper.FromString<StatusUsuario>(v))
            .HasComment("Status do usuário: Ativo ou Inativo")
            .IsRequired();

        builder.Property(e => e.CriadoEm)
            .HasColumnName("criado_em")
            .HasComment("Data e hora de criação do registro")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        // Relacionamentos
        builder.HasOne(e => e.Perfils)
            .WithMany(p => p.Usuarios)
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