using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Services;

namespace ProjectHermes.Xipona.Api.Core;

public static class ServiceCollectionExtensions
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeService, DateTimeService>();
    }
}