using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using System.Threading;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;

public static class RecipeTagEndpoints
{
    private const string _routeBase = "v1/recipe-tags";

    public static void RegisterRecipeTagEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .RegisterGetAllRecipeTags()
            .RegisterCreateRecipeTag();
    }

    private static IEndpointRouteBuilder RegisterGetAllRecipeTags(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/all", GetAllRecipeTags)
            .WithName("GetAllRecipeTags")
            .Produces<List<RecipeTagContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetAllRecipeTags(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IRecipeTag, RecipeTagContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new GetAllQuery();
        var recipeTags = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (recipeTags.Count == 0)
            return Results.NoContent();

        var contracts = contractConverter.ToContract(recipeTags).ToList();
        return Results.Ok(contracts);
    }

    private static IEndpointRouteBuilder RegisterCreateRecipeTag(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}", CreateRecipeTag)
            .WithName("CreateRecipeTag")
            .Produces<RecipeTagContract>(StatusCodes.Status201Created)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> CreateRecipeTag(
        [FromBody] CreateRecipeTagContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<CreateRecipeTagContract, CreateRecipeTagCommand> domainConverter,
        [FromServices] IToContractConverter<IRecipeTag, RecipeTagContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var command = domainConverter.ToDomain(contract);
        try
        {
            var result = await commandDispatcher.DispatchAsync(command, cancellationToken);
            var resultContract = contractConverter.ToContract(result);
            return Results.CreatedAtRoute("GetAllRecipeTags", new { }, resultContract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            return Results.UnprocessableEntity(errorContract);
        }
    }
}