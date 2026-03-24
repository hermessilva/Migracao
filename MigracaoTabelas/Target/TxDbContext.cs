using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;

using MigracaoTabelas.Source;

namespace MigracaoTabelas.Target
{

    public class TxDbContext : DbContext
    {
        public string Connection = "";
        public TxDbContext(DbContextOptions<TxDbContext> options)
            : base(options)
        {
        }

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
        public DbSet<SeguroCancelamento> SeguroCancelamento { get; set; }
        public DbSet<Parcela> Parcela { get; set; }
        public DbSet<Parametrizacao> Parametrizacao { get; set; }
        public DbSet<LancamentoEfetivar> LancamentoEfetivar { get; set; }
        public DbSet<IntegracaoSenior> IntegracaoSenior { get; set; }
        public DbSet<ComissaoSeguradora> ComissaoSeguradora { get; set; }
        public DbSet<ContaCorrenteSeguradora> ContaCorrenteSeguradora { get; set; }
        public DbSet<PropostaSeguradora> PropostaSeguradora { get; set; }
        public DbSet<SeguroParametro> SeguroParametro { get; set; }
        public DbSet<EventoOutbox> EventosOutbox { get; set; }
        public DbSet<FaturamentoParcela> FaturamentoParcela { get; set; }
        public DbSet<FaturamentoImportacaoHistorico> FaturamentoImportacaoHistorico { get; set; }
        public DbSet<ArmazenamentoDocumento> ArmazenamentoDocumento { get; set; }
        public DbSet<BaixaComissao> BaixaComissao { get; set; }
        public DbSet<SeguroHistoricoPagamento> SeguroHistoricoPagamento{ get; set; }

        public List<(string Table, string Insert)> CapturedInserts { get; } = new List<(string Table, string Insert)>();

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
            pBuilder.AddInterceptors(new MySqlAuditInterceptor(this));

            if (!pBuilder.IsConfigured)
            {
                var str = Utils.Read("TARGET_DB", "");
                if (Connection != "")
                    str = Connection;
                pBuilder.UseMySQL(str);
            }

            pBuilder.LogTo(s => Debug.WriteLine(s), (eventId, logLevel) =>
            {
                return logLevel == LogLevel.Error || eventId.Id == RelationalEventId.CommandError.Id;
            });

            pBuilder.EnableDetailedErrors(true);
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

    public class MySqlAuditInterceptor : SaveChangesInterceptor
    {
        // Lista temporária para segurar as referências das entidades que entraram como 'Added'
        private List<EntityEntry>? _entriesToBeInserted;
        private TxDbContext txDbContext;

        public MySqlAuditInterceptor(TxDbContext txDbContext)
        {
            this.txDbContext = txDbContext;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            CaptureInserts(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            CaptureInserts(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void CaptureInserts(DbContext? context)
        {
            if (context == null)
                return;

            // Capturamos as referências das entidades ANTES de elas perderem o estado 'Added'
            _entriesToBeInserted = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .ToList();
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            ProcessCapturedInserts();
            return base.SavedChanges(eventData, result);
        }

        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            ProcessCapturedInserts();
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        private void ProcessCapturedInserts()
        {
            if (_entriesToBeInserted == null || !_entriesToBeInserted.Any())
                return;

            foreach (var entry in _entriesToBeInserted)
            {
                var entityType = entry.Metadata;
                var tableName = entityType.GetTableName();
                var columns = new List<string>();
                var values = new List<string>();

                foreach (var property in entry.CurrentValues.Properties)
                {
                    columns.Add($"`{property.GetColumnName()}`");

                    // Como já estamos no SavedChanges, o valor aqui já será o '64' (ID real)
                    var value = entry.CurrentValues[property];
                    values.Add(FormatValueForMySql(value));
                }

                var sql = $"INSERT INTO `{tableName}` ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)});";
                txDbContext.CapturedInserts.Add((tableName, sql));
                //File.AppendAllText(@$"D:\CrediSIS\DBs\inserts.sql", sql + Environment.NewLine);
            }

            // Limpa a lista para não duplicar no próximo SaveChanges do mesmo DbContext
            _entriesToBeInserted.Clear();
        }

        private string FormatValueForMySql(object? value)
        {
            if (value == null)
                return "NULL";

            if (value.GetType().IsEnum)
            {
                var field = value.GetType().GetField(value.ToString());
                var attr = field.GetCustomAttribute<DescriptionAttribute>();
                return $"'{(attr == null ? value.ToString() : attr.Description)}'";
            }

            return value switch
            {
                string s => $"'{s.Replace("'", "''")}'",
                bool b => b ? "1" : "0",
                DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
                Guid g => $"'{g}'",
                long l => l.ToString(CultureInfo.InvariantCulture),
                ulong ul => ul.ToString(CultureInfo.InvariantCulture),
                int i => i.ToString(CultureInfo.InvariantCulture),
                _ => value.ToString()?.Replace(",", ".") ?? "NULL"
            };
        }
    }

    public class xMySqlAuditInterceptor : SaveChangesInterceptor
    {
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            GenerateMySqlInserts(eventData.Context);
            return base.SavedChanges(eventData, result);
        }

        private void GenerateMySqlInserts(DbContext? context)
        {
            if (context == null)
                return;

            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                var entityType = entry.Metadata;
                var tableName = entityType.GetTableName();

                var columns = new List<string>();
                var values = new List<string>();

                foreach (var property in entry.CurrentValues.Properties)
                {
                    // Removida a restrição de ValueGenerated.OnAdd para incluir as PKs
                    columns.Add($"`{property.GetColumnName()}`");

                    var val = entry.Property(property.Name).CurrentValue;
                    if (entry.Property(property.Name).IsTemporary)
                    {
                        // Se for temporário, o EF ainda não "sabe" o valor final do banco.
                        // Mas se você setou 64 na mão, tente ler direto da entidade:
                        val = entry.Entity.GetType().GetProperty(property.Name)?.GetValue(entry.Entity);
                    }
                    values.Add(FormatValueForMySql(val));
                }

                var sql = $"INSERT INTO `{tableName}` ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)});";
                File.AppendAllText(@$"D:\CrediSIS\DBs\inserts.sql", sql + Environment.NewLine);

            }
        }


        private string FormatValueForMySql(object? value)
        {
            if (value == null)
                return "NULL";

            // 1. Trata Enums primeiro
            if (value.GetType().IsEnum)
            {
                return $"'{value.ToString()?.Replace("'", "''")}'";
            }

            return value switch
            {
                string s => $"'{s.Replace("'", "''")}'",
                bool b => b ? "1" : "0",
                DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
                Guid g => $"'{g}'",

                // 2. Números Inteiros de 64 bits (Onde mora o problema)
                // Usamos a cultura invariante para evitar qualquer separador de milhar
                long l => l.ToString(CultureInfo.InvariantCulture),
                ulong ul => ul.ToString(CultureInfo.InvariantCulture),
                int i => i.ToString(CultureInfo.InvariantCulture),
                uint ui => ui.ToString(CultureInfo.InvariantCulture),

                // 3. Números Decimais
                float f => f.ToString(CultureInfo.InvariantCulture),
                double d => d.ToString(CultureInfo.InvariantCulture),
                decimal dec => dec.ToString(CultureInfo.InvariantCulture),

                _ => $"'{value.ToString()?.Replace("'", "''")}'"
            };
        }
    }
}


