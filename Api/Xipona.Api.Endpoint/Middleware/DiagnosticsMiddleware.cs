using Microsoft.AspNetCore.Builder;

namespace ProjectHermes.Xipona.Api.Endpoint.Middleware;

public static class DiagnosticsMiddleware
{
    public static IApplicationBuilder UseDiagnosticsMiddleware(this IApplicationBuilder app)
    {
        return app.Use((context, next) =>
        {
            using var activity = Diagnostics.Instance.StartActivity(context.Request.Path);
            return next();
        });
    }
}