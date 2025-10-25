using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using SmtpServer.Storage;
using System.Runtime.InteropServices;

namespace SmtpTelegramGateway;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddYamlFile("appsettings.yaml", optional: true, reloadOnChange: true)
            .AddYamlFile($"appsettings.{builder.Environment.EnvironmentName}.yaml", optional: true, reloadOnChange: true);

        builder.Services
            .AddOptionsWithValidateOnStart<Configuration>()
            .Bind(builder.Configuration)
            .ValidateDataAnnotations();

        builder.Services
            .AddHostedService<Smtp>()
            .AddSingleton<MessageStore, Telegram>()
            .AddSystemd()
            .AddWindowsService(options => options.ServiceName = "SMTP Telegram Gateway");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);
        }

        var host = builder.Build();
        host.Run();
    }
}