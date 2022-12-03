using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sentry;
using Serilog;
using Serilog.Events;

namespace Evo.Scm;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
#if DEBUG
            .WriteTo.Async(c => c.Console())
#endif  
            .CreateLogger();

        try
        {
            Log.Information("Starting Evo.Scm.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(args);
#if !DEBUG
            builder.WebHost.UseSentry(); //debug模式下不上报异常到sentry
#endif
            builder.Host
                .AddAppSettingsSecretsJson()
                .UseAutofac()
#if DEBUG
                .UseSerilog()
#endif
                ;
           
            await builder.AddApplicationAsync<ScmHttpApiHostModule>();
            var app = builder.Build();

            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
