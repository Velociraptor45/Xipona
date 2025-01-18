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
using ProjectHermes.Xipona.Api.Core.Files;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectHermes.Xipona.Api.WebApp.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOtel(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment, IFileLoadingService fileLoadingService)
    {
        var logsEndpointUrl = configuration["OpenTelemetry:LogsEndpoint"];
        var tracesEndpointUrl = configuration["OpenTelemetry:TracesEndpoint"];

        if (string.IsNullOrWhiteSpace(logsEndpointUrl) || string.IsNullOrWhiteSpace(tracesEndpointUrl))
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

        var includeApiKeyHeader = TryGetApiKeyHeader(configuration, fileLoadingService, out var apiKeyHeader);
        if (!includeApiKeyHeader)
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
                    tracing.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(tracesEndpointUrl);
                        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                        if (includeApiKeyHeader)
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
                    logOpt.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(logsEndpointUrl);
                        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                        if (includeApiKeyHeader)
                            opt.Headers = apiKeyHeader;
                    });
                }
            });
            if (environment.IsProduction())
                logging.SetMinimumLevel(LogLevel.Information);
        });

        return services;
    }

    private static bool TryGetApiKeyHeader(IConfiguration configuration, IFileLoadingService fileLoadingService,
        out string header)
    {
        var headerPrefix = configuration["OpenTelemetry:ApiKeyHeaderPrefix"];
        var apiKey = configuration["PH_XIPONA_OTEL_API_KEY"];
        var apiKeyFile = configuration["PH_XIPONA_OTEL_API_KEY_FILE"];

        if (string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(apiKeyFile))
            apiKey = fileLoadingService.ReadFile(apiKeyFile);

        if (string.IsNullOrWhiteSpace(headerPrefix) || string.IsNullOrWhiteSpace(apiKey))
        {
            header = null;
            return false;
        }

        header = $"{headerPrefix}{apiKey}";
        return true;
    }
}