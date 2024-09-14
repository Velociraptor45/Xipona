using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProjectHermes.Xipona.Api.Core.Files;
using ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.WebApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOtel(this IServiceCollection services, IConfiguration configuration,
        string environmentName, IFileLoadingService fileLoadingService)
    {
        var logsEndpointUrl = configuration["OpenTelemetry:LogsEndpoint"];
        var tracesEndpointUrl = configuration["OpenTelemetry:TracesEndpoint"];

        if (string.IsNullOrWhiteSpace(logsEndpointUrl) || string.IsNullOrWhiteSpace(tracesEndpointUrl))
        {
            Console.WriteLine("OTEL logs and/or traces endpoint not provided. Skipping OTEL configuration and integrating standard logging");
            services.AddLogging();
            return services;
        }

        var includeApiKeyHeader = TryGetApiKeyHeader(configuration, fileLoadingService, out var apiKeyHeader);
        if (!includeApiKeyHeader)
            Console.WriteLine("OTEL API key not provided. Skipping API key header configuration");

        var defResourceBuilder = ResourceBuilder.CreateEmpty()
            .AddService("Xipona.Api", serviceVersion: configuration["APP_VERSION"])
            .AddAttributes(new Dictionary<string, object>()
            {
                ["deployment.environment"] = environmentName
            });

        services
            .AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.AddHttpClientInstrumentation();
                tracing.AddAspNetCoreInstrumentation();
                tracing.SetResourceBuilder(defResourceBuilder);
                tracing.AddSource(
                    ItemCategoryController.ActivitySourceName,
                    ItemController.ActivitySourceName,
                    ManufacturerController.ActivitySourceName,
                    RecipeController.ActivitySourceName,
                    RecipeTagController.ActivitySourceName,
                    ShoppingListController.ActivitySourceName,
                    StoreController.ActivitySourceName
                    );
                tracing.AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(tracesEndpointUrl);
                    opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                    if (includeApiKeyHeader)
                        opt.Headers = apiKeyHeader;
                });
                tracing.AddConsoleExporter();
            });

        services.AddLogging(logging =>
        {
            logging.AddOpenTelemetry(logOpt =>
            {
                logOpt.SetResourceBuilder(defResourceBuilder);
                logOpt.IncludeFormattedMessage = true;
                logOpt.IncludeScopes = true;
                logOpt.ParseStateValues = true;
                logOpt.AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(logsEndpointUrl);
                    opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                    if (includeApiKeyHeader)
                        opt.Headers = apiKeyHeader;
                });
                logOpt.AddConsoleExporter();
            });
        });

        return services;
    }

    private static bool TryGetApiKeyHeader(IConfiguration configuration, IFileLoadingService fileLoadingService,
        out string header)
    {
        var headerPrefix = configuration["OpenTelemetry:ApiKeyHeaderPrefix"];
        var apiKey = configuration["PH_OTEL_API_KEY"];
        var apiKeyFile = configuration["PH_OTEL_API_KEY_FILE"];

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