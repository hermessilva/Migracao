using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace MigracaoTabelas.Target
{

    public class TxDbContext : DbContext
    {
        public TxDbContext(DbContextOptions<TxDbContext> options)
            : base(options)
        {
        }

        public DbSet<Auditoria> Auditoria { get; set; }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Tela> Tela { get; set; }
        public DbSet<Acao> Acao { get; set; }
        public DbSet<TelaAcao> TelaAcao { get; set; }
        public DbSet<TelaAcaoPerfil> TelaAcaoPerfil { get; set; }
        public DbSet<Agencia> Agencia { get; set; }
        public DbSet<PontoAtendimento> PontoAtendimento { get; set; }
        public DbSet<Seguradora> Seguradora { get; set; }
        public DbSet<CondicaoSeguradora> CondicaoSeguradora { get; set; }
        public DbSet<SeguradoraLimite> SeguradoraLimite { get; set; }
        public DbSet<ContabilizacaoSeguradora> ContabilizacaoSeguradora { get; set; }
        public DbSet<ApoliceGrupoSeguradora> ApoliceGrupoSeguradora { get; set; }
        public DbSet<GestaoDocumento> GestaoDocumento { get; set; }
        public DbSet<Cooperado> Cooperado { get; set; }
        public DbSet<CooperadoAgenciaConta> CooperadoAgenciaConta { get; set; }
        public DbSet<Seguro> Seguro { get; set; }
        public DbSet<SeguroParametro> SeguroParametro { get; set; }
        public DbSet<SeguroCancelamento> SeguroCancelamento { get; set; }
        public DbSet<Parcela> Parcela { get; set; }
        public DbSet<Parametrizacao> Parametrizacao { get; set; }
        public DbSet<ParametrizacaoResposta> ParametrizacaoResposta { get; set; }
        public DbSet<LancamentoEfetivar> LancamentoEfetivar { get; set; }
        public DbSet<IntegracaoSenior> IntegracaoSenior { get; set; }
        public DbSet<ComissaoSeguradora> ComissaoSeguradora { get; set; }
        public DbSet<ContaCorrenteSeguradora> ContaCorrenteSeguradora { get; set; }
        public DbSet<PropostaSeguradora> PropostaSeguradora { get; set; }
        public DbSet<AgenciaSeguradora> AgenciaSeguradora { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TxDbContext).Assembly);

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Conventions.Add(_ => (IConvention)new NoForeignKeyIndexConvention());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder pBuilder)
        {
            if (!pBuilder.IsConfigured)
            {
                var str = Utils.Read("TARGET_DB", "");
                pBuilder.UseMySQL(str);
            }
#if DEBUG

            pBuilder.EnableSensitiveDataLogging(true);
            pBuilder.LogTo(s => Debug.WriteLine(s));
            pBuilder.EnableDetailedErrors(true);
            pBuilder.EnableSensitiveDataLogging(true);
#endif
        }
    }
    public sealed class NoForeignKeyIndexConvention : IForeignKeyAddedConvention
    {
        public void ProcessForeignKeyAdded(
            IConventionForeignKeyBuilder relationshipBuilder,
            IConventionContext<IConventionForeignKeyBuilder> context)
        {
            var fk = relationshipBuilder.Metadata;
            var entity = fk.DeclaringEntityType;

            // Localiza o índice criado automaticamente pela convenção
            var index = entity.FindIndex(fk.Properties);
            if (index == null)
                return;

            // Verifica se foi criado por convenção (implícito)
            var source = index.GetConfigurationSource();
            if (source == ConfigurationSource.Convention)
                entity.RemoveIndex(index);
        }
    }
}


