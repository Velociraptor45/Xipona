using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.IdentityModel.Tokens.Jwt;
using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.ApplicationServices.Users.Commands.Login;
using Xipona.Api.ApplicationServices.Users.Commands.UpdateGeneralSettings;
using Xipona.Api.ApplicationServices.Users.Queries.AllCurrencies;
using Xipona.Api.ApplicationServices.Users.Queries.GetGeneralSettings;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Users.Commands.AllCurrencies;
using Xipona.Api.Contracts.Users.Commands.Login;
using Xipona.Api.Contracts.Users.Commands.UpdateGeneralSettings;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Users.Models;
using Xipona.Api.Domain.Users.Services.Queries;
using Xipona.Api.Endpoint.Middleware;

namespace Xipona.Api.Endpoint.v1.Endpoints;

public static class UserEndpoints
{
    private const string _routeBase = "v1/users";

    public static void RegisterUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .RegisterLogin()
            .RegisterUpdateGeneralSettings()
            .RegisterGetAllCurrencies()
            .RegisterGetGeneralSettings();
    }

    private static IEndpointRouteBuilder RegisterLogin(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}/login", Login)
            .WithName("Login")
            .Produces<UserInfoContract>()
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> Login(HttpContext httpContext,
        [FromServices] JwtSecurityTokenHandler handler,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] AuthenticationOptions authOptions,
        CancellationToken cancellationToken)
    {
        var auth = httpContext.Request.Headers.Authorization.First()!;
        var token = handler.ReadJwtToken(auth[7..]);

        if (!Guid.TryParse(token.Subject, out Guid subject))
        {
            return Results.BadRequest($"Token contains invalid 'sub' claim: {token.Subject}");
        }

        var userId = new UserId(subject);

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

        if (!token.Payload.TryGetValue(authOptions.NameClaimType, out var name))
        {
            return Results.BadRequest($"Token doesn't contain a '{authOptions.NameClaimType}' claim");
        }

        var userInfo = new UserInfoContract
        {
            DisplayName = (string)name
        };

        return Results.Ok(userInfo);
    }

    private static IEndpointRouteBuilder RegisterUpdateGeneralSettings(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/general-settings", UpdateGeneralSettings)
            .WithName("UpdateGeneralSettings")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> UpdateGeneralSettings([FromBody] GeneralSettingsContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        var command = new UpdateGeneralSettingsCommand(contract.CurrencyId.ToEnum<Currency>());

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterGetAllCurrencies(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/all-currencies", GetAllCurrencies)
            .WithName("AllCurrencies")
            .Produces<List<CurrencyContract>>()
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetAllCurrencies(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<CurrencyReadModel, CurrencyContract> toContractConverter,
        CancellationToken cancellationToken)
    {
        var query = new AllCurrenciesQuery();
        var result = await queryDispatcher.DispatchAsync(query, cancellationToken);
        var contracts = toContractConverter.ToContract(result).ToList();

        return Results.Ok(contracts);
    }

    private static IEndpointRouteBuilder RegisterGetGeneralSettings(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/general-settings", GetGeneralSettings)
            .WithName("GetGeneralSettings")
            .Produces<GeneralSettingsContract>()
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetGeneralSettings(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IGeneralSetting, Contracts.Users.Queries.GetGeneralSettings.GeneralSettingsContract> toContractConverter,
        CancellationToken cancellationToken)
    {
        var query = new GetGeneralSettingsQuery();
        var result = await queryDispatcher.DispatchAsync(query, cancellationToken);
        var contract = toContractConverter.ToContract(result);

        return Results.Ok(contract);
    }
}
