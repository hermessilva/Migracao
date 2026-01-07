
using jsreport.Binary;

using Microsoft.AspNetCore.Mvc;

using MigracaoTabelas.DEVDB;
using MigracaoTabelas.Source;
using MigracaoTabelas.Target;
using MigracaoTabelas.Worker;

using Serilog; // Adicionar Serilog
using Serilog.Exceptions; // Adicionar Serilog.Exceptions
using Serilog.Exceptions.Core; // Adicionar Serilog.Exceptions.EntityFrameworkCore
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;

namespace MigracaoTabelas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TesteReport();
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build())
               .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers().WithDestructurers([new DbUpdateExceptionDestructurer()]))
               .Enrich.FromLogContext()
               .CreateLogger();

            try
            {
                Log.Information("Iniciando aplicação");

                var builder = WebApplication.CreateBuilder();
                builder.Host.UseSerilog();
                builder.Services.AddTransient<SxDbContext>();
                builder.Services.AddDbContext<SxDbContext>();
                builder.Services.AddScoped<TxDbContext>();
                builder.Services.AddDbContext<TxDbContext>();           
                builder.Services.AddScoped<DEVDBContext>();
                builder.Services.AddDbContext<DEVDBContext>();
                var app = builder.Build();
                var thds = Environment.ProcessorCount;

                Log.Information("Iniciando a migração das tabelas...");
                using (var scope = app.Services.CreateScope())
                {
                    var TxContext = scope.ServiceProvider.GetRequiredService<TxDbContext>();
                    var migrator = new MigratorWorker(scope, TxContext);
                    migrator.Migrate();
                    Console.WriteLine();
                    Log.Information($"Migração Consluida com sucesso, Total de Contratos Migrados [{migrator.TotalMigrado:#,##0}] de Um Total Geral [{migrator.TotalContratos:#,##0}].");
                }
                Console.WriteLine("Tecle Enter para finalizar.");

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void TesteReport()
        {
            var data = CreateData();
            var rs = new jsreport.Local.LocalReporting()
                        .UseBinary(JsReportBinary.GetBinary())
                        .AsUtility()
                        .Create();
            var html = File.ReadAllText("D:\\CrediSIS\\Temp\\Modelo\\Modelo.html");
            // Renderizando o PDF com dados (campos mesclados)
            var report = rs.RenderAsync(new jsreport.Types.RenderRequest()
            {
                Template = new jsreport.Types.Template()
                {
                    Content = html,
                    Engine = jsreport.Types.Engine.Handlebars,
                    Recipe = jsreport.Types.Recipe.ChromePdf
                },
                Data = data
            }).Result;

            // Salva o resultado localmente
            using (var fs = File.Create("C:\\temp\\Relatorio.pdf"))
            {
                report.Content.CopyTo(fs);
            }
        }

        private static object CreateData()
        {
            var proposta = new PropostaPrestamistaCampos
            {
                // --- CABEÇALHO E ESTIPULANTE ---
                proposta_numero = "2025.001.98765",
                estipulante_nome = "BANCO DE INVESTIMENTOS ALFA S.A.",
                estipulante_cnpj = "12.345.678/0001-99",
                agencia_nome = "AGÊNCIA CENTRAL MATRIZ",
                apolice_numero = "PR-99887766",
                codigo_grupo = "GRP-VITA-01",
                vigencia_inicio = "18/12/2025",
                vigencia_fim = "18/12/2027",
                subestipulante_nome = "CORRETORA DE SEGUROS BETA LTDA",
                subestipulante_cnpj = "98.765.432/0001-11",

                // --- DADOS DO PROPONENTE ---
                proponente_nome = "CARLOS EDUARDO DOS SANTOS",
                proponente_matricula = "889900-X",
                proponente_cpf = "123.456.789-00",
                proponente_nome_social = "", // Campo opcional
                proponente_rg = "MG-12.345.678",
                rg_orgao = "SSP/MG",
                rg_data_expedicao = "15/05/2012",
                rg_pais_expedicao = "BRASIL",
                sexo_m = "☑", // Simboliza o checkbox marcado
                sexo_f = "☐", // Simboliza o checkbox desmarcado
                data_nascimento = "22/10/1980",
                estado_civil = "CASADO",
                profissao = "ENGENHEIRO CIVIL",
                renda_mensal = "12.500,00",
                patrimonio = "450.000,00",
                email = "carlos.santos@exemplo.com.br",
                nacionalidade = "BRASILEIRA",
                endereco_logradouro = "AVENIDA PAULISTA",
                endereco_numero = "1000",
                endereco_complemento = "APTO 151",
                endereco_bairro = "BELA VISTA",
                endereco_cidade = "SÃO PAULO",
                endereco_uf_cep = "SP - 01310-100",
                telefone = "(11) 99888-7766",
                emprestimo_valor = "100.000,00",
                emprestimo_vigencia = "24 MESES",
                ppe_sim = "☐",
                ppe_nao = "☑",
                residente_sim = "☑",
                residente_nao = "☐",

                // --- DADOS DO PLANO ---
                check_morte = "☑",
                capital_morte = "100.000,00",
                premio_morte = "450,00",
                check_ipta = "☑",
                capital_ipta = "100.000,00",
                premio_ipta = "50,00",
                valor_iof = "1,90",
                premio_total = "501,90",
                prazo_emprestimo = "24",
                pgto_pu = "☑", // Pagamento Único
                pgto_mensal = "☐",
                qtd_parcelas = "1",

                // --- DECLARAÇÃO PESSOAL DE SAÚDE (DPS) ---
                dps_q1 = "NÃO",
                dps_q2 = "NÃO",
                dps_q3 = "NÃO",
                dps_q4 = "NÃO",
                dps_q5 = "NÃO",
                dps_q6 = "NÃO",
                dps_q7 = "NÃO",
                dps_q8 = "NÃO",
                dps_q9 = "NÃO",
                dps_q10 = "NÃO",
                dps_q11 = "SIM",
                dps_q12 = "NÃO",
                dps_q13 = "NÃO",
                dps_q14 = "NÃO",
                peso_kg = "85",
                altura_m = "1.82",
                dps_justificativa = "Possui seguro de vida individual em outra companhia.",

                // --- BENEFICIÁRIOS E ASSINATURA ---
                benef_nome_1 = "LUCIA REGINA SANTOS",
                benef_afin_1 = "CÔNJUGE",
                benef_perc_1 = "100%",
                benef_nome_2 = "",
                benef_afin_2 = "",
                benef_perc_2 = "",
                autorizacao_debito = "SIM, AUTORIZADO",
                local_data_assinatura = "SÃO PAULO, 18 DE DEZEMBRO DE 2025",
                assinatura_imagem = "base64_string_da_assinatura_aqui"
            };
            return proposta;
        }
    }

    public class PropostaPrestamistaCampos
    {
        // --- CABEÇALHO E ESTIPULANTE ---
        public string proposta_numero { get; set; }
        public string estipulante_nome { get; set; }
        public string estipulante_cnpj { get; set; }
        public string agencia_nome { get; set; }
        public string apolice_numero { get; set; }
        public string codigo_grupo { get; set; }
        public string vigencia_inicio { get; set; }
        public string vigencia_fim { get; set; }
        public string subestipulante_nome { get; set; }
        public string subestipulante_cnpj { get; set; }

        // --- DADOS DO PROPONENTE ---
        public string proponente_nome { get; set; }
        public string proponente_matricula { get; set; }
        public string proponente_cpf { get; set; }
        public string proponente_nome_social { get; set; }
        public string proponente_rg { get; set; }
        public string rg_orgao { get; set; }
        public string rg_data_expedicao { get; set; }
        public string rg_pais_expedicao { get; set; }
        public string sexo_m { get; set; } // Enviar "X" ou "☑"
        public string sexo_f { get; set; }
        public string data_nascimento { get; set; }
        public string estado_civil { get; set; }
        public string profissao { get; set; }
        public string renda_mensal { get; set; }
        public string patrimonio { get; set; }
        public string email { get; set; }
        public string nacionalidade { get; set; }
        public string endereco_logradouro { get; set; }
        public string endereco_numero { get; set; }
        public string endereco_complemento { get; set; }
        public string endereco_bairro { get; set; }
        public string endereco_cidade { get; set; }
        public string endereco_uf_cep { get; set; }
        public string telefone { get; set; }
        public string emprestimo_valor { get; set; }
        public string emprestimo_vigencia { get; set; }
        public string ppe_sim { get; set; }
        public string ppe_nao { get; set; }
        public string residente_sim { get; set; }
        public string residente_nao { get; set; }

        // --- DADOS DO PLANO (TABELA) ---
        public string check_morte { get; set; }
        public string capital_morte { get; set; }
        public string premio_morte { get; set; }
        public string check_ipta { get; set; }
        public string capital_ipta { get; set; }
        public string premio_ipta { get; set; }
        public string valor_iof { get; set; }
        public string premio_total { get; set; }
        public string prazo_emprestimo { get; set; }
        public string pgto_pu { get; set; }
        public string pgto_mensal { get; set; }
        public string qtd_parcelas { get; set; }

        // --- DPS (PÁGINA 2) ---
        public string dps_q1 { get; set; }
        public string dps_q2 { get; set; }
        public string dps_q3 { get; set; }
        public string dps_q4 { get; set; }
        public string dps_q5 { get; set; }
        public string dps_q6 { get; set; }
        public string dps_q7 { get; set; }
        public string dps_q8 { get; set; }
        public string dps_q9 { get; set; }
        public string dps_q10 { get; set; }
        public string dps_q11 { get; set; }
        public string dps_q12 { get; set; }
        public string dps_q13 { get; set; }
        public string dps_q14 { get; set; }
        public string peso_kg { get; set; }
        public string altura_m { get; set; }
        public string dps_justificativa { get; set; }

        // --- BENEFICIÁRIOS E ASSINATURA ---
        public string benef_nome_1 { get; set; }
        public string benef_afin_1 { get; set; }
        public string benef_perc_1 { get; set; }
        public string benef_nome_2 { get; set; }
        public string benef_afin_2 { get; set; }
        public string benef_perc_2 { get; set; }
        public string autorizacao_debito { get; set; }
        public string local_data_assinatura { get; set; }
        public string assinatura_imagem { get; set; }
    }
}
