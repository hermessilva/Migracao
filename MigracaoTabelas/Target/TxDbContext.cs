using System.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace MigracaoTabelas.Target
{

    public class TxDbContext : DbContext
    {
        public TxDbContext(DbContextOptions<TxDbContext> options)
            : base(options)
        {
        }

        // DbSets para todas as entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<Tela> Telas { get; set; }
        public DbSet<Acao> Acoes { get; set; }
        public DbSet<TelaAcao> TelasAcoes { get; set; }
        public DbSet<TelaAcaoPerfil> TelasAcoesPerfis { get; set; }
        public DbSet<Agencia> Agencias { get; set; }
        public DbSet<PontoAtendimento> PontosAtendimento { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Seguradora> Seguradoras { get; set; }
        public DbSet<ParametroSeguradora> ParametrosSeguradora { get; set; }
        public DbSet<Limite> Limites { get; set; }
        public DbSet<ParametroSeguradoraLimite> ParametrosLimites { get; set; }
        public DbSet<ContaContabil> ContasContabeis { get; set; }
        public DbSet<ParametroSeguradoraContaContabil> ParametrosContasContabeis { get; set; }
        public DbSet<GrupoSeguradora> GruposSeguradora { get; set; }
        public DbSet<GestaoDocumento> GestaoDocumentos { get; set; }
        public DbSet<Priorizacao> Priorizacoes { get; set; }
        public DbSet<Cooperado> Cooperados { get; set; }
        public DbSet<CooperadoAgenciaConta> CooperadosAgenciasContas { get; set; }
        public DbSet<Seguro> Seguros { get; set; }
        public DbSet<Parcela> Parcelas { get; set; }
        public DbSet<Parametrizacao> Parametrizacoes { get; set; }
        public DbSet<ParametrizacaoResposta> ParametrizacaoRespostas { get; set; }
        public DbSet<LancamentoEfetivar> LancamentosEfetivar { get; set; }
        public DbSet<IntegracaoSenior> IntegracaoSenior { get; set; }
        public DbSet<AgenciaSeguradora> AgenciasSeguradoras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de chaves primárias e relacionamentos

            // Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Agencia)
                      .WithMany(p => p.Usuarios)
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.PontoAtendimento)
                      .WithMany(p => p.Usuarios)
                      .HasForeignKey(d => d.PontoAtendimentoId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Perfil)
                      .WithMany(p => p.Usuarios)
                      .HasForeignKey(d => d.PerfilId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // TelasAcoes - Tem Id como PK, mas também índice único em (tela_id, acao_id)
            modelBuilder.Entity<TelaAcao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.TelaId, e.AcaoId }).IsUnique();
                entity.HasOne(d => d.Tela)
                      .WithMany(p => p.TelasAcoes)
                      .HasForeignKey(d => d.TelaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Acao)
                      .WithMany(p => p.TelasAcoes)
                      .HasForeignKey(d => d.AcaoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // TelasAcoesPerfis - Tem Id como PK, mas também índice único em (tela_id, acao_id, perfil_id)
            modelBuilder.Entity<TelaAcaoPerfil>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.TelaId, e.AcaoId, e.PerfilId }).IsUnique();
                entity.HasOne(d => d.Tela)
                      .WithMany(p => p.TelasAcoesPerfis)
                      .HasForeignKey(d => d.TelaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Acao)
                      .WithMany(p => p.TelasAcoesPerfis)
                      .HasForeignKey(d => d.AcaoId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Perfil)
                      .WithMany(p => p.TelasAcoesPerfis)
                      .HasForeignKey(d => d.PerfilId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // PontosAtendimento
            modelBuilder.Entity<PontoAtendimento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Agencia)
                      .WithMany(p => p.PontosAtendimento)
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.AgenciaId, e.Codigo }).IsUnique();
            });

            // Auditoria
            modelBuilder.Entity<Auditoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Usuario)
                      .WithMany(p => p.Auditorias)
                      .HasForeignKey(d => d.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Agencia)
                      .WithMany(p => p.Auditorias)
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ParametrosSeguradora
            modelBuilder.Entity<ParametroSeguradora>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Seguradora)
                      .WithMany(p => p.ParametrosSeguradora)
                      .HasForeignKey(d => d.SeguradoraId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ParametrosLimites - Tem Id como PK, mas também índice único em (parametro_id, limite_id)
            modelBuilder.Entity<ParametroSeguradoraLimite>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ParametroId, e.LimiteId }).IsUnique();
                entity.HasOne(d => d.Parametro)
                      .WithMany(p => p.ParametrosLimites)
                      .HasForeignKey(d => d.ParametroId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Limite)
                      .WithMany(p => p.ParametrosLimites)
                      .HasForeignKey(d => d.LimiteId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ParametrosContasContabeis - Tem Id como PK, mas também índice único em (parametro_seguradora_id, conta_contabil_id)
            modelBuilder.Entity<ParametroSeguradoraContaContabil>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ParametroSeguradoraId, e.ContaContabilId }).IsUnique();
                entity.HasOne(d => d.ParametroSeguradora)
                      .WithMany(p => p.ParametrosContasContabeis)
                      .HasForeignKey(d => d.ParametroSeguradoraId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.ContaContabil)
                      .WithMany(p => p.ParametrosContasContabeis)
                      .HasForeignKey(d => d.ContaContabilId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // GruposSeguradora
            modelBuilder.Entity<GrupoSeguradora>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Agencia)
                      .WithMany(p => p.GruposSeguradora)
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // GestaoDocumentos
            modelBuilder.Entity<GestaoDocumento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Seguradora)
                      .WithMany(p => p.GestaoDocumentos)
                      .HasForeignKey(d => d.SeguradoraId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Priorizacoes
            modelBuilder.Entity<Priorizacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Agencia)
                      .WithMany(p => p.Priorizacoes)
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Seguradora)
                      .WithMany(p => p.Priorizacoes)
                      .HasForeignKey(d => d.SeguradoraId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Cooperados
            modelBuilder.Entity<Cooperado>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // CooperadosAgenciasContas
            modelBuilder.Entity<CooperadoAgenciaConta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Cooperado)
                      .WithMany(p => p.CooperadosAgenciasContas)
                      .HasForeignKey(d => d.CooperadoId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Agencia)
                      .WithMany(p => p.CooperadosAgenciasContas)
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.CooperadoId, e.AgenciaId, e.ContaCorrente }).IsUnique();
            });

            // Seguros
            modelBuilder.Entity<Seguro>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.PontoAtendimento)
                      .WithMany(p => p.Seguros)
                      .HasForeignKey(d => d.PontoAtendimentoId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.ContaCorrente)
                      .WithMany(p => p.Seguros)
                      .HasForeignKey(d => d.CooperadoAgenciaContaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.AgenciaSeguradora)
                      .WithMany(p => p.Seguros)
                      .HasForeignKey(d => d.AgenciaSeguradoraId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Usuario)
                      .WithMany()
                      .HasForeignKey(d => d.UsuarioId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Parcelas
            modelBuilder.Entity<Parcela>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Seguro)
                      .WithMany(p => p.Parcelas)
                      .HasForeignKey(d => d.SeguroId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Parametrizacoes
            modelBuilder.Entity<Parametrizacao>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // ParametrizacaoResposta
            modelBuilder.Entity<ParametrizacaoResposta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Parametrizacao)
                      .WithMany(p => p.Respostas)
                      .HasForeignKey(d => d.ParametrizacaoId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Agencia)
                      .WithMany(p => p.ParametrizacaoRespostas)
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // LancamentosEfetivar
            modelBuilder.Entity<LancamentoEfetivar>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Agencia)
                      .WithMany(p => p.LancamentosEfetivar)
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Cooperado)
                      .WithMany(p => p.LancamentosEfetivar)
                      .HasForeignKey(d => d.CooperadoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // IntegracaoSenior
            modelBuilder.Entity<IntegracaoSenior>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Agencia)
                      .WithMany(p => p.IntegracaoSenior)
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // AgenciasSeguradoras
            modelBuilder.Entity<AgenciaSeguradora>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Agencia)
                      .WithMany()
                      .HasForeignKey(d => d.AgenciaId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Seguradora)
                      .WithMany()
                      .HasForeignKey(d => d.SeguradoraId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurações globais
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.GetTableName());
            }

            modelBuilder.Entity<Agencia>(entity =>
            {
                entity.HasIndex(e => e.Codigo).IsUnique();
                entity.HasIndex(e => e.Nome).IsUnique();
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasIndex(e => e.Nome).IsUnique();
            });

            modelBuilder.Entity<Seguradora>(entity =>
            {
                entity.HasIndex(e => e.Cnpj).IsUnique();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder pBuilder)
        {
            if (!pBuilder.IsConfigured)
            {
                var str = Utils.Read("TARGET_DB", "");
                pBuilder.UseMySQL(str);
            }
//#if DEBUG

//            pBuilder.EnableSensitiveDataLogging(true);
//            pBuilder.LogTo(s => Debug.WriteLine(s));
//            pBuilder.EnableDetailedErrors(true);
//            pBuilder.EnableSensitiveDataLogging(true);
//#endif
        }
    }
}

