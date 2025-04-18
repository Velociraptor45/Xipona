using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Domain.Accounts.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Accounts.Ports;
using ProjectHermes.Xipona.Api.Domain.Accounts.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.Accounts;

public static class ServiceCollectionExtensions
{
    public static void AddAccounts(this IServiceCollection services)
    {
        services.AddTransient<IUserFactory, UserFactory>();
        services.AddTransient<Func<CancellationToken, IUserCreationService>>(provider =>
        {
            return ct => new UserCreationService(
                provider.GetRequiredService<IUserFactory>(),
                provider.GetRequiredService<Func<CancellationToken, IUserRepository>>()(ct));
        });
    }
}
