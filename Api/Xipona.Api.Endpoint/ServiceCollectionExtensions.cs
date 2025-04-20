using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using System.IdentityModel.Tokens.Jwt;

namespace ProjectHermes.Xipona.Api.Endpoint;

public static class ServiceCollectionExtensions
{
    public static void AddEndpointConverters(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddImplementationOfGenericType(assembly, typeof(IToContractConverter<,>));
        services.AddImplementationOfGenericType(assembly, typeof(IToDomainConverter<,>));
        services.AddTransient<JwtSecurityTokenHandler>();
    }
}