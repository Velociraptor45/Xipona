using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Domain.Users.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Users.Ports;
using ProjectHermes.Xipona.Api.Domain.Users.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.Users;

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
    }
}
