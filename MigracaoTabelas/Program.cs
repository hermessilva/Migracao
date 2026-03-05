
using System.Diagnostics;
using System.Text;

using Google.Protobuf.WellKnownTypes;

using jsreport.Binary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
                builder.Services.AddTransient<TxDbContext>();
                builder.Services.AddDbContext<TxDbContext>();
                var app = builder.Build();
                var thds = Environment.ProcessorCount;

                if (args.Length > 0)
                {
                    Console.WriteLine("Argumentos de linha de comando:");
                    if (File.Exists(args[0]))
                        ImportaDados(args[0], app.Services);
                    else
                        Console.WriteLine("Arquivo não encontrado: " + args[0]);
                    return;
                }
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

        private static void ImportaDados(string pScriptSQLFile, IServiceProvider services)
        {
            Console.Clear();
            string lcmd = "";
            TxDbContext ctx = null;
            try
            {
                var script = File.ReadAllText(pScriptSQLFile, Encoding.UTF8);
                var cmds = script.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var total = cmds.Length;
                var sw = Stopwatch.StartNew();

                using var scope = services.CreateScope();
                ctx = scope.ServiceProvider.GetRequiredService<TxDbContext>();
                ctx.Database.OpenConnection();
                ctx.Database.ExecuteSqlRaw("start transaction");

                for (int i = 0; i < total; i++)
                {
                    lcmd = cmds[i];
                    if (lcmd?.Length < "insert".Length)
                        continue;
                    ctx.Database.ExecuteSqlRaw(lcmd);
                    PrintEta(i + 1, total, sw.Elapsed);
                }

                ctx.Database.ExecuteSqlRaw("commit");
                Console.WriteLine();
                Console.WriteLine("Importação Concluída com Sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Erro ao importar dados: " + ex.Message + "\r\n[" + lcmd + "]");
                ctx.Database.ExecuteSqlRaw("rollback");
            }
        }

        private static void PrintEta(int pCurrent, int pTotal, TimeSpan pElapsed)
        {
            var avgPerItem = pElapsed.TotalSeconds / pCurrent;
            var remaining = TimeSpan.FromSeconds(avgPerItem * (pTotal - pCurrent));
            Console.Write($"\r[{pCurrent:#,##0}/{pTotal:#,##0}] Decorrido: {pElapsed:hh\\:mm\\:ss} | ETA: {remaining:hh\\:mm\\:ss}   ");
        }
    }
}
