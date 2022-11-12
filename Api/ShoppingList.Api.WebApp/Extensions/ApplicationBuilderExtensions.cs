using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ProjectHermes.ShoppingList.Api.WebApp.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(app =>
        {
            app.Run(async ctx =>
            {
                var logger = ctx.RequestServices.GetRequiredService<ILogger<Startup>>();
                var handler =
                    ctx.Features.Get<IExceptionHandlerPathFeature>();

                if (handler is null)
                {
                    logger.LogError("No exception handler registered");
                    return;
                }

                if (handler.Error is OperationCanceledException)
                {
                    ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await ctx.Response.WriteAsync("The request was canceled");
                }
                else
                {
                    logger.LogError(handler.Error, "An uncaught exception occurred");
                }
            });
        });
    }
}