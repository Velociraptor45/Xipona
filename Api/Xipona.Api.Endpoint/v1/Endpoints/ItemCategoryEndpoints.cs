using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;
using System.Threading;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;

public static class ItemCategoryEndpoints
{
    private const string _routeBase = "v1/item-categories";

    public static void RegisterItemCategoryEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .RegisterGetItemCategoryById()
            .RegisterSearchItemCategoriesByName()
            .RegisterGetAllActiveItemCategories()
            .RegisterModifyItemCategory()
            .RegisterCreateItemCategory()
            .RegisterDeleteItemCategory();
    }

    private static IEndpointRouteBuilder RegisterGetItemCategoryById(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/{{id:guid}}", GetItemCategoryById)
            .WithName("GetItemCategoryById")
            .Produces<ItemCategoryContract>()
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetItemCategoryById(
        [FromRoute] Guid id,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IItemCategory, ItemCategoryContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)

    {
        try
        {
            var query = new ItemCategoryByIdQuery(new ItemCategoryId(id));
            var result = await queryDispatcher.DispatchAsync(query, cancellationToken);
            var contract = contractConverter.ToContract(result);
            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound)
                return Results.NotFound(errorContract);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterSearchItemCategoriesByName(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}", SearchItemCategoriesByName)
            .WithName("SearchItemCategoriesByName")
            .Produces<List<ItemCategorySearchResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> SearchItemCategoriesByName(
        [FromQuery] string searchInput,
        [FromQuery] bool includeDeleted,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<ItemCategorySearchResultReadModel, ItemCategorySearchResultContract> contractConverter,
        CancellationToken cancellationToken)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return Results.BadRequest("Search input mustn't be null or empty");
        }

        var query = new ItemCategorySearchQuery(searchInput, includeDeleted);
        var itemCategoryReadModels = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (itemCategoryReadModels.Count == 0)
            return Results.NoContent();

        var itemCategoryContracts = contractConverter.ToContract(itemCategoryReadModels).ToList();

        return Results.Ok(itemCategoryContracts);
    }

    private static IEndpointRouteBuilder RegisterGetAllActiveItemCategories(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/active", GetAllActiveItemCategories)
            .WithName("GetAllActiveItemCategories")
            .Produces<List<ItemCategoryContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetAllActiveItemCategories(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<ItemCategoryReadModel, ItemCategoryContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new AllActiveItemCategoriesQuery();
        var readModels = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (readModels.Count == 0)
            return Results.NoContent();

        var contracts = contractConverter.ToContract(readModels).ToList();

        return Results.Ok(contracts);
    }

    private static IEndpointRouteBuilder RegisterModifyItemCategory(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}", ModifyItemCategory)
            .WithName("ModifyItemCategory")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> ModifyItemCategory(
        [FromBody] ModifyItemCategoryContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<ModifyItemCategoryContract, ModifyItemCategoryCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = domainConverter.ToDomain(contract);
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterCreateItemCategory(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}", CreateItemCategory)
            .WithName("CreateItemCategory")
            .Produces<ItemCategoryContract>(StatusCodes.Status201Created)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> CreateItemCategory(
        [FromQuery] string name,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IItemCategory, ItemCategoryContract> contractConverter,
        [FromServices] IToDomainConverter<string, CreateItemCategoryCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        var command = domainConverter.ToDomain(name);
        var model = await commandDispatcher.DispatchAsync(command, cancellationToken);

        var contract = contractConverter.ToContract(model);

        return Results.CreatedAtRoute("GetItemCategoryById", new { id = contract.Id }, contract);
    }

    private static IEndpointRouteBuilder RegisterDeleteItemCategory(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete($"/{_routeBase}/{{id:guid}}", DeleteItemCategory)
            .WithName("DeleteItemCategory")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> DeleteItemCategory(
        [FromRoute] Guid id,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<Guid, DeleteItemCategoryCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = domainConverter.ToDomain(id);
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }
}