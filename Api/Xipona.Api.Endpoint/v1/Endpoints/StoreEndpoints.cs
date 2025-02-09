using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.CreateStore;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.DeleteStore;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Commands.ModifyStore;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Api.ApplicationServices.Stores.Queries.StoreById;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.ModifyStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using System.Threading;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;

public static class StoreEndpoints
{
    private const string _routeBase = "v1/stores";

    public static void RegisterStoreEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .RegisterGetStoreById()
            .RegisterGetActiveStoresForShopping()
            .RegisterGetActiveStoresForItem()
            .RegisterGetActiveStoresOverview()
            .RegisterCreateStore()
            .RegisterModifyStore()
            .RegisterDeleteStore();
    }

    private static IEndpointRouteBuilder RegisterGetStoreById(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/{{id:guid}}", GetStoreById)
            .WithName("GetStoreById")
            .Produces<StoreContract>()
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetStoreById(
        [FromRoute] Guid id,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IStore, StoreContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetStoreByIdQuery(new StoreId(id));
            var model = await queryDispatcher.DispatchAsync(query, cancellationToken);
            var contract = contractConverter.ToContract(model);
            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterGetActiveStoresForShopping(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/active-for-shopping", GetActiveStoresForShopping)
            .WithName("GetActiveStoresForShopping")
            .Produces<List<StoreForShoppingContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetActiveStoresForShopping(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IStore, StoreForShoppingContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new GetActiveStoresForShoppingQuery();
        var models = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (models.Count == 0)
            return Results.NoContent();

        var contracts = contractConverter.ToContract(models).ToList();
        return Results.Ok(contracts);
    }

    private static IEndpointRouteBuilder RegisterGetActiveStoresForItem(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/active-for-item", GetActiveStoresForItem)
            .WithName("GetActiveStoresForItem")
            .Produces<List<StoreForItemContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetActiveStoresForItem(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IStore, StoreForItemContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new GetActiveStoresForItemQuery();
        var models = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (models.Count == 0)
            return Results.NoContent();

        var contracts = contractConverter.ToContract(models).ToList();
        return Results.Ok(contracts);
    }

    private static IEndpointRouteBuilder RegisterGetActiveStoresOverview(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/active-overview", GetActiveStoresOverview)
            .WithName("GetActiveStoresOverview")
            .Produces<List<StoreSearchResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetActiveStoresOverview(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IStore, StoreSearchResultContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new GetActiveStoresOverviewQuery();
        var models = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (models.Count == 0)
            return Results.NoContent();

        var contracts = contractConverter.ToContract(models).ToList();
        return Results.Ok(contracts);
    }

    private static IEndpointRouteBuilder RegisterCreateStore(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}", CreateStore)
            .WithName("CreateStore")
            .Produces<StoreContract>(StatusCodes.Status201Created)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> CreateStore(
        [FromBody] CreateStoreContract createStoreContract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToDomainConverter<CreateStoreContract, CreateStoreCommand> domainConverter,
        [FromServices] IToContractConverter<IStore, StoreContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var command = domainConverter.ToDomain(createStoreContract);
        var model = await commandDispatcher.DispatchAsync(command, cancellationToken);
        var contract = contractConverter.ToContract(model);

        return Results.CreatedAtRoute("GetStoreById", new { id = contract.Id }, contract);
    }

    private static IEndpointRouteBuilder RegisterModifyStore(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}", ModifyStore)
            .WithName("ModifyStore")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> ModifyStore(
        [FromBody] ModifyStoreContract modifyStoreContract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<ModifyStoreContract, ModifyStoreCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        var command = domainConverter.ToDomain(modifyStoreContract);

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterDeleteStore(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete($"/{_routeBase}/{{id:guid}}", DeleteStore)
            .WithName("DeleteStore")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> DeleteStore(
        [FromRoute] Guid id,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        var command = new DeleteStoreCommand(new StoreId(id));

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }
}