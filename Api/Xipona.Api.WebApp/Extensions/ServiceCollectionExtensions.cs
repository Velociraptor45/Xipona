using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProjectHermes.Xipona.Api.Core.Constants;
using ProjectHermes.Xipona.Api.WebApp.Configs;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace ProjectHermes.Xipona.Api.WebApp.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static async Task<IServiceCollection> AddOtelAsync(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment, ISecretRetriever secretRetriever)
    {
        var otelConfig = new OtelConfig();
        configuration.GetSection("OpenTelemetry").Bind(otelConfig);

        if (string.IsNullOrWhiteSpace(otelConfig.LogsEndpoint) || string.IsNullOrWhiteSpace(otelConfig.TracesEndpoint))
        {
            Console.WriteLine("OTEL logs and/or traces endpoint not provided. Skipping OTEL configuration and integrating standard logging");
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                if (environment.IsProduction())
                    logging.SetMinimumLevel(LogLevel.Information);
            });
            return services;
        }

        var apiKeyHeader = await GetApiKeyHeaderAsync(otelConfig, secretRetriever);
        if (apiKeyHeader is null)
            Console.WriteLine("OTEL API key not provided. Skipping API key header configuration");

        var defResourceBuilder = ResourceBuilder.CreateEmpty()
            .AddService(Application.ServiceName, serviceVersion: configuration["APP_VERSION"])
            .AddAttributes(new Dictionary<string, object>()
            {
                ["deployment.environment"] = environment.EnvironmentName
            });

        services
            .AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.AddHttpClientInstrumentation();
                tracing.AddAspNetCoreInstrumentation();
                tracing.SetResourceBuilder(defResourceBuilder);
                tracing.AddSource(Application.ActivitySourceName);
                if (environment.IsEnvironment("Local"))
                {
                    tracing.AddConsoleExporter();
                }
                else
                {
                    tracing.AddConsoleExporter();
                    tracing.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(otelConfig.TracesEndpoint);
                        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                        if (apiKeyHeader is not null)
                            opt.Headers = apiKeyHeader;
                    });
                }
            });

        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddOpenTelemetry(logOpt =>
            {
                logOpt.SetResourceBuilder(defResourceBuilder);
                logOpt.IncludeFormattedMessage = true;
                logOpt.IncludeScopes = true;
                logOpt.ParseStateValues = true;
                if (environment.IsEnvironment("Local"))
                {
                    logOpt.AddConsoleExporter();
                }
                else
                {
                    logOpt.AddConsoleExporter();
                    logOpt.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(otelConfig.LogsEndpoint);
                        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                        if (apiKeyHeader is not null)
                            opt.Headers = apiKeyHeader;
                    });
                }
            });

            if (environment.IsProduction())
                logging.SetMinimumLevel(LogLevel.Information);
        });

        return services;
    }

    private static async Task<string?> GetApiKeyHeaderAsync(OtelConfig otelConfig, ISecretRetriever secretRetriever)
    {
        if (string.IsNullOrWhiteSpace(otelConfig.ApiKeyHeaderPrefix))
            return null;

        var apiKey = await secretRetriever.LoadLoggingApiKey();

        if (string.IsNullOrWhiteSpace(apiKey))
            return null;

        return $"{otelConfig.ApiKeyHeaderPrefix}{apiKey}";
    }

    internal sealed class OtelConfig
    {
        public string LogsEndpoint { get; set; } = string.Empty;
        public string TracesEndpoint { get; set; } = string.Empty;
        public string ApiKeyHeaderPrefix { get; set; } = string.Empty;
    }
}