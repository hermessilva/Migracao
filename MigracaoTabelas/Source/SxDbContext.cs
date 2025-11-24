using System.Diagnostics;

using Microsoft.EntityFrameworkCore;


namespace MigracaoTabelas.Source
{

    public class SxDbContext : DbContext
    {
        public static string Schema = "unico";
        public SxDbContext(DbContextOptions<SxDbContext> options)
            : base(options)
        {
        }

        public DbSet<SxEpSegParcela> EpSegParcelas { get; set; }
        public DbSet<SxEpSegPrestamista> EpSegPrestamista { get; set; }
        public DbSet<SxAgencia> Agencia { get; set; }
        public DbSet<SxPontoAtendimento> PontoAtendimento { get; set; }
        public DbSet<SxClientes> Cooperados { get; set; }
        public DbSet<SxContas> Contas { get; set; }
        public DbSet<SxSeguradoras> Seguradoras { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TODO : codigo da seguradora e demais campos

            modelBuilder.Entity<SxSeguradoras>(entity =>
            {
                entity.HasNoKey();
                entity.ToSqlQuery(@"select PST_CODIGO as Codigo, pst_nome as nome,'00000000000000' as cnpj,pst_nome  as razaosocial,'00000000' as cep,
                                    'RUA' as rua,'COMPLEMENTO' as complemento,'0' as numero,  'BAIRRO' as bairro,
                                    'CIDADE' as cidade,  'UF' as uf,  'TELEFONE' as telefone,  'EMAIL' as email from unico.cd_prestamista ");
            });

            modelBuilder.Entity<SxEpSegPrestamista>(entity =>
            {
                entity.HasNoKey();
                entity.ToSqlQuery(@"select pm.* from ep_segprestamista pm join cc_conta c on c.cco_conta = pm.CCO_CONTA where pm.SEG_MODALIDADE = 4 and c.CCO_SITUACAO = 1");
            });

            modelBuilder.Entity<SxEpSegParcela>(entity =>
            {
                entity.HasNoKey();
                entity.ToSqlQuery(@"select * from ep_segparcela");
            });

            modelBuilder.Entity<SxAgencia>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Codigo).HasColumnName("CODIGO");
                entity.Property(e => e.Nome).HasColumnName("NOME");

                entity.ToSqlQuery(@"select ca.AG_CODIGO as CODIGO ,ca.AG_SIGLA as NOME from unico.cd_agencia ca");
            });

            modelBuilder.Entity<SxContas>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CPFCNPJ).HasColumnName("CPFCNPJ");
                entity.Property(e => e.Codigo).HasColumnName("Codigo");
                entity.ToSqlQuery(@"select cc.CLI_CPFCNPJ CPFCNPJ ,cc.CCO_CONTA Codigo,PA_CODIGO PaCodigo  from cc_conta cc");
            });

            modelBuilder.Entity<SxPontoAtendimento>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Codigo).HasColumnName("CODIGO");
                entity.Property(e => e.Nome).HasColumnName("NOME");
                entity.Property(e => e.Agencia).HasColumnName("AGENCIA");

                entity.ToSqlQuery(@"select cp.PA_CODIGO as CODIGO,cp.AG_CODIGO AS AGENCIA,cp.PA_SIGLA As NOME  from unico.cd_pa cp");
            });
            modelBuilder.Entity<SxClientes>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Email).HasColumnName("Email");
                entity.Property(e => e.Tipo).HasColumnName("Tipo");
                entity.Property(e => e.Agencia).HasColumnName("Agencia");
                entity.Property(e => e.NumeroDocumento).HasColumnName("NumeroDocumento");
                entity.Property(e => e.NomeFantasia).HasColumnName("NomeFantasia");
                entity.Property(e => e.Nome).HasColumnName("Nome");

                entity.ToSqlQuery(@"SELECT COALESCE(cl.CLI_CPFCNPJ, '') AS numerodocumento, cl.CLI_TIPPES AS tipo, COALESCE(cl.cli_nome, '') AS nome,
                                    cl.CLI_NFANTA AS nomefantasia,cl.CLI_EMAIL AS email,cl.AG_CODIGO  AS Agencia
                                    FROM unico.cd_cliente cl");
            });


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.GetTableName());
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder pBuilder)
        {
            if (!pBuilder.IsConfigured)
            {
                var str = Utils.Read("SOURCE_DB", "");
                str = str.Replace("unico", Schema);
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
}
