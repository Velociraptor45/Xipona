using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xipona.Api.Domain.Users.Models.Factories;
using Xipona.Api.Domain.Users.Ports;
using Xipona.Api.Domain.Users.Services.Creations;
using Xipona.Api.Domain.Users.Services.Queries;
using Xipona.Api.Domain.Users.Services.Update;

namespace Xipona.Api.Domain.Users;

public static class ServiceCollectionExtensions
{
    public static void AddUsers(this IServiceCollection services)
    {
        services.AddTransient<IUserFactory, UserFactory>();
        services.AddTransient<Func<CancellationToken, IUserCreationService>>(provider =>
        {
            return ct => new UserCreationService(
                provider.GetRequiredService<IUserFactory>(),
                provider.GetRequiredService<Func<CancellationToken, IUserRepository>>()(ct));
        });

        services.AddTransient<Func<CancellationToken, IGeneralSettingsUpdateService>>(provider =>
        {
            return ct => new GeneralSettingsUpdateService(
                provider.GetRequiredService<Func<CancellationToken, IGeneralSettingRepository>>()(ct),
                provider.GetRequiredService<IMemoryCache>());
        });

        services.AddTransient<Func<CancellationToken, IGeneralSettingsQueryService>>(provider =>
        {
            return ct => new GeneralSettingsQueryService(
                provider.GetRequiredService<Func<CancellationToken, IGeneralSettingRepository>>()(ct));
        });
    }
}
