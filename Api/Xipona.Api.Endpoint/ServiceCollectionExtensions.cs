using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Xipona.Api.Endpoint;

public static class ServiceCollectionExtensions
{
    public static void AddEndpointConverters(this IServiceCollection services)
    {
        services.AddToContractConverter();
        services.AddToDomainConverter();
        services.AddTransient<JwtSecurityTokenHandler>();
    }
}