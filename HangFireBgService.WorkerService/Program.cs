using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace HangFireBgService.WorkerService;
public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            var minhaTarefa = scope.ServiceProvider.GetRequiredService<IMeuTeste>();

            RecurringJobOptions options = new()
            {
                TimeZone = TimeZoneInfo.Local
            };

            recurringJobs.AddOrUpdate(
                "executar-job",
                () => minhaTarefa.ExecuteAsync(CancellationToken.None),
                "37 8 * * *",
                 options
            );
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHangfire(x => x.UseMemoryStorage());
                services.AddHangfireServer();
                services.AddHostedService<Worker>();
                services.AddSingleton<IMeuTeste, MeuTeste>();
                services.AddControllers();
            })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.Configure(app =>
            {
                app.UseHangfireDashboard();
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        })
        .UseSerilog();
}