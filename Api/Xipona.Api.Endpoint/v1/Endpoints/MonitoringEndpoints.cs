using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;

public static class MonitoringEndpoints
{
    private const string _routeBase = "v1/monitoring";

    public static void RegisterMonitoringEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.RegisterIsAlive();
    }

    private static IEndpointRouteBuilder RegisterIsAlive(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/alive", IsAlive)
            .WithName("IsAlive")
            .Produces<bool>()
            .RequireAuthorization("User");

        return builder;
    }

    internal static Task<IResult> IsAlive()
    {
        return Task.FromResult(Results.Ok(true));
    }
}