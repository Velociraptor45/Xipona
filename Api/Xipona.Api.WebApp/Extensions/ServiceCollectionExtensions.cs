using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.WebApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOtel(this IServiceCollection services, IConfiguration configuration,
        string environmentName)
    {
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
                tracing.AddSource("Example.Source"); // todo
                tracing.AddOtlpExporter(options =>
                {
                    // todo
                    options.Endpoint = new Uri(configuration["OpenTelemetry:TracesEndpoint"]!);
                    options.Protocol = OtlpExportProtocol.HttpProtobuf;
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
                    opt.Endpoint = new Uri(configuration["OpenTelemetry:LogsEndpoint"]!);
                    opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                });
                logOpt.AddConsoleExporter();
            });
        });

        return services;
    }
}