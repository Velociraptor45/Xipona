using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Users.Commands.Login;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Users.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;

public static class UserEndpoints
{
    private const string _routeBase = "v1/users";

    public static void RegisterUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .RegisterLogin();
    }

    private static IEndpointRouteBuilder RegisterLogin(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}/login", Login)
            .WithName("Login")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> Login(HttpContext httpContext,
        [FromServices] JwtSecurityTokenHandler handler,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        var auth = httpContext.Request.Headers["Authorization"].First()!;
        var token = handler.ReadJwtToken(auth[7..]);
        var subClaim = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (subClaim == null)
        {
            return Results.BadRequest("Token does not contain 'sub' claim");
        }

        var userId = new UserId(Guid.Parse(subClaim));

        try
        {
            var command = new LoginCommand(userId);
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }
}
