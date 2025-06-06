using Microsoft.Extensions.DependencyInjection;
using Xipona.Api.Core.Services;

namespace Xipona.Api.Core;

public static class ServiceCollectionExtensions
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeService, DateTimeService>();
    }
}