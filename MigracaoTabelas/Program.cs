
using jsreport.Binary;
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
                builder.Services.AddScoped<TxDbContext>();
                builder.Services.AddDbContext<TxDbContext>();           
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
    }
}
