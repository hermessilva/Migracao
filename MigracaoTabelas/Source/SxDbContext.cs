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
                entity.ToSqlQuery(@"SELECT 
                    PST_CODIGO AS Codigo,
                    PST_NOME AS Nome,
                    PST_CHEQUE AS Cheque,
                    PST_ECR AS Ecr,
                    PST_DESCONTO AS Desconto,
                    PST_EMPRESTIMO AS Emprestimo,
                    PST_LIMITE30 AS Limite30,
                    PST_COEF30 AS Coef30,
                    PST_DPS30 AS Dps30,
                    PST_LIMITE35 AS Limite35,
                    PST_COEF35 AS Coef35,
                    PST_DPS35 AS Dps35,
                    PST_LIMITE40 AS Limite40,
                    PST_COEF40 AS Coef40,
                    PST_DPS40 AS Dps40,
                    PST_LIMITE45 AS Limite45,
                    PST_COEF45 AS Coef45,
                    PST_DPS45 AS Dps45,
                    PST_LIMITE50 AS Limite50,
                    PST_COEF50 AS Coef50,
                    PST_DPS50 AS Dps50,
                    PST_LIMITE55 AS Limite55,
                    PST_COEF55 AS Coef55,
                    PST_DPS55 AS Dps55,
                    PST_LIMITE60 AS Limite60,
                    PST_COEF60 AS Coef60,
                    PST_DPS60 AS Dps60,
                    PST_LIMITE65 AS Limite65,
                    PST_COEF65 AS Coef65,
                    PST_DPS65 AS Dps65,
                    PST_LIMITE70 AS Limite70,
                    PST_COEF70 AS Coef70,
                    PST_DPS70 AS Dps70,
                    PST_LIMITE75 AS Limite75,
                    PST_COEF75 AS Coef75,
                    PST_DPS75 AS Dps75,
                    PST_LIMITE80 AS Limite80,
                    PST_COEF80 AS Coef80,
                    PST_DPS80 AS Dps80,
                    PST_LIMITE85 AS Limite85,
                    PST_COEF85 AS Coef85,
                    PST_DPS85 AS Dps85,
                    PST_MAXMESES AS MaxMeses,
                    PST_MINDIAS AS MinDias,
                    PST_VALORDPS AS ValorDps,
                    PST_LIMITEIDADE AS LimiteIdade,
                    EDITANDO AS Editando,
                    PST_ATIVO AS Ativo,
                    CONTA_CONTABIL_CREDITO AS ContaContabilCredito,
                    CONTA_CONTABIL_DEBITO AS ContaContabilDebito,
                    CONTA_CONTABIL_CREDITO_COMISSAO AS ContaContabilCreditoComissao,
                    CONTA_CONTABIL_DEBITO_COMISSAO AS ContaContabilDebitoComissao,
                    CONTA_CONTABIL_CREDITO_4966 AS ContaContabilCredito4966,
                    CONTA_CONTABIL_DEBITO_4966 AS ContaContabilDebito4966,
                    CONTA_CONTABIL_CREDITO_COMISSAO_4966 AS ContaContabilCreditoComissao4966,
                    CONTA_CONTABIL_DEBITO_COMISSAO_4966 AS ContaContabilDebitoComissao4966,
                    PORCENTAGEM_COMISSAO AS PorcentagemComissao
                FROM unico.cd_prestamista");
            });

            modelBuilder.Entity<SxEpSegPrestamista>(entity =>
            {
                entity.HasNoKey();
                entity.ToSqlQuery(@"select ec.con_debseguro tipopagamento,ec.con_seq contratosequencia, pm.*
                                    from ep_segprestamista pm 
                                    join cc_conta c on c.cco_conta = pm.cco_conta
                                    join ep_contrato ec on ec.cco_conta = pm.cco_conta and ec.con_ndoc = pm.seg_contrato 
									join ep_parcela as p on p.con_ndoc = ec.con_ndoc and p.cco_conta = c.cco_conta   
                                    where pm.seg_modalidade = 4 and c.cco_situacao = 1 and pm.seg_canctipo = 0 and pm.seg_fim  >= '2025-12-10' and
                                    ec.con_pgto is null and p.emp_creli is null and p.emp_pgto is null and
                                    (select count(*) from ep_segparcela es where es.seg_contrato = pm.seg_contrato and es.cco_conta = pm.cco_conta ) > 1");
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
