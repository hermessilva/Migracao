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
        public DbSet<SxEpParcela> EpParcela { get; set; }
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

            modelBuilder.Entity<SxEpParcela>(entity =>
            {
                entity.HasNoKey();
                entity.ToSqlQuery(@"select es.* from ep_parcela  es join ep_segprestamista es2 on es2.CCO_CONTA = es.CCO_CONTA and 
                                    es2.SEG_CONTRATO = es.CON_NDOC and es2.CON_SEQ = es.CON_SEQ where es.EMP_VLRSEG is not null");
            });

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
                    PORCENTAGEM_COMISSAO AS PorcentagemComissao
                FROM unico.cd_prestamista");
            });

            modelBuilder.Entity<SxEpSegPrestamista>(entity =>
            {
                entity.HasNoKey();
                entity.ToSqlQuery(@$"
WITH ParcelasPendentes AS (
    SELECT con_ndoc, con_seq, 'P' as origem
    FROM ep_parcela 
    WHERE emp_pagseg IS NULL AND emp_vlrseg > 0
    UNION 
    SELECT seg_contrato, con_seq, 'S' as origem
    FROM ep_segparcela 
    WHERE seg_pgto IS NULL
),
ResumoFinanceiro AS (
    SELECT 
        seg_contrato AS contrato, con_seq,
        SUM(seg_valor) AS total_seg,
        COUNT(seg_parcela) AS qtd_seg
    FROM ep_segparcela
    GROUP BY 1, 2
)
SELECT 
    CASE 
        WHEN C.CON_DEBSEGURO = 2 OR C.CON_PARCELAS = 1 THEN 1 
        WHEN COALESCE(RF.total_seg, 0) > 0 THEN 2 
        ELSE 3 
    END AS tipo_seguro,
    CASE WHEN C.MOD_CALCULO IN (2, 3) THEN 1 ELSE 2 END AS tipo_saldo,
    C.CON_PARCELAS AS parc_emprestimo,
    COALESCE(RF.total_seg, 0) AS Soma_Das_Parcelas,
    COALESCE(RF.qtd_seg, 0) AS Qtd_Parcelas_Seguro,
    C.con_seq AS contratosequencia,
    S.*
FROM ep_segprestamista S
INNER JOIN ep_contrato C ON S.SEG_CONTRATO = C.CON_NDOC AND S.CON_SEQ = C.CON_SEQ
INNER JOIN cc_conta cc ON cc.cco_conta = S.cco_conta
LEFT JOIN ResumoFinanceiro RF ON S.SEG_CONTRATO = RF.contrato AND S.CON_SEQ = RF.con_seq
LEFT JOIN ParcelasPendentes PP ON S.SEG_CONTRATO = PP.con_ndoc AND S.CON_SEQ = PP.con_seq
WHERE 
    S.seg_modalidade = 4 
    AND cc.cco_situacao = 1 
    AND S.seg_canctipo = 0 
    AND S.sql_deleted = 'F' 
    AND S.PST_CODIGO <> '0007'
    AND ((C.con_pgto IS NULL AND S.seg_fim >= '2026-03-03') OR 
        (C.con_pgto IS NOT NULL AND PP.con_ndoc IS NOT NULL))
GROUP BY S.SEG_CONTRATO, S.CON_SEQ, C.CON_NDOC, C.CON_SEQ
");
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
                entity.Property(e => e.Sigla).HasColumnName("SIGLA");

                entity.ToSqlQuery(@"select ca.AG_CODIGO as CODIGO ,ca.AG_SIGLA as SIGLA, ca.AG_RAZAO as NOME from unico.cd_agencia ca");
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
                                    cl.CLI_NFANTA AS nomefantasia,cl.CLI_EMAIL AS email,cl.AG_CODIGO  AS Agencia , cl.CLI_NASC Nascimento,
                                    if(cl.CLI_SEXO=1,'M','F') Sexo FROM unico.cd_cliente cl");
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
