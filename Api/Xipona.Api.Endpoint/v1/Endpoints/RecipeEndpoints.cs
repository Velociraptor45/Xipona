using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using Xipona.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using Xipona.Api.ApplicationServices.Recipes.Queries.AllIngredientQuantityTypes;
using Xipona.Api.ApplicationServices.Recipes.Queries.ItemAmountsForOneServing;
using Xipona.Api.ApplicationServices.Recipes.Queries.RecipeById;
using Xipona.Api.ApplicationServices.Recipes.Queries.SearchByTagIds;
using Xipona.Api.ApplicationServices.Recipes.Queries.SearchRecipesByName;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using Xipona.Api.Contracts.Recipes.Queries.Get;
using Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Xipona.Api.Domain.Recipes.Services.Queries.Quantities;
using Xipona.Api.Domain.RecipeTags.Models;
using System.Threading;

namespace Xipona.Api.Endpoint.v1.Endpoints;

public static class RecipeEndpoints
{
    private const string _routeBase = "v1/recipes";

    public static void RegisterRecipeEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .RegisterGetRecipeById()
            .RegisterSearchRecipesByName()
            .RegisterSearchRecipesByTags()
            .RegisterGetAllIngredientQuantityTypes()
            .RegisterGetItemAmountsForOneServing()
            .RegisterCreateRecipe()
            .RegisterModifyRecipe();
    }

    private static IEndpointRouteBuilder RegisterGetRecipeById(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/{{id:guid}}", GetRecipeById)
            .WithName("GetRecipeById")
            .Produces<RecipeContract>()
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetRecipeById(
        [FromRoute] Guid id,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<RecipeReadModel, RecipeContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new RecipeByIdQuery(new RecipeId(id));
            var result = await queryDispatcher.DispatchAsync(query, cancellationToken);
            var contract = contractConverter.ToContract(result);
            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.RecipeNotFound)
                return Results.NotFound(errorContract);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterSearchRecipesByName(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/search-by-name", SearchRecipesByName)
            .WithName("SearchRecipesByName")
            .Produces<List<RecipeSearchResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> SearchRecipesByName(
        [FromQuery] string searchInput,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<RecipeSearchResult, RecipeSearchResultContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorConverter,
        CancellationToken cancellationToken)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return Results.BadRequest("Search input mustn't be null or empty");
        }

        try
        {
            var query = new SearchRecipesByNameQuery(searchInput);
            var results = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

            if (results.Count == 0)
                return Results.NoContent();

            var contracts = contractConverter.ToContract(results).ToList();

            return Results.Ok(contracts);
        }
        catch (DomainException e)
        {
            var errorContract = errorConverter.ToContract(e.Reason);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterSearchRecipesByTags(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/search-by-tags", SearchRecipesByTags)
            .WithName("SearchRecipesByTags")
            .Produces<List<RecipeSearchResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> SearchRecipesByTags(
        [FromQuery] Guid[] tagIds,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<RecipeSearchResult, RecipeSearchResultContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new SearchRecipesByTagsQuery(tagIds.Select(t => new RecipeTagId(t)));

        var results = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();
        if (results.Count == 0)
            return Results.NoContent();

        var contracts = contractConverter.ToContract(results).ToList();
        return Results.Ok(contracts);
    }

    private static IEndpointRouteBuilder RegisterGetAllIngredientQuantityTypes(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/ingredient-quantity-types", GetAllIngredientQuantityTypes)
            .WithName("GetAllIngredientQuantityTypes")
            .Produces<List<IngredientQuantityTypeContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetAllIngredientQuantityTypes(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IngredientQuantityTypeReadModel, IngredientQuantityTypeContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new AllIngredientQuantityTypesQuery();
        var results = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (results.Count == 0)
            return Results.NoContent();

        var contracts = contractConverter.ToContract(results)
            .ToList();

        return Results.Ok(contracts);
    }

    private static IEndpointRouteBuilder RegisterGetItemAmountsForOneServing(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/{{id:guid}}/item-amounts-for-one-serving", GetItemAmountsForOneServing)
            .WithName("GetItemAmountsForOneServing")
            .Produces<ItemAmountsForOneServingContract>()
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetItemAmountsForOneServing(
        [FromRoute] Guid id,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IEnumerable<ItemAmountForOneServing>, ItemAmountsForOneServingContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new ItemAmountsForOneServingQuery(new RecipeId(id));
            var result = await queryDispatcher.DispatchAsync(query, cancellationToken);

            var contract = contractConverter.ToContract(result);

            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.RecipeNotFound or ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterCreateRecipe(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}", CreateRecipe)
            .WithName("CreateRecipe")
            .Produces<RecipeContract>(StatusCodes.Status201Created)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> CreateRecipe(
        [FromBody] CreateRecipeContract createRecipeContract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToDomainConverter<CreateRecipeContract, CreateRecipeCommand> commandConverter,
        [FromServices] IToContractConverter<RecipeReadModel, RecipeContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = commandConverter.ToDomain(createRecipeContract);

            var model = await commandDispatcher.DispatchAsync(command, cancellationToken);

            var contract = contractConverter.ToContract(model);

            return Results.CreatedAtRoute("GetRecipeById", new { id = contract.Id }, contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorConverter.ToContract(e.Reason);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterModifyRecipe(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/modify", ModifyRecipe)
            .WithName("ModifyRecipe")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> ModifyRecipe(
        [FromRoute] Guid id,
        [FromBody] ModifyRecipeContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToDomainConverter<(Guid, ModifyRecipeContract), ModifyRecipeCommand> commandConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = commandConverter.ToDomain((id, contract));

            await commandDispatcher.DispatchAsync(command, cancellationToken);

            return Results.NoContent();
        }
        catch (DomainException e)
        {
            var errorContract = errorConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode
                is ErrorReasonCode.RecipeNotFound
                or ErrorReasonCode.IngredientNotFound
                or ErrorReasonCode.PreparationStepNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }
    }
}