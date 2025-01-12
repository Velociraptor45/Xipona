using Microsoft.AspNetCore.Http;

namespace ProjectHermes.Xipona.Api.Endpoint.Middleware;

public class DiagnosticsMiddleware
{
    private readonly RequestDelegate _next;

    public DiagnosticsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var activity = Diagnostics.Instance.StartActivity(context.Request.Path);

        await _next(context);
    }
}
